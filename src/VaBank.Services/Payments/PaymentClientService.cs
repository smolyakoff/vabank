using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Newtonsoft.Json.Linq;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
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

        public PaymentsLookupModel GetLookup(IIdentityQuery<Guid> userId)
        {
            EnsureIsValid(userId);
            try
            {
                var operationsByUser = DbQuery.For<UserBankOperation>()
                    .FilterBy(x => x.User.Id == userId.Id);
                var codes = _deps.UserBankOperations
                    .Select(operationsByUser, x => x.Operation.Category.Code)
                    .Distinct()
                    .ToList();
                var query = DbQuery.For<PaymentCategoryModel>().FilterBy(x => codes.Contains(x.Code));
                return new PaymentsLookupModel
                {
                    Categories = _deps.CardPayments.ProjectThenQuery<PaymentCategoryModel>(query).DistinctBy(x => x.Code).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get payments lookup.", ex);
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
                    .FilterBy(x => x.User.Id == query.UserId);
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

        public PaymentsTreeItemModel GetPaymentsTree()
        {
            try
            {
                var model = _deps.OperationCategories.SurelyFind("PAYMENT").ToModel<PaymentsTreeItemModel>();
                //var nodes = new List<PaymentsTreeItemModel>(model.Children);
                //var level = 1;
                //var temp = new List<PaymentsTreeItemModel>();
                //while (nodes.Count != 0)
                //{
                //    foreach (var node in nodes)
                //    {
                //        node.Level = level;
                //        if (node.Children != null)
                //            temp.AddRange(node.Children);
                //    }
                //    if (temp.Count == 0)
                //        break;
                //    ++level;
                //    nodes.Clear();
                //    nodes.AddRange(temp);
                //    temp.Clear();
                //}
                PaymentsTreeLoop(model, 1);
                return model;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get payments tree.", ex);
            }
        }

        private void PaymentsTreeLoop(PaymentsTreeItemModel node, int level)
        {
            foreach (var child in node.Children)
            {
                child.Level = level;
                if (child.Children == null || child.Children.Count == 0) continue;
                PaymentsTreeLoop(child, level + 1);
            }
        }
    }
}
