using System;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public class ItemGoodsReturn
    {
        public Guid? ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Guid ReceiveUnitID { get; set; }

        public DateTime? MFGDate { get; set; }
        public Guid? LineID { get; set; }
        public string LineCode { get; set; }
        public string ReceiveLot { get; set; }
        public decimal ConversionQty { get; set; }
        public string UnitName { get; set; }
        public Guid GoodsReturnDetailId { get; set; }
        public Guid GoodsReturnId { get; set; }
        public Guid ReceiveDetailID { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string Description { get; set; }

        public bool IsReject { get; set; }
        public bool IsReprocess { get; set; }
        public decimal? ReceiveQTY { get; set; }
        public decimal? RejectQty { get; set; }
        public decimal? ReprocessQty { get; set; }
        public GoodsReturnStatusEnum? GoodsReturnStatus { get; set; }
        public bool RejectStatus { get; set; }

        public bool ReprocessStatus { get; set; }
    }
}
