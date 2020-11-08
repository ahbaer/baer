using NFine.Code;
using NFine.Data.Extensions;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.SystemManage
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

        public TableFieldEntity GetForm(string f_Id)
        {
            return service.FindEntity(f_Id);
        }

        public void DeleteForm(string f_Id)
        {
            TableFieldEntity entity = GetForm(f_Id);
            string tableName = new TableApp().GetForm(entity.TableId).TableName;
            DbHelper.ExecuteNonQuery("alter table " + tableName + " drop column " + entity.FieldName);
            service.Delete(t => t.F_Id == f_Id);
        }

        public void SubmitForm(TableFieldEntity entity, string f_Id)
        {
            string tableName = new TableApp().GetForm(entity.TableId).TableName;
            string strSql = "";
            if (!string.IsNullOrEmpty(f_Id))
            {
                entity.Modify(f_Id);
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
                case "varchar":
                case "nvarchar":
                    strSql += "(" + entity.FieldLength + ")";
                    break;
                case "decimal":
                    strSql += "(" + entity.FieldLength + "," + entity.FieldAccuracy + ")";
                    break;
                case "int":
                case "bigint":
                case "float":
                case "bit":
                case "date":
                case "datetime":
                case "text":
                default:
                    break;
            }
            DbHelper.ExecuteNonQuery(strSql);
        }
    }
}
