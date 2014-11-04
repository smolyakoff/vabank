using System;
using System.Net.Mail;
using System.Text;
using NLog;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.App.Repositories;
using VaBank.Services.Contracts.Infrastructure.Email;
using VaBank.Services.Contracts.Infrastructure.Sms;

namespace VaBank.Services.Infrastructure.Sms
{
    [Injectable]
    public class SmsLogger
    {
        private readonly IEmailService _emailService;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly SmsLoggerSettings _settings;

        public SmsLogger(ISettingRepository settingRepository, IEmailService emailService)
        {
            Argument.NotNull(settingRepository, "settingRepository");
            Argument.NotNull(emailService, "emailService");

            _emailService = emailService;
            _settings = settingRepository.GetOrDefault<SmsLoggerSettings>();
            if (_settings == null)
            {
                throw new InvalidOperationException("Settings for sms logger were not found.");
            }
        }

        public void Log(SmsModel sms)
        {
            _logger.Info("Going to send following sms message: {0}", sms);
            if (_settings.EmailAddresses.Count == 0)
            {
                _logger.Warn("No email addresses are configured for email logger.");
                return;
            }
            var emailBuilder = new StringBuilder();
            emailBuilder.AppendFormat("From: {0}", sms.From).AppendLine()
                        .AppendFormat("To:   {0}", sms.From).AppendLine()
                        .AppendFormat("Text: {0}", sms.Text).AppendLine();
            var email = new SendEmailCommand
            {
                From = new MailAddress("noreply@vabank.com", "SMS Logger"),
                Subject = string.Format("New SMS for {0}", sms.To),
                Text = emailBuilder.ToString()
            };
            foreach (var emailAddress in _settings.EmailAddresses)
            {
                email.To.Add(emailAddress);
            }
            _emailService.SendEmail(email);
        }
    }
}
