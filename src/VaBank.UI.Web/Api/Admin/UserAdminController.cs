using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Membership;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Queries;
using VaBank.UI.Web.Api.Infrastructure.Filters;

namespace VaBank.UI.Web.Api.Admin
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/users")]
    public class UserAdminController : ApiController
    {
        private readonly IUserService _userService;

        public UserAdminController(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }
            _userService = userService;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Get(UsersQuery query)
        {
            var page = _userService.GetUsers(query);
            return Ok(page);
        }

        [HttpGet]
        [Route("{id:guid}", Name = "GetUser")]
        public IHttpActionResult Get([FromUri] IdentityQuery<Guid> query)
        {
            var user = _userService.GetUser(query);
            return user == null ? (IHttpActionResult) NotFound() : Ok(user);
        }

        [HttpGet]
        [Route("{id:guid}/profile/full")]
        public IHttpActionResult GetFullProfile([FromUri] IdentityQuery<Guid> query)
        {
            var profile = _userService.GetFullProfile(query);
            return profile == null ? (IHttpActionResult)NotFound() : Ok(profile);
        }

        [HttpPost]
        [Route("")]
        [Transaction]
        public IHttpActionResult Create(CreateUserCommand command)
        {
            var user = _userService.CreateUser(command);
            return Created(Url.Route("GetUser", new {id = user.UserId}), user);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Transaction]
        public IHttpActionResult Update([FromUri] Guid id, UpdateUserCommand command)
        {
            command.UserId = id;
            _userService.UpdateUser(command);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }
    }
}