using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace VaBank.Data.Migrations
{
    public static class SeedHelper
    {
        #region ADO.NET Helpers

        public static void AddParameters(this IDbCommand command, params DbParameter[] sqlParameters)
        {
            foreach (var param in sqlParameters)
            {
                command.Parameters.Add(param);
            }
        }

        public static SqlParameter CreateSqlParameter(this IDbCommand command, string name, SqlDbType sqlDbType, object value = null)
        {
            var param = new SqlParameter() { ParameterName = name, SqlDbType = sqlDbType, Value = value };
            command.Parameters.Add(param);
            return param;
        }

        public static SqlParameter CreateSqlParameter(this IDbCommand command, string name, object value = null)
        {
            var param = new SqlParameter() { ParameterName = name, Value = value };
            command.Parameters.Add(param);
            return param;
        }

        #endregion

        public static string GenerateRandomStringOfNumbers(int length)
        {
            var sb = new StringBuilder();
            var rand = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < length; i++)
            {
                sb.Append(rand.Next(10));
            }

            return sb.ToString();
        }
    }
}
