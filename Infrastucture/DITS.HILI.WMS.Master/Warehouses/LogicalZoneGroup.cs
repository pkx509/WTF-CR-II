using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class LogicalZoneGroup : BaseEntity
    {
        public LogicalZoneGroup()
        {
            LogicalZoneGroupDetail = new List<LogicalZoneGroupDetail>();
            LogicalZoneMapGroup = new List<LogicalZoneMapGroup>();
        }

        public System.Guid LogicalZoneGroupID { get; set; }
        public string LogicalZoneGroupName { get; set; }
        public virtual ICollection<LogicalZoneGroupDetail> LogicalZoneGroupDetail { get; set; }
        public virtual ICollection<LogicalZoneMapGroup> LogicalZoneMapGroup { get; set; }
    }
}
