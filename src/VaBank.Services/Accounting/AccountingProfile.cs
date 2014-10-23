using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Accounting;
using VaBank.Services.Contracts.Accounting.Models;

namespace VaBank.Services.Accounting
{
    internal class AccountingProfile: Profile
    {
        protected override void Configure()
        {
            CreateMap<Currency, CurrencyModel>();
            CreateMap<UserCard, UserCardModel>(); //TO DO
        }
    }
}
