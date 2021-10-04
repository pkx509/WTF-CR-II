using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class EquipZoneConfig : BaseEntity
    {
        public Guid EquipID { get; set; } // EquipID (Primary key)
        public string EquipName { get; set; } // EquipName (length: 50)
        public string Barcode { get; set; } // Barcode (length: 250)
        public string Serialnumber { get; set; } // Serialnumber (length: 50)
        public Guid? ZoneID { get; set; } // ZoneID
        public Guid? TruckTypeID { get; set; } // TruckTypeID

        public virtual Zone Zone { get; set; }
        public virtual TruckType TruckType { get; set; }
    }
}
