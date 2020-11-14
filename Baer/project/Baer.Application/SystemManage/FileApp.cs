using Baer.Code;
using Baer.Domain.Entity.SystemManage;
using Baer.Domain.IRepository.SystemManage;
using Baer.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Baer.Application.SystemManage
{
    public class FileApp
    {
        private IFileRepository service = new FileRepository();

        public List<FileEntity> GetList(string related_Id)
        {
            return service.IQueryable(t => t.Related_Id == related_Id).OrderBy(t => t.CreatorTime).ToList();
        }

        public void Delete(string related_Id, string fileName)
        {
            if (service.IQueryable().Count(t => t.Related_Id.Equals(related_Id)) > 0)
            {
                throw new Exception("数据不存在。");
            }
            else
            {
                var expression = ExtLinq.True<FileEntity>();
                expression = expression.And(t => t.Related_Id.Equals(related_Id));
                expression = expression.And(t => t.FileName.Equals(fileName));

                service.Delete(expression);
            }
        }

        public string SaveFile(FileEntity fileEntity)
        {
            string f_Id = fileEntity.Create();
            service.Insert(fileEntity);
            return f_Id;
        }

        public void DeleteFile(string f_Id)
        {
            service.Delete(t => t.Id == f_Id);
        }
    }
}
