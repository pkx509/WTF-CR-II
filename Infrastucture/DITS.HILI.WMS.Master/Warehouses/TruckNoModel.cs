using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class TruckNoModel
    {
        public Guid TruckID { get; set; } // DockConfigID (Primary key)
        public string TruckNo { get; set; } // DockName (length: 20)
        public Guid? TruckTypeID { get; set; } // TruckTypeID
        public string TypeName { get; set; }
        public Guid UserCreated { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UserModified { get; set; }
        public DateTime DateModified { get; set; }

        public bool IsActive { get; set; }

    }
}
