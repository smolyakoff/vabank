using FluentMigrator.Builders.Create.Table;

namespace VaBank.Data.Migrations
{
    internal static class MigrationExtensions
    {
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

        public static ICreateTableColumnOptionOrWithColumnSyntax AsSecurityString(this ICreateTableColumnAsTypeSyntax syntax)
        {
            const int length = 256;
            return syntax.AsString(length);
        }
    }
}
