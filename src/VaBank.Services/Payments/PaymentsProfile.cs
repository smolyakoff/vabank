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
                .ForMember(x => x.HierarchicalName, cfg => cfg.MapFrom(x => x.HierarchicalName));
            CreateMap<CardPayment, PaymentArchiveItemModel>()
                .ForMember(x => x.DateUtc, cfg => cfg.MapFrom(x => x.CreatedDateUtc))
                .ForMember(x => x.OperationId, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.PaymentName, cfg => cfg.MapFrom(x => x.Category.Name))
                .ForMember(x => x.PaymentCode, cfg => cfg.MapFrom(x => x.Category.Code))
                .ForMember(x => x.Status, cfg => cfg.MapFrom(x => x.Status));
            CreateMap<PaymentOrder, PaymentOrderModel>();
            CreateMap<CardPayment, PaymentArchiveDetailsModel>()
                .ForMember(x => x.DateUtc, cfg => cfg.MapFrom(x => x.CompletedDateUtc))
                .ForMember(x => x.OperationId, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.PaymentName, cfg => cfg.MapFrom(x => x.Category.Name))
                .ForMember(x => x.Status, cfg => cfg.MapFrom(x => x.Status));
                
        }
    }
}
