using System;
using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(8, "Seed application client for development.")]
    [Tags("Development", "Test")]
    public class SeedClientDev : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("ApplicationClient").InSchema("Membership")
                .Row(new
                {
                    Id = Guid.NewGuid(),
                    Name = "VaBank Website",
                    Active = 1,
                    RefreshTokenLifeTime = 1200,
                    ApplicationType = 0,
                    AllowedOrigin = "*",
                    Secrete = Guid.NewGuid().GetHashCode().ToString()
                });
        }

        public override void Down()
        {
            //do nothing
        }
    }
}
