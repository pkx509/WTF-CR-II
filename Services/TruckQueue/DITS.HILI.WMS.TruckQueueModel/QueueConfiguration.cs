using DITS.HILI.WMS.MasterModel;
using System;

namespace DITS.HILI.WMS.TruckQueueModel
{
    public class QueueConfiguration : BaseEntity
    {
        public Guid ConfigurationID { get; set; }
        public string Message { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public bool EnableMessage { get; set; }
    }
}
