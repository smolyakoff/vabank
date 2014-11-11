using System;
using VaBank.Common.Validation;
using VaBank.Core.App.Repositories;

namespace VaBank.Jobs.Common.Settings
{
    internal class JobSettingsProvider
    {
        private readonly ISettingRepository _settingRepository;

        private const string KeyPattern = "VaBank.Jobs.{0}";

        public JobSettingsProvider(ISettingRepository settingRepository)
        {
            Argument.NotNull(settingRepository, "settingRepository");
            _settingRepository = settingRepository;
        }

        public TSettings SurelyGetSettings<TSettings>(string jobName) where TSettings : class
        {
            Argument.NotEmpty(jobName, "jobName");
            var key = string.Format(KeyPattern, jobName);
            var setttings = _settingRepository.GetOrDefault<TSettings>(key);
            if (setttings == null)
            {
                var message = string.Format("Settings for job {0} were not found.", jobName);
                throw new InvalidOperationException(message);
            }
            return setttings;
        }
    }
}
