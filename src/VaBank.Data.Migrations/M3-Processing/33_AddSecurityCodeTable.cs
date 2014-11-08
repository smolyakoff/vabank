using System;
using FluentMigrator;
using Newtonsoft.Json;

namespace VaBank.Data.Migrations
{
    [Migration(33, "Added security code table to store sms security codes.")]
    [Tags("Development", "Test", "Production")]
    public class AddSecurityCodeTable : Migration
    {
        private const string SecurityCodeExpirationKey = "VaBank.Core.App.Settings.SecurityCodeSettings";

        public override void Up()
        {
            Create.Table("SecurityCode").InSchema("App")
                .WithColumn("ID").AsGuid().PrimaryKey("PK_SecurityCode")
                .WithColumn("ExpirationDateUtc").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("CodeHash").AsSecurityString().NotNullable();

            Insert.IntoTable("Setting").InSchema("App")
                .Row(new
                {
                    Key = SecurityCodeExpirationKey, 
                    Value = JsonConvert.SerializeObject(new
                    {
                        SmsCodeExpirationPeriod = TimeSpan.FromMinutes(5)
                    })
                });
        }

        public override void Down()
        {
            Delete.FromTable("Setting").InSchema("App").Row(new {Key = SecurityCodeExpirationKey});
            Delete.Table("SecurityCode");
        }
    }
}
