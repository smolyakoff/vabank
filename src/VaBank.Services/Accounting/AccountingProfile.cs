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

            CreateMap<CardAccount, CardAccountBriefModel>();
            CreateMap<UserCard, CardModel>()
                .ForMember(x => x.CardholderFirstName, cfg => cfg.MapFrom(x => x.HolderFirstName))
                .ForMember(x => x.CardholderLastName, cfg => cfg.MapFrom(x => x.HolderLastName))
                .Include<UserCard, OwnedCardModel>();

            CreateMap<UserCard, OwnedCardModel>()
                .ForMember(x => x.AccountNo, cfg => cfg.MapFrom(src => src.Account.AccountNo));

        }
    }
}
