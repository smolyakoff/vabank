using System;
using System.Globalization;
using System.Security.Cryptography;
using VaBank.Common.Validation;

namespace VaBank.Core.App.Entities
{
    public class SecurityCodePair
    {
        private readonly SecurityCode _privateSecurityCode;
        private readonly PublicSecurityCode _publicSecurityCode;

        private SecurityCodePair(SecurityCode privateSecurityCode, PublicSecurityCode publicSecurityCode)
        {
            _privateSecurityCode = privateSecurityCode;
            _publicSecurityCode = publicSecurityCode;
        }

        public static SecurityCodePair GenerateSmsCode(TimeSpan expirationPeriod)
        {
            Argument.Satisfies(expirationPeriod, x => x.Ticks > 0, "expirationPeriod", "Expiration period should be positive");
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteCode = new byte[8];
                rng.GetBytes(byteCode);
                var code = BitConverter.ToUInt64(byteCode, 0);
                var smsCode = (code % 100000UL).ToString(CultureInfo.InvariantCulture).PadLeft(6, '0');
                var id = Guid.NewGuid();
                var privateCode = new SecurityCode(id, expirationPeriod, smsCode);
                var publicCode = new PublicSecurityCode(id, smsCode);
                return new SecurityCodePair(privateCode, publicCode);
            }
        }

        public SecurityCode PrivateSecurityCode
        {
            get { return _privateSecurityCode; }
        }

        public PublicSecurityCode PublicSecurityCode
        {
            get { return _publicSecurityCode; }
        }
    }
}
