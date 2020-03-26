using ZDY.DMS.EventStore.AdoNet;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;

namespace ZDY.DMS.EventStore.MySQL
{
    public sealed class MySqlEventStore : AdoNetEventStore
    {
        public MySqlEventStore(AdoNetEventStoreConfiguration config, IObjectSerializer payloadSerializer)
            : base(config, payloadSerializer)
        {
        }

        protected override char ParameterChar => '@';

        protected override string BeginLiteralEscapeChar => "`";

        protected override string EndLiteralEscapeChar => "`";

        protected override IDbConnection CreateDatabaseConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}
