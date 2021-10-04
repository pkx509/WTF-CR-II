using DITS.HILI.WMS.MasterModel.Products;
using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class LogicalZoneGroupDetail : BaseEntity
    {
        public System.Guid LogicalGroupDetailID { get; set; }
        public System.Guid LogicalZoneGroupID { get; set; }
        public Nullable<System.Guid> ProductGroupLevel3ID { get; set; }
        public Nullable<System.Guid> ProductID { get; set; }
        public virtual LogicalZoneGroup LogicalZoneGroup { get; set; }
        public virtual ProductGroupLevel3 ProductGroupLevel3 { get; set; }
    }
}
