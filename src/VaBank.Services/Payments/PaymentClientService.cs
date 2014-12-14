using System;
using System.Collections.Generic;
using VaBank.Common.Data;
using VaBank.Core.Payments.Entities;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Payments;
using VaBank.Services.Contracts.Payments.Commands;
using VaBank.Services.Contracts.Payments.Models;
using VaBank.Services.Contracts.Payments.Queries;
using VaBank.Services.Contracts.Processing.Events;
using VaBank.Services.Contracts.Processing.Models;
using VaBank.Services.Processing;

namespace VaBank.Services.Payments
{
    public class PaymentClientService : BaseService, IPaymentClientService
    {
        private readonly PaymentClientServiceDependencies _deps;

        public PaymentClientService(BaseServiceDependencies dependencies, PaymentClientServiceDependencies deps)
            : base(dependencies)
        {
            deps.EnsureIsResolved();
            _deps = deps;
        }

        public PaymentTemplateModel GetTemplate(IdentityQuery<string> code)
        {
            EnsureIsValid(code);
            try
            {
                var template = _deps.PaymentTemplates.Find(code.Id);
                return template == null ? null : template.ToModel<PaymentTemplateModel>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get payment template.", ex);
            }
        }

        public BankOperationModel Submit(SubmitPaymentCommand command)
        {
            EnsureIsValid(command);
            EnsureIsSecure<SubmitPaymentCommand, CardSecurityValidator>(command);
            try
            {
                var userCard = _deps.UserCards.SurelyFind(command.FromCardId);
                var template = _deps.PaymentTemplates.SurelyFind(command.TemplateCode);
                var payment = _deps.CardPaymentFactory.Create(userCard, template, command.Form);
                var paymentLink = new PaymentTransactionLink(payment.Withdrawal, payment.Order);
                _deps.Payments.Create(payment);
                _deps.PaymentTransactionLinks.Create(paymentLink);
                var userOperation = new UserBankOperation(payment, Identity.User);
                _deps.UserBankOperations.Create(userOperation);
                Commit();
                var model = payment.ToModel<BankOperation, BankOperationModel>();
                Publish(new OperationProgressEvent(Operation.Id, model));
                return model;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't submit payment.", ex);
            }
        }

        public IList<PaymentArchiveItemModel> QueryArchive(PaymentArchiveQuery query)
        {
            throw new NotImplementedException();
        }

        public PaymentArchiveDetailsModel GetArchiveDetails(IdentityQuery<long> operationId)
        {
            throw new NotImplementedException();
        }
    }
}
