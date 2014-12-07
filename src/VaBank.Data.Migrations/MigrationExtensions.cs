

using FluentMigrator.Builders.Alter.Column;
using FluentMigrator.Builders.Create.Column;
using FluentMigrator.Builders.Create.Table;
namespace VaBank.Data.Migrations
{
    internal static class MigrationExtensions
    {
        #region CreateTableSyntax

        public static ICreateTableColumnOptionOrWithColumnSyntax AsUserId(this ICreateTableColumnAsTypeSyntax syntax)
        {
            return syntax.AsGuid();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsClientId(this ICreateTableColumnAsTypeSyntax syntax)
        {
            return syntax.AsGuid();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsShortName(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 50;
            return syntax.AsString(length);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsName(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 100;
            return syntax.AsString(length);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsEmail(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 50;
            return syntax.AsString(length);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsPhoneNumber(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 30;
            return syntax.AsString(length);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsBigString(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 1024;
            return syntax.AsString(length);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsText(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 4096;
            return syntax.AsString(length);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsSecurityString(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 256;
            return syntax.AsString(length);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsAccountNumber(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 13;
            return syntax.AsString(length);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsCurrencyISOName(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 3;
            return syntax.AsString(length);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsCardId(this ICreateTableColumnAsTypeSyntax syntax)
        {
            return syntax.AsGuid();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsCardNumber(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 16;
            return syntax.AsString(length);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsCardVendorId(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 16;
            return syntax.AsString(length);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsBankCode(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 9;
            return syntax.AsString(length);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsTIN(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 9;
            return syntax.AsString(length);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsPaymentCode(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 4;
            return syntax.AsString(length);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsPaymentTemplateCode(this ICreateTableColumnAsTypeSyntax syntax)
        {
            return syntax.AsBigString();
        }

        #endregion

        #region CreateColumnSyntax

        public static ICreateColumnOptionSyntax AsUserId(this ICreateColumnAsTypeSyntax syntax)
        {
            return syntax.AsGuid();
        }

        public static ICreateColumnOptionSyntax AsClientId(this ICreateColumnAsTypeSyntax syntax)
        {
            return syntax.AsName();
        }

        public static ICreateColumnOptionSyntax AsShortName(this ICreateColumnAsTypeSyntax syntax)
        {
            const int length = 50;
            return syntax.AsString(length);
        }

        public static ICreateColumnOptionSyntax AsName(this ICreateColumnAsTypeSyntax syntax)
        {
            const int length = 100;
            return syntax.AsString(length);
        }

        public static ICreateColumnOptionSyntax AsEmail(this ICreateColumnAsTypeSyntax syntax)
        {
            const int length = 50;
            return syntax.AsString(length);
        }

        public static ICreateColumnOptionSyntax AsPhoneNumber(this ICreateColumnAsTypeSyntax syntax)
        {
            const int length = 30;
            return syntax.AsString(length);
        }

        public static ICreateColumnOptionSyntax AsBigString(this ICreateColumnAsTypeSyntax syntax)
        {
            const int length = 1024;
            return syntax.AsString(length);
        }

        public static ICreateColumnOptionSyntax AsText(this ICreateColumnAsTypeSyntax syntax)
        {
            const int length = 4096;
            return syntax.AsString(length);
        }

        public static ICreateColumnOptionSyntax AsSecurityString(this ICreateColumnAsTypeSyntax syntax)
        {
            const int length = 256;
            return syntax.AsString(length);
        }

        public static ICreateColumnOptionSyntax AsAccountNumber(this ICreateColumnAsTypeSyntax syntax)
        {
            const int length = 13;
            return syntax.AsString(length);
        }

        public static ICreateColumnOptionSyntax AsCurrencyISOName(this ICreateColumnAsTypeSyntax syntax)
        {
            const int length = 3;
            return syntax.AsString(length);
        }

        public static ICreateColumnOptionSyntax AsCardId(this ICreateColumnAsTypeSyntax syntax)
        {
            return syntax.AsGuid();
        }

        public static ICreateColumnOptionSyntax AsCardNumber(this ICreateColumnAsTypeSyntax syntax)
        {
            const int length = 16;
            return syntax.AsString(length);
        }

        public static ICreateColumnOptionSyntax AsCardVendorId(this ICreateColumnAsTypeSyntax syntax)
        {
            const int length = 16;
            return syntax.AsString(length);
        }

        public static ICreateColumnOptionSyntax AsBankCode(this ICreateColumnAsTypeSyntax syntax)
        {
            const int length = 9;
            return syntax.AsString(length);
        }

        public static ICreateColumnOptionSyntax AsTIN(this ICreateColumnAsTypeSyntax syntax)
        {
            const int length = 9;
            return syntax.AsString(length);
        }

        public static ICreateColumnOptionSyntax AsPaymentCode(this ICreateColumnAsTypeSyntax syntax)
        {
            const int length = 4;
            return syntax.AsString(length);
        }

        public static ICreateColumnOptionSyntax AsPaymentTemplateCode(this ICreateColumnAsTypeSyntax syntax)
        {
            return syntax.AsBigString();
        }

        #endregion

        #region AlterColumnSyntax

        public static IAlterColumnOptionSyntax AsUserId(this IAlterColumnAsTypeSyntax syntax)
        {
            return syntax.AsGuid();
        }

        public static IAlterColumnOptionSyntax AsClientId(this IAlterColumnAsTypeSyntax syntax)
        {
            return syntax.AsGuid();
        }

        public static IAlterColumnOptionSyntax AsShortName(this IAlterColumnAsTypeSyntax syntax)
        {
            const int length = 50;
            return syntax.AsString(length);
        }

        public static IAlterColumnOptionSyntax AsName(this IAlterColumnAsTypeSyntax syntax)
        {
            const int length = 100;
            return syntax.AsString(length);
        }

        public static IAlterColumnOptionSyntax AsEmail(this IAlterColumnAsTypeSyntax syntax)
        {
            const int length = 50;
            return syntax.AsString(length);
        }

        public static IAlterColumnOptionSyntax AsPhoneNumber(this IAlterColumnAsTypeSyntax syntax)
        {
            const int length = 30;
            return syntax.AsString(length);
        }

        public static IAlterColumnOptionSyntax AsBigString(this IAlterColumnAsTypeSyntax syntax)
        {
            const int length = 1024;
            return syntax.AsString(length);
        }

        public static IAlterColumnOptionSyntax AsText(this IAlterColumnAsTypeSyntax syntax)
        {
            const int length = 4096;
            return syntax.AsString(length);
        }

        public static IAlterColumnOptionSyntax AsSecurityString(this IAlterColumnAsTypeSyntax syntax)
        {
            const int length = 256;
            return syntax.AsString(length);
        }

        public static IAlterColumnOptionSyntax AsAccountNumber(this IAlterColumnAsTypeSyntax syntax)
        {
            const int length = 13;
            return syntax.AsString(length);
        }

        public static IAlterColumnOptionSyntax AsCurrencyISOName(this IAlterColumnAsTypeSyntax syntax)
        {
            const int length = 3;
            return syntax.AsString(length);
        }

        public static IAlterColumnOptionSyntax AsCardId(this IAlterColumnAsTypeSyntax syntax)
        {
            return syntax.AsGuid();
        }

        public static IAlterColumnOptionSyntax AsCardNumber(this IAlterColumnAsTypeSyntax syntax)
        {
            const int length = 16;
            return syntax.AsString(length);
        }

        public static IAlterColumnOptionSyntax AsBankCode(this IAlterColumnAsTypeSyntax syntax)
        {
            const int length = 9;
            return syntax.AsString(length);
        }

        public static IAlterColumnOptionSyntax AsTIN(this IAlterColumnAsTypeSyntax syntax)
        {
            const int length = 9;
            return syntax.AsString(length);
        }

        public static IAlterColumnOptionSyntax AsPaymentCode(this IAlterColumnAsTypeSyntax syntax)
        {
            const int length = 4;
            return syntax.AsString(length);
        }

        public static IAlterColumnOptionSyntax AsPaymentTemplateCode(this IAlterColumnAsTypeSyntax syntax)
        {
            const int length = 10;
            return syntax.AsString(length);
        }

        #endregion
    }
}
