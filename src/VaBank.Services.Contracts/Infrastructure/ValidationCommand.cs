namespace VaBank.Services.Contracts.Infrastructure
{
    public class ValidationCommand
    {
        public string ValidatorName { get; set; }

        public object Value { get; set; }
    }
}
