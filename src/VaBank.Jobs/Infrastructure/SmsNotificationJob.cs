using System;
using System.Linq;
using Autofac;
using VaBank.Common.Data;
using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Accounting.Events;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Infrastructure.Sms;
using VaBank.Services.Contracts.Membership.Models;
using VaBank.Services.Contracts.Processing.Events;

namespace VaBank.Jobs.Infrastructure
{
    public class SmsNotificationJob : EventListenerJob<SmsNotificationJobContext, ISmsEvent>
    {
        private readonly TimeZoneInfo _timezone = 
            TimeZoneInfo.CreateCustomTimeZone("BY", TimeSpan.FromHours(3), "MINSK - BY", "MINSK - BY");

        public SmsNotificationJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override void Execute(SmsNotificationJobContext context)
        {
            var dynamicEvent = (dynamic) context.Data;
            Handle(context, dynamicEvent);
        }

        private void Handle(SmsNotificationJobContext context, SmsCodeCreated smsCodeCreated)
        {
            var profile = VerifyProfile(context, smsCodeCreated.UserId);
            if (profile == null)
            {
                return;
            }
            if (!profile.SmsConfirmationEnabled)
            {
                Logger.Error("Sms code was created for user with sms confirmation feature disabled.");
                return;
            }
            var sms = new SendSmsCommand
            {
                RecipientPhoneNumber = profile.PhoneNumber,
                Text = string.Format(SmsMessages.SecurityCode, smsCodeCreated.Code)
            };
            context.SmsService.SendSms(sms);
        }

        private void Handle(SmsNotificationJobContext context, UserCardBlocked cardBlocked)
        {
            var profile = VerifyProfile(context, cardBlocked.UserId);
            if (profile == null)
            {
                return;
            }
            if (!profile.SmsNotificationEnabled)
            {
                return;
            }
            var card = context.CardAccountService.GetCard(new IdentityQuery<Guid>(cardBlocked.Card.CardId));
            if (card == null)
            {
                throw new InvalidOperationException("Can't find card.");
            }
            var secureCardNo = string.Format(
                    "{0}****{1}",
                    new string(card.CardNo.Take(8).ToArray()),
                    new string(card.CardNo.Skip(12).ToArray()));
            var cardName = string.Format("{0}('{1}')", 
                secureCardNo, 
                cardBlocked.Card.FriendlyName ?? cardBlocked.Card.CardVendor.Name);
            var sms = new SendSmsCommand
            {
                RecipientPhoneNumber = profile.PhoneNumber,
                Text = string.Format(SmsMessages.CardBlocked, cardName)
            };
            context.SmsService.SendSms(sms);
        }

        private void Handle(SmsNotificationJobContext context, TransactionProcessedEvent smsEvent)
        {
            var transaction = context.ProcessingService.GetCardTransaction(new IdentityQuery<Guid>(smsEvent.TransactionId));
            if (transaction == null)
            {
                return;
            }
            if (transaction.Status == ProcessStatusModel.Pending)
            {
                return;
            }
            var account = context.CardAccountService.GetCardAccountBrief(new IdentityQuery<string>(transaction.AccountNo));
            if (account == null)
            {
                Logger.Info("Couldn't find account for card transaction #{0}.", smsEvent.TransactionId);
                return;
            }
            var profile = VerifyProfile(context, account.Owner.UserId);
            if (profile == null || !profile.SmsNotificationEnabled)
            {
                return;
            }
            var sms = new SendSmsCommand { RecipientPhoneNumber = profile.PhoneNumber };
            var secureCardNo = string.Format(
                    "{0}****{1}",
                    new string(transaction.CardNo.Take(8).ToArray()),
                    new string(transaction.CardNo.Skip(12).ToArray()));
            if (transaction.Status == ProcessStatusModel.Failed)
            {

                var message = string.Format(
                    SmsMessages.CardError,
                    secureCardNo,
                    string.Format("{0:F2} {1}", transaction.TransactionAmount, transaction.Currency.ISOName),
                    transaction.Location,
                    TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timezone),
                    string.Format("{0:F2} {1}", account.Balance, account.Currency.ISOName));
                sms.Text = message;
                context.SmsService.SendSms(sms);
                return;
            }
            if (transaction.TransactionAmount < 0)
            {
                var message = string.Format(
                    SmsMessages.CardWithdrawal,
                    secureCardNo,
                    string.Format("{0:F2} {1}", - transaction.TransactionAmount, transaction.Currency.ISOName),
                    transaction.Location,
                    TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timezone),
                    string.Format("{0:F2} {1}", account.Balance, account.Currency.ISOName));
                sms.Text = message;
                context.SmsService.SendSms(sms);
            }
            else
            {
                var message = string.Format(
                    SmsMessages.CardDeposit,
                    secureCardNo,
                    string.Format("{0:F2} {1}", transaction.TransactionAmount, transaction.Currency.ISOName),
                    transaction.Location,
                    TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timezone),
                    string.Format("{0:F2} {1}", account.Balance, account.Currency.ISOName));
                sms.Text = message;
                context.SmsService.SendSms(sms);
            }
        }

        private UserProfileModel VerifyProfile(SmsNotificationJobContext context, Guid? userId)
        {
            if (!userId.HasValue)
            {
                Logger.Error("Sms event didn't contain user id.");
                return null;
            }
            var profile = context.UserService.GetProfile(new IdentityQuery<Guid>(userId.Value));
            if (profile == null)
            {
                Logger.Error("Profile was not found for sms event.");
                return null;
            }
            if (!profile.PhoneNumberConfirmed)
            {
                //TODO: should not send sms here but for now it's ok to ignore confirmation state
                Logger.Warn("Phone number not confirmed.");
            }
            if (string.IsNullOrEmpty(profile.PhoneNumber))
            {
                Logger.Error("Tried to send sms to user without phone number.");
                return null;
            }
            return profile;
        }

        private void Handle(SmsNotificationJobContext context, ISmsEvent smsEvent)
        {
            Logger.Warn("Unknown sms event [{0}] arrived.", smsEvent.GetType());
        }
    }
}
