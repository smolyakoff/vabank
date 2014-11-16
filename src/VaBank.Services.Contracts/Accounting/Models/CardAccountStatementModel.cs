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

        //TODO: change to some short version of model
        public CustomerCardModel Card { get; set; }

        public string AccountNo { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public decimal StatementBalance { get; set; }

        public CurrencyModel AccountCurrency { get; set; }

        public Range<DateTime> DateRange { get; set; }
 
        public List<CardAccountStatementItemModel> Transactions { get; private set; } 
    }
}