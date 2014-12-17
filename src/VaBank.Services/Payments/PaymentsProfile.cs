using AutoMapper;
using Newtonsoft.Json.Linq;
using VaBank.Core.Payments.Entities;
using VaBank.Services.Contracts.Common.Models;
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
                .ForMember(x => x.PaymentName, cfg => cfg.MapFrom(x => x.Withdrawal.Description))
                .ForMember(x => x.PaymentCode, cfg => cfg.MapFrom(x => x.Category.Code))
                .ForMember(x => x.Status, cfg => cfg.MapFrom(x => (ProcessStatusModel)x.Status));
            CreateMap<PaymentOrder, PaymentOrderModel>();
            CreateMap<CardPayment, PaymentArchiveDetailsModel>()
                .ForMember(x => x.DateUtc, cfg => cfg.MapFrom(x => x.CreatedDateUtc))
                .ForMember(x => x.OperationId, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.PaymentName, cfg => cfg.MapFrom(x => x.Withdrawal.Description))
                .ForMember(x => x.Status, cfg => cfg.MapFrom(x => x.Status));
            CreateMap<Payment, PaymentArchiveFormModel>()
                .ForMember(x => x.Form, cfg => cfg.MapFrom(x => JObject.Parse(x.Form)))
                .ForMember(x => x.Template, cfg => cfg.MapFrom(x => (PaymentTemplate) x.Category));
        }
    }
}
