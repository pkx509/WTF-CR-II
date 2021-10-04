using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class LogicalZoneConfig : BaseEntity
    {
        public System.Guid LogicalConfigID { get; set; }
        public System.Guid LogicalZoneID { get; set; }
        public Nullable<System.Guid> ConfigID { get; set; }
        public System.Guid? ConfigValueID { get; set; }
        public string ConfigValue { get; set; }
        public Nullable<int> PrioritySeq { get; set; }
        public virtual LogicalZone LogicalZone { get; set; }
    }
}
