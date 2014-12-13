using FluentMigrator;

namespace VaBank.Data.Migrations.M4_Payments
{
    [Migration(49, "Move operation category to accounting.")]
    [Tags("Development", "Test", "Production")]
    public class MoveOperationCategory : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Alter.Table("OperationCategory").InSchema("Processing").ToSchema("Accounting");            
        }
    }
}
