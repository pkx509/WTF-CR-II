using DITS.HILI.WMS.MasterModel;
using System;

namespace DITS.HILI.WMS.ProductionControlModel
{
    public class ProductionControlDetail : BaseEntity
    {
        public Guid PackingID { get; set; }
        public Guid ControlID { get; set; }
        public string PalletCode { get; set; }
        public string LotNo { get; set; }
        public int? Sequence { get; set; }
        public decimal? StockQuantity { get; set; }
        public decimal? BaseQuantity { get; set; }
        public decimal? ConversionQty { get; set; }
        public decimal? RemainQTY { get; set; }
        public decimal? RemainBaseQTY { get; set; }
        public decimal? ReserveQTY { get; set; }
        public decimal? ReserveBaseQTY { get; set; }
        public Guid? StockUnitID { get; set; }
        public Guid? BaseUnitID { get; set; }
        public Guid? ProductStatusID { get; set; }
        public Guid? ProductSubStatusID { get; set; }
        public Guid? RemainStockUnitID { get; set; }
        public Guid? RemainBaseUnitID { get; set; }
        public DateTime? MFGDate { get; set; }
        public TimeSpan? MFGTimeStart { get; set; }
        public TimeSpan? MFGTimeEnd { get; set; }
        public Guid? LocationID { get; set; }
        public Guid? WarehouseID { get; set; }
        public Guid? ReceiveDetailID { get; set; }
        public PackingStatusEnum? PackingStatus { get; set; }
        public Guid? SugguestLocationID { get; set; }
        public bool? IsNormal { get; set; }
        public string OptionalSuffix { get; set; }
        public bool? IsNonProduction { get; set; }
        public string RefPalletCode { get; set; }

        public virtual ProductionControl ProductionControl { get; set; }
    }
}
