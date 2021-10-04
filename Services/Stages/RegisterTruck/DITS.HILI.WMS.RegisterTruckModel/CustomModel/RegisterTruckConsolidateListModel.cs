using System;

namespace DITS.HILI.WMS.RegisterTruckModel
{
    public class RegisterTruckConsolidateListModel
    {
        public System.Guid DeliveryId { get; set; }
        public DateTime? DateCreated { get; set; }
        public string PoNo { get; set; }
        public string DocumentNo { get; set; }
        public int ConsolidateStatusId { get; set; }
        public string ConsolidateStatusName { get; set; }
        public string LicensePlate { get; set; }

    }
}
