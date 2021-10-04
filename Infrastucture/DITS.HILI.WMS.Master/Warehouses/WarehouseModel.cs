using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class WarehouseModel
    {
        public Guid WarehouseID { get; set; }
        public Guid WarehouseTypeID { get; set; }
        public Guid? SiteID { get; set; }
        public string SiteName { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseShortName { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseTypeShortName { get; set; }
        public string WarehouseTypeName { get; set; }
        public string ReferenceCode { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
