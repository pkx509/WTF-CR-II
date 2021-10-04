using DITS.HILI.WMS.MasterModel;
using System;

namespace DITS.HILI.WMS.TruckQueueModel
{
    public class QueueRunning : BaseEntity
    {
        public Guid QueueRunId { get; set; }
        public DateTime QueuDate { get; set; }
        public int QueueRun { get; set; }
    }
}
