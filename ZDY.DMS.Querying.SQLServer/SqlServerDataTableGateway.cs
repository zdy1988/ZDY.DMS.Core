using System;
using System.Data;
using System.Data.SqlClient;

namespace ZDY.DMS.Querying.DataTableGateway.SQLServer
{
    public class SqlServerDataTableGateway : AdoNetDataTableGateway
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
