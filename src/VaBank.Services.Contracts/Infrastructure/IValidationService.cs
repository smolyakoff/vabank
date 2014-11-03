namespace VaBank.Services.Contracts.Infrastructure
{
    public interface IValidationService : IService
    {
        ValidationResultModel Validate(ValidationCommand validationCommand);
    }
}
