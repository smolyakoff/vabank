using System;
using FluentValidation;
using FluentValidation.Results;
using NLog;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Validation;
using VaBank.Core.Payments;
using VaBank.Core.Payments.Entities;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
using VaBank.Services.Common.Security;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Infrastructure.Validation;
using VaBank.Services.Contracts.Payments.Commands;

namespace VaBank.Services.Payments
{
    internal class SubmitPaymentCommandValidator : AbstractValidator<SubmitPaymentCommand>
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ICardAccountService _cardAccountService;

        private readonly IValidationService _validationService;

        private readonly IRepository<PaymentOrderTemplate> _paymentOrderTemplateRepository; 

        public SubmitPaymentCommandValidator(
            ICardAccountService cardAccountService,
            IValidationService validationService,
            IRepository<PaymentOrderTemplate> paymentOrderTemplateRepository)
        {
            Argument.NotNull(cardAccountService, "cardAccountService");
            Argument.NotNull(validationService, "validationService");
            Argument.NotNull(paymentOrderTemplateRepository, "paymentOrderTemplateRepository");

            _cardAccountService = cardAccountService;
            _validationService = validationService;
            _paymentOrderTemplateRepository = paymentOrderTemplateRepository;
            
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.TemplateCode)
                .NotEmpty()
                .Must(OrderTemplateExists);
            RuleFor(x => x.FromCardId)
                .NotEqual(Guid.Empty);
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .Must(LessThenCardBalance)
                .WithLocalizedMessage(() => Messages.InsufficientFunds);
        }

        public override ValidationResult Validate(SubmitPaymentCommand command)
        {
            var result = base.Validate(command);
            if (!result.IsValid)
            {
                return result;
            }
            var validationResult = _validationService.Validate(new ValidationCommand
            {
                ValidatorName = command.TemplateCode,
                Value = command.Form
            });
            if (!validationResult.IsValidatorFound)
            {
                _logger.Warn("Form validation was skipped for [{0}] payment.", command.TemplateCode);
                return result;
            }
            foreach (var validationFault in validationResult.ValidationFaults)
            {
                var propertyName = string.Format("Form.{0}", validationFault.PropertyName);
                result.Errors.Add(new ValidationFailure(propertyName, validationFault.Message));
            }
            return result;
        }

        private bool OrderTemplateExists(string templateCode)
        {
            if (string.IsNullOrEmpty(templateCode))
            {
                return false;
            }
            return _paymentOrderTemplateRepository.Find(templateCode) != null;
        }

        private bool LessThenCardBalance(SubmitPaymentCommand command, decimal amount)
        {
            var orderTemplate = _paymentOrderTemplateRepository.Find(command.TemplateCode);
            var paymentForm = new PaymentForm(command.Form);
            var currencyISOName = paymentForm.RenderValueOrDefault<string>(orderTemplate.CurrencyISOName);
            var balance = _cardAccountService.GetCardBalance(
            new CardBalanceQuery
            {
                CurrencyISOName = currencyISOName,
                Id = command.FromCardId
            });
            return balance.RequestedBalance >= amount;
        }
    }

    internal class UserBankOperationSecurityValidator : SecurityValidator<IdentityQuery<long>>
    {
        private readonly IRepository<UserBankOperation> _userBankOperationRepository;

        public UserBankOperationSecurityValidator(VaBankIdentity identity, IRepository<UserBankOperation> userBankOperationRepository)
            :base(identity)
        {
            Argument.NotNull(userBankOperationRepository, "userBankOperationRepository");
            _userBankOperationRepository = userBankOperationRepository;
        }

        public UserBankOperationSecurityValidator(VaBankIdentity identity) : base(identity)
        {
            Inherit(new AuthenticatedSecurityValidator(identity));
            Custom(ValidateUserOwnsOperation);
        }

        private ValidationFailure ValidateUserOwnsOperation(IdentityQuery<long> id)
        {
            var userOperation = _userBankOperationRepository.QueryIdentity(id);
            if (userOperation != null && userOperation.User.Id != Identity.UserId)
            {
                return new ValidationFailure(RootPropertyName, Common.Security.Messages.InsufficientRights);
            }
            return null;
        }
    }
}
