using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.App.Entities;
using VaBank.Core.App.Repositories;
using VaBank.Core.App.Settings;

namespace VaBank.Core.App.Factories
{
    [Injectable]
    public class SecurityCodeFactory
    {
        private readonly ISettingRepository _settingsRepository;

        public SecurityCodeFactory(ISettingRepository settingsRepository)
        {
            Argument.NotNull(settingsRepository, "settingsRepository");
            _settingsRepository = settingsRepository;
        }

        public SecurityCodePair CreateSmsCode()
        {
            var settings = _settingsRepository.GetOrDefault<SecurityCodeSettings>();
            return SecurityCodePair.GenerateSmsCode(settings.SmsCodeExpirationPeriod);
        }
    }
}
