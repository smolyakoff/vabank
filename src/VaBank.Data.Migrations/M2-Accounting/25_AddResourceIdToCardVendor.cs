using FluentMigrator;
using System;

namespace VaBank.Data.Migrations.M2_Accounting
{
    [Migration(25, "Add ResourceId column to [Accounting].[CardVendor] with FK to [App].[Resource] Id")]
    [Tags("Accounting", "Development", "Production", "Test")]
    public class AddResourceIdToCardVendor : Migration
    {
        public override void Down()
        {
            //Do nothing
        }

        public override void Up()
        {
            Alter.Table("CardVendor").InSchema("Accounting").AddColumn("ResourceId").AsGuid().Nullable()
                .ForeignKey("FK_CardVendor_To_Resource", "App", "Resource", "ID");
        }
    }
}
