using Dapper;
using FluentMigrator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VaBank.Data.Migrations
{
    [Migration(48, "Seed payments data.")]
    [Tags("Development", "Test", "Production")]
    public class SeedPaymentsData : Migration
    {
        private const string VaBankPrefix = "3014"; 
        
        public override void Down()
        {            
        }

        public override void Up()
        {
            SeedCategoriesTree();
            SeedCorrespondentAccounts();
            SeedPaymentTemplates();
            SeedPaymentOrderTemplates();
            SeedUserPaymentProfiles();
        }

        private void SeedCategoriesTree()
        {
            Insert.IntoTable("OperationCategory").InSchema("Processing")
                .Row(new { Code = "PAYMENT", Name = new ExplicitUnicodeString("Платёж") })
                .Row(new { Code = "PAYMENT-CUSTOM", Name = new ExplicitUnicodeString("Произвольный платёж"), Parent = "PAYMENT" })
                .Row(new { Code = "PAYMENT-CUSTOM-PAYMENTORDER", Name = new ExplicitUnicodeString("Платёж по реквизитам"), Parent = "PAYMENT-CUSTOM" })
                .Row(new { Code = "PAYMENT-CELL", Name = new ExplicitUnicodeString("Мобильная связь"), Parent = "PAYMENT" })
                .Row(new { Code = "PAYMENT-CELL-VELCOM", Name = "Velcom", Parent = "PAYMENT-CELL" })
                .Row(new { Code = "PAYMENT-CELL-VELCOM-PHONENO", Name = new ExplicitUnicodeString("По номеру телефона"), Parent = "PAYMENT-CELL-VELCOM" });
        }

        private void SeedCorrespondentAccounts()
        {
            Execute.WithConnection((connection, transaction) => 
            {
                var codes = connection.Query<string>("SELECT [Code] FROM [Accounting].[Bank] WHERE [Code] <> '153001966'", null, transaction).ToList();
                foreach (var code in codes)
                {
                    var accountNo = string.Format("{0}{1}", VaBankPrefix, code);
                    connection.Execute("INSERT INTO [Accounting].[Account] ([AccountNo],[CurrencyISOName],[OpenDateUtc],[ExpirationDateUtc],[Type]) VALUES (@AccountNo,@CurrencyISOName,@OpenDateUtc,@ExpirationDateUtc,@Type)",
                        new { AccountNo = accountNo, CurrencyISOName = "BYR", OpenDateUtc = DateTime.UtcNow, ExpirationDateUtc = DateTime.UtcNow.AddYears(1), Type = "CorrespondentAccount" }, transaction);
                    connection.Execute("INSERT INTO [Accounting].[CorrespondentAccount] ([AccountNo],[BankCode]) VALUES (@AccountNo,@BankCode)", new { AccountNo = accountNo, BankCode = code }, transaction);
                }
            });
        }

        private void SeedPaymentTemplates()
        {
            var assembly = Assembly.GetExecutingAssembly();            
            Insert.IntoTable("PaymentTemplate").InSchema("Payments")
                .Row(new { Code = "PAYMENT-CELL-VELCOM-PHONENO", FormTemplate = new ExplicitUnicodeString(JObject.Parse(new StreamReader(assembly.GetManifestResourceStream("VaBank.Data.Migrations.M4_Payments.Templates.cell-velcom-phoneno.json")).ReadToEnd()).ToString(Formatting.None)) })
                .Row(new { Code = "PAYMENT-CUSTOM-PAYMENTORDER", FormTemplate = new ExplicitUnicodeString(JObject.Parse(new StreamReader(assembly.GetManifestResourceStream("VaBank.Data.Migrations.M4_Payments.Templates.custom-paymentorder.json")).ReadToEnd()).ToString(Formatting.None)) });
        }

        private void SeedPaymentOrderTemplates()
        {
            Insert.IntoTable("PaymentOrderTemplate").InSchema("Payments")
                .Row(new 
                {
                    PaymentTemplateCode = "PAYMENT-CELL-VELCOM-PHONENO",
                    PayerName = "{{payer.name}}",
                    PayerTIN = "{{payer.tin}}",
                    PayerBankCode = "{{payer.bankCode}}",
                    PayerAccountNo = "{{payer.accountNo}}",
                    BeneficiaryName =  new ExplicitUnicodeString("Унитарное предприятие по оказанию услуг \"Велком\""),
                    BeneficiaryBankCode = "153001749",
                    BeneficiaryAccountNo = "3012202410089",
                    BeneficiaryTIN = "101528843",
                    Amount = "{{form.amount}}",
                    Purpose = new ExplicitUnicodeString("Пополнение счета. Номер телефона: {{form.phoneNo}}"),
                    CurrencyISOName = "BYR"
                })
                .Row(new 
                {
                    PaymentTemplateCode = "PAYMENT-CUSTOM-PAYMENTORDER",
                    PayerName = "{{payer.name}}",
                    PayerTIN = "{{payer.tin}}",
                    PayerBankCode = "{{payer.bankCode}}",
                    PayerAccountNo = "{{payer.accountNo}}",
                    BeneficiaryName =  "{{form.beneficiaryName}}",
                    BeneficiaryBankCode = "{{form.beneficiaryBankCode}}",
                    BeneficiaryAccountNo = "{{form.beneficiaryAccountNo}}",
                    BeneficiaryTIN = "{{form.beneficiaryTIN}}",
                    Amount = "{{form.amount}}",
                    Purpose = "{{form.purpose}}",
                    CurrencyISOName = "BYR"
                });
        }

        private void SeedUserPaymentProfiles()
        {
            Execute.WithConnection((connection, transaction) =>
            {
                var userProfiles = connection.Query<PartialUserProfile>("SELECT [UserID],[FirstName],[LastName] FROM [Membership].[UserProfile]", null, transaction).ToList();
                var num = 1;
                foreach (var userProfile in userProfiles)
                {
                    var param = new
                    {
                        UserId = userProfile.UserID,
                        FullName = userProfile.GetFullName(),
                        Address = "Minsk, Belarus",
                        PayerTIN = GenerateTIN(ref num)
                    };
                    connection.Execute("INSERT INTO [Payments].[UserPaymentProfile] ([UserId],[FullName],[Address],[PayerTIN]) VALUES (@UserId,@FullName,@Address,@PayerTIN)", param, transaction);
                    ++num;
                }
            });
        }

        private string GenerateTIN(ref int number)
        {
            if ((number / 100000000) > 1)
                throw new InvalidOperationException("All TINs are already in use.");

            var sb = new StringBuilder();
            var coffs = new[] { 23, 19, 17, 13, 7, 5, 3 };
            var dict = new Dictionary<int, char>
            {
                {0, 'A'},
                {1, 'B'},
                {2, 'C'},
                {3, 'E'},
                {4, 'H'},
                {5, 'K'},
                {6, 'M'},
                {7, 'O'},
                {8, 'P'},
                {9, 'T'}
            };

            while (true)
            {
                sb.Append("A");
                var str = number.ToString();
                for (int i = 0; i < 7 - str.Length; i++)
                {
                    sb.Append('0');
                }
                sb.Append(str);

                str = sb.ToString();
                var sum = 10 * 29;

                for (int i = 1; i < str.Length; i++)
                {
                    sum += coffs[i - 1] * (int)char.GetNumericValue(str[i]);
                }
                sum %= 11;

                if (sum == 10)
                {
                    ++number;
                    if ((number / 100000000) > 1)
                        throw new InvalidOperationException("All TINs are already in use.");
                    sb.Clear();
                    continue;
                }
                else
                {
                    str = sb.ToString();
                    sb.Append(sum.ToString());
                    sb.Replace(str[1], dict[sum], 1, 1);
                    break;
                }
            }        

            return sb.ToString();
        }

        private class PartialUserProfile
        {
            public Guid UserID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public string GetFullName()
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
    }
}
