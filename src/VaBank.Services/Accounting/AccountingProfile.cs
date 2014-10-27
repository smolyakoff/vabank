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
                .ForMember(x => x.CardId, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.CardholderFirstName, cfg => cfg.MapFrom(x => x.HolderFirstName))
                .ForMember(x => x.CardholderLastName, cfg => cfg.MapFrom(x => x.HolderLastName))
                .Include<UserCard, UserCardModel>();

            CreateMap<UserCard, UserCardModel>()
                .ForMember(x => x.AccountNo, cfg => cfg.MapFrom(src => src.Account.AccountNo));

            CreateMap<CardLimits, CardLimitsModel>();

            CreateMap<UserCard, CustomerCardModel>()
                .ForMember(x => x.SecureCardNo, cfg => cfg.MapFrom(x => x.CardNo.Substring(0, 6) + "__" + x.CardNo.Substring(11, 4)))
                .ForMember(x => x.CardholerFirstName, cfg => cfg.MapFrom(x => x.HolderFirstName))
                .ForMember(x => x.CardholderLastName, cfg => cfg.MapFrom(x => x.HolderLastName))
                .ForMember(x => x.Blocked, cfg => cfg.MapFrom(x => x.Settings.Blocked))
                .ForMember(x => x.Currency, cfg => cfg.MapFrom(x => x.Account.Currency))
                .ForMember(x => x.Balance, cfg => cfg.MapFrom(x => x.Account.Balance))
                .ForMember(x => x.FriendlyName, cfg => cfg.MapFrom(x => x.Settings.FriendlyName))
                .ForMember(x => x.CardLimits, cfg => cfg.MapFrom(x => x.Settings.Limits));
        }
    }
}
