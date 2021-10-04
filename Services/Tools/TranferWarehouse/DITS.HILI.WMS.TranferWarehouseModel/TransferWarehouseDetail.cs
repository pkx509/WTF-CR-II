using DITS.HILI.WMS.MasterModel;
using System;

namespace DITS.HILI.WMS.TransferWarehouseModel
{
    public class TransferWarehouseDetail : BaseEntity
    {
        public Guid TranDetailID { get; set; }
        public Guid? TranID { get; set; }
        public Guid? ProductID { get; set; }
        public string PalletCode { get; set; }
        public Guid FromLocationID { get; set; }
        public Guid? ToLocationID { get; set; }
        public decimal? StockQuantity { get; set; }
        public Guid? StockUnitID { get; set; }
        public decimal? BaseQuantity { get; set; }
        public Guid? BaseUnitID { get; set; }
        public decimal? ConversionQty { get; set; }
        public bool IsActive { get; set; }
        public Guid? ProductOwnerID { get; set; }
        public Guid? SupplierID { get; set; }
        public Guid? ProductStatusID { get; set; }
        public Guid? ProductSubStatusID { get; set; }
        public Guid? ReferenceID { get; set; }
        public Guid? PackageID { get; set; }
        public DateTime? StartDT { get; set; }
        public DateTime? FinishDT { get; set; }
        public virtual TransferWarehouse TransferWarehouse { get; set; }
    }
}
