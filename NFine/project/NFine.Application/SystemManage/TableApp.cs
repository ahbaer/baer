using NFine.Code;
using NFine.Data.Extensions;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.SystemManage
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

        public void DeleteForm(string f_Id)
        {
            DbHelper.ExecuteNonQuery("drop table " + GetForm(f_Id).TableName);
            service.Delete(t => t.F_Id == f_Id);
        }

        public void SubmitForm(TableEntity entity, string f_Id)
        {
            if (!string.IsNullOrEmpty(f_Id))
            {
                entity.Modify(f_Id);
                service.Update(entity);
            }
            else
            {
                entity.Create();
                service.Insert(entity);

                string strSql = "create table " + entity.TableName + " (";
                strSql += "[RowId] bigint primary key identity(1,1),";//自增序列
                strSql += "[F_Id] varchar(50),";//唯一标识
                strSql += "[F_CreatorTime] datetime,"; //允许新增（默认）
                strSql += "[F_CreatorUserId] varchar(50),";
                if((bool)entity.AllowEdit)
                {
                    strSql += "[F_LastModifyTime] datetime,"; //允许编辑
                    strSql += "[F_LastModifyUserId] varchar(50),";
                }
                if((bool)entity.AllowDelete)
                {
                    strSql += "[F_DeleteTime] datetime,"; //允许删除
                    strSql += "[F_DeleteUserId] varchar(50),";
                    strSql += "[F_DeleteMark] bit,";
                }
                strSql = (strSql.Trim(',') + ")");
                DbHelper.ExecuteNonQuery(strSql);
            }
        }

        public void CodeGeneratorUpdate(TableEntity entity)
        {
            FRow fRow = new FRow("Sys_TableInfo", entity.F_Id);
            fRow["ClassPrefix"] = entity.ClassPrefix;
            fRow["OutputModel"] = entity.OutputModel;
            fRow["SelectListField"] = entity.SelectListField;
            fRow["SelectFormField"] = entity.SelectFormField;
            fRow.Update();
        }
    }
}
