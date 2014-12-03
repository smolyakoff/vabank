using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Data.Migrations
{
    [Migration(48, "Add payments schema tables.")]
    public class Payments: Migration
    {
        private const string SchemaName = "Payments";

        public override void Up()
        {
            Create.Table("Bank").InSchema(SchemaName)
                .WithColumn("Code").AsBankCode().PrimaryKey("PK_Bank")
                .WithColumn("Name").AsName().NotNullable();

            Create.Table("Payment").InSchema(SchemaName)
                .WithColumn("OperationID").AsGuid().PrimaryKey("PK_Payment").ForeignKey("FK_Payment_To_Operation", "Processing", "Operation", "ID")
                .WithColumn("PayerAccountNo").AsAccountNumber().NotNullable()
                .WithColumn("PayerName").AsName().NotNullable()
                .WithColumn("PayerPSN").AsPSN().NotNullable()
                .WithColumn("Details").AsBigString().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
