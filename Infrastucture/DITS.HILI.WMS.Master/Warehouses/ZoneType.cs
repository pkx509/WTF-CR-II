using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class ZoneType : BaseEntity
    {
        public Guid ZoneTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SystemKeyEnum KeyType { get; set; }

        public virtual ICollection<Zone> ZoneCollection { get; set; }

        public ZoneType()
        {
            ZoneCollection = new List<Zone>();
        }
    }
}
