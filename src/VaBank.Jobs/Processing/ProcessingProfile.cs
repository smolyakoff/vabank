using AutoMapper;
using VaBank.Services.Contracts.Processing.Commands;
using VaBank.Services.Contracts.Processing.Events;

namespace VaBank.Jobs.Processing
{
    public class ProcessingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<IBankOperationEvent, ProcessBankOperationCommand>()
                .ForMember(x => x.BankOperationId, cfg => cfg.MapFrom(x => x.BankOperationId));
        }
    }
}
