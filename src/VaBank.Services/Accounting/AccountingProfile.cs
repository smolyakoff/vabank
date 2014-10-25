using AutoMapper;
using VaBank.Core.Accounting.Entities;
using VaBank.Services.Contracts.Accounting.Models;

namespace VaBank.Services.Accounting
{
    internal class AccountingProfile: Profile
    {
        protected override void Configure()
        {
            CreateMap<Currency, CurrencyModel>();
            CreateMap<CardVendor, CardVendorModel>();
        }
    }
}
