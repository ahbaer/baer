/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using NFine.Code;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace NFine.Data.Extensions
{
    public class DbHelper
    {
        private static string ConnectionString = DBConnection.GetConnectionString();

        public static int ExecuteSqlCommand(string cmdText)
        {
            using (DbConnection conn = new SqlConnection(ConnectionString))
            {
                DbCommand cmd = new SqlCommand();
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, null);
                return cmd.ExecuteNonQuery();
            }
        }

        #region ToNonQuery
        public static int ExecuteNonQuery(string cmdText, string connectionName)
        {
            int count = 0;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString(connectionName)))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand(cmdText, conn);
                    count = cmd.ExecuteNonQuery();
                }
                conn.Close();

                return count;
            }
        }

        public static int ExecuteNonQuery(string cmdText)
        {
            return ExecuteNonQuery(cmdText, ConnectionString);
        }

        public static int ExecuteNonQuery(string cmdText, string connectionName, bool encrypt)
        {
            return ExecuteNonQuery(cmdText, DBConnection.GetConnectionString(connectionName, encrypt));
        }
        #endregion

        #region ToString
        public static string ExecuteToString(string cmdText, string connectionName)
        {
            string ret = "";
            using (SqlConnection conn = new SqlConnection(connectionName))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand(cmdText, conn);
                    new SqlDataAdapter(cmd).Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            ret = dr[0].ToString();
                        }
                    }
                }
                conn.Close();

                return ret;
            }
        }

        public static string ExecuteToString(string cmdText)
        {
            return ExecuteToString(cmdText, ConnectionString);
        }

        public static string ExecuteToString(string cmdText, string connectionName, bool encrypt)
        {
            return ExecuteToString(cmdText, DBConnection.GetConnectionString(connectionName, encrypt));
        }
        #endregion

        #region ToDataView
        public static DataView ExecuteToDataView(string cmdText, string connectionName)
        {
            DataView dv = new DataView();
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString(connectionName)))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand(cmdText, conn);
                    new SqlDataAdapter(cmd).Fill(ds);
                    dv = new DataView(ds.Tables[0]);
                }
                conn.Close();

                return dv;
            }
        }

        public static DataView ExecuteToDataView(string cmdText)
        {
            return ExecuteToDataView(cmdText, ConnectionString);
        }

        public static DataView ExecuteToDataView(string cmdText, string connectionName, bool encrypt)
        {
            return ExecuteToDataView(cmdText, DBConnection.GetConnectionString(connectionName, encrypt));
        }
        #endregion

        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction isOpenTrans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (isOpenTrans != null)
                cmd.Transaction = isOpenTrans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                cmd.Parameters.AddRange(cmdParms);
            }
        }
    }
}
