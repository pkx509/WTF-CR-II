using System;

namespace DITS.WMS.Data.CustomModel
{


    public class ProductSelectModel
    {
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Dispatch_Quatity { get; set; }
        public decimal? Confirm_Quatity { get; set; }
        public string ProductLot { get; set; }
        public Guid? StockUnitId { get; set; }
        public string StockUnitName { get; set; }
        public Guid? PriceUnitId { get; set; }
        public string PriceUnitName { get; set; }
        public Guid? RuleId { get; set; }
        public string RuleName { get; set; }

    }

    public class ProductCustomModel
    {
        public Guid ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Guid? DispatchID { get; set; }
        public string PONo { get; set; }
        public DateTime? MFGDate { get; set; }
    }

    public class ProductSearchModel
    {
        public string PONo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public bool IsCreditNote { get; set; }
        public bool IsNormal { get; set; }
        public bool ToReprocess { get; set; }
        public bool FromReprocess { get; set; }
        public bool IsItemChange { get; set; }
        public bool IsWithoutGoods { get; set; }
        public Guid? ReferenceDispatchTypeID { get; set; }
    }
}
