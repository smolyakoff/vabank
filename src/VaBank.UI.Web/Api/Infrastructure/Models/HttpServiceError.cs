using System;
using System.Net;
using System.Web.Http;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Validation;

namespace VaBank.UI.Web.Api.Infrastructure.Models
{
    internal class HttpServiceError
    {
        private readonly bool _includeErrorDetail;

        private readonly HttpError _httpError;

        private readonly HttpStatusCode _statusCode;

        public HttpServiceError(ServiceException exception, bool includeErrorDetail = false)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            _includeErrorDetail = includeErrorDetail;
            _httpError = Populate((dynamic) exception, out _statusCode);
        }

        public HttpError HttpError
        {
            get { return _httpError; }
        }

        public HttpStatusCode StatusCode
        {
            get { return _statusCode; }
        }

        private HttpError Populate(ServiceException exception, out HttpStatusCode statusCode)
        {
            var httpError = new HttpError(exception, _includeErrorDetail)
            {
                {"errorType", "service"}
            };
            statusCode = HttpStatusCode.InternalServerError;
            return httpError;
        }

        private HttpError Populate(ValidationException exception, out HttpStatusCode statusCode)
        {
            var httpError = new HttpError(exception, _includeErrorDetail)
            {
                { "errorType", "validation" },
                { "faults", exception.Faults }
            };
            statusCode = HttpStatusCode.BadRequest;
            return httpError;
        }

        private HttpError Populate(UserMessageException exception, out HttpStatusCode statusCode)
        {
            var httpError = new HttpError(exception, _includeErrorDetail)
            {
                { "errorType", "message" },
                { "code", exception.UserMessage.Code }
            };
            statusCode = HttpStatusCode.InternalServerError;
            return httpError;
        }
    }
}