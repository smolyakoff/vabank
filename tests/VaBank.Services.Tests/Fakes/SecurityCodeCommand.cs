using VaBank.Services.Contracts.Common.Commands;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Tests.Fakes
{
    internal class TestSecurityCodeCommand : ISecurityCodeCommand
    {
        public SecurityCodeModel SecurityCode { get; set; }
    }
}
