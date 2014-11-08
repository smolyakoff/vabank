namespace VaBank.Services.Contracts.Infrastructure.Validation
{
    public interface IValidationService : IService
    {
        ValidationResultModel Validate(ValidationCommand validationCommand);
    }
}
