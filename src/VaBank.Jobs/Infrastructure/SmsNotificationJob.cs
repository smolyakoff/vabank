using System;
using Autofac;
using VaBank.Common.Data;
using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Accounting.Events;
using VaBank.Services.Contracts.Infrastructure.Sms;

namespace VaBank.Jobs.Infrastructure
{
    public class SmsNotificationJob : EventListenerJob<SmsNotificationJobContext, ISmsEvent>
    {
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
            if (!smsCodeCreated.UserId.HasValue)
            {
                Logger.Error("Sms code event didn't contain user id.");
                return;
            }
            var profile = context.UserService.GetProfile(new IdentityQuery<Guid>(smsCodeCreated.UserId.Value));
            if (profile == null)
            {
                Logger.Error("Sms code was created for user without a profile.");
                return;
            }
            if (!profile.SmsConfirmationEnabled)
            {
                Logger.Error("Sms code was created for user with sms confirmation feature disabled.");
                return;
            }
            if (!profile.PhoneNumberConfirmed)
            {
                Logger.Error("Sms code was created for user with not confirmed phone number.");
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
            if (!cardBlocked.UserId.HasValue)
            {
                Logger.Error("Card blocked event event didn't contain user id.");
                return;
            }
            var profile = context.UserService.GetProfile(new IdentityQuery<Guid>(cardBlocked.UserId.Value));
            if (profile == null)
            {
                Logger.Error("Sms event was created for user without a profile.");
                return;
            }
            if (!profile.SmsNotificationEnabled)
            {
                return;
            }
            var cardName = string.Format("{0}('{1}')", 
                cardBlocked.Card.SecureCardNo, 
                cardBlocked.Card.FriendlyName ?? cardBlocked.Card.CardVendor.Name);
            var sms = new SendSmsCommand
            {
                RecipientPhoneNumber = profile.PhoneNumber,
                Text = string.Format(SmsMessages.CardBlocked, cardName)
            };
            context.SmsService.SendSms(sms);
        }

        private void Handle(SmsNotificationJobContext context, ISmsEvent smsEvent)
        {
            Logger.Warn("Unknown sms event [{0}] arrived.", smsEvent.GetType());
        }
    }
}
