using FluentValidation;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Payments.Resources;

namespace VaBank.Core.Payments
{
    [ValidatorName("TIN")]
    [StaticValidator]
    public class TINValidator : PropertyValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.Matches(@"^\d{9}$").WithLocalizedMessage(() => Messages.TINFormat);
        }
    }

    [ValidatorName("BankCode")]
    public class BankCodeValidator : PropertyValidator<string>
    {
        private readonly IRepository<Bank> _bankRepository; 

        public BankCodeValidator(IRepository<Bank> bankRepository)
        {
            Argument.NotNull(bankRepository, "bankRepository");
            _bankRepository = bankRepository;
        }

        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.Matches(@"^\d{9}$").WithLocalizedMessage(() => Messages.BankCodeFormat)
                .Must(BankExists).WithLocalizedMessage(() => Messages.BankCodeInvalid);
        }

        private bool BankExists(string bankCode)
        {
            return _bankRepository.Find(bankCode) != null;
        }
    }

    [ValidatorName("PaymentCode")]
    [StaticValidator]
    public class PaymentCodeValidator : PropertyValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.Matches(@"^\d{4}$").WithLocalizedMessage(() => Messages.PaymentCodeFormat);
        }
    }
}
