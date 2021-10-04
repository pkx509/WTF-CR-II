using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class LogicalZoneModel
    {

        public System.Guid LogicalZoneID { get; set; }
        public string LogicalZoneName { get; set; }
        public System.Guid ZoneID { get; set; }
        public string ZoneName { get; set; }
        public System.Guid WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public bool IsPallet { get; set; }


        public virtual ICollection<LogicalZoneDetailModel> LogicalZoneDetailModelCollection { get; set; }
        public virtual ICollection<LogicalZoneConfigModel> LogicalZoneConfigModelCollection { get; set; }

        public LogicalZoneModel()
        {
            LogicalZoneDetailModelCollection = new List<LogicalZoneDetailModel>();
            LogicalZoneConfigModelCollection = new List<LogicalZoneConfigModel>();
        }
    }
}
