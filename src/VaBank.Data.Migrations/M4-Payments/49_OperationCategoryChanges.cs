using FluentMigrator;

namespace VaBank.Data.Migrations.M4_Payments
{
    [Migration(49, "Change OperationCatrgory table.")]
    [Tags("Development", "Test", "Production")]
    public class OperationCategoryChanges : Migration
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
