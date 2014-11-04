using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Common.Commands
{
    public interface ISecurityCodeCommand
    {
        SecurityCodeModel SecurityCode { get; }
    }
}
