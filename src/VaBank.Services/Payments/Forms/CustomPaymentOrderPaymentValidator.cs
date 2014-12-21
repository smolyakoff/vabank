
using FluentValidation;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Validation;
using VaBank.Core.Accounting;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Payments;
using VaBank.Core.Processing;

namespace VaBank.Services.Payments.Forms
{
    [ValidatorName("payment-custom-paymentorder")]
    public class CustomPaymentOrderPaymentValidator : ObjectValidator<CustomPaymentOrderPaymentValidator.Form>
    {
        private readonly IRepository<Bank> _bankRepository;

        private readonly IRepository<Account> _accountRepository;

        private readonly BankSettings _bankSettings;

        public CustomPaymentOrderPaymentValidator(
            IRepository<Bank> bankRepository,
            IRepository<Account> accountRepository)
        {
            Argument.NotNull(bankRepository, "bankRepository");
            Argument.NotNull(accountRepository, "accountRepository");

            _bankSettings = new BankSettings();
            _bankRepository = bankRepository;
            _accountRepository = accountRepository;

            RuleFor(x => x.Amount)
                   .GreaterThanOrEqualTo(100)
                   .WithLocalizedMessage(() => Messages.MinAmountPayment, 100)
                   .LessThan(5000000)
                   .WithLocalizedName(() => Names.Amount);
            RuleFor(x => x.BeneficiaryAccountNo)
                .NotEmpty()
                .UseValidator(new AccountNumberValidator())
                .Must(VaBankAccountExists)
                .When(x => x.BeneficiaryBankCode == _bankSettings.VaBankCode)
                .WithLocalizedMessage(() => Messages.AccountDoesntExist);
            RuleFor(x => x.BeneficiaryName)
                .NotEmpty()
                .Length(1, 100)
                .WithLocalizedName(() => Names.BeneficiaryName);
            RuleFor(x => x.BeneficiaryTIN)
                .NotEmpty()
                .UseValidator(new TINValidator())
                .WithLocalizedName(() => Names.BeneficiaryTIN);
            RuleFor(x => x.BeneficiaryBankCode)
                .NotEmpty()
                .UseValidator(new BankCodeValidator(bankRepository))
                .WithLocalizedName(() => Names.BeneficiaryBankCode);
            RuleFor(x => x.PaymentCode)
                .UseValidator(new PaymentCodeValidator())
                .When(x => !string.IsNullOrEmpty(x.PaymentCode))
                .WithLocalizedName(() => Names.PaymentCode);
            RuleFor(x => x.Purpose)
                .NotEmpty()
                .Length(1, 256)
                .WithLocalizedName(() => Names.Purpose);

        }

        private bool VaBankAccountExists(string accountNo)
        {
            return _accountRepository.Find(accountNo) != null;
        }

        public class Form
        {
            public decimal Amount { get; set; }

            public string BeneficiaryAccountNo { get; set; }

            public string BeneficiaryName { get; set; }

            public string BeneficiaryTIN { get; set; }

            public string BeneficiaryBankCode { get; set; }

            public string PaymentCode { get; set; }

            public string Purpose { get; set; }
        }
    }
}
