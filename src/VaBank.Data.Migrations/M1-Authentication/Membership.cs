using FluentMigrator;


namespace VaBank.Data.Migrations
{
    [Migration(2, "Tables for membership system")]
    [Tags("Membership", "Development", "Production")]
    public class Membership : Migration
    {
        private const string SchemaName = "Membership";

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
            Create.Table("User").InSchema(SchemaName).WithColumn("UserID").AsUserId().PrimaryKey("PK_User").WithDefault(SystemMethods.NewGuid)
                .WithColumn("PasswordHash").AsSecurityString().NotNullable()
                .WithColumn("PasswordSalt").AsSecurityString().NotNullable()
                .WithColumn("SecurityStamp").AsSecurityString().NotNullable()
                .WithColumn("LockoutEnabled").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("LockoutEndDateUtc").AsDateTime().Nullable()
                .WithColumn("UserName").AsShortName().Indexed("IX_User_UserName")
                .WithColumn("AccessFailedCount").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("Deleted").AsBoolean().NotNullable().Indexed("IX_User_Deleted").WithDefaultValue(false);

            Create.Table("UserClaim").InSchema(SchemaName).WithColumn("UserID").AsGuid().PrimaryKey("PK_UserClaim")
                .ForeignKey("FK_UserClaim_To_User", SchemaName, "User", "UserID")
                .WithColumn("Type").AsName().PrimaryKey("PK_UserClaim")
                .WithColumn("Value").AsBigString().NotNullable();

            Create.Table("UserProfile").InSchema(SchemaName).WithColumn("UserID").AsGuid().PrimaryKey("PK_UserProfile")
                .ForeignKey("FK_UserProfile_To_User", SchemaName, "User", "UserID")
                .WithColumn("FirstName").AsName().NotNullable()
                .WithColumn("LastName").AsName().NotNullable()
                .WithColumn("Email").AsEmail().NotNullable().Indexed("IX_UserProfile_Email")
                .WithColumn("PhoneNumber").AsPhoneNumber().Nullable().Indexed("IX_UserProfile_PhoneNumber")
                .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("SmsConfirmationEnabled").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("SmsNotificationEnabled").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("SecretPhrase").AsBigString().NotNullable();

            Create.Table("ApplicationClient").InSchema(SchemaName).WithColumn("ID").AsClientId().PrimaryKey("PK_ApplicationClient").WithDefault(SystemMethods.NewGuid)
                .WithColumn("Name").AsName().NotNullable().Indexed("IX_ApplicationClient_Name")
                .WithColumn("Active").AsBoolean().NotNullable().Indexed("IX_ApplicationClient_Active")
                .WithColumn("RefreshTokenLifeTime").AsInt32().NotNullable()
                .WithColumn("ApplicationType").AsByte().NotNullable()
                .WithColumn("AllowedOrigin").AsString(256).Nullable();

            Create.Table("ApplicationToken").InSchema(SchemaName).WithColumn("ID").AsGuid().PrimaryKey("PK_ApplicationToken").WithDefault(SystemMethods.NewGuid)
                .WithColumn("ClientID").AsClientId().ForeignKey("FK_ApplicationToken_ClientID_ApplicationClient", SchemaName, "ApplicationClient", "ID")
                .WithColumn("IssuedUtc").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("ExpiresUtc").AsDateTime().NotNullable()
                .WithColumn("ProtectedTicket").AsSecurityString().NotNullable();
        }
    }
}
