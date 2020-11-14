using Baer.Application.SystemSecurity;
using Baer.Code;
using Baer.Data.Extensions;
using Baer.Domain.Entity.Application;
using Baer.Domain.IRepository.Application;
using Baer.Repository.Application;
using System.Collections.Generic;

namespace Baer.Application.Application
{
    public class InventoryApp
    {
        private IInventoryRepository service = new InventoryRepository();

        public List<InventoryEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<InventoryEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["productType"].IsEmpty())
            {
                string productType = queryParam["productType"].ToString();
                expression = expression.And(t => t.ProductType.Equals(productType));
            }
            if (!queryParam["grade"].IsEmpty())
            {
                string grade = queryParam["grade"].ToString();
                expression = expression.And(t => t.Grade.Equals(grade));
            }
            if (!queryParam["length"].IsEmpty())
            {
                string length = queryParam["length"].ToString();
                expression = expression.And(t => t.Length.Equals(length));
            }
            if (!queryParam["strength"].IsEmpty())
            {
                string strength = queryParam["strength"].ToString();
                int curr = strength.IndexOf('~');
                if (curr > -1)//存在~
                {
                    if (curr > 0)//有最小值
                    {
                        double strengthMin = strength.Split('~')[0].ToDouble();
                        expression = expression.And(t => t.Strength >= strengthMin);
                    }

                    if (curr < strength.Length -1)//有最大值
                    {
                        double strengthMax = strength.Split('~')[1].ToDouble();
                        expression = expression.And(t => t.Strength <= strengthMax);
                    }
                }
            }
            if (!queryParam["horseValue"].IsEmpty())
            {
                string horseValue = queryParam["horseValue"].ToString();
                expression = expression.And(t => t.HorseValue.Equals(horseValue));
            }
            if (!queryParam["wareId"].IsEmpty())
            {
                string wareId = queryParam["wareId"].ToString();
                expression = expression.And(t => t.WareId.Equals(wareId));
            }
            if (!queryParam["orderNo"].IsEmpty())
            {
                string orderNo = queryParam["orderNo"].ToString();
                expression = expression.And(t => t.OrderNo.Contains(orderNo));
            }
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                expression = expression.And(t => t.Status.Equals(status));
            }
            if (!queryParam["quoteType"].IsEmpty())
            {
                string quoteType = queryParam["quoteType"].ToString();
                expression = expression.And(t => t.QuoteType.Equals(quoteType));
            }
            if (!queryParam["isRecommend"].IsEmpty())
            {
                if (queryParam["isRecommend"].ToString().Equals("1"))
                {
                    expression = expression.And(t => t.IsRecommend);
                }
                else
                {
                    expression = expression.And(t => !t.IsRecommend);
                }
            }

            var data = service.FindList(expression, pagination);
            for (int i = 0; i < data.Count; i++)
            {
                data[i].ProductType = GetProductName(data[i].ProductType);
                data[i].Weight = GetWeight(data[i].Id, data[i].Weight);
            }

            return data;
        }

        public InventoryEntity GetForm(string id)
        {
            return service.FindEntity(id);
        }

        public string GetTotalWeight(string queryJson)
        {
            string where = "";
            var queryParam = queryJson.ToJObject();
            if (!queryParam["productType"].IsEmpty())
            {
                string productType = queryParam["productType"].ToString();
                where += " and ProductType='" + productType + "'";
            }
            if (!queryParam["grade"].IsEmpty())
            {
                string grade = queryParam["grade"].ToString();
                where += " and Grade='" + grade + "'";
            }
            if (!queryParam["length"].IsEmpty())
            {
                string length = queryParam["length"].ToString();
                where += " and Length='" + length + "'";
            }
            if (!queryParam["strengthMin"].IsEmpty())
            {
                double strengthMin = queryParam["strengthMin"].ToDouble();
                where += " and Strength>='" + strengthMin + "'";
            }
            if (!queryParam["strengthMax"].IsEmpty())
            {
                double strengthMax = queryParam["strengthMax"].ToDouble();
                where += " and Strength<='" + strengthMax + "'";
            }
            if (!queryParam["horseValue"].IsEmpty())
            {
                string horseValue = queryParam["horseValue"].ToString();
                where += " and HorseValue='" + horseValue + "'";
            }
            if (!queryParam["wareId"].IsEmpty())
            {
                string wareId = queryParam["wareId"].ToString();
                where += " and WareId='" + wareId + "'";
            }
            if (!queryParam["orderNo"].IsEmpty())
            {
                string orderNo = queryParam["orderNo"].ToString();
                where += " and OrderNo like '%" + orderNo + "%'";
            }
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                where += " and Status='" + status + "'";
            }
            if (!queryParam["quoteType"].IsEmpty())
            {
                string quoteType = queryParam["quoteType"].ToString();
                where += " and QuoteType='" + quoteType + "'";
            }
            if (!queryParam["isRecommend"].IsEmpty())
            {
                string quoteType = queryParam["isRecommend"].ToString();
                where += " and IsRecommend='" + quoteType + "'";
            }

            string strSql = "select (select isnull(sum(Weight),0) from Inventory where 1=1 " + where + ")-(select isnull(sum(Weight),0) from InventoryOut where InventoryId in (select Id from Inventory where 1=1 " + where + "))";
            return DbHelper.ExecuteToString(strSql);
        }

        public void DeleteForm(string id)
        {
            new LogApp().WriteDbLog("删除产品：" + new ProductApp().GetNameByCode(GetForm(id).ProductType), DbLogType.Delete);

            service.Delete(t => t.Id == id);
        }

        public string SubmitForm(InventoryEntity inventoryEntity, string id)
        {
            string _id;
            if (!string.IsNullOrEmpty(id))
            {
                inventoryEntity.Modify(id);
                service.Update(inventoryEntity);

                _id = id;

                new LogApp().WriteDbLog("修改产品：" + new ProductApp().GetNameByCode(inventoryEntity.ProductType), DbLogType.Update);
            }
            else
            {
                _id = inventoryEntity.Create();
                service.Insert(inventoryEntity);

                new LogApp().WriteDbLog("新增产品：" + new ProductApp().GetNameByCode(inventoryEntity.ProductType), DbLogType.Create);
            }

            return _id;
        }

        private string GetProductName(string productCode)
        {
            return DbHelper.ExecuteToString("select ProductName from Product where ProductCode='" + productCode + "'");
        }

        private double GetWeight(string id, double initWeight)
        {
            double outWeight = DbHelper.ExecuteToString("select isnull(sum(Weight),0) from InventoryOut where InventoryId='" + id + "'").ToDouble();
            return initWeight - outWeight;
        }
    }
}
