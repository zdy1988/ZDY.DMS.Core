using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace ZDY.DMS.Querying.DataTableGateway.MySQL
{
    public class MySqlDataTableGateway : AdoNetDataTableGateway
    {
        public MySqlDataTableGateway()
        { 
        
        }

        public MySqlDataTableGateway(string connectionString)
            : base(connectionString)
        {

        }

        protected override IDbConnection CreateDatabaseConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        protected override IDbDataAdapter CreateDatabaseDataAdapter()
        {
            return new MySqlDataAdapter();
        }
    }
}
