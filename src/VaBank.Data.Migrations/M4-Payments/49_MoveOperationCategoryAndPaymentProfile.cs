using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(49, "Move operation category and payment profile to accounting")]
    [Tags("Development", "Test", "Production")]
    public class MoveOperationCategoryAndPaymentProfile : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Alter.Table("OperationCategory").InSchema("Processing").ToSchema("Accounting");
            Alter.Table("UserPaymentProfile").InSchema("Payments").ToSchema("Accounting");
        }
    }
}
