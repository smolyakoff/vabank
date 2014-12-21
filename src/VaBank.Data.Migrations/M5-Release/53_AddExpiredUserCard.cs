using System;
using System.Linq;
using Dapper;
using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(53, "AddExpiredUserCard to terminator.")]
    [Tags("Development", "Test", "Production")]
    public class AddExpiredUserCard : Migration
    {
        public override void Down()
        {
            //Do nothing
        }

        public override void Up()
        {
            Execute.WithConnection((connection, transaction) =>
                {
                    var userId = connection.Query<Guid>("SELECT [UserID] FROM [Membership].[User] WHERE [UserName] = @UserName", new { UserName = "terminator" }, transaction).First();

                    var accountParams = new 
                    {
                        AccountNo = "3014" + Seed.RandomStringOfNumbers(9),
                        CurrencyISOName = "USD",
                        Balance = 10000m,
                        OpenDateUtc = new DateTime(2014, 09, 1),
                        ExpirationDateUtc = new DateTime(2015, 2, 1),
                        Type = "CardAccount"
                    };

                    connection.Execute(@"INSERT INTO [Accounting].[Account] ([AccountNo], [CurrencyISOName], [Balance], [OpenDateUtc], [ExpirationDateUtc], [Type]) " +
                                    "VALUES (@AccountNo, @CurrencyISOName, @Balance, @OpenDateUtc, @ExpirationDateUtc, @Type)", accountParams, transaction);
                    connection.Execute(@"INSERT INTO [Accounting].[User_Account] ([AccountNo], [UserID]) VALUES (@AccountNo, @UserID)", new { AccountNo = accountParams.AccountNo, UserID = userId }, transaction);

                    var cardParams = new
                    {
                        CardId = Guid.NewGuid(),
                        CardNo = "4666" + Seed.RandomStringOfNumbers(2) + "00" + Seed.RandomStringOfNumbers(8),
                        CardVendorId = "visa",
                        HolderFirstName = "TERMINATOR",
                        HolderLastName = "TERMINATOV",
                        ExpirationDateUtc = new DateTime(2014, 12, 1)
                    };
                                                                                                                                    
                    connection.Execute("INSERT INTO [Accounting].[Card] ([CardId], [CardNo], [CardVendorId], [HolderFirstName], [HolderLastName], [ExpirationDateUtc]) " + 
                                    "VALUES (@CardId, @CardNo, @CardVendorId, @HolderFirstName, @HolderLastName, @ExpirationDateUtc)", cardParams, transaction);
                    connection.Execute("INSERT INTO [Accounting].[User_Card_Account] ([CardId], [UserId], [AccountNo]) VALUES (@CardId, @UserId, @AccountNo)", new { CardId = cardParams.CardId, UserId = userId, AccountNo = accountParams.AccountNo }, transaction);
                    connection.Execute("INSERT INTO [Accounting].[CardSettings] ([CardId], [Blocked], [LimitOperationsPerDayLocal], [LimitOperationsPerDayAbroad], [LimitAmountPerDayLocal], [LimitAmountPerDayAbroad]) " +
                                    "VALUES (@CardId, 0, 20, 10, 1000.0, 500.0)", new { CardId = cardParams.CardId }, transaction);
                });
        }
    }
}
