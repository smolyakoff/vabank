using System;
using System.Linq;
using SendGrid;
using VaBank.Common.Validation;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Infrastructure.Email;

namespace VaBank.Services.Infrastructure.Email
{
    public class EmailService : BaseService, IEmailService
    {
        private readonly EmailServiceDependencies _deps;

        public EmailService(BaseServiceDependencies dependencies, EmailServiceDependencies emailServiceDependencies) : base(dependencies)
        {
            Argument.NotNull(emailServiceDependencies, "emailServiceDependencies");
            _deps = emailServiceDependencies;
            _deps.EnsureIsResolved();

        }

        public void SendEmail(SendEmailCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var message = new SendGridMessage
                {
                    From = command.From,
                    To = command.To.ToArray(),
                    Subject = command.Subject,
                    Text = command.Text
                };
                var client = _deps.SendGridClientFactory.CreateWebClient();
                client.Deliver(message);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't send email.", ex);
            }
        }
    }
}
