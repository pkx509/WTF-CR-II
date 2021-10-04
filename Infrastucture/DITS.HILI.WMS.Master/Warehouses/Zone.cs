using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class Zone : BaseEntity
    {
        public Guid ZoneID { get; set; }
        public Guid ZoneTypeID { get; set; }
        public Guid WarehouseID { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsBlock { get; set; }

        public virtual ZoneType ZoneType { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        public virtual ICollection<Location> LocationCollection { get; set; }
        public virtual ICollection<LogicalZone> LogicalZone { get; set; }
        public virtual ICollection<EquipZoneConfig> EquipZoneConfigCollection { get; set; }

        public Zone()
        {
            LocationCollection = new List<Location>();
            LogicalZone = new List<LogicalZone>();
            EquipZoneConfigCollection = new List<EquipZoneConfig>();
        }

    }
}
