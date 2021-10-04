using System;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public partial class CycleCountAssign
    {
        public System.Guid CyclecountAssignID { get; set; }
        public Nullable<System.Guid> CyclecountDetailID { get; set; }
        public Nullable<decimal> BasickQuantity { get; set; }
        public Nullable<System.Guid> BasicUnit { get; set; }
        public Nullable<System.Guid> StockUnitID { get; set; }
        public Nullable<decimal> StockQuantity { get; set; }
        public string PalletCode { get; set; }
        public virtual CycleCountDetail CycCountDetail { get; set; }
    }
}
