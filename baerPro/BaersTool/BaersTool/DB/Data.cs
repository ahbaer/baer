using BaersTool.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BaersTool.DB
{
    public class Data
    {
        public static string ConnectionString { get; set; }

        private List<Field> Field;
        private string TableName;
        private string RowGuid;
        public object this[string FieldName]
        {
            set
            {
                this.Field.Add(new Field(FieldName, value));
            }
        }

        public Data(string tableName)
        {
            this.TableName = tableName;
            this.Field = new List<Field>();

            ConnectionString = new Conn("Database=baer;Server=(local);User ID=sa;Password=11111;Min Pool Size=100;Max Pool Size=200;").ConnectionString;
        }

        public Data(string tableName, Conn connect)
        {
            this.TableName = tableName;
            this.Field = new List<Field>();

            ConnectionString = connect.ConnectionString;
        }

        public Data(string tableName, string rowGuid)
        {
            this.TableName = tableName;
            this.RowGuid = rowGuid;
            this.Field = new List<Field>();

            ConnectionString = new Conn("Database=baer;Server=(local);User ID=sa;Password=11111;Min Pool Size=100;Max Pool Size=200;").ConnectionString;
        }

        public Data(string tableName, string rowGuid, Conn connect)
        {
            this.TableName = tableName;
            this.RowGuid = rowGuid;
            this.Field = new List<Field>();

            ConnectionString = connect.ConnectionString;
        }

        /// <summary>
        /// 插入数据
        /// add by baer
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool Insert()
        {
            try
            {
                int ret = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string strSql = "insert into " + TableName + "(" + getInsertSql("Pre") + ") values(" + getInsertSql("Next") + ")";
                        SqlCommand cmd = new SqlCommand(strSql, conn);
                        ret = cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

                return ret != 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            try
            {
                int ret = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if(conn.State == ConnectionState.Open)
                    {
                        string strSql = "update " + TableName + " set " + getUpdateSql() + " where RowGuid = '" + RowGuid + "'";
                        SqlCommand cmd = new SqlCommand(strSql, conn);
                        ret = cmd.ExecuteNonQuery();
                    }
                }

                return ret != 0;
            }
            catch (Exception ex)
            {
                throw ex;
                throw;
            }
        }

        public bool Delete()
        {
            try
            {
                int ret = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string strSql = "delete " + TableName + " where RowGuid = '" + RowGuid + "'";
                        SqlCommand cmd = new SqlCommand(strSql, conn);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

                return ret != 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ExecuteNonQuery(string strSql)
        {
            try
            {
                int ret = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        SqlCommand cmd = new SqlCommand(strSql, conn);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

                return ret != 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ExecuteNonQuery(string strSql, Conn connect)
        {
            try
            {
                int ret = 0;
                using (SqlConnection conn = new SqlConnection(connect.ConnectionString))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        SqlCommand cmd = new SqlCommand(strSql, conn);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

                return ret != 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataView ExecuteToDataView(string strSql)
        {
            try
            {
                DataView dv = new DataView();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        DataSet ds = new DataSet();
                        SqlCommand cmd = new SqlCommand(strSql, conn);
                        new SqlDataAdapter(cmd).Fill(ds);
                        dv = new DataView(ds.Tables[0]);
                    }
                    conn.Close();

                    return dv;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataView ExecuteToDataView(string strSql, Conn connect)
        {
            try
            {
                DataView dv = new DataView();
                using (SqlConnection conn = new SqlConnection(connect.ConnectionString))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        DataSet ds = new DataSet();
                        SqlCommand cmd = new SqlCommand(strSql, conn);
                        new SqlDataAdapter(cmd).Fill(ds);
                        dv = new DataView(ds.Tables[0]);
                    }
                    conn.Close();

                    return dv;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 私有方法

        /// <summary>
        /// 获取sql的field和value
        /// add by baer
        /// </summary>
        /// <param name="type">Pre/Next</param>
        /// <returns></returns>
        private string getInsertSql(string type)
        {
            string fieldPre = null;
            string fieldNext = null;
            try
            {
                foreach (Field field in this.Field)
                {
                    fieldPre += (field.fieldName + ",");
                    fieldNext += ("'" + field.fieldValue + "',");
                }

                //所有表全部添加RowGuid
                fieldPre += "RowGuid";
                fieldNext += "newid()";

                if (type.Equals("Pre"))
                {
                    return fieldPre;
                }
                else
                {
                    return fieldNext;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取更新的sql
        /// add by baer
        /// </summary>
        /// <returns></returns>
        private string getUpdateSql()
        {
            string ret = null;
            try
            {
                foreach (Field field in this.Field)
                {
                    ret += (field.fieldName + " = '" + field.fieldValue + "',");
                }

                return ret.Trim(',');
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
