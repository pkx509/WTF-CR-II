using DITS.HILI.WMS.MasterModel.Rule;
using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class LogicalZoneMapGroup : BaseEntity
    {
        public System.Guid LogicalZoneID { get; set; }
        public System.Guid LogicalZoneGroupID { get; set; }
        public Nullable<System.Guid> PutAwayRuleID { get; set; }
        public Nullable<int> Seq { get; set; }
        public virtual LogicalZone LogicalZone { get; set; }
        public virtual LogicalZoneGroup LogicalZoneGroup { get; set; }
        public virtual SpecialPutawayRule SpecialPutawayRule { get; set; }
    }
}
