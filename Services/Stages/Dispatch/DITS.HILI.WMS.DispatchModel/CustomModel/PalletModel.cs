using System;

namespace DITS.HILI.WMS.DispatchModel.CustomModel
{
    public class PalletModel
    {

        public string WarehouseName { get; set; }
        public string LocationNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductUnitName { get; set; }
        public string PalletCode { get; set; }
        public string ProductStatusName { get; set; }
        public string ProductSubStatusName { get; set; }
        public string LotNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime? MFGDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public decimal? Quantity { get; set; }

        public int TotalRecords { get; set; }
    }
}
