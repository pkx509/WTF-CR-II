using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class TruckType : BaseEntity
    {
        public Guid TruckTypeID { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public int? EsitmateTime { get; set; }
        public bool? EquipmentFlag { get; set; }
        public bool? IsTransferWH { get; set; }
        public bool? IsDefault { get; set; }

        public virtual ICollection<Truck> TruckCollection { get; set; }
        public virtual ICollection<DockConfig> DockConfigCollection { get; set; }
        public virtual ICollection<EquipZoneConfig> EquipZoneConfigCollection { get; set; }

        public TruckType()
        {
            TruckCollection = new List<Truck>();
            DockConfigCollection = new List<DockConfig>();
            EquipZoneConfigCollection = new List<EquipZoneConfig>();
        }
    }
}
