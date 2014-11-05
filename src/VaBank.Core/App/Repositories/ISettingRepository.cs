using System.Collections.Generic;

namespace VaBank.Core.App.Repositories
{
    public interface ISettingRepository
    {
        T GetOrDefault<T>(string key);

        T GetOrDefault<T>() where T : class;

        void Set<T>(string key, T value);

        Dictionary<string, T> BatchGet<T>(params string[] keys);
    }
}
