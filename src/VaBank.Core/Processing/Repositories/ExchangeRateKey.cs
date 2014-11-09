using System;
using VaBank.Common.Validation;

namespace VaBank.Core.Processing.Repositories
{
    public struct ExchangeRateKey : IEquatable<ExchangeRateKey>
    {
        private readonly string _currency1ISOName;
        private readonly string _currency2ISOName;

        public ExchangeRateKey(string currency1ISOName, string currency2ISOName)
        {
            Argument.NotEmpty(currency1ISOName, "currency1ISOName");
            Argument.NotEmpty(currency2ISOName, "currency2ISOName");
            var reverse = string.CompareOrdinal(currency1ISOName, currency2ISOName) < 0;

            if (reverse)
            {
                _currency1ISOName = currency2ISOName;
                _currency2ISOName = currency1ISOName;
            }
            else
            {
                _currency1ISOName = currency1ISOName;
                _currency2ISOName = currency2ISOName;
            }
        }

        public string FirstCurrencyISOName
        {
            get { return _currency1ISOName; }
        }

        public string SecondCurrencyISOName
        {
            get { return _currency2ISOName; }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", FirstCurrencyISOName, SecondCurrencyISOName);
        }

        public bool Equals(ExchangeRateKey other)
        {
            return string.Equals(_currency1ISOName, other._currency1ISOName) && string.Equals(_currency2ISOName, other._currency2ISOName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ExchangeRateKey && Equals((ExchangeRateKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_currency1ISOName.GetHashCode() * 397) ^ _currency2ISOName.GetHashCode();
            }
        }

        public static bool operator ==(ExchangeRateKey left, ExchangeRateKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ExchangeRateKey left, ExchangeRateKey right)
        {
            return !left.Equals(right);
        }
    }
}
