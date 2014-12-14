using AutoMapper;
using System;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Common;
using VaBank.Core.Membership.Entities;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Common.Models;

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

            CreateMap<Card, CardNameModel>()
                .ForMember(x => x.CardId, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.SecureCardNo, cfg => cfg.MapFrom(x => x.CardNo.Substring(0, 6) + "__" + x.CardNo.Substring(12, 4)))
                .Include<UserCard, CardNameModel>();

            CreateMap<UserCard, CardNameModel>()
                .ForMember(x => x.FriendlyName, cfg => cfg.MapFrom(x => x.Settings.FriendlyName));

            CreateMap<CardLimits, CardLimitsModel>();
            CreateMap<CardLimitsModel, CardLimits>();

            // Customer Cards
            CreateMap<UserCard, CustomerCardModel>()
                .ForMember(x => x.CardId, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.AccountNo, cfg => cfg.MapFrom(x => x.Account.AccountNo))
                .ForMember(x => x.SecureCardNo, cfg => cfg.MapFrom(x => x.CardNo.Substring(0, 6) + "__" + x.CardNo.Substring(12, 4)))
                .ForMember(x => x.CardholderFirstName, cfg => cfg.MapFrom(x => x.HolderFirstName))
                .ForMember(x => x.CardholderLastName, cfg => cfg.MapFrom(x => x.HolderLastName))
                .ForMember(x => x.Blocked, cfg => cfg.MapFrom(x => x.Settings.Blocked))
                .ForMember(x => x.Currency, cfg => cfg.MapFrom(x => x.Account.Currency))
                .ForMember(x => x.Balance, cfg => cfg.MapFrom(x => x.Account.Balance))
                .ForMember(x => x.FriendlyName, cfg => cfg.MapFrom(x => x.Settings.FriendlyName))
                .ForMember(x => x.CardLimits, cfg => cfg.MapFrom(x => x.Settings.Limits));

            CreateMap<UserCard, CustomerCardBriefModel>()
                .ForMember(x => x.CardId, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.AccountNo, cfg => cfg.MapFrom(x => x.Account.AccountNo))
                .ForMember(x => x.SecureCardNo, cfg => cfg.MapFrom(x => x.CardNo.Substring(0, 6) + "__" + x.CardNo.Substring(12, 4)))
                .ForMember(x => x.CardholderFirstName, cfg => cfg.MapFrom(x => x.HolderFirstName))
                .ForMember(x => x.CardholderLastName, cfg => cfg.MapFrom(x => x.HolderLastName))
                .ForMember(x => x.Currency, cfg => cfg.MapFrom(x => x.Account.Currency))
                .ForMember(x => x.Balance, cfg => cfg.MapFrom(x => x.Account.Balance))
                .ForMember(x => x.FriendlyName, cfg => cfg.MapFrom(x => x.Settings.FriendlyName))
                .ForMember(x => x.Owner, cfg => cfg.MapFrom(x => x.Owner));
            
            // Account Statement
            CreateMap<Transaction, CardAccountStatementItemModel>()
                .ForMember(x => x.TransactionId, cfg => cfg.MapFrom(x => x.Id))
                .Include<CardTransaction, CardAccountStatementItemModel>();
            CreateMap<CardTransaction, CardAccountStatementItemModel>();
            CreateMap<UserCard, CardAccountStatementModel>()
                .ForMember(x => x.AccountCurrency, cfg => cfg.MapFrom(x => x.Account.Currency))
                .ForMember(x => x.CreatedDateUtc, cfg => cfg.UseValue(DateTime.UtcNow))
                .ForMember(x => x.StatementBalance, cfg => cfg.MapFrom(x => x.Account.Balance));
        }
    }
}
