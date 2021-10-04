using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class DockConfig : BaseEntity
    {
        public Guid DockConfigID { get; set; } // DockConfigID (Primary key)
        public string DockName { get; set; } // DockName (length: 20)
        public string Barcode { get; set; } // Barcode (length: 150)
        public Guid? WarehouseID { get; set; } // WarehouseID
        public Guid? TruckTypeID { get; set; } // TruckTypeID

        // Foreign keys
        public virtual TruckType TruckType { get; set; } // FK_Truck
        public virtual Warehouse Warehouse { get; set; } // FK_Warehouse
    }
}
