using DITS.HILI.WMS.MasterModel.Warehouses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Rule
{
    public class SpecialPutawayRule : BaseEntity
    {
        public SpecialPutawayRule()
        {
            LogicalZoneMapGroup = new List<LogicalZoneMapGroup>();
        }

        public System.Guid PutAwayRuleID { get; set; }
        public System.Guid LogicalZoneID { get; set; }
        public string Remark { get; set; }
        public int PeriodLot { get; set; }
        public int Priority { get; set; }
        public string Condition { get; set; }

        [NotMapped]
        public string LogicalZoneGroupName { get; set; }
        public virtual ICollection<LogicalZoneMapGroup> LogicalZoneMapGroup { get; set; }

    }
}
