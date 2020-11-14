using Baer.Code;
using Baer.Data.Extensions;
using Baer.Domain.Entity.SystemManage;
using Baer.Domain.IRepository.SystemManage;
using Baer.Repository.SystemManage;
using System.Collections.Generic;
using System.Linq;

namespace Baer.Application.SystemManage
{
    public class TableApp
    {
        private ITableRepository service = new TableRepository();

        public List<TableEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<TableEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.TableName.Contains(keyword));
                expression = expression.Or(t => t.TableChineseName.Contains(keyword));
            }
            return service.FindList(expression, pagination);
        }

        public TableEntity GetForm(string f_Id)
        {
            return service.FindEntity(f_Id);
        }

        public void DeleteForm(string id)
        {
            DbHelper.ExecuteNonQuery("drop table " + GetForm(id).TableName);
            service.Delete(t => t.Id == id);
        }

        public void SubmitForm(TableEntity entity, string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                entity.Modify(id);
                service.Update(entity);
            }
            else
            {
                entity.Create();
                service.Insert(entity);

                string strSql = "create table " + entity.TableName + " (";
                strSql += "[RowId] bigint primary key identity(1,1),";//自增序列
                strSql += "[Id] varchar(50),";//唯一标识
                strSql += "[CreatorUserId] varchar(50),";//创建者
                strSql += "[CreatorTime] datetime,";
                strSql += "[LastModifyUserId] varchar(50),";//修改者
                strSql += "[LastModifyTime] datetime,";
                strSql += "[CompanyId] varchar(50),"; //公司id
                strSql += "[DepartmentId] varchar(50),";//部门id
                strSql = (strSql.Trim(',') + ")");
                DbHelper.ExecuteNonQuery(strSql);
            }
        }

        public void CodeGeneratorUpdate(TableEntity entity)
        {
            FRow fRow = new FRow("Sys_TableInfo", entity.Id);
            fRow["ClassPrefix"] = entity.ClassPrefix;
            fRow["Description"] = entity.Description;
            fRow["OutputModel"] = entity.OutputModel;
            fRow["SelectListField"] = entity.SelectListField;
            fRow["SelectFormField"] = entity.SelectFormField;
            fRow.Update();
        }
    }
}
