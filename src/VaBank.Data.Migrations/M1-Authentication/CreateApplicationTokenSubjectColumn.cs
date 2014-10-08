using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(10, "Add subject column to ApplicationTokenClient table")]
    public class CreateApplicationTokenSubjectColumn : Migration
    {
        public override void Down()
        {
            Delete.Column("Subject").FromTable("ApplicationToken").InSchema("Membership");
        }

        public override void Up()
        {
            Create.Column("Subject").OnTable("ApplicationToken").InSchema("Membership").AsString(50).NotNullable();
        }
    }
}
