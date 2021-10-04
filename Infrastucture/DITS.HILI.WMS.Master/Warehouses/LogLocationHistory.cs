using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class LogLocationHistory : BaseEntity
    {
        public System.Guid LocationHistoryID { get; set; }
        public System.Guid LogicalZoneID { get; set; }
        public System.Guid LocationID { get; set; }
        public string OrderNo { get; set; }
        public Nullable<System.Guid> PackingID { get; set; }
        public System.Guid ProductID { get; set; }
        public string Lot { get; set; }
        public System.DateTime MFGDate { get; set; }
        public decimal StockQty { get; set; }
        public decimal BasicQty { get; set; }
        public decimal BalanceStockQty { get; set; }
        public System.Guid StockUnitID { get; set; }
        public System.Guid BasicUnitID { get; set; }
        public virtual LogicalZone LogicalZone { get; set; }
    }
}
