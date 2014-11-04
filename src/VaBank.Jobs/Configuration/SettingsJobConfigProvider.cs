using System;
using VaBank.Core.App.Repositories;

namespace VaBank.Jobs.Configuration
{
    public class SettingsJobConfigProvider : IJobConfigProvider
    {
        private const string SettingKey = "VaBank.Jobs.{0}";

        private readonly ISettingRepository _settingRepository;

        public SettingsJobConfigProvider(ISettingRepository settingsRepository)
        {
            if (settingsRepository == null)
                throw new ArgumentNullException("settingsRepository");
            _settingRepository = settingsRepository;
        }

        public TJobConfig Get<TJobConfig>(string jobName) where TJobConfig : class, IJobConfig
        {
            if (string.IsNullOrEmpty(jobName))
                throw new ArgumentNullException("jobName");
            return _settingRepository.GetOrDefault<TJobConfig>(GetKey(jobName));
        }

        private string GetKey(string jobName)
        {
            return string.Format(SettingKey, jobName);
        }
    }
}
