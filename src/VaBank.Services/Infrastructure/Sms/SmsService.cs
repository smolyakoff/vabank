using System;
using VaBank.Common.Validation;
using VaBank.Core.App.Entities;
using VaBank.Services.Common;
using VaBank.Services.Common.Security;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Infrastructure.Secuirty;
using VaBank.Services.Contracts.Infrastructure.Sms;

namespace VaBank.Services.Infrastructure.Sms
{
    public class SmsService : BaseService, ISmsService, ISecurityCodeService
    {
        private readonly SmsServiceDependencies _deps;

        private readonly SmsServiceSettings _settings;

        public SmsService(BaseServiceDependencies dependencies, SmsServiceDependencies smsServiceDependencies) : base(dependencies)
        {
            Argument.NotNull(smsServiceDependencies, "smsServiceDependencies");
            smsServiceDependencies.EnsureIsResolved();
            _deps = smsServiceDependencies;

            _settings = LoadSettings<SmsServiceSettings>();
        }

        public void SendSms(SendSmsCommand command)
        {
            EnsureIsValid(command);
            try
            {
                if (_settings.UseLogger)
                {
                    var smsModel = new SmsModel
                    {
                        From = _settings.OutboundPhoneNumber,
                        Text = command.Text,
                        To = command.RecipientPhoneNumber
                    };
                    _deps.SmsLogger.Log(smsModel);
                }
                var client = _deps.TwilioClientFactory.Create();
                client.SendSmsMessage(_settings.OutboundPhoneNumber, command.RecipientPhoneNumber, command.Text);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't send sms.", ex);
            }
        }

        public SecurityCodeTicketModel GenerateSecurityCode()
        {
            EnsureIsSecure<AuthenticatedSecurityValidator>();
            try
            {
                var codePair = _deps.SecurityCodeFactory.CreateSmsCode();
                _deps.SecurityCodes.Create(codePair.PrivateSecurityCode);
                Commit();
                Publish(new SmsCodeCreated(codePair.PublicSecurityCode.Id, codePair.PublicSecurityCode.Code));
                return codePair.PrivateSecurityCode.ToModel<SecurityCode, SecurityCodeTicketModel>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't generate security code.", ex);
            }
        }
    }
}