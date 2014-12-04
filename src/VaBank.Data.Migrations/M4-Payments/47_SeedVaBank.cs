using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Data.Migrations.M4_Payments
{
    [Migration(47, "Seed VaBank bank.")]
    [Tags("Development", "Test", "Production")]
    public class SeedVaBank: Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Bank").InSchema("Accounting").Row(new { Code = "153001966", Name = "VaBank" });
        }

        public override void Down()
        {
        }
    }
}
