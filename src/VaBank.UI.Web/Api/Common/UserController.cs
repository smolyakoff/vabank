using System;
using System.Web.Http;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Membership;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Queries;

namespace VaBank.UI.Web.Api.Common
{
    [RoutePrefix("api/users")]
    [Authorize]
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
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Get(UsersQuery query)
        {
            var page = _userManagementService.GetUsers(query);
            return Ok(page);
        }

        [HttpGet]
        [Route("{id}", Name = "GetUser")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Get([FromUri] IdentityQuery<Guid> query)
        {
            var user = _userManagementService.GetUser(query);
            return user == null ? (IHttpActionResult)NotFound() : Ok(user);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Create(CreateUserCommand command)
        {
            var user = _userManagementService.CreateUser(command);
            return Created(Url.Route("GetUser", new {id = user.UserId}), user);
        }


        [HttpGet]
        [Route("{id}/profile")]
        public IHttpActionResult GetProfile([FromUri] IdentityQuery<Guid> query)
        {
            var profile = _userManagementService.GetProfile(query);
            return profile == null ? (IHttpActionResult)NotFound() : Ok(profile);
        }
    }
}