using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Data.Migrations.M4_Payments
{
    [Migration(47, "Add PSN column to [Accounting].[User_Account] table.")]
    [Tags("Development", "Test", "Production")]
    public class AddPSNToUser_Account: Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Alter.Table("User_Account").InSchema("Accounting").AddColumn("PSN").AsString(9).Nullable();
        }
    }
}
