using AutoMapper;
using VaBank.Core.Payments.Entities;
using VaBank.Services.Contracts.Payments.Models;

namespace VaBank.Services.Payments
{
    public class PaymentsProfile : Profile
    {
        protected override void Configure()
        {  
            CreateMap<PaymentTemplate, PaymentTemplateModel>()
                .ForMember(x => x.Name, cfg => cfg.MapFrom(x => x.HierarchicalName));
        }
    }
}
