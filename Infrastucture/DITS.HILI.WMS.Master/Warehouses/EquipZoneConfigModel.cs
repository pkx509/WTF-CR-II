using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class EquipZoneConfigModel
    {
        public Guid EquipID { get; set; }
        public string EquipName { get; set; }
        public string Barcode { get; set; }
        public string Serialnumber { get; set; }
        public Guid? ZoneID { get; set; }
        public Guid? TruckTypeID { get; set; }
        public Guid? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string ZoneName { get; set; }
        public string TruckTypeName { get; set; }
        public bool IsActive { get; set; }

    }
}
