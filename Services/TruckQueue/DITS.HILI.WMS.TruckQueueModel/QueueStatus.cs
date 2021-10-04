using DITS.HILI.WMS.MasterModel;

namespace DITS.HILI.WMS.TruckQueueModel
{
    public class QueueStatus : BaseEntity
    {
        public int QueueStatusID { get; set; }
        public string QueueStatusName { get; set; }
        public string QueueStatusDesc { get; set; } 
    }
    public enum QueueStatusEnum : int
    {
        All = 0,
        Register = 1,
        WaitingDocument = 2,
        WaitingCall = 3,
        WaitinLoad = 4,
        Loading = 5,
        Completed = 6,
        Cancel = 7,
        WaitDocJob = 8,
        WaitRegNo = 9
    }
}
