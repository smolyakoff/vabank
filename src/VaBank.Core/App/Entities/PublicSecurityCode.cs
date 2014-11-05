using System;

namespace VaBank.Core.App.Entities
{
    public class PublicSecurityCode
    {
        private readonly Guid _id;
        private readonly string _code;

        internal PublicSecurityCode(Guid id, string code)
        {
            _id = id;
            _code = code;
        }

        public Guid Id
        {
            get { return _id; }
        }

        public string Code
        {
            get { return _code; }
        }
    }
}
