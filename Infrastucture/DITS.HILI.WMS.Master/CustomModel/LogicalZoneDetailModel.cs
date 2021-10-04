using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class LogicalZoneDetailModel
    {

        public string LogicalZoneCode { get; set; }
        public Guid LogicalZoneDetailID { get; set; }
        public string LocationNo { get; set; }
        public Guid LocationId { get; set; }
        public int Seq { get; set; }


        public decimal? LocationCapacity { get; set; }
        public string ZoneName { get; set; }

    }
}
