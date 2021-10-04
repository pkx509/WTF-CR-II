using System;

namespace DITS.HILI.WMS.RegisterTruckModel.CustomModel
{
    public class RegisterTruckConsolidateDeatilModel
    {
        public int OrderPick { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public Guid? LocationId { get; set; }
        public string LocationCode { get; set; }
        public string DocumentNo { get; set; }
        public Guid? LocationSuggestId { get; set; }
        public string LocationSuggestCode { get; set; }
        public Guid? PickStockUnitId { get; set; }
        public string PickStockUnitName { get; set; }
        public decimal PickStockQty { get; set; }
        public string PalletCode { get; set; }
        public Guid? PalletUnitId { get; set; }
        public string PalletUnitName { get; set; }
        public decimal PalletQty { get; set; }
        public decimal PickBaseQty { get; set; }
        public Guid? DeliveryID { get; set; }
        public decimal? ConsolidateQty { get; set; }
        public Guid? ConsolidateUnitId { get; set; }
        public string ConsolidateUnitName { get; set; }
        public string DispatchCode { get; set; }
        public Nullable<decimal> ConfirmQuantity { get; set; }
        public Nullable<decimal> ConfirmQty { get; set; }
        public Nullable<System.Guid> ConfirmUnitID { get; set; }
        public Nullable<decimal> ConfirmBasicQuantity { get; set; }
        public Nullable<System.Guid> ConfirmBasicUnitID { get; set; }
        public decimal ConversionQty { get; set; }
        public System.Guid BasicUnitID { get; set; }
        public decimal? TotalCoso { get; set; }
        public decimal? TotalCosoBypallet { get; set; }
        public decimal? BookingQtyByPo { get; set; }
        public int PickStatus { get; set; }
        public int CountPick { get; set; }
        public int CountConso { get; set; }
        public decimal? ConsolidateQtyByPo { get; set; }
        public System.Guid ShippingID { get; set; }
        public Nullable<System.Guid> DockTypeID { get; set; }
        public string DockName { get; set; }
        public bool IsSuccess { get; set; }
        public System.Guid? ShippingDetailID { get; set; }
        public System.Guid? AssignID { get; set; }
        public string Pono { get; set; }
        public decimal? StockQtyAssign { get; set; }

    }
}
