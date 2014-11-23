using System;
using System.Collections.Generic;

namespace VaBank.Core.App.Repositories
{
    public interface ISettingRepository
    {
        T GetOrDefault<T>(string key);

        object GetOrDefault(string key, Type settingsType);

        T GetOrDefault<T>() where T : class;

        object GetOrDefault(Type settingsType);

        void Set<T>(string key, T value);

        Dictionary<string, T> BatchGet<T>(params string[] keys);
    }
}
