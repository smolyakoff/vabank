using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Core.Processing.Entities
{
    public class CardTransfer : Transfer
    {
        internal CardTransfer(OperationCategory category, UserCard from, UserCard to, decimal amount) 
            : base(category, from.Account, to.Account, from.Account.Currency, amount)
        {
            Argument.NotNull(from, "from");
            Argument.Satisfies(from, x => x.Account != null, "from", "Source card should be bound to a bank account.");
            Argument.NotNull(to, "to");
            Argument.Satisfies(to, x => x.Account != null, "to", "Destination card should be bound to a bank account.");

            From = from.Account;
            To = to.Account;
            Currency = from.Account.Currency;
            Amount = amount;
            Type = from.Owner.Id == to.Owner.Id 
                ? CardTransferType.Personal 
                : CardTransferType.Interbank;
        }

        public CardTransferType Type { get; protected set; }
    }
}
