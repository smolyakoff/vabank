using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Core.Processing
{
    public class Money
    {
        private readonly Currency _currency;
        private readonly decimal _amount;

        public Money(Currency currency, decimal amount)
        {
            Argument.NotNull(currency, "currency");
            _currency = currency;
            _amount = amount;
        }

        public Currency Currency
        {
            get { return _currency; }
        }

        public decimal Amount
        {
            get { return _amount; }
        }
    }
}
