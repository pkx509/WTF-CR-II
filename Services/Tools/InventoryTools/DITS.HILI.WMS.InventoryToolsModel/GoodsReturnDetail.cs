using DITS.HILI.WMS.MasterModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public class GoodsReturnDetail : BaseEntity
    {
        public Guid GoodsReturnDetailID { get; set; }
        public Guid? GoodsReturnID { get; set; }
        public Guid? ProductID { get; set; }
        public decimal? ReceiveQTY { get; set; }
        public decimal? ReceiveBaseQTY { get; set; }
        public decimal? ConversionQTY { get; set; }
        public Guid? ReceiveUnitID { get; set; }
        public Guid? ReceiveBaseUnitID { get; set; }
        public DateTime? MFGDate { get; set; }
        public Guid? LineID { get; set; }
        public string ReceiveLot { get; set; }

        public Guid ReceiveDetailID { get; set; }
        public decimal? RejectQty { get; set; }
        public decimal? ReprocessQty { get; set; }

        public bool RejectStatus { get; set; }

        public bool ReprocessStatus { get; set; }
        public virtual GoodsReturn GoodsReturn { get; set; }

        public Guid? ReFromWarehouse { get; set; }

        public bool? IsSentInterfaceChangeLot { get; set; }
        public bool? IsSentInterfaceChangeStatus { get; set; }

        [NotMapped]
        public string ProductCode { get; set; }
        [NotMapped]
        public string ProductName { get; set; }
        [NotMapped]
        public string ReceiveUnitName { get; set; }
        [NotMapped]
        public string LineName { get; set; }

    }
}
