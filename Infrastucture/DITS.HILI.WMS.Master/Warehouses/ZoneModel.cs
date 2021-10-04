using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class ZoneModel
    {
        public Guid ZoneID { get; set; }
        public Guid ZoneTypeID { get; set; }
        public Guid WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public Guid WarehouseTypeID { get; set; }
        public Guid? SiteID { get; set; }
        public string SiteName { get; set; }
        public string ZoneTypeCode { get; set; }
        public string ZoneTypeName { get; set; }
        public string ReferenceCode { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
