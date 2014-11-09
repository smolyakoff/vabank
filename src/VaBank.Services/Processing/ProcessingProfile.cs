using AutoMapper;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Processing
{
    internal class ProcessingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ExchangeRate, ExchangeRateModel>()
                .ForMember(x => x.BaseCurrency, cfg => cfg.MapFrom(x => x.Base))
                .ForMember(x => x.ForeignCurrency, cfg => cfg.MapFrom(x => x.Foreign));
        }
    }
}
