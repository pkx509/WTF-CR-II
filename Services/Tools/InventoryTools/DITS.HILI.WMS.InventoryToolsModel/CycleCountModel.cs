using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public partial class CycleCountModel : BaseEntity
    {
        public System.Guid CyclecountDetailID { get; set; }
        public Nullable<System.Guid> CycleCountID { get; set; }
        public string CycleCountCode { get; set; }
        public System.Guid ProductID { get; set; }
        public string Product_Lot { get; set; }
        public string Barcode { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> CountingQty { get; set; }
        public Nullable<decimal> DiffQty { get; set; }
        public System.Guid? LocationID { get; set; }
        public Nullable<int> DetailStatus { get; set; }
        public Nullable<System.DateTime> CycleCountStartDate { get; set; }
        public Nullable<System.DateTime> CycleCountCompleteDate { get; set; }
        public Nullable<System.DateTime> CycleCountAssignDate { get; set; }
        public Nullable<int> CycleCountStatus { get; set; }
        public CycleCountStatusEnum Status { get; set; }
        public DateTime ProductionDate { get; set; }
        public TimeSpan ProductionTime { get; set; }
        public string Lot { get; set; }
        public Guid LineID { get; set; }
        public Guid ProductUnitID { get; set; }
        public Guid? BaseUnitID { get; set; }
        public decimal? ConversionQty { get; set; }
        public Guid? StockUnitID { get; set; }
        public string OrderType { get; set; }
        public string OrderNo { get; set; }
        public int PcControlStatus { get; set; }
        public Guid? ReferenceID { get; set; }
        public Guid? PackageID { get; set; }
        public decimal StandardPalletQty { get; set; }
        public Guid? ProductStatusID { get; set; }
        public Guid? ProductSubStatusID { get; set; }

        public Guid PackingID { get; set; }
        public Guid ControlID { get; set; }
        public string PalletCode { get; set; }
        public string LotNo { get; set; }
        public int? Sequence { get; set; }
        public decimal? StockQuantity { get; set; }
        public decimal? BaseQuantity { get; set; }
        public decimal? RemainQTY { get; set; }
        public decimal? RemainBaseQTY { get; set; }
        public Guid? RemainStockUnitID { get; set; }
        public Guid? RemainBaseUnitID { get; set; }
        public DateTime? MFGDate { get; set; }
        public TimeSpan? MFGTimeStart { get; set; }
        public TimeSpan? MFGTimeEnd { get; set; }
        public Guid? ZoneID { get; set; }
        public string ZoneName { get; set; }
        public Guid? WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public int PackingStatus { get; set; }
        public Guid? ReceiveDetailID { get; set; }
        public Guid? SugguestLocationID { get; set; }

        public string LocationNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductUnitName { get; set; }

        public virtual ICollection<CycleCountDetails> CycleCountDetails { get; set; }

        public CycleCountModel()
        {
            CycleCountDetails = new List<CycleCountDetails>();
        }

    }
    public class CycleCountDetails : BaseEntity
    {
        public System.Guid CyclecountDetailID { get; set; }
        public Nullable<System.Guid> CycleCountID { get; set; }
        public string CycleCountCode { get; set; }
        public System.Guid ProductID { get; set; }
        public System.Guid ProductUnitID { get; set; }
        public string Lot { get; set; }
        public string Barcode { get; set; }
        public Nullable<decimal> BaseQuantity { get; set; }
        public Nullable<decimal> CountingStockQty { get; set; }
        public Nullable<decimal> StockQuantity { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> CountingQty { get; set; }
        public Nullable<decimal> DiffQty { get; set; }
        public decimal? ConversionQty { get; set; }
        public System.Guid? LocationID { get; set; }
        public Nullable<int> DetailStatus { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductUnitName { get; set; }
        public string StatusName { get; set; }
        public Guid? ZoneID { get; set; }
        public Guid? WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string ZoneName { get; set; }
        public string LocationNo { get; set; }
        public decimal? RemainQTY { get; set; }
        public string PalletCode { get; set; }

    }

}
