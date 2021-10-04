using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public partial class CycleCountDetail : BaseEntity
    {
        public CycleCountDetail()
        {
            CycCountAssign = new List<CycleCountAssign>();
        }

        public System.Guid CyclecountDetailID { get; set; }
        public Nullable<System.Guid> CycleCountID { get; set; }
        public System.Guid ProductID { get; set; }
        public System.Guid ProductUnitID { get; set; }
        public string Product_Lot { get; set; }
        public string Barcode { get; set; }
        public Nullable<decimal> StockQuantity { get; set; }
        public Nullable<decimal> ConversionQty { get; set; }
        public Nullable<decimal> BaseQuantity { get; set; }
        public Nullable<decimal> CountingStockQty { get; set; }
        public Nullable<decimal> DiffQty { get; set; }
        public System.Guid? LocationID { get; set; }
        public Nullable<int> DetailStatus { get; set; }
        public string PalletCode { get; set; }
        public virtual CycleCount Cyclecount { get; set; }
        public virtual ICollection<CycleCountAssign> CycCountAssign { get; set; }
    }
}
