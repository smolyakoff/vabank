using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace VaBank.Common.Security
{
    public static class Hash
    {
        public static string Compute(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentException("You might not want to hash empty string.", "source");
            }
            var provider = new SHA512CryptoServiceProvider();
            var message = Encoding.UTF8.GetBytes(source);
            var hash = provider.ComputeHash(message);
            return Convert.ToBase64String(hash);
        }

        public static string Compute(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentException("You might not want to hash empty object.", "obj");
            }
            var provider = new SHA512CryptoServiceProvider();
            var message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
            var hash = provider.ComputeHash(message);
            return Convert.ToBase64String(hash);
        }
    }
}
