using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Linq;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Core.App.Entities;
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

        public PaymentArchiveFormModel GetFormWithTemplate(IdentityQuery<long> operationId)
        {
            EnsureIsValid(operationId);
            try
            {
                var payment = _deps.CardPayments.QueryIdentity(operationId);
                if (payment == null)
                {
                    return null;
                }
                var template = _deps.PaymentTemplates.SurelyFind(payment.Category.Code);
                return new PaymentArchiveFormModel
                {
                    Form = JObject.Parse(payment.Form),
                    Template = template.ToModel<PaymentTemplateModel>()
                };
            }
            catch (Exception ex)
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
                var operationIds =_deps.UserBankOperations
                    .Select(operationsByUser, x => x.OperationId)
                    .ToList();
                var paymentsQuery = DbQuery.For<PaymentArchiveItemModel>()
                    .FromClientQuery(query)
                    .AndFilterBy(x => operationIds.Contains(x.OperationId));
                var payments = _deps.CardPayments
                    .ProjectThenQuery<PaymentArchiveItemModel>(paymentsQuery)
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
