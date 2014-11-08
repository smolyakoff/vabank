using System;
using System.Web.Http;
using VaBank.Services.Contracts.Infrastructure.Secuirty;

namespace VaBank.UI.Web.Api.Customer
{
    [RoutePrefix("api/security-codes")]
    [Authorize(Roles = "Customer")]
    public class SecurityCodeController : ApiController
    {
        private readonly ISecurityCodeService _securityCodeService;

        public SecurityCodeController(ISecurityCodeService securityCodeService)
        {
            if (securityCodeService == null)
            {
                throw new ArgumentNullException("securityCodeService");
            }
            _securityCodeService = securityCodeService;
        }

        [HttpPost]
        [Route]
        public IHttpActionResult Generate()
        {
            var code = _securityCodeService.GenerateSecurityCode();
            return Ok(code);
        }
    }
}