using System;
using FluentMigrator;
using VaBank.Common.Serialization;

namespace VaBank.Data.Migrations.M3_Processing
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

            Insert.IntoTable("App").InSchema("Setting")
                .Row(new
                {
                    Key = SecurityCodeExpirationKey, 
                    Value = JsonNetXml.SerializeObject(new
                    {
                        SmsCodeExpirationPeriod = TimeSpan.FromMinutes(5)
                    }, "Setting")
                });
        }

        public override void Down()
        {
            Delete.FromTable("Setting").InSchema("App").Row(new {Key = SecurityCodeExpirationKey});
            Delete.Table("SecurityCode");
        }
    }
}
