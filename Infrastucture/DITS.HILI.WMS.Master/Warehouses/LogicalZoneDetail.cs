using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class LogicalZoneDetail : BaseEntity
    {
        public System.Guid LogicalZoneDetailID { get; set; }
        public System.Guid LogicalZoneID { get; set; }
        public System.Guid LocationID { get; set; }
        public int Seq { get; set; }
        public Nullable<System.Guid> UserCreated { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.Guid> UserModified { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public bool IsActive { get; set; }
        public virtual Location Location { get; set; }
        public virtual LogicalZone LogicalZone { get; set; }
        public decimal? ReserveQty { get; set; }
        public decimal? AvailableQty { get; set; }
    }
}
