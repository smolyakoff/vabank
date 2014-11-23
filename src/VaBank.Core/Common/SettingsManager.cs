using System;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.App.Repositories;

namespace VaBank.Core.Common
{
    [Injectable]
    public class SettingsManager
    {
        private readonly ISettingRepository _settingRepository;

        public SettingsManager(ISettingRepository settingRepository)
        {
            Argument.NotNull(settingRepository, "settingRepository");
            _settingRepository = settingRepository;
        }

        public object Load(Type settingsType)
        {
            return _settingRepository.GetOrDefault(settingsType);
        }
    }
}
