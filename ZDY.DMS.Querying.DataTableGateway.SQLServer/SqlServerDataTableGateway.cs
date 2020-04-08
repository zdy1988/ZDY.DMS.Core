using System;
using System.Data;
using System.Data.SqlClient;

namespace ZDY.DMS.Querying.DataTableGateway.SQLServer
{
    public class SqlServerDataTableGateway : AdoNetDataTableGateway
    {
        public SqlServerDataTableGateway()
        {

        }

        public SqlServerDataTableGateway(string connectionString)
            : base(connectionString)
        {

        }

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
