using System.Collections.Generic;
using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(26, "Seed basic card vendors.")]
    [Tags("Development", "Test", "Production")]
    public class SeedCardVendors : Migration
    {
        public override void Up()
        {
            var images = new Dictionary<string, M2Resource>
            {
                {"maestro", M2Resource.CreateWebServerFile("/Client/app/images/icons/cards/maestro-curved-128px.png")},
                {"mastercard", M2Resource.CreateWebServerFile("/Client/app/images/icons/cards/mastercard-curved-128px.png")},
                {"visa", M2Resource.CreateWebServerFile("/Client/app/images/icons/cards/visa-curved-128px.png")}
            };

            foreach (var image in images.Values)
            {
                Insert.IntoTable("Resource").InSchema("App").Row(image);
            }

            Insert.IntoTable("CardVendor").InSchema("Accounting")
                .Row( new
                {
                    Id = "maestro",
                    Name = "Maestro",
                    ResourceId = images["maestro"].Id
                })
                .Row( new
                {
                    Id = "mastercard",
                    Name = "MasterCard",
                    ResourceId = images["mastercard"].Id
                })
                .Row(new
                {
                    Id = "visa",
                    Name = "Visa",
                    ResourceId = images["visa"].Id
                });
        }

        public override void Down()
        {
            //do nothing
        }
    }
}
