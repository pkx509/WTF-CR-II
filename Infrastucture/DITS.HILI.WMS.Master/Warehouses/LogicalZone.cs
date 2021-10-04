using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class LogicalZone : BaseEntity
    {
        public LogicalZone()
        {
            LogLocationHistory = new List<LogLocationHistory>();
            LogicalZoneDetail = new List<LogicalZoneDetail>();
            LogicalZoneConfig = new List<LogicalZoneConfig>();
            LogicalZoneMapGroup = new List<LogicalZoneMapGroup>();
        }

        public System.Guid LogicalZoneID { get; set; }
        public Nullable<System.Guid> ZoneID { get; set; }
        public string LogicalZoneName { get; set; }
        public Nullable<int> LogicalZoneAutoLocation { get; set; }
        public Nullable<int> ZoneBalanceLocation { get; set; }
        public Nullable<int> LogicalZoneStatus { get; set; }
        public bool IsPallet { get; set; }
        public virtual ICollection<LogLocationHistory> LogLocationHistory { get; set; }
        public virtual ICollection<LogicalZoneDetail> LogicalZoneDetail { get; set; }
        public virtual ICollection<LogicalZoneConfig> LogicalZoneConfig { get; set; }
        public virtual ICollection<LogicalZoneMapGroup> LogicalZoneMapGroup { get; set; }
        public virtual Zone Zone { get; set; }
    }
}
