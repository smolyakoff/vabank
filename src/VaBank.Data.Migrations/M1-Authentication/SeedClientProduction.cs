using System;
using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(7, "Seed application client for production.")]
    [Tags("Production")]
    public class SeedClientProduction : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("ApplicationClient").InSchema("Membership")
                .Row(new
                {
                    Id = Guid.NewGuid(),
                    Name = "VaBank Website",
                    Active = 1,
                    RefreshTokenLifeTime = 600,
                    ApplicationType = 0,
                    AllowedOrigin = "https://vabank.azurewebsites.net",
                    Secrete = Guid.NewGuid().GetHashCode().ToString()
                });
        }

        public override void Down()
        {
            //do nothing
        }
    }
}
