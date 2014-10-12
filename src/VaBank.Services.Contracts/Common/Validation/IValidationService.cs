namespace VaBank.Services.Contracts.Common.Validation
{
    public interface IValidationService : IService
    {
        ValidationResponse Validate(ValidationRequest validationRequest);
    }
}
