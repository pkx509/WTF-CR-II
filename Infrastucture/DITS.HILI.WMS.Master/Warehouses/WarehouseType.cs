using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class WarehouseType : BaseEntity
    {
        public Guid WarehouseTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SystemKeyEnum KeyType { get; set; }
        public virtual ICollection<Warehouse> WarehouseCollection { get; set; }

        public WarehouseType()
        {
            WarehouseCollection = new List<Warehouse>();
        }
    }
}
