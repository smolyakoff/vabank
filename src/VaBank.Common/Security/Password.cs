using System;
using SimpleCrypto;

namespace VaBank.Common.Security
{
    public class Password
    {
        private const int HashIterations = 100000;

        public static Password Create(string plainTextPassword, int saltSize = 64)
        {
            if (string.IsNullOrWhiteSpace(plainTextPassword))
            {
                throw new ArgumentException("Password should not be empty.");
            }
            if (saltSize < 16 || saltSize > 512)
            {
                throw new ArgumentOutOfRangeException("saltSize", saltSize, "Salt size should be between 16 and 512.");
            }
            var cryptoService = new PBKDF2();
            var salt = cryptoService.GenerateSalt(HashIterations, saltSize);
            var hash = cryptoService.Compute(plainTextPassword, salt);
            return new Password(hash, salt);
        }

        public static bool Validate(string savedPasswordHash, string savedPasswordSalt, string plainTextPassword)
        {
            if (string.IsNullOrEmpty(savedPasswordHash))
            {
                throw new ArgumentNullException("savedPasswordHash");
            }
            if (string.IsNullOrEmpty(savedPasswordHash))
            {
                throw new ArgumentNullException("savedPasswordSalt");
            }
            if (string.IsNullOrEmpty(savedPasswordHash))
            {
                throw new ArgumentNullException("plainTextPassword");
            }
            var cryptoService = new PBKDF2();
            var passwordHash = cryptoService.Compute(plainTextPassword, savedPasswordSalt);
            return cryptoService.Compare(passwordHash, savedPasswordHash);
        }

        private Password(string passwordHash, string passwordSalt)
        {
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public string PasswordHash { get; private set; }

        public string PasswordSalt { get; private set; }

    }
}
