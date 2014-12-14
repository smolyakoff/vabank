using System;
using System.Collections.Generic;
using VaBank.Common.Util;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class CardAccountStatementModel
    {
        public CardAccountStatementModel()
        {
            Transactions = new List<CardAccountStatementItemModel>();
        }

        public string AccountNo { get; set; }

        public UserNameModel AccountOwner { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public decimal StatementDeposits { get; set; }

        public decimal StatementWithdrawals { get; set; }

        public decimal StatementBalance { get; set; }

        public CurrencyModel AccountCurrency { get; set; }

        public Range<DateTime> DateRange { get; set; }
 
        public List<CardAccountStatementItemModel> Transactions { get; set; } 
    }
}