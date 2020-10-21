using NFine.Code;
using NFine.Data.Extensions;
using NFine.Domain.Entity.Application;
using NFine.Domain.IRepository.Application;
using NFine.Repository.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.Application
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
            if (!queryParam["strengthMin"].IsEmpty())
            {
                double strengthMin = queryParam["strengthMin"].ToDouble();
                expression = expression.And(t => t.Strength >= strengthMin);
            }
            if (!queryParam["strengthMax"].IsEmpty())
            {
                double strengthMax = queryParam["strengthMax"].ToDouble();
                expression = expression.And(t => t.Strength <= strengthMax);
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
                data[i].Weight = GetWeight(data[i].F_Id, data[i].Weight);
            }

            return data;
        }

        public InventoryEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public string GetAllWeight(string queryJson)
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

            string strSql = "select (select isnull(sum(Weight),0) from Inventory where 1=1 " + where + ")-(select isnull(sum(Weight),0) from InventoryOut where InventoryId in (select F_Id from Inventory where 1=1 " + where + "))";
            return DbHelper.ExecuteToString(strSql);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }

        public string SubmitForm(InventoryEntity inventoryEntity, string keyValue)
        {
            string f_Id;
            if (!string.IsNullOrEmpty(keyValue))
            {
                inventoryEntity.Modify(keyValue);
                service.Update(inventoryEntity);

                f_Id = keyValue;
            }
            else
            {
                f_Id = inventoryEntity.Create();
                service.Insert(inventoryEntity);
            }

            return f_Id;
        }

        private string GetProductName(string productCode)
        {
            return DbHelper.ExecuteToString("select ProductName from Product where ProductCode='" + productCode + "'");
        }

        private double GetWeight(string f_Id, double initWeight)
        {
            double outWeight = DbHelper.ExecuteToString("select isnull(sum(Weight),0) from InventoryOut where InventoryId='" + f_Id + "'").ToDouble();
            return initWeight - outWeight;
        }
    }
}
