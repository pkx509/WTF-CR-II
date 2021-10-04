using System;

namespace DITS.HILI.WMS.DispatchModel.CustomModel
{
    public class BackOrderModel
    {
        public string DispatchCode { get; set; }
        public string Pono { get; set; }
        public string OrderNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public decimal? DispatchQty { get; set; }
        public decimal? BackOrderQty { get; set; }
        public string UnitName { get; set; }
        public DateTime? EstDispatchDate { get; set; }
        public bool? IsBackOrder { get; set; }
        public bool? IsConfirm { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? StockUnitId { get; set; }
        public Guid? BaseUnitId { get; set; }
        public decimal? ConversionQty { get; set; }
        public Guid? ProductStatusId { get; set; }
        public Guid? ProductOwnerId { get; set; }
        public Guid? DispatchDetailId { get; set; }
        public Guid? RuleId { get; set; }
        public Guid BookingId { get; set; }
    }
}
