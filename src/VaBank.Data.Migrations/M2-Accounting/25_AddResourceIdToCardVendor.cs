using System.Data;
using FluentMigrator;

namespace VaBank.Data.Migrations 
{
    [Migration(25, "Add ResourceId column to [Accounting].[CardVendor] with FK to [App].[Resource] ID")]
    [Tags("Accounting", "Development", "Production", "Test")]
    public class AddResourceIdToCardVendor : Migration
    {
        public override void Down()
        {
            Delete.ForeignKey("FK_CardVendor_To_Resource").OnTable("CardVendor").InSchema("Accounting");
            Delete.Column("ResourceId").FromTable("CardVendor").InSchema("Accounting");
        }

        public override void Up()
        {
            Alter.Table("CardVendor").InSchema("Accounting")
                .AddColumn("ResourceID").AsGuid().NotNullable();
            Create.ForeignKey("FK_CardVendor_To_Resource")
                .FromTable("CardVendor").InSchema("Accounting").ForeignColumn("ResourceId")
                .ToTable("Resource").InSchema("App").PrimaryColumn("ID")
                .OnDelete(Rule.Cascade);
        }
    }
}
