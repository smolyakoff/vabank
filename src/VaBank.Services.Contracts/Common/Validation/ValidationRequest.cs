namespace VaBank.Services.Contracts.Common.Validation
{
    public class ValidationRequest
    {
        public string ValidatorName { get; set; }

        public object Value { get; set; }
    }
}
