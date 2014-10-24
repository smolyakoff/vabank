using AutoMapper;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Maintenance.Commands;

namespace VaBank.Jobs
{
    public class JobsProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<IAuditedEvent, LogAppActionCommand>();
        }
    }
}
