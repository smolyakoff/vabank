using System;
using VaBank.Common.Validation;
using VaBank.Core.Processing.Repositories;

namespace VaBank.Core.Processing
{
    public struct CurrencyConversion : IEquatable<CurrencyConversion>
    {
        private readonly string _fromCurrencyISOName;

        private readonly string _toCurrencyISOName;

        private readonly ExchangeRateKey _exchangeRateKey;

        public CurrencyConversion(string fromCurrencyISOName, string toCurrencyISOName)
        {
            Argument.NotNull(fromCurrencyISOName, "fromCurrencyISOName");
            Argument.NotNull(toCurrencyISOName, "toCurrencyISOName");
            _fromCurrencyISOName = fromCurrencyISOName;
            _toCurrencyISOName = toCurrencyISOName;
            _exchangeRateKey = new ExchangeRateKey(fromCurrencyISOName, toCurrencyISOName);
        }

        public string From
        {
            get { return _fromCurrencyISOName; }
        }

        public string To
        {
            get { return _toCurrencyISOName; }
        }

        public ExchangeRateKey ExchangeRateKey
        {
            get { return _exchangeRateKey; }
        }

        public bool Equals(CurrencyConversion other)
        {
            return string.Equals(_toCurrencyISOName, other._toCurrencyISOName) && string.Equals(_fromCurrencyISOName, other._fromCurrencyISOName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CurrencyConversion && Equals((CurrencyConversion)obj);
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", _fromCurrencyISOName, _toCurrencyISOName);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_toCurrencyISOName.GetHashCode() * 397) ^ _fromCurrencyISOName.GetHashCode();
            }
        }

        public static bool operator ==(CurrencyConversion left, CurrencyConversion right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CurrencyConversion left, CurrencyConversion right)
        {
            return !left.Equals(right);
        }
    }
}
