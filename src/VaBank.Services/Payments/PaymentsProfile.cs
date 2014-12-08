using AutoMapper;
using Newtonsoft.Json.Linq;
using VaBank.Core.Payments.Entities;
using VaBank.Services.Contracts.Payments.Models;

namespace VaBank.Services.Payments
{
    public class PaymentsProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<PaymentTemplate, PaymentTemplateModel>()
                .ForMember(x => x.FormTemplate, cfg => cfg.MapFrom(x => JObject.Parse(x.Form)))
                .ForMember(x => x.Name, cfg => cfg.Ignore()); //Temporary shit
        }
    }
}
