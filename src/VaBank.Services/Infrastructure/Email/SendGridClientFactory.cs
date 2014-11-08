using System;
using System.Net;
using SendGrid;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.App.Repositories;

namespace VaBank.Services.Infrastructure.Email
{
    [Injectable]
    public class SendGridClientFactory
    {
        private readonly ISettingRepository _settingRepository;

        public SendGridClientFactory(ISettingRepository settingRepository)
        {
            Argument.NotNull(settingRepository, "settingRepository");
            _settingRepository = settingRepository;
        }

        public Web CreateWebClient()
        {
            var settings = _settingRepository.GetOrDefault<SendGridClientSettings>();
            if (settings == null)
            {
                throw new InvalidOperationException("Settings for email client were not found.");
            }
            return new Web(new NetworkCredential(settings.Username, settings.Password));
        }
    }
}