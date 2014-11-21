using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Core.Processing.Entities
{
    public class CardTransfer : Transfer
    {
        internal CardTransfer(OperationCategory category, UserCard source, UserCard destination, decimal amount) 
            : base(category, source.Account, destination.Account, source.Account.Currency, amount)
        {
            Argument.NotNull(source, "from");
            Argument.Satisfies(source, x => x.Account != null, "from", "Source card should be bound to a bank account.");
            Argument.NotNull(destination, "to");
            Argument.Satisfies(destination, x => x.Account != null, "to", "Destination card should be bound to a bank account.");
            Argument.Satisfies(destination, x => x.Id != source.Id, "to", "Destination card can't be the same as source card.");

            From = source.Account;
            To = destination.Account;
            Currency = source.Account.Currency;
            SourceCard = source;
            DestinationCard = destination;
            Amount = amount;
            Type = source.Owner.Id == destination.Owner.Id 
                ? CardTransferType.Personal 
                : CardTransferType.Interbank;
        }

        public virtual Card SourceCard { get; set; }

        public virtual Card DestinationCard { get; set; }

        public CardTransferType Type { get; protected set; }
    }
}
