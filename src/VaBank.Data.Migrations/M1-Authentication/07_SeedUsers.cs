using System.Collections.Generic;
using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(7, "Some users for testing.")]
    [Tags("Development", "Test", "Production")]
    public class SeedUsers : Migration
    {
        private const string DevPassword = "vabank2014!";

        public override void Up()
        {
            var users = new List<M1User>()
            {
                M1User.Create("smolyakoff", DevPassword, "Admin", new M1Profile()
                {
                    Email = "smolyakoff.ko@gmail.com",
                    FirstName = "Константин",
                    LastName = "Смоляков",
                    PhoneNumber = "+375293593295",
                    PhoneNumberConfirmed = true
                }),
                M1User.Create("losik", DevPassword, "Admin"),
                M1User.Create("sherbetr", DevPassword, "Admin"),
                M1User.Create("jaymz", DevPassword, "Admin"),
                M1User.Create("gostop", DevPassword, "Admin"),
                M1User.Create("bradpitt", DevPassword, "Customer", new M1Profile()
                {
                    Email = "bradpitt@vabank.com",
                    FirstName = "Брэд",
                    LastName = "Питт"
                }),
                M1User.Create("terminator", DevPassword, "Customer",
                new M1Profile()
                {
                    Email = "terminator@vabank.com",
                    FirstName = "Арнольд",
                    LastName = "Шварцнеггер"
                }),
                M1User.Create("meganfox", DevPassword, "Customer",new M1Profile()
                {
                    Email = "meganfox@vabank.com",
                    FirstName = "Меган",
                    LastName = "Фокс"
                }),
            };
            users.ForEach(x =>
            {
                InsertUser(x);
                InsertClaims(x);
                InsertProfile(x);
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

        private void InsertProfile(M1User user)
        {
            if (user.Profile != null)
            {
                Insert.IntoTable("UserProfile").InSchema("Membership").Row(new
                {
                    UserId = user.UserId,
                    FirstName = new ExplicitUnicodeString(user.Profile.FirstName),
                    LastName = new ExplicitUnicodeString(user.Profile.LastName),
                    Email = user.Profile.Email,
                    PhoneNumber = user.Profile.PhoneNumber,
                    SecretPhrase = new ExplicitUnicodeString(user.Profile.SecretPhrase),
                    PhoneNumberConfirmed = user.Profile.PhoneNumberConfirmed
                });
            }
        }

        public override void Down()
        {
            //data migration, not used
        }
    }
}
