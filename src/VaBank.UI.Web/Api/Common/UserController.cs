using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using VaBank.Common.Data.Sorting;
using VaBank.Services.Contracts.Membership;
using VaBank.Services.Contracts.Membership.Queries;
using VaBank.UI.Web.Api.Infrastructure.Models;

namespace VaBank.UI.Web.Api.Common
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserManagementService _userManagementService;

        public UserController(IUserManagementService userManagementService)
        {
            if (userManagementService == null)
            {
                throw new ArgumentNullException("userManagementService");
            }
            _userManagementService = userManagementService;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Get(UsersQuery query)
        {
            var page = _userManagementService.GetUsers(query);
            return Ok(page);
        }
    }
}