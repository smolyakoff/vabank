using FluentMigrator.Builders;
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

        #endregion


    }
}
