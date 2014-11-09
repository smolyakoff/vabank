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
                .ForMember(x => x.BaseCurrencyISOName, cfg => cfg.MapFrom(x => x.Base.ISOName))
                .ForMember(x => x.ForeignCurrencyISOName, cfg => cfg.MapFrom(x => x.Foreign.ISOName));
        }
    }
}
