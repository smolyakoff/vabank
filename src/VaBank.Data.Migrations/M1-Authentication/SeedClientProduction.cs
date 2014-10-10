using System;
using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(8, "Seed application client for production.")]
    [Tags("Production")]
    public class SeedClientProduction : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("ApplicationClient").InSchema("Membership")
                .Row(new
                {
                    Id = "vabank.website",
                    Active = 1,
                    RefreshTokenLifeTime = 600,
                    ApplicationType = 0,
                    AllowedOrigin = "https://vabank.azurewebsites.net",
                });
        }

        public override void Down()
        {
            //do nothing
        }
    }
}
