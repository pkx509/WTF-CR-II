using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class LogicalZoneGroupModel
    {

        public Guid LogicalZoneGroupId { get; set; }
        public string LogicalZoneGroupName { get; set; }
        public string LogicalZoneGroupLevel3Name { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public ICollection<LogicalZoneGroupDetailModel> LogicalZoneGroupDetailCollection { get; set; }
        public LogicalZoneGroupModel()
        {
            LogicalZoneGroupDetailCollection = new List<LogicalZoneGroupDetailModel>();
        }
    }
}
