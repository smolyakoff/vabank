using System;
using System.Collections.Generic;
using VaBank.Common.Util;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class CardAccountStatementModel
    {
        public CardAccountStatementModel()
        {
            Transactions = new List<CardAccountStatementItemModel>();
        }

        public CustomerCardBriefModel Card { get; set; }

        public string AccountNo { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public decimal StatementDeposits { get; set; }

        public decimal StatementWithdrawals { get; set; }

        public decimal StatementBalance { get; set; }

        public CurrencyModel AccountCurrency { get; set; }

        public Range<DateTime> DateRange { get; set; }
 
        public List<CardAccountStatementItemModel> Transactions { get; set; } 
    }
}