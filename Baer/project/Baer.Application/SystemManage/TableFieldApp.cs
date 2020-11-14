using Baer.Code;
using Baer.Data.Extensions;
using Baer.Domain.Entity.SystemManage;
using Baer.Domain.IRepository.SystemManage;
using Baer.Repository.SystemManage;
using System.Collections.Generic;
using System.Linq;

namespace Baer.Application.SystemManage
{
    public class TableFieldApp
    {
        private ITableFieldRepository service = new TableFieldRepository();
        private ITableRepository tableService = new TableRepository();

        public List<TableFieldEntity> GetList(string tableId)
        {
            return service.IQueryable(t => t.TableId.Equals(tableId)).OrderBy(t => t.FieldName).ToList();
        }

        public List<TableFieldEntity> GetList(Pagination pagination, string tableId, string keyword)
        {
            var expression = ExtLinq.True<TableFieldEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FieldName.Contains(keyword));
            }
            expression = expression.And(t => t.TableId.Equals(tableId));
            return service.FindList(expression, pagination);
        }

        public TableFieldEntity GetForm(string id)
        {
            return service.FindEntity(id);
        }

        public void DeleteForm(string id)
        {
            TableFieldEntity entity = GetForm(id);
            string tableName = new TableApp().GetForm(entity.TableId).TableName;
            DbHelper.ExecuteNonQuery("alter table " + tableName + " drop column " + entity.FieldName);
            service.Delete(t => t.Id == id);
        }

        public void SubmitForm(TableFieldEntity entity, string id)
        {
            string tableName = new TableApp().GetForm(entity.TableId).TableName;
            string strSql = "";
            if (!string.IsNullOrEmpty(id))
            {
                entity.Modify(id);
                service.Update(entity);

                strSql = "alter table " + tableName + " alter column " + entity.FieldName + " " + entity.FieldType;
            }
            else
            {
                entity.Create();
                service.Insert(entity);

                strSql = "alter table " + tableName + " add " + entity.FieldName + " " + entity.FieldType;
            }

            switch (entity.FieldType)
            {
                case "binary":
                case "char":
                case "nchar":
                case "varchar":
                case "nvarchar":
                case "datetime2":
                case "datetimeoffset":
                    strSql += "(" + entity.FieldLength + ")";
                    break;
                case "numeric":
                case "decimal":
                    strSql += "(" + entity.FieldLength + "," + entity.FieldAccuracy + ")";
                    break;
                case "image":
                case "bit":
                case "text":
                case "int":
                case "bigint":
                case "float":
                case "money":
                case "date":
                case "datetime":
                default:
                    break;
            }

            DbHelper.ExecuteNonQuery(strSql);
        }
    }
}
