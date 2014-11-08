using System.Linq;
using System.Xml;
using Dapper;
using FluentMigrator;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace VaBank.Data.Migrations
{
    [Migration(35, "Changed xml column to json for [App].[Setting]")]
    [Tags("Development", "Test", "Production")]
    public class ChangeXmlToJsonForSettings : Migration
    {
        public override void Up()
        {
            Execute.WithConnection((connection, transaction) =>
            {
                var settings = connection.Query<KeyValue>("SELECT [Key], [Value] FROM [App].[Setting]",
                    null, transaction).ToList();
                foreach (var keyValue in settings.Where(x => !string.IsNullOrEmpty(x.Value) && !x.Value.Trim().StartsWith("{")))
                {
                    var xml = new XmlDocument();
                    xml.LoadXml(keyValue.Value);
                    var json = JsonConvert.SerializeXmlNode(xml, Formatting.None, true);
                    connection.Execute("UPDATE [App].[Setting] SET [Value] = @json WHERE [Key] = @key", new
                    {
                        json,
                        key = keyValue.Key
                    }, transaction);
                }
                connection.Execute("ALTER TABLE [App].[Setting] ALTER COLUMN [Value] nvarchar(1024) NULL", null, transaction);
            });
        }

        public override void Down()
        {
            //do nothing
        }

        private class KeyValue
        {
             public string Key { get; set; }

             public string Value { get; set; }
        }
    }
}
