using System.Collections.Generic;
using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(5, "Some users for testing.")]
    [Tags("Development", "Test", "Production")]
    public class SeedUsers : Migration
    {
        private const string DevPassword = "vabank2014!";

        public override void Up()
        {
            var users = new List<M1User>()
            {
                M1User.Create("smolyakoff", DevPassword, "Admin"),
                M1User.Create("losik", DevPassword, "Admin"),
                M1User.Create("sherbetr", DevPassword, "Admin"),
                M1User.Create("jaymz", DevPassword, "Admin"),
                M1User.Create("gostop", DevPassword, "Admin"),
                M1User.Create("customer1", DevPassword, "Customer"),
                M1User.Create("customer2", DevPassword, "Customer"),
                M1User.Create("customer3", DevPassword, "Customer"),
            };
            users.ForEach(x =>
            {
                InsertUser(x);
                InsertClaims(x);
            });
        }

        private void InsertUser(M1User user)
        {
            Insert.IntoTable("User").InSchema("Membership").Row(user.ToTableRow());
        }

        private void InsertClaims(M1User user)
        {
            foreach (var claim in user.Claims)
            {
                Insert.IntoTable("UserClaim").InSchema("Membership").Row(new
                {
                    UserId = user.UserId,
                    Type = claim.Type,
                    Value = claim.Value
                });
            }
        }

        public override void Down()
        {
            //data migration, not used
        }
    }
}
