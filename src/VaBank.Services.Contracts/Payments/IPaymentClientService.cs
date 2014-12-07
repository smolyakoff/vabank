using System.Collections.Generic;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Payments.Commands;
using VaBank.Services.Contracts.Payments.Models;
using VaBank.Services.Contracts.Payments.Queries;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Payments
{
    public interface IPaymentClientService
    {
        BankOperationModel Submit(SubmitPaymentCommand command);

        PaymentTemplateModel GetTemplate(IdentityQuery<string> code);

        PaymentFormModel GetForm(IdentityQuery<long> operationId);

        IList<PaymentArchiveItemModel> QueryArchive(PaymentArchiveQuery query);

        PaymentArchiveDetailsModel GetArchiveDetails(IdentityQuery<long> operationId);
    }
}
