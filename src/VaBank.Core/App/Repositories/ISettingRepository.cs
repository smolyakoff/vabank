using System.Collections.Generic;

namespace VaBank.Core.App.Repositories
{
    public interface ISettingRepository
    {
        T GetOrDefault<T>(string key);

        void Set<T>(string key, T value);

        Dictionary<string, T> BatchGet<T>(params string[] keys);
    }
}
