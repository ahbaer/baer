using Baer.Application.SystemSecurity;
using Baer.Code;
using Baer.Domain.Entity.Application;
using Baer.Domain.IRepository.Application;
using Baer.Repository.Application;
using System.Collections.Generic;
using System.Linq;

namespace Baer.Application.Application
{
    public class ProductApp
    {
        private IProductRepository service = new ProductRepository();

        public List<ProductEntity> GetList(string queryJson)
        {
            return service.IQueryable().OrderBy(t => t.Sort).ToList();
        }

        public ProductEntity GetForm(string id)
        {
            return service.FindEntity(id);
        }

        public string GetNameByCode(string projectCode)
        {
            return service.FindEntity(t => t.ProductCode == projectCode).ProductName;
        }

        public void DeleteForm(string id)
        {
            new LogApp().WriteDbLog("删除产品分类：" + GetForm(id).ProductName, DbLogType.Delete);
            service.Delete(t => t.Id == id);
        }

        public string SubmitForm(ProductEntity productEntity, string id)
        {
            string _id;
            if (!string.IsNullOrEmpty(id))
            {
                productEntity.Modify(id);
                service.Update(productEntity);

                _id = id;

                new LogApp().WriteDbLog("修改产品分类：" + productEntity.ProductName, DbLogType.Update);
            }
            else
            {
                _id = productEntity.Create();
                service.Insert(productEntity);
                new LogApp().WriteDbLog("新增产品分类：" + productEntity.ProductName, DbLogType.Create);
            }

            return _id;
        }
    }
}
