using System;

namespace DITS.HILI.WMS.ReceiveModel
{
    public class PalletTagModel
    {
        public Guid? ProductID { get; set; }
        public Guid? TransDetailID { get; set; }
        public Guid ReceivingID { get; set; }
        public Guid ReceiveDetailId { get; set; }
        public string PalletCode { get; set; }
        public decimal? Qty { get; set; }
        public decimal? ConfirmQty { get; set; }
        public string Location { get; set; }
        public string SuggestLocation { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string UnitName { get; set; }
        public decimal? ReceivingQty { get; set; }
        public Guid? WarehouseID { get; set; }
        public int PackingStatus { get; set; }
        public string LotNo { get; set; }
        public bool IsTransfer { get; set; }
        public bool IsPutAway { get; set; }
        public DateTime? MFGDate { get; set; }
        public string LineCode { get; set; }
        public Guid? ProductStatusID { get; set; }
    }
}
