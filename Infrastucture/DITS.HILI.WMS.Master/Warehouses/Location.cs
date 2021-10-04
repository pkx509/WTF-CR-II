using DITS.HILI.WMS.MasterModel.Stock;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class Location : BaseEntity
    {
        public Guid LocationID { get; set; }
        public Guid ZoneID { get; set; }
        public Guid? PalletTypeID { get; set; }
        public LocationTypeEnum LocationType { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int RowNo { get; set; }
        public int ColumnNo { get; set; }
        public int LevelNo { get; set; }
        public int Seq { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public decimal ReserveWeight { get; set; }
        public decimal? PalletCapacity { get; set; }
        public decimal? SizeCapacity { get; set; }
        public decimal? LocationReserveQty { get; set; }
        public bool IsBlock { get; set; }

        public virtual Zone Zone { get; set; }
        public virtual PalletType PalletType { get; set; }
        public virtual ICollection<LogicalZoneDetail> LogicalZoneDetail { get; set; }

        [NotMapped]
        public virtual ICollection<StockTransaction> StockTransactionCollection { get; set; }
        public Location()
        {
            StockTransactionCollection = new List<StockTransaction>();
            LogicalZoneDetail = new List<LogicalZoneDetail>();
        }
    }
}
