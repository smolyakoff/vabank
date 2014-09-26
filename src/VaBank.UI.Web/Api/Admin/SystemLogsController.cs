﻿using System;
using System.Net.Http;
using System.Web.Http;
using VaBank.Services.Contracts.Admin.Maintenance;

namespace VaBank.UI.Web.Api.Admin
{
    [RoutePrefix("api/logs/system")]
    public class SystemLogsController : ApiController
    {
        private readonly ILogManagementService _logManagementService;

        public SystemLogsController(ILogManagementService logManagementService)
        {
            _logManagementService = logManagementService;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Query()
        {
            var filter = this.Request.GetQueryNameValuePairs();
            var query = new SystemLogQuery();
            var logs = _logManagementService.GetSystemLogEntries(query);
            return Ok(logs);
        }

        [HttpGet]
        [Route("lookup")]
        public IHttpActionResult Lookup()
        {
            var lookup = _logManagementService.GetSystemLogLookup();
            return Ok(lookup);
        }
    }
}