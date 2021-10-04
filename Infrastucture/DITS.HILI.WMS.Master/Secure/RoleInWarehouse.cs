using DITS.HILI.WMS.MasterModel.Warehouses;
using System;

namespace DITS.HILI.WMS.MasterModel.Secure
{
    public class RoleInWarehouse : BaseEntity
    {
        public Guid RoleID { get; set; }
        public Guid WarehouseID { get; set; }


        public virtual Roles Role { get; set; }
        public virtual Warehouse Warehouses { get; set; }
        public RoleInWarehouse()
        {
        }
    }
}
