using MySql.Data.MySqlClient;
using System;
using System.Data;
using ZDY.DMS.Querying.AdoNet;

namespace ZDY.DMS.Querying.MySql
{
    public class MySqlDbCommand : AdoNetDbCommand
    {
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
