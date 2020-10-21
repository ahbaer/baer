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

        public ProductEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }

        public string SubmitForm(ProductEntity productEntity, string keyValue)
        {
            string f_Id;
            if (!string.IsNullOrEmpty(keyValue))
            {
                productEntity.Modify(keyValue);
                service.Update(productEntity);

                f_Id = keyValue;
            }
            else
            {
                f_Id = productEntity.Create();
                service.Insert(productEntity);
            }

            return f_Id;
        }
    }
}
