using DITS.HILI.WMS.MasterModel;
using System;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public class AdjustDetail : BaseEntity
    {
        public System.Guid AdjustDetailID { get; set; }
        public System.Guid AdjustID { get; set; }
        public Guid? ReferenceID { get; set; }
        public string AdjustTransactionType { get; set; }
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
        public bool? IsSentInterface { get; set; }
        public virtual Adjust Adjust { get; set; }
    }
}
