using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using VaBank.Common.Data.Database;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Util;
using VaBank.Core.App.Repositories;

namespace VaBank.Data.EntityFramework.App
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

        public T GetOrDefault<T>(string key)
        {
            try
            {
                var keyParam = new SqlParameter("@Key", key);
                const string sql = "SELECT [Value] FROM [App].[Setting] WHERE [Key] = @Key";
                var xml = Context.Database.SqlQuery<string>(sql, keyParam).ToList().SingleOrDefault();
                return xml == null ? default(T) : Deserialize<T>(xml);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public void Set<T>(string key, T value)
        {
            try
            {
                var json = JsonConvert.SerializeObject(value);
                var xml = JsonConvert.DeserializeXNode(json, "Setting").ToString();
                var keyParam = new SqlParameter("@Key", key);
                var valueParam = new SqlParameter("@Value", System.Data.SqlDbType.Xml) {Value = xml};
                const string sql = @"MERGE [App].[Setting] AS target
                                    USING (SELECT @Key, @Value) AS source ([Key], [Value])
                                    ON (target.[Key] = source.[Key])
                                    WHEN MATCHED THEN 
                                        UPDATE SET [Value] = source.[Value]
                                    WHEN NOT MATCHED THEN
                                        INSERT ([Key], [Value])
                                        VALUES (source.[Key], source.[Value]);";
                Context.Database.ExecuteSqlCommand(sql, valueParam, keyParam);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public Dictionary<string, T> BatchGet<T>(params string[] keys)
        {
            Assert.NotNull("keys", keys);
            try
            {
                const string sql = "SELECT [Key], [Value] FROM [App].[Setting] WHERE [Key] IN ({0})";
                var parameters = Enumerable.Range(0, keys.Length)
                    .Select(i => new SqlParameter(string.Format("@p{0}", i), SqlDbType.NVarChar) {Value = keys[i]})
                    .ToArray();
                var dynamicSql = string.Format(sql, string.Join(", ", parameters.Select(x => x.ParameterName)));
                var settings = Context.Database.SqlQuery<KeyValue>(dynamicSql, parameters.Cast<object>().ToArray()).ToList();
                return settings.ToDictionary(x => x.Key, x => Deserialize<T>(x.Value));
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        private static T Deserialize<T>(string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);
            var json = JsonConvert.SerializeXmlNode(document, Newtonsoft.Json.Formatting.None, true);
            var value = JsonConvert.DeserializeObject<T>(json);
            return value;
        }

        private class KeyValue
        {
            public string Key { get; set; }

            public string Value { get; set; }
        }
    }
}
