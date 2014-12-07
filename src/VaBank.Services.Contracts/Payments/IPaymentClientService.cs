using System.Collections.Generic;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Infrastructure.Validation;
using VaBank.Services.Contracts.Payments.Commands;
using VaBank.Services.Contracts.Payments.Models;
using VaBank.Services.Contracts.Payments.Queries;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Payments
{
    public interface IPaymentClientService
    {
        //Need this ASAP
        PaymentTemplateModel GetTemplate(IdentityQuery<string> code);

        //Need this ASAP
        ValidationResultModel Validate(ValidateFormCommand command);
 
        BankOperationModel Submit(SubmitPaymentCommand command);

        IList<PaymentArchiveItemModel> QueryArchive(PaymentArchiveQuery query);

        PaymentArchiveDetailsModel GetArchiveDetails(IdentityQuery<long> operationId);
    }
}
