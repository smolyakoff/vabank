using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(10, "Add subject column to ApplicationTokenClient table")]
    public class CreateApplicationTokenSubjectColumn : Migration
    {
        public override void Down()
        {
            Delete.ForeignKey("FK_User_To_ApplicationToken").OnTable("ApplicationToken").InSchema("Membership");
            Delete.Column("Subject").FromTable("ApplicationToken").InSchema("Membership");
        }

        public override void Up()
        {
            Create.Column("Subject").OnTable("ApplicationToken").InSchema("Membership").AsString(50).NotNullable();
            Delete.Index("IX_User_UserName").OnTable("User").InSchema("Membership");
            Create.Index("IX_User_UserName").OnTable("User").InSchema("Membership").OnColumn("UserName").Unique();
            Create.ForeignKey("FK_User_To_ApplicationToken").FromTable("ApplicationToken").InSchema("Membership")
                .ForeignColumn("Subject").ToTable("User").InSchema("Membership").PrimaryColumn("UserName");
        }
    }
}
