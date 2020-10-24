using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.Application;
using NFine.Domain.IRepository.Application;
using NFine.Repository.Application;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.Application
{
    public class ProductApp
    {
        private IProductRepository service = new ProductRepository();

        public List<ProductEntity> GetList(string queryJson)
        {
            return service.IQueryable().OrderBy(t => t.Sort).ToList();
        }

        public ProductEntity GetForm(string f_Id)
        {
            return service.FindEntity(f_Id);
        }

        public string GetNameByCode(string projectCode)
        {
            return service.FindEntity(t => t.ProductCode == projectCode).ProductName;
        }

        public void DeleteForm(string f_Id)
        {
            new LogApp().WriteDbLog("删除产品分类：" + GetForm(f_Id).ProductName, DbLogType.Delete);
            service.Delete(t => t.F_Id == f_Id);
        }

        public string SubmitForm(ProductEntity productEntity, string keyValue)
        {
            string f_Id;
            if (!string.IsNullOrEmpty(keyValue))
            {
                productEntity.Modify(keyValue);
                service.Update(productEntity);

                f_Id = keyValue;

                new LogApp().WriteDbLog("修改产品分类：" + productEntity.ProductName, DbLogType.Update);
            }
            else
            {
                f_Id = productEntity.Create();
                service.Insert(productEntity);
                new LogApp().WriteDbLog("新增产品分类：" + productEntity.ProductName, DbLogType.Create);
            }

            return f_Id;
        }
    }
}
