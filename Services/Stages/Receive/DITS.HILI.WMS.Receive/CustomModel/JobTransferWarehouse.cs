using System;

namespace DITS.HILI.WMS.ReceiveModel
{
    public class JobTransferWarehouse
    {
        public Guid TicketCode { get; set; }
        public Guid FromWarehouseID { get; set; }
        public Guid ToWarehouseID { get; set; }
        public string FromWarehouse { get; set; }
        public string ToWarehouse { get; set; }
        public string TransferStatus { get; set; }
        public string StartDTTrans { get; set; }
    }
}
