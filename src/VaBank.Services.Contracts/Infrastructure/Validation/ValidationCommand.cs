namespace VaBank.Services.Contracts.Infrastructure.Validation
{
    public class ValidationCommand
    {
        public string ValidatorName { get; set; }

        public object Value { get; set; }
    }
}
