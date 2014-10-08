using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace VaBank.UI.Web.Api.Infrastructure.MessageHandlers
{
    public class GlobalExceptionLogger: ExceptionLogger
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public override void Log(ExceptionLoggerContext context)
        {
            _logger.Error(context.Exception.Message, context.Exception);
        }
    }
}