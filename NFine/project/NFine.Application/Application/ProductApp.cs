using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.Application;
using NFine.Domain.IRepository.Application;
using NFine.Repository.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.Application
{
    public class ProductApp
    {
        private IProductRepository service = new ProductRepository();

        public List<ProductEntity> GetList(string keyword = "")
        {
            var expression = ExtLinq.True<ProductEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Pro_Name.Contains(keyword));
            }
            return service.IQueryable(expression).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public ProductEntity GetForm(string pro_Id)
        {
            return service.FindEntity(pro_Id);
        }

        public void DeleteForm(string pro_Id)
        {
            service.Delete(t => t.F_Id == pro_Id);
        }

        public string SubmitForm(ProductEntity productEntity, string pro_Id)
        {
            string f_Id = "";
            if (!string.IsNullOrEmpty(pro_Id))
            {
                f_Id = pro_Id;

                productEntity.Modify(pro_Id);
                service.Update(productEntity);
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
