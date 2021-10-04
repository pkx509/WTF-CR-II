using System;

namespace DITS.HILI.WMS.ProductionControlModel
{
    public class ProductionControlCustom
    {

    }

    public class PC_PackingModel
    {
        public Guid ControlID { get; set; }
        public Guid? PackingID { get; set; }
        public Guid? ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Guid? LineID { get; set; }
        public string LineCode { get; set; }
        public Guid? BaseUnitID { get; set; }
        public string BaseUnit { get; set; }
        public Guid? UnitID { get; set; }
        public string Unit { get; set; }
        public string OrderNo { get; set; }
        public string OrderType { get; set; }
        public int? PalletCount { get; set; }
        public decimal? QTY { get; set; }
        public decimal? StdPalletQTY { get; set; }
        public decimal? ConversionQTY { get; set; }
        public DateTime? StartDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool? IsLastestPallet { get; set; }
    }

    public class PC_PackedModel : PC_PackingModel
    {
        public Guid? ReceivingID { get; set; }
        public decimal? CompleteQTY { get; set; }
        public int? Sequence { get; set; }
        public string PalletCode { get; set; }
        public string Lot { get; set; }
        public string ProductStatusName { get; set; }
        public string MFGTime { get; set; }

        public decimal? RemainQTY { get; set; }
        public decimal? DOQty { get; set; }
        public DateTime? MFGDate { get; set; }
        public Guid? PickingDetailID { get; set; }
    }

    public class PrintPalletModel
    {
        public Guid ControlID { get; set; }
        public string LineCode { get; set; }
        public decimal QTYPerPallet { get; set; }
        public Guid? ProductStatusID { get; set; }
        public string Suffix { get; set; }
        public string OptionalSuffix { get; set; }
    }

    public class PalletInfoModel
    {
        public Guid? PackingID { get; set; }
        public Guid? LocationID { get; set; }
        public string Location { get; set; }
        public string Lot { get; set; }
        public decimal? RemainStockQTY { get; set; }
        public decimal? RemainBaseQTY { get; set; }
        public Guid? RemainStockUnitID { get; set; }
        public string RemainStockUnit { get; set; }
        public Guid? RemainBaseUnitID { get; set; }
        public string RemainBaseUnit { get; set; }
    }

    public class CancelPalletModel
    {
        public Guid ControlID { get; set; }
        public Guid PackingID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Remark { get; set; }
    }
}
