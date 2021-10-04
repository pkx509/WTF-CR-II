using DITS.HILI.WMS.MasterModel.Companies;
using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class Warehouse : BaseEntity
    {
        public Guid WarehouseID { get; set; }
        public Guid WarehouseTypeID { get; set; }
        public Guid? SiteID { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string ReferenceCode { get; set; }
        public string Description { get; set; }
        public int Seqno { get; set; }
        public virtual WarehouseType WarehouseType { get; set; }
        public virtual Site Site { get; set; }
        public virtual ICollection<Zone> ZoneCollection { get; set; }
        public virtual ICollection<DockConfig> DockConfigCollection { get; set; }
        public virtual ICollection<RoleInWarehouse> RoleInWarehouseCollection { get; set; }

        public Warehouse()
        {
            ZoneCollection = new List<Zone>();
            DockConfigCollection = new List<DockConfig>();
            RoleInWarehouseCollection = new List<RoleInWarehouse>();
        }
    }
}
