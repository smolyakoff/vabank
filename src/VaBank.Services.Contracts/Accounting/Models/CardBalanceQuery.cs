using System;
using VaBank.Common.Data;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class CardBalanceQuery : IdentityQuery<Guid>
    {
        public string CurrencyISOName { get; set; }
    }
}
