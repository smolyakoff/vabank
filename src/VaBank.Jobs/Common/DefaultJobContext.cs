using System.Collections.Generic;
using Hangfire;

namespace VaBank.Jobs.Common
{
    public class DefaultJobContext : IJobContext
    {
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>(); 

        public IJobCancellationToken CancellationToken { get; set; }

        public void Set(string key, object value)
        {
            _dictionary[key] = value;
        }

        public object Get(string key)
        {
            if (_dictionary.ContainsKey(key))
            {
                return _dictionary[key];
            }
            return null;
        }
    }
}
