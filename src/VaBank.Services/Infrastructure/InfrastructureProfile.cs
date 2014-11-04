using AutoMapper;
using VaBank.Core.App.Entities;
using VaBank.Services.Contracts.Infrastructure.Secuirty;

namespace VaBank.Services.Infrastructure
{
    public class InfrastructureProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<SecurityCode, SecurityCodeTicketModel>();
        }
    }
}
