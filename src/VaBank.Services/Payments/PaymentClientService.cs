using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Core.Payments.Entities;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
using VaBank.Services.Common.Security;
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

        public PaymentArchiveFormModel GetForm(IdentityQuery<long> operationId)
        {
            EnsureIsValid(operationId);
            EnsureIsSecure<IdentityQuery<long>, UserBankOperationSecurityValidator>(operationId);
            try
            {
                var formModel = _deps.Payments.FindAndProject<PaymentArchiveFormModel>(operationId.Id);
                return formModel;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get payment archive form.", ex);
            }
        }

        public IList<PaymentArchiveItemModel> QueryArchive(PaymentArchiveQuery query)
        {
            EnsureIsValid(query);
            EnsureIsSecure<PaymentArchiveQuery, UserQueryValidator>(query);
            try
            {
                var operationsByUser = DbQuery.For<UserBankOperation>()
                    .FilterBy(x => x.User.Id == query.UserId && x.Operation is CardPayment);
                var operations = _deps.UserBankOperations.Query(operationsByUser);
                var payments = operations.Select(x => x.Operation)
                    .Cast<CardPayment>()
                    .Map<CardPayment, PaymentArchiveItemModel>()
                    .ToList();
                return payments;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get payments archive.", ex);
            }
        }

        public PaymentArchiveDetailsModel GetArchiveDetails(IdentityQuery<long> operationId)
        {
            EnsureIsValid(operationId);
            EnsureIsSecure<IdentityQuery<long>, UserBankOperationSecurityValidator>(operationId);
            try
            {
                var payment = _deps.CardPayments.FindAndProject<PaymentArchiveDetailsModel>(operationId.Id);
                return payment;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get payment details.", ex);
            }
        }
    }
}
