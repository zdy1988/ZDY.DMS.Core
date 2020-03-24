using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ZDY.DMS.Querying.AdoNet
{
    public interface IAdoNetDbCommand
    {
        #region ExecuteNonQuery

        /// <summary>
        /// 普通SQL语句执行增删改
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        int ExecuteNonQuery(string connectionString, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 普通SQL语句执行增删改
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        int ExecuteNonQuery(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 普通SQL语句执行增删改
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        int ExecuteNonQuery(string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 存储过程执行增删改
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        int ExecuteNonQueryByProc(string connectionString, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 存储过程执行增删改
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        int ExecuteNonQueryByProc(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 存储过程执行增删改
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        int ExecuteNonQueryByProc(string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行增删改
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        int ExecuteNonQuery(string connectionString, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行增删改
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        int ExecuteNonQuery(IDbConnection conn, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行增删改
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        int ExecuteNonQuery(string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        #endregion

        #region ExecuteReader

        /// <summary>
        /// SQL语句得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        IDataReader ExecuteReader(string connectionString, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// SQL语句得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        IDataReader ExecuteReader(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// SQL语句得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        IDataReader ExecuteReader(string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 存储过程得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        IDataReader ExecuteReaderByProc(string connectionString, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 存储过程得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        IDataReader ExecuteReaderByProc(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 存储过程得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        IDataReader ExecuteReaderByProc(string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        IDataReader ExecuteReader(string connectionString, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        IDataReader ExecuteReader(IDbConnection conn, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 得到 MySqlDataReader 对象
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>MySqlDataReader 对象</returns>
        IDataReader ExecuteReader(string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 执行SQL语句, 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        DataSet ExecuteDataSet(string connectionString, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行SQL语句, 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        DataSet ExecuteDataSet(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行SQL语句, 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        DataSet ExecuteDataSet(string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行存储过程, 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        DataSet ExecuteDataSetByProc(string connectionString, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行存储过程, 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        DataSet ExecuteDataSetByProc(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行存储过程, 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        DataSet ExecuteDataSetByProc(string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        DataSet ExecuteDataSet(string connectionString, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        DataSet ExecuteDataSet(IDbConnection conn, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataSet </returns>
        DataSet ExecuteDataSet(string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        #endregion

        #region ExecuteDataTable

        /// <summary>
        /// 执行SQL语句, 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        DataTable ExecuteDataTable(string connectionString, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行SQL语句, 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        DataTable ExecuteDataTable(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行SQL语句, 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        DataTable ExecuteDataTable(string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行存储过程, 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        DataTable ExecuteDataTableByProc(string connectionString, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行存储过程, 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        DataTable ExecuteDataTableByProc(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行存储过程, 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        DataTable ExecuteDataTableByProc(string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        DataTable ExecuteDataTable(string connectionString, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        DataTable ExecuteDataTable(IDbConnection conn, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns> DataTable </returns>
        DataTable ExecuteDataTable(string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 普通SQL语句执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        object ExecuteScalar(string connectionString, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 普通SQL语句执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        object ExecuteScalar(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 普通SQL语句执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        object ExecuteScalar(string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 存储过程执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        object ExecuteScalarByProc(string connectionString, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 存储过程执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        object ExecuteScalarByProc(IDbConnection conn, string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 存储过程执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        object ExecuteScalarByProc(string cmdText, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        object ExecuteScalar(string connectionString, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        object ExecuteScalar(IDbConnection conn, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        /// <summary>
        /// 执行ExecuteScalar
        /// </summary>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">可变参数</param>
        /// <returns>受影响行数</returns>
        object ExecuteScalar(string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters);

        #endregion
    }
}
