using System;
using Twilio;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.App.Repositories;

namespace VaBank.Services.Infrastructure.Sms
{
    [Injectable]
    public class TwilioClientFactory
    {
        private readonly ISettingRepository _settingRepository;

        public TwilioClientFactory(ISettingRepository settingRepository)
        {
            Argument.NotNull(settingRepository, "settingRepository");
            _settingRepository = settingRepository;
        }

        public TwilioRestClient Create()
        {
            var settings = _settingRepository.GetOrDefault<TwilioClientSettings>();
            if (settings == null)
            {
                throw new InvalidOperationException("Settings for sms client were not found.");
            }
            return new TwilioRestClient(settings.AccountSid, settings.AuthToken);
        }
    }
}
