using System.Net.Mail;

namespace VaBank.Services.Contracts.Infrastructure.Email
{
    public class SendEmailCommand
    {
        public SendEmailCommand()
        {
            To = new MailAddressCollection();
        }

        public MailAddress From { get; set; }

        public MailAddressCollection To { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }
    }
}
