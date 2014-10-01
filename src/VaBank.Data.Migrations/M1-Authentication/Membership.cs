using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Data.Migrations
{
    [Migration(2, "Tables for membership system")]
    [Tags("Membership", "Development", "Production")]
    public class Membership : Migration
    {
        private static string SchemaName = "Membership";

        public override void Down()
        {
            Delete.Table("UserClaim").InSchema(SchemaName);
            Delete.Table("UserProfile").InSchema(SchemaName);
            Delete.Table("User").InSchema(SchemaName);

            Delete.Table("ApplicationClient").InSchema(SchemaName);
            Delete.Table("ApplicationToken").InSchema(SchemaName);
            
            Delete.Schema(SchemaName);
        }

        public override void Up()
        {
            Create.Schema(SchemaName);

            Create.Table("User").InSchema(SchemaName).WithColumn("UserID").AsGuid().PrimaryKey("PK_User").WithDefault(SystemMethods.NewGuid)
                .WithColumn("PasswordHash").AsString(100).NotNullable()
                .WithColumn("SecurityStamp").AsString(100).NotNullable()
                .WithColumn("LockoutEnabled").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("LockoutEndDateUtc").AsDateTime().Nullable()
                .WithColumn("UserName").AsString(100).NotNullable().Indexed("IX_User_UserName")
                .WithColumn("AccessFailedCount").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("Deleted").AsBoolean().NotNullable().Indexed("IX_User_Deleted").WithDefaultValue(false);

            Create.Table("UserClaim").InSchema(SchemaName).WithColumn("UserID").AsGuid().ForeignKey("FK_UserClaim_UserID_User", SchemaName, "User", "UserID")
                .WithColumn("Type").AsString(100).NotNullable().Indexed("IX_UserClaim_Type")
                .WithColumn("Value").AsString(100).NotNullable();

            Create.Table("UserProfile").InSchema(SchemaName).WithColumn("UserID").AsGuid().ForeignKey("FK_UserProfile_UserID_User", SchemaName, "User", "UserID")
                .WithColumn("FirstName").AsString(100).NotNullable()
                .WithColumn("LastName").AsString(100).NotNullable()
                .WithColumn("Email").AsString(100).NotNullable().Indexed("IX_UserProfile_Email")
                .WithColumn("PhoneNumber").AsString(100).Nullable().Indexed("IX_UserProfile_PhoneNumber")
                .WithColumn("PhoneNumberConfirmed").AsString(100).NotNullable().WithDefaultValue(false)
                .WithColumn("SmsOperationConfirmationEnabled").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("SmsDoneOperationConfirmationEnabled").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("SecretPhrase").AsString(100).NotNullable();

            Create.Table("ApplicationClient").InSchema(SchemaName).WithColumn("ID").AsGuid().PrimaryKey("PK_ApplicationClient").WithDefault(SystemMethods.NewGuid)
                .WithColumn("Name").AsString(100).NotNullable().Indexed("IX_ApplicationClient_Name")
                .WithColumn("Active").AsBoolean().NotNullable().Indexed("IX_ApplicationClient_Active")
                .WithColumn("RefreshTokenLifeTime").AsInt32().NotNullable()
                .WithColumn("ApplicationType").AsByte().NotNullable().Indexed("IX_ApplicationClient_ApplicationType")
                .WithColumn("AllowedOrigin").AsString(256).Nullable();

            Create.Table("ApplicationToken").InSchema(SchemaName).WithColumn("ID").AsGuid().PrimaryKey("PK_ApplicationToken").WithDefault(SystemMethods.NewGuid)
                .WithColumn("ClientID").AsGuid().ForeignKey("FK_ApplicationToken_ClientID_ApplicationClient", SchemaName, "ApplicationClient", "ID")
                .WithColumn("IssuedUtc").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("ExpiresUtc").AsDateTime().NotNullable()
                .WithColumn("ProtectedTicket").AsString(100).NotNullable();
        }
    }
}
