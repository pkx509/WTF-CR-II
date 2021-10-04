
using DITS.HILI.WMS.MasterModel;
using System;

namespace DITS.HILI.WMS.TruckQueueModel
{
    public class QueueDock : BaseEntity
    {
        public Guid QueueDockID { get; set; }
        public string QueueDockName { get; set; }
        public string QueueDockDesc { get; set; }
    }
}
