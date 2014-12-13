using System;
using System.Collections.Generic;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Infrastructure.Validation;
using VaBank.Services.Contracts.Payments;
using VaBank.Services.Contracts.Payments.Commands;
using VaBank.Services.Contracts.Payments.Models;
using VaBank.Services.Contracts.Payments.Queries;
using VaBank.Services.Contracts.Processing.Models;

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

        public ValidationResultModel Validate(ValidateFormCommand command)
        {
            throw new NotImplementedException();
        }

        public BankOperationModel Submit(SubmitPaymentCommand command)
        {
            throw new NotImplementedException();
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
