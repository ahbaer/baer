using System;
using System.Collections.Generic;
using System.Text;

namespace DianZiFaPiaoApi.Model_MS
{

    public class InvoiceResult
    {
        public int code { get; set; }
        public string msg { get; set; }
        public InvoiceResultData data { get; set; }
    }

    public class InvoiceResultData
    {
        public string taxControlCode { get; set; }
        public string invoiceCheckCode { get; set; }
        public string invoiceQrCode { get; set; }
        public string invoiceTotalPrice { get; set; }
        public string invoiceTotalTax { get; set; }
        public string invoiceNo { get; set; }
        public string invoiceDate { get; set; }
        public string invoiceCode { get; set; }
        public string invoiceTotalPriceTax { get; set; }
        public List<InvoiceDetail> invoiceDetailsList { get; set; }
    }

    public class InvoiceDetail
    {
        public string goodsName { get; set; }
        public string goodsCode { get; set; }
        public string goodsPrice { get; set; }
        public string goodsUnit { get; set; }
        public string goodsQuantity { get; set; }
        public string goodsTotalPrice { get; set; }
        public string goodsTaxRate { get; set; }
        public string goodsTotalTax { get; set; }
    }

}
