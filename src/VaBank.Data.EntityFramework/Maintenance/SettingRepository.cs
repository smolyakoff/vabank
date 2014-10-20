using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Maintenance;

namespace VaBank.Data.EntityFramework.Maintenance
{
    public class SettingRepository : IRepository, ISettingRepository
    {
        protected readonly DbContext Context;

        public SettingRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "DbContext should not be empty");
            }
            Context = context;
        }

        public T Get<T>(string key)
            where T : class
        {
            try
            {
                var keyParam = new SqlParameter("@Key", key);
                const string sql = "SELECT [Value] FROM [Maintenance].[Setting] WHERE [Key] = @Key";
                var xml = Context.Database.SqlQuery<string>(sql, keyParam).ToList().SingleOrDefault();
                if (xml == null)
                    return null;
                var document = new XmlDocument();
                document.LoadXml(xml);
                var json = JsonConvert.SerializeXmlNode(document, Newtonsoft.Json.Formatting.None, true);
                var value = JsonConvert.DeserializeObject<T>(json);
                return value;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public void Set<T>(string key, T value)
            where T : class
        {
            try
            {
                var json = JsonConvert.SerializeObject(value);
                var xml = JsonConvert.DeserializeXNode(json, "Setting").ToString();
                var keyParam = new SqlParameter("@Key", key);
                var valueParam = new SqlParameter("@Value", System.Data.SqlDbType.Xml) { Value = xml };
                const string sql = "UPDATE [Maintenance].[Setting] SET [Value] = @Value WHERE [Key] = @Key";
                Context.Database.ExecuteSqlCommand(sql, valueParam, keyParam);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }
    }
}
