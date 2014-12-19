using System.Collections.Generic;
using FluentMigrator;
using Newtonsoft.Json;

namespace VaBank.Data.Migrations
{
    [Migration(43, "Seed minimal card transfer amounts.")]
    [Tags("Development", "Production", "Test")]
    public class SeedMinimalCardTransferAmounts : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Setting").InSchema("App")
                .Row(new
                {
                    Key = "VaBank.Core.Processing.CardTransferSettings",
                    Value = JsonConvert.SerializeObject(new
                    {
                        MinimalAmounts = new Dictionary<string, decimal>
                        {
                            {"USD", 5},
                            {"EUR", 3},
                            {"BYR", 10000},
                            {"RUB", 40}
                        }
                    })
                });
        }

        public override void Down()
        {
            //do nothing
        }
    }
}
