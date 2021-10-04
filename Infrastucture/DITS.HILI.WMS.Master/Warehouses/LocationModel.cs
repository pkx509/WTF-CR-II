using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class LocationModel
    {
        public Guid WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseShortName { get; set; }
        public string WarehouseCode { get; set; }
        public Guid LocationID { get; set; }
        public Guid ZoneID { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneShortName { get; set; }
        public string ZoneName { get; set; }
        public LocationTypeEnum LocationType { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int RowNo { get; set; }
        public int ColumnNo { get; set; }
        public int LevelNo { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public decimal ReserveWeight { get; set; }
        public decimal? PalletCapacity { get; set; }
        public decimal? SizeCapacity { get; set; }
        public decimal? LocationReserveQty { get; set; }
        public bool IsBlock { get; set; }
    }
}
