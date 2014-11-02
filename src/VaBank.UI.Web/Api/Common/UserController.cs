using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Membership;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Queries;
using VaBank.UI.Web.Api.Infrastructure.Filters;

namespace VaBank.UI.Web.Api.Common
{
    [RoutePrefix("api/users")]
    [Authorize]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }
            _userService = userService;
        }

        [HttpGet]
        [Route]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Get(UsersQuery query)
        {
            var page = _userService.GetUsers(query);
            return Ok(page);
        }

        [HttpGet]
        [Route("{id}", Name = "GetUser")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Get([FromUri] IdentityQuery<Guid> query)
        {
            var user = _userService.GetUser(query);
            return user == null ? (IHttpActionResult)NotFound() : Ok(user);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "Admin")]
        [Transaction]
        public IHttpActionResult Create(CreateUserCommand command)
        {
            var user = _userService.CreateUser(command);
            return Created(Url.Route("GetUser", new {id = user.UserId}), user);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        [Transaction]
        public IHttpActionResult Update([FromUri]Guid id, UpdateUserCommand command)
        {
            command.UserId = id;
            _userService.UpdateUser(command);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }


        [HttpGet]
        [Route("{id}/profile")]
        public IHttpActionResult GetProfile([FromUri] IdentityQuery<Guid> query)
        {
            var profile = _userService.GetProfile(query);
            return profile == null ? (IHttpActionResult)NotFound() : Ok(profile);
        }

        [HttpPut]
        [Route("{id}/profile")]
        [Transaction]
        public IHttpActionResult UpdateProfile([FromUri] Guid id, UpdateProfileCommand command)
        {
            command.UserId = id;
            _userService.UpdateProfile(command);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        [HttpPost]
        [Route("{id}/profile/change-password")]
        [Transaction]
        public IHttpActionResult ChangePassword([FromUri] Guid id, ChangePasswordCommand command)
        {
            command.UserId = id;
            var message = _userService.ChangePassword(command);
            return message == null
                ? (IHttpActionResult) InternalServerError()
                : Ok(message);
        }


    }
}