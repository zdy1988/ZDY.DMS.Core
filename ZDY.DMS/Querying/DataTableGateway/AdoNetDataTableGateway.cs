using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.Common;

namespace ZDY.DMS.Querying.DataTableGateway
{
    public abstract class AdoNetDataTableGateway : IDataTableGateway
    {
        private readonly string connectionString;

        public AdoNetDataTableGateway()
        { 

        }

        public AdoNetDataTableGateway(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected abstract IDbConnection CreateDatabaseConnection(string connectionString);

        protected abstract IDbDataAdapter CreateDatabaseDataAdapter();

        #region ExecuteNonQuery

        /// <summary>
        /// 普通SQL语句执行增删改
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public int ExecuteNonQuery(string connectionString, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteNonQuery(connectionString, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// 普通SQL语句执行增删改
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public int ExecuteNonQuery(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteNonQuery(conn, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 普通SQL语句执行增删改
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public int ExecuteNonQuery(string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteNonQuery(this.connectionString, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 存储过程执行增删改
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public int ExecuteNonQueryByProc(string connectionString, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteNonQuery(connectionString, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 存储过程执行增删改
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public int ExecuteNonQueryByProc(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteNonQuery(conn, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 存储过程执行增删改
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public int ExecuteNonQueryByProc(string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteNonQuery(this.connectionString, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 执行增删改
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public int ExecuteNonQuery(string connectionString, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            return ExecuteNonQuery(CreateDatabaseConnection(connectionString), cmdText, cmdType, commandParameters);
        }

        /// <summary>
        /// 执行增删改
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public int ExecuteNonQuery(IDbConnection conn, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            int result = 0;

            using (conn)
            {
                try
                {                
                    var command = conn.CreateCommand();
                    PrepareCommand(command, conn, cmdType, cmdText, commandParameters);
                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn != null && conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 执行增删改
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public int ExecuteNonQuery(string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            return ExecuteNonQuery(CreateDatabaseConnection(this.connectionString), cmdText, cmdType, commandParameters);
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// SQL语句得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        public IDataReader ExecuteReader(string connectionString, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteReader(connectionString, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// SQL语句得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        public IDataReader ExecuteReader(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteReader(conn, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// SQL语句得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        public IDataReader ExecuteReader(string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteReader(this.connectionString, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// 存储过程得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        public IDataReader ExecuteReaderByProc(string connectionString, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteReader(connectionString, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 存储过程得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        public IDataReader ExecuteReaderByProc(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteReader(conn, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 存储过程得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        public IDataReader ExecuteReaderByProc(string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteReader(this.connectionString, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        public IDataReader ExecuteReader(string connectionString, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            return ExecuteReader(CreateDatabaseConnection(connectionString), cmdText, cmdType, commandParameters);
        }

        /// <summary>
        /// 得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        public IDataReader ExecuteReader(IDbConnection conn, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            IDataReader result = null;

            using (conn)
            {
                try
                {
                    IDbCommand command = conn.CreateCommand();
                    PrepareCommand(command, conn, cmdType, cmdText, commandParameters);
                    result = command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn != null && conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        public IDataReader ExecuteReader(string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            return ExecuteReader(CreateDatabaseConnection(this.connectionString), cmdText, cmdType, commandParameters);
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 执行SQL语句, 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        public DataSet ExecuteDataSet(string connectionString, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataSet(connectionString, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// 执行SQL语句, 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        public DataSet ExecuteDataSet(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataSet(conn, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// 执行SQL语句, 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        public DataSet ExecuteDataSet(string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataSet(this.connectionString, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// 执行存储过程, 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        public DataSet ExecuteDataSetByProc(string connectionString, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataSet(connectionString, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 执行存储过程, 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        public DataSet ExecuteDataSetByProc(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataSet(conn, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 执行存储过程, 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        public DataSet ExecuteDataSetByProc(string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataSet(this.connectionString, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        public DataSet ExecuteDataSet(string connectionString, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataSet(CreateDatabaseConnection(connectionString), cmdText, cmdType, commandParameters);
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        public DataSet ExecuteDataSet(IDbConnection conn, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            DataSet result = null;

            using (conn)
            {
                try
                {
                    IDbCommand command = conn.CreateCommand();
                    PrepareCommand(command, conn, cmdType, cmdText, commandParameters);
                    IDbDataAdapter adapter = CreateDatabaseDataAdapter();
                    adapter.SelectCommand = command;
                    result = new DataSet();
                    adapter.Fill(result);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn != null && conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        public DataSet ExecuteDataSet(string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataSet(CreateDatabaseConnection(this.connectionString), cmdText, cmdType, commandParameters);
        }

        #endregion

        #region ExecuteDataTable

        /// <summary>
        /// 执行SQL语句, 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        public DataTable ExecuteDataTable(string connectionString, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataTable(connectionString, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// 执行SQL语句, 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        public DataTable ExecuteDataTable(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataTable(conn, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// 执行SQL语句, 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        public DataTable ExecuteDataTable(string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataTable(this.connectionString, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// 执行存储过程, 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        public DataTable ExecuteDataTableByProc(string connectionString, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataTable(connectionString, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 执行存储过程, 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        public DataTable ExecuteDataTableByProc(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataTable(conn, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 执行存储过程, 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        public DataTable ExecuteDataTableByProc(string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataTable(this.connectionString, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        public DataTable ExecuteDataTable(string connectionString, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataTable(CreateDatabaseConnection(connectionString), cmdText, cmdType, commandParameters);
        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        public DataTable ExecuteDataTable(IDbConnection conn, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            DataTable dtResult = null;
            DataSet ds = ExecuteDataSet(conn, cmdText, cmdType, commandParameters);

            if (ds != null && ds.Tables.Count > 0)
            {
                dtResult = ds.Tables[0];
            }
            return dtResult;
        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        public DataTable ExecuteDataTable(string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            return ExecuteDataTable(CreateDatabaseConnection(this.connectionString), cmdText, cmdType, commandParameters);
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 普通SQL语句执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public object ExecuteScalar(string connectionString, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteScalar(connectionString, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// 普通SQL语句执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public object ExecuteScalar(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteScalar(conn, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// 普通SQL语句执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public object ExecuteScalar(string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteScalar(this.connectionString, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// 存储过程执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public object ExecuteScalarByProc(string connectionString, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteScalar(connectionString, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 存储过程执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public object ExecuteScalarByProc(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteScalar(conn, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 存储过程执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public object ExecuteScalarByProc(string cmdText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteScalar(this.connectionString, cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public object ExecuteScalar(string connectionString, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            return ExecuteScalar(CreateDatabaseConnection(connectionString), cmdText, cmdType, commandParameters);
        }

        /// <summary>
        /// 执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public object ExecuteScalar(IDbConnection conn, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            object result = null;

            using (conn)
            {
                try
                {
                    IDbCommand command = conn.CreateCommand();
                    PrepareCommand(command, conn, cmdType, cmdText, commandParameters);
                    result = command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn != null && conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        public object ExecuteScalar(string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            return ExecuteScalar(CreateDatabaseConnection(this.connectionString), cmdText, cmdType, commandParameters);
        }

        #endregion

        #region PrepareCommand
        /// <summary>
        /// Command对象执行前预处理
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        private static void PrepareCommand(IDbCommand command, IDbConnection connection, CommandType cmdType, string cmdText, IDbDataParameter[] commandParameters, IDbTransaction trans = null)
        {
            try
            {
                if (connection.State != ConnectionState.Open) connection.Open();

                command.Connection = connection;
                command.CommandText = cmdText;
                command.CommandType = cmdType;
                //command.CommandTimeout = 3600;    //此处请自定义

                if (trans != null)
                {
                    command.Transaction = trans;
                }

                if (commandParameters != null)
                {
                    foreach (IDbDataParameter parm in commandParameters)
                        command.Parameters.Add(parm);
                }
            }
            catch
            {

            }
        }
        #endregion
    }
}
