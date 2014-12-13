using VaBank.Services.Contracts.Accounting.Models;

namespace VaBank.Services.Contracts.Processing.Models
{
    public class AccountBalanceModel
    {
        public string AccountNo { get; set; }

        public CurrencyModel AccountCurrency { get; set; }

        public decimal AccountBalance { get; set; }

        public CurrencyModel RequestedCurrency { get; set; }

        public decimal RequestedBalance { get; set; }
    }
}
