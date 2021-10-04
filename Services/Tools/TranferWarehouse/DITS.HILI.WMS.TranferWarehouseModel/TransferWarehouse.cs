using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.TransferWarehouseModel
{
    public class TransferWarehouse : BaseEntity
    {
        public TransferWarehouse()
        {
            TransferWarehouseDetailCollection = new List<TransferWarehouseDetail>();
        }

        public Guid TranID { get; set; }
        public Guid FromWarehouseID { get; set; }
        public Guid ToWarehouseID { get; set; }
        public TransferWarehouseEnum TransferWarehouseStatus { get; set; }
        public DateTime? CloseDTTrans { get; set; }
        public DateTime? StartDTTrans { get; set; }
        public Guid? TruckID { get; set; }
        public virtual Truck Truck { get; set; }
        public virtual ICollection<TransferWarehouseDetail> TransferWarehouseDetailCollection { get; set; }
    }
}
