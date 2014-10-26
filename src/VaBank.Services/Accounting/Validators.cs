using System;
using FluentValidation;
using JetBrains.Annotations;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Validation;
using VaBank.Core.Accounting;
using VaBank.Core.Accounting.Entities;
using VaBank.Services.Contracts.Accounting.Commands;

namespace VaBank.Services.Accounting
{
    internal class CreateCardCommandValidator : AbstractValidator<CreateCardCommand>
    {
        private readonly IRepository<CardAccount> _cardAccountRepository;

        public CreateCardCommandValidator(IRepository<CardAccount> cardAccountRepository)
        {
            _cardAccountRepository = cardAccountRepository;

            RuleFor(x => x.AccountNo)
                .NotEmpty()
                .Must(CardAccountExists);
            RuleFor(x => x.CardVendorId)
                .NotEmpty();
            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty);
            RuleFor(x => x.CardholderFirstName)
                .NotEmpty();
            RuleFor(x => x.CardholderLastName)
                .NotEmpty();
            RuleFor(x => x.ExpirationDateUtc)
                .UseValidator(new CardExpirationDateValidator())
                .Must(IsExpirationDateValid);
        }

        private bool IsExpirationDateValid(CreateCardCommand command, DateTime expirationDate)
        {
            var account = _cardAccountRepository.Find(command.AccountNo);
            if (account == null)
            {
                return false;
            }
            return account.ExpirationDateUtc >= expirationDate;
        }

        private bool CardAccountExists(string accountNo)
        {
            return _cardAccountRepository.Find(accountNo) != null;
        }
    }

    internal class SetCardAssignmentCommandValidator : AbstractValidator<SetCardAssignmentCommand>
    {
        private readonly IRepository<UserCard> _userCardRepository;

        public SetCardAssignmentCommandValidator(IRepository<UserCard> userCardRepository)
        {
            if (userCardRepository == null)
            {
                throw new ArgumentNullException("userCardRepository");
            }
            _userCardRepository = userCardRepository;

            RuleFor(x => x.AccountNo)
                .NotEmpty()
                .When(x => !x.Assigned).Must(IsAssignedToSpecifiedAccount);
            RuleFor(x => x.CardId)
                .NotEqual(Guid.Empty);
        }

        private bool IsAssignedToSpecifiedAccount(SetCardAssignmentCommand command, string accountNo)
        {
            var userCard = _userCardRepository.Find(command.CardId);
            return userCard.Account == null || userCard.Account.AccountNo == command.AccountNo;
        }
    }
}
