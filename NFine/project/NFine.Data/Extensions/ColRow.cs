using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Data.Extensions
{
    public class ColRow
    {
        private List<Field> _field;
        private string _tableName;
        private string _fieldName;
        private string _fieldValue;
        private bool _saveEditor;
        private string _conn;

        public bool R_HasField
        {
            get
            {
                return DbHelper.ExecuteToDataView("select 1 from " + _tableName + " where " + _fieldName + "='" + _fieldValue + "'", _conn).Count > 0;
            }
        }

        public object this[string name]
        {
            set
            {
                this._field.Add(new Field(name, value));
            }
            get
            {
                return _field.Find(t => t.fieldName.Equals(name));
            }
        }

        /// <summary>
        /// 如果设置conn，f_Id为空就传null
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="f_Id"></param>
        /// <param name="conn"></param>
        public ColRow(string tableName, string fieldName, string fieldValue, bool saveEditor = true, string conn = "")
        {
            this._field = new List<Field>();
            this._tableName = tableName;
            this._fieldName = fieldName;
            this._fieldValue = fieldValue;
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
            if (_field.Count == 0)
            {
                return false;
            }

            string updateSql = GetUpdateSql();
            return DbHelper.ExecuteNonQuery(updateSql) > 0;
        }

        public bool Delete()
        {
            string deleteSql = "delete " + _tableName + " where " + _fieldName + "='" + _fieldValue + "'";
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
                //创建时间
                fieldName += "F_CreatorTime,";
                fieldValue += "getdate(),";
                //创建人员
                fieldName += "F_CreatorUserId,";
                fieldValue += "'" + OperatorProvider.Provider.GetCurrent().UserId + "',";
            }

            //搜索项数据
            fieldName += _fieldName;
            fieldValue += "'" + _fieldValue + "'";

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

            strSql += " where " + _fieldName + " = '" + _fieldValue + "'";
            return strSql;
        }

        #endregion
    }
}
