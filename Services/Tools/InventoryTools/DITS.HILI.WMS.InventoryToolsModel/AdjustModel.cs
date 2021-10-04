using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public partial class AdjustModel : BaseEntity
    {
        public System.Guid AdjustID { get; set; }
        public string AdjustCode { get; set; }
        public System.Guid AdjustTypeID { get; set; }
        public string AdjustTypeName { get; set; }
        public Nullable<bool> IsEffect { get; set; }
        public int AdjustStatus { get; set; }
        public string AdjustStatusName { get; set; }
        public DateTime? AdjustStartDate { get; set; }
        public DateTime? AdjustCompleteDate { get; set; }
        public string ReferenceDoc { get; set; }

        public System.Guid ProductID { get; set; }
        public string Product_Lot { get; set; }
        public string Barcode { get; set; }
        public System.Guid? LocationID { get; set; }

        public string Lot { get; set; }
        public Guid LineID { get; set; }
        public Guid ProductUnitID { get; set; }
        public Guid? BaseUnitID { get; set; }
        public decimal? ConversionQty { get; set; }
        public Guid? StockUnitID { get; set; }
        public string OrderType { get; set; }
        public string OrderNo { get; set; }

        public Guid? ZoneID { get; set; }
        public string ZoneName { get; set; }
        public Guid? WarehouseID { get; set; }
        public string WarehouseName { get; set; }

        public string LocationNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductUnitName { get; set; }
        public string PalletCode { get; set; }
        public string ProductStatusName { get; set; }
        public string ProductSubStatusName { get; set; }


        public Guid PackingID { get; set; }
        public Guid ControlID { get; set; }
        public string LotNo { get; set; }
        public int? Sequence { get; set; }
        public decimal? DiffQty { get; set; }
        public decimal? StockQuantity { get; set; }
        public decimal? BaseQuantity { get; set; }
        public decimal? RemainQTY { get; set; }
        public decimal? RemainBaseQTY { get; set; }
        public Guid? RemainStockUnitID { get; set; }
        public Guid? RemainBaseUnitID { get; set; }
        public decimal? AdjustStockQty { get; set; }
        public Guid? AdjustStockUnitID { get; set; }
        public Guid? AdjustBaseUnitID { get; set; }
        public TimeSpan? MFGTimeStart { get; set; }
        public TimeSpan? MFGTimeEnd { get; set; }
        public DateTime? MFGDate { get; set; }

        public int PcControlStatus { get; set; }
        public Guid? ReferenceID { get; set; }
        public Guid? PackageID { get; set; }
        public decimal StandardPalletQty { get; set; }
        public Guid? ProductStatusID { get; set; }
        public Guid? ProductSubStatusID { get; set; }
        public int PackingStatus { get; set; }
        public DateTime ProductionDate { get; set; }

        public bool? IsSentInterface { get; set; }
        public virtual ICollection<AdjustModelDetails> AdjustModelDetails { get; set; }
        public bool? IsHideItem { get; set; }

        public AdjustModel()
        {
            AdjustModelDetails = new List<AdjustModelDetails>();
        }

    }
    public class AdjustModelDetails : BaseEntity
    {
        public System.Guid AdjustDetailID { get; set; }
        public System.Guid AdjustID { get; set; }
        public string AdjustCode { get; set; }
        public Guid? ReferenceID { get; set; }
        public int? AdjustTransactionType { get; set; }
        public Guid? ProductID { get; set; }
        public string ProductLot { get; set; }
        public Guid? LocationID { get; set; }
        public string Barcode { get; set; }
        public int? AdjustStatus { get; set; }
        public Guid? ProductUnitID { get; set; }
        public decimal? AdjustStockQty { get; set; }
        public Guid? AdjustStockUnitID { get; set; }
        public decimal? AdjustBaseQty { get; set; }
        public Guid? AdjustBaseUnitID { get; set; }
        public string PalletCode { get; set; }
        public decimal? ConversionQty { get; set; }
        public DateTime? MFGDate { get; set; }


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
        public Guid? ProductStatusID { get; set; }
        public Guid? ProductSubStatusID { get; set; }



    }

}
