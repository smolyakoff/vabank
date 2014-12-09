using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Payments.Entities;

namespace VaBank.Data.Tests.EntityFramework.Mocks
{
    public class PaymentOrderMock: PaymentOrder
    {
        public PaymentOrderMock(
        long   no,
        string payerName,
        string payerBankCode,
        string payerAccountNo,
        string payerTIN,
        string beneficiaryName,
        string beneficiaryBankCode,
        string beneficiaryAccountNo,
        string beneficiaryTIN,
        string purpose,
        decimal amount,
        string currencyISOName,
        string paymentCode
        ): base()
        {
            No = no;
            PayerName = payerName;
            PayerBankCode = payerBankCode;
            PayerAccountNo = payerAccountNo;
            PayerTIN = payerTIN;
            BeneficiaryName = beneficiaryName;
            BeneficiaryBankCode = beneficiaryBankCode;
            BeneficiaryAccountNo = beneficiaryAccountNo;
            BeneficiaryTIN = beneficiaryTIN;
            Purpose = purpose;
            Amount = amount;
            CurrencyISOName = currencyISOName;
            PaymentCode = paymentCode;
        }
    }
}
