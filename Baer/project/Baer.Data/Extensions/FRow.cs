using Baer.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baer.Data.Extensions
{
    public class FRow
    {
        private List<Field> _field;
        private string _tableName;
        private string _id;
        private bool _saveEditor;
        private string _conn;

        public bool R_HasField
        {
            get
            {
                return DbHelper.ExecuteToDataView("select 1 from " + _tableName + " where Id='" + _id + "'", _conn).Count > 0;
            }
        }

        public object this[string name]
        {
            set
            {
                this._field.Add(new Field(name, value));
            }
        }

        /// <summary>
        /// 如果设置conn，Id为空就传null
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="Id"></param>
        /// <param name="conn"></param>
        public FRow(string tableName, string Id = "", bool saveEditor = true, string conn = "")
        {
            this._field = new List<Field>();
            this._tableName = tableName;
            this._id = Id;
            this._saveEditor = saveEditor;
            this._conn = conn;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            if (_field.Count == 0)
            {
                return false;
            }

            string insertSql = GetInsertSql();
            return DbHelper.ExecuteNonQuery(insertSql, _conn) > 0;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            if(string.IsNullOrEmpty(_id))
            {
                return false;
            }

            if (_field.Count == 0)
            {
                return false;
            }

            string updateSql = GetUpdateSql();
            return DbHelper.ExecuteNonQuery(updateSql) > 0;
        }

        public bool Delete()
        {
            if(string.IsNullOrEmpty(_id))
            {
                return false;
            }

            string deleteSql = "delete " + _tableName + " where Id='" + _id + "'";
            return DbHelper.ExecuteNonQuery(deleteSql, _conn) > 0;
        }

        #region 私有方法

        /// <summary>
        /// 获取插入sql
        /// </summary>
        /// <returns></returns>
        private string GetInsertSql()
        {
            string fieldName = null;
            string fieldValue = null;

            foreach (Field field in this._field)
            {
                fieldName += (field.fieldName + ",");
                fieldValue += ("'" + field.fieldValue + "',");
            }

            if(_saveEditor)
            {
                var current = OperatorProvider.Provider.GetCurrent();
                //创建时间
                fieldName += "CreatorTime,";
                fieldValue += "getdate(),";
                //创建人员
                fieldName += "CreatorUserId,";
                fieldValue += "'" + current.UserId + "',";
                //所属公司
                fieldName += "CompanyId,";
                fieldValue += "'" + current.CompanyId + "',";
                //创建部门
                fieldName += "DepartmentId,";
                fieldValue += "'" + current.CompanyId + "',";
            }

            //所有表全部添加Id
            fieldName += "Id";
            if (string.IsNullOrEmpty(_id))
            {
                fieldValue += "newid()";
            }
            else
            {
                fieldValue += "'" + _id + "'";
            }

            string strSql = "insert into " + _tableName + "(" + fieldName + ") values(" + fieldValue + ")";
            return strSql;
        }

        /// <summary>
        /// 获取更新的sql
        /// add by baer
        /// </summary>
        /// <returns></returns>
        private string GetUpdateSql()
        {
            string strSql = string.Concat(
                "update ",
                _tableName,
                " set ");
            foreach (Field field in this._field)
            {
                strSql += (field.fieldName + "='" + field.fieldValue + "',");
            }
            strSql = strSql.Trim(',');

            strSql += " where Id = '" + _id + "'";
            return strSql;
        }

        #endregion
    }
}
