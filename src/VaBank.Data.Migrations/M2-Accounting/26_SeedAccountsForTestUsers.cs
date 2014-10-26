using Dapper;
using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;

namespace VaBank.Data.Migrations
{
    [Migration(26, "Create accounts for test users.")]
    [Tags("Accounting", "Development", "Production", "Test")]
    public class CreateAccountsForTestUsers : Migration
    {
        public override void Down()
        {
            //Do nothing
        }

        public override void Up()
        {
            Execute.WithConnection((connection, transaction) =>
            {
                var nowUtc = DateTime.UtcNow;
                var expireUtc = nowUtc.AddYears(1);
                foreach (var idPair in GetUserIdPairs(connection, transaction))
                {
                    var accountNo = Seed.RandomStringOfNumbers(13);
                    var account = M2Account.Create(accountNo, "USD", 10000, nowUtc, expireUtc, "CardAccount");
                    var card = M2Card.Create(accountNo, idPair.UserId, idPair.UserName);
                    InsertAccount(account, connection, transaction);
                    InsertUserAccount(account.ToUserAccount(idPair.UserId), connection, transaction);
                    InsertCard(card, connection, transaction);
                }
            });
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
                "VALUES (@CardId, @CardNo, @CardVendorId, @HolderFirstName, @HolderLastName, @ExpirationDateUtc)", card.ToCard(),
                transaction);
            connection.Execute(
                "INSERT INTO [Accounting].[User_Card_Account] ([CardId], [UserId], [AccountNo]) VALUES (@CardId, @UserId, @AccountNo)",
                card.ToUserCard(), transaction);
        }

        private List<UserIdPair> GetUserIdPairs(IDbConnection connection, IDbTransaction transaction)
        {
            var userIdPairs = new List<UserIdPair>();
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "SELECT [UserID], [UserName] FROM [Membership].[User] WHERE [UserName] IN ('bradpitt', 'meganfox', 'terminator')";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pair = new UserIdPair((string)reader["UserName"], (Guid)reader["UserID"]);
                        userIdPairs.Add(pair);
                    }
                }
            }
            return userIdPairs;
        }

        private class UserIdPair
        {
            public UserIdPair(string userName, Guid userId)
            {
                UserName = userName;
                UserId = userId;
            }

            public string UserName { get; private set; }

            public Guid UserId { get; private set; }
        }
    }
}
