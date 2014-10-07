using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(5, "Change ApplicationClientId type to string. ")]
    [Tags("Maintenance", "Development", "Production")]
    public class ChangeApplicationClientId : Migration
    {
        private const string SchemaName = "Membership";
        
        public override void Down()
        {
            Delete.ForeignKey("FK_ApplicationToken_To_ApplicationClient").OnTable("ApplicationToken").InSchema(SchemaName);
            Delete.PrimaryKey("PK_ApplicationClient").FromTable("ApplicationClient").InSchema(SchemaName);
            Delete.Column("ID").FromTable("ApplicationClient").InSchema(SchemaName);

            Create.Column("ID").OnTable("ApplicationClient").InSchema(SchemaName).AsGuid();
            Create.PrimaryKey("PK_ApplicationClient").OnTable("ApplicationClient").WithSchema(SchemaName).Column("ID");
            Alter.Column("ClientID").OnTable("ApplicationToken").InSchema(SchemaName).AsGuid();
            Create.ForeignKey("FK_ApplicationToken_To_ApplicationClient").FromTable("ApplicationToken").InSchema(SchemaName)
                .ForeignColumn("ClientID").ToTable("ApplicationClient").InSchema(SchemaName).PrimaryColumn("ID");

            Delete.Column("Secrete").FromTable("ApplicationClient").InSchema(SchemaName);
        }

        public override void Up()
        {
            Delete.ForeignKey("FK_ApplicationToken_To_ApplicationClient").OnTable("ApplicationToken").InSchema(SchemaName);
            Delete.PrimaryKey("PK_ApplicationClient").FromTable("ApplicationClient").InSchema(SchemaName);
            Delete.Column("ID").FromTable("ApplicationClient").InSchema(SchemaName);
            
            Create.Column("ID").OnTable("ApplicationClient").InSchema(SchemaName).AsString(1024);
            Create.PrimaryKey("PK_ApplicationClient").OnTable("ApplicationClient").WithSchema(SchemaName).Column("ID");
            Alter.Column("ClientID").OnTable("ApplicationToken").InSchema(SchemaName).AsString(1024);
            Create.ForeignKey("FK_ApplicationToken_To_ApplicationClient").FromTable("ApplicationToken").InSchema(SchemaName)
                .ForeignColumn("ClientID").ToTable("ApplicationClient").InSchema(SchemaName).PrimaryColumn("ID");
            
            Create.Column("Secrete").OnTable("ApplicationClient").InSchema(SchemaName).AsString(256);                
        }
    }
}
