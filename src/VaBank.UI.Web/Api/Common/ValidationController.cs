using System;
using System.Web.Http;
using VaBank.Services.Contracts.Common.Validation;
using VaBank.Services.Contracts.Infrastructure;

namespace VaBank.UI.Web.Api.Common
{
    public class ValidationController : ApiController
    {
        private readonly IValidationService _validationService;

        public ValidationController(IValidationService validationService)
        {
            if (validationService == null)
            {
                throw new ArgumentNullException("validationService");
            }
            _validationService = validationService;
        }

        [Route("api/validate/{name}")]
        [HttpPost]
        public IHttpActionResult Validate([FromUri]string name, [FromBody] object value)
        {
            var request = new ValidationCommand {ValidatorName = name, Value = value};
            var result = _validationService.Validate(request);
            if (result == null || !result.IsValidatorFound)
            {
                return NotFound();
            }
            return Ok(new {faults = result.ValidationFaults, isValid = result.IsValid});
        }
    }
}