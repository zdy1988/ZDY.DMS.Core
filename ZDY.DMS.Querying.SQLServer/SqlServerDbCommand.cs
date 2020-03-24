using System;
using System.Data;
using System.Data.SqlClient;
using ZDY.DMS.Querying.AdoNet;

namespace ZDY.DMS.Querying.SQLServer
{
    public class SqlServerDbCommand : AdoNetDbCommand
    {
        protected override IDbConnection CreateDatabaseConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        protected override IDbDataAdapter CreateDatabaseDataAdapter()
        {
            return new SqlDataAdapter();
        }
    }
}
