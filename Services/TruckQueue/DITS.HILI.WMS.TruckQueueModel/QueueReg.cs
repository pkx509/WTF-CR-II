
using DITS.HILI.WMS.MasterModel;
using System;

namespace DITS.HILI.WMS.TruckQueueModel
{
    public class QueueReg : BaseEntity
    {
        public Guid QueueId { get; set; }
        public string QueueNo { get; set; }
        public int Sequence { get; set; }
        public int? EstimateTime { get; set; }
        public string TruckRegNo { get; set; }
        public string PONO { get; set; }
        public DateTime QueueDate { get; set; }
      //  public Guid? TruckRegProviceId { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public int QueueStatusID { get; set; }
        public Guid? TruckTypeID { get; set; }
        public Guid? ShipFromId { get; set; }
        public Guid? ShipToId { get; set; }
        public Guid? QueueDockID { get; set; }
        public Guid? QueueRegisterTypeID { get; set; }
       // public string TruckRegProvice { get; set; }
        public string QueueStatus { get; set; }
        public string TruckType { get; set; }
        public string ShipFrom { get; set; }
        public string ShippTo { get; set; }
        public string QueueDock { get; set; }
        public string QueueRegisterType { get; set; }
        public int RemainingTime { get; set; }
        public string CreateByName { get; set; }
        public string TimeInString { get; set; }
        public double UsageTime
        {
            get
            {
                try
                {
                    if (TimeIn == DateTime.MinValue) return 0;
                    if (!TimeOut.HasValue || TimeOut == DateTime.MinValue)
                    {
                        return  Math.Ceiling(DateTime.Now.Subtract(TimeIn).TotalMinutes);
                    }
                    return Math.Ceiling(TimeOut.GetValueOrDefault().Subtract(TimeIn).TotalMinutes);
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
    //public enum QueueState
    //{
    //    Register = 0,
    //    WaitDoc = 1,
    //    WaitCall = 2,
    //    InQueue = 3,
    //    Loading = 4,
    //    Completed = 5,
    //    Cancel = 6
    //}
}
