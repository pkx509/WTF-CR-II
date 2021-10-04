using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class DockConfigModel
    {
        public Guid DockConfigID { get; set; } // DockConfigID (Primary key)
        public string DockName { get; set; } // DockName (length: 20)
        public string Barcode { get; set; } // Barcode (length: 150)
        public Guid? TruckTypeID { get; set; } // TruckTypeID
        public string TypeName { get; set; }
        public Guid? WarehouseID { get; set; } // WarehouseID
        public string WarehouseCode { get; set; }
        public string WarehouseShortName { get; set; }
        public string WarehouseName { get; set; }
        public Guid UserCreated { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UserModified { get; set; }
        public DateTime DateModified { get; set; }

        public bool IsActive { get; set; }

    }
}
