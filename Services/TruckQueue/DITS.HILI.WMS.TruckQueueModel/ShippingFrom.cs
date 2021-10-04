using DITS.HILI.WMS.MasterModel;
using System;

namespace DITS.HILI.WMS.TruckQueueModel
{
    public class ShippingFrom : BaseEntity
    {
        public Guid ShipFromId { get; set; } // ShipToID (Primary key)
        public string Name { get; set; } // Name_TH (length: 50)
        public string ShortName { get; set; } // Name_TH (length: 50)
        public string Description { get; set; } // Name_EN (length: 50)
        public string Address { get; set; } // Name_EN (length: 50)
    }
}
