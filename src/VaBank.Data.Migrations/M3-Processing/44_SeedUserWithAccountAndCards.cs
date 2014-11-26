using FluentMigrator;
using System;
using System.Data;
using Dapper;

namespace VaBank.Data.Migrations
{
    [Migration(44, "Seed Chuck Norris user with account and card.")]
    [Tags("Development", "Test", "Production")]
    public class SeedUserWithAccountAndCards : Migration
    {
        private const string DevPassword = "vabank2014!";
        private const string UserName = "chuck";

        public override void Up()
        {
            var user = M3User.Create(UserName, DevPassword, "Customer",
                new M1Profile()
                {
                    Email = "chuck@vabank.com",
                    FirstName = "Chuck",
                    LastName = "Norris",
                    PhoneNumber = "+375296140302",
                    PhoneNumberConfirmed = true
                }
            );

            InsertUser(user);
            InsertClaims(user);
            InsertProfile(user);

            Execute.WithConnection((connection, transaction) =>
            {
                var nowUtc = DateTime.UtcNow;
                var expireUtc = nowUtc.AddYears(10);
                var id = GetUserId(connection, transaction, UserName);
                var accountNo = "3014" + Seed.RandomStringOfNumbers(9);
                var account = M2Account.Create(accountNo, "USD", 999999999, nowUtc, expireUtc, "CardAccount");
                var card1 = M2Card.Create(accountNo, id, UserName);
                var card2 = M2Card.Create(accountNo, id, UserName);
                InsertAccount(account, connection, transaction);
                InsertUserAccount(account.ToUserAccount(id), connection, transaction);
                InsertCard(card1, connection, transaction);
                InsertCard(card2, connection, transaction);
            });
        }

        private void InsertUser(M3User user)
        {
            Insert.IntoTable("User").InSchema("Membership").Row(user.ToTableRow());
        }

        private void InsertClaims(M3User user)
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

        private void InsertProfile(M3User user)
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

        private Guid GetUserId(IDbConnection connection, IDbTransaction transaction, string userName)
        {
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = String.Format("SELECT [UserID] FROM [Membership].[User] WHERE [UserName] = '{0}'", userName);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = (Guid)reader["UserID"];
                        return id;
                    }
                }
            }
            return Guid.Empty;
        }

        private void InsertAccount(M2Account account, IDbConnection connection, IDbTransaction transaction)
        {
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"INSERT INTO [Accounting].[Account] ([AccountNo], [CurrencyISOName], [Balance], [OpenDateUtc], [ExpirationDateUtc], [Type]) 
                                    VALUES (@AccountNo, @CurrencyISOName, @Balance, @OpenDateUtc, @ExpirationDateUtc, @Type)";
                command.CreateSqlParameter("@AccountNo", account.AccountNo);
                command.CreateSqlParameter("@CurrencyISOName", account.CurrencyISOName);
                command.CreateSqlParameter("@Balance", SqlDbType.Decimal, account.Balance);
                command.CreateSqlParameter("@OpenDateUtc", account.OpenDateUtc);
                command.CreateSqlParameter("@ExpirationDateUtc", account.ExpirationDateUtc);
                command.CreateSqlParameter("@Type", SqlDbType.NVarChar, account.Type.Text);
                command.ExecuteNonQuery();
            }
        }

        private void InsertUserAccount(M2UserAccount userAccount, IDbConnection connection, IDbTransaction transaction)
        {
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "INSERT INTO [Accounting].[User_Account] ([AccountNo], [UserID]) VALUES (@AccountNo, @UserID)";
                command.CreateSqlParameter("@AccountNo", userAccount.AccountNo);
                command.CreateSqlParameter("@UserID", userAccount.UserId);
                command.ExecuteNonQuery();
            }
        }

        private void InsertCard(M2Card card, IDbConnection connection, IDbTransaction transaction)
        {
            connection.Execute(
                "INSERT INTO [Accounting].[Card] ([CardId], [CardNo], [CardVendorId], [HolderFirstName], [HolderLastName], [ExpirationDateUtc])" +
                "VALUES (@CardId, @CardNo, @CardVendorId, @HolderFirstName, @HolderLastName, @ExpirationDateUtc)",
                card.ToCard(), transaction);
            connection.Execute(
                "INSERT INTO [Accounting].[User_Card_Account] ([CardId], [UserId], [AccountNo]) VALUES (@CardId, @UserId, @AccountNo)",
                card.ToUserCard(), transaction);
            connection.Execute(
                "INSERT INTO [Accounting].[CardSettings] " +
                "([CardId], [Blocked], [LimitOperationsPerDayLocal], [LimitOperationsPerDayAbroad], [LimitAmountPerDayLocal], [LimitAmountPerDayAbroad])" +
                "VALUES (@CardId, 0, 20, 10, 1000.0, 500.0)", card, transaction);
        }

        public override void Down()
        {
            //nothing to do
        }
    }
}
