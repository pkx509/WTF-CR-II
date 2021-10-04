using DITS.HILI.WMS.MasterModel;

namespace DITS.HILI.WMS.TruckQueueModel
{
    public class QueueRegisterType:BaseEntity
    {
        public System.Guid QueueRegisterTypeID { get; set; }
        public string QueueRegisterTypeName { get; set; }
        public string QueueRegisterTypeDesc { get; set; }
    }
}
