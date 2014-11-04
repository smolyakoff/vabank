using System.Collections.Generic;
using FluentMigrator;
using Newtonsoft.Json;

namespace VaBank.Data.Migrations.M3_Processing
{
    [Migration(36, "Created settings for sms and email services (dev).")]
    [Tags("Development", "Test")]
    public class SmsAndEmailSettingsDev : Migration
    {
        private static class SettingsKeys
        {
            public const string Twilio = "VaBank.Services.Infrastructure.Sms.TwilioClientSettings";

            public const string SendGrid = "VaBank.Services.Infrastructure.Email.SendGridClientSettings";

            public const string SmsService = "VaBank.Services.Infrastructure.Sms.SmsServiceSettings";

            public const string SmsLogger = "VaBank.Services.Infrastructure.Sms.SmsLoggerSettings";
        }

        public override void Up()
        {
            Insert.IntoTable("Setting").InSchema("App")
                .Row(new {Key = SettingsKeys.Twilio, Value = JsonConvert.SerializeObject(TwilioSettings)})
                .Row(new {Key = SettingsKeys.SendGrid, Value = JsonConvert.SerializeObject(SendGridSettings)})
                .Row(new {Key = SettingsKeys.SmsService, Value = JsonConvert.SerializeObject(SmsServiceSettings)})
                .Row(new {Key = SettingsKeys.SmsLogger, Value = JsonConvert.SerializeObject(SmsLoggerSettings)});

        }

        public override void Down()
        {
            Delete.FromTable("Setting").InSchema("App")
                .Row(new {Key = SettingsKeys.Twilio})
                .Row(new {Key = SettingsKeys.SendGrid})
                .Row(new {Key = SettingsKeys.SmsService})
                .Row(new {Key = SettingsKeys.SmsLogger});
        }

        private readonly object SmsServiceSettings = new
        {
            UseLogger = true,
            OutboundPhoneNumber = "+15005550006"
        };

        private readonly object SmsLoggerSettings = new
        {
            EmailAddresses = new List<string>()
            {
                "vabank@yopmail.com"
            }
        };

        private readonly object SendGridSettings = new
        {
            Username = "azure_a0dd14650ca733d39a45bcfaf75a672d@azure.com",
            Password = "iI7mpLHavr0Ht25"
        };

        private readonly object TwilioSettings = new
        {
            AccountSid = "AC6c28da9b9c4e1e7f180ff20ad27f06be",
            AuthToken = "9249c0016094a1775702047e13b55caf"
        };
    }
}
