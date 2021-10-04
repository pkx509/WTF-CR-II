using System;

namespace DITS.HILI.WMS.DailyPlanModel
{
    public class ProductionPlanCustomModel
    {
        public int? RowIndex { get; set; }
        public Guid? ProductionID { get; set; }
        public Guid? ProductionDetailID { get; set; }
        public DateTime ProductionDate { get; set; }
        public Guid? PeriodID { get; set; }
        public string OrderNo { get; set; }
        public string OrderType { get; set; }
        public int? DailyPlanStatus { get; set; }
        public Guid? LineId { get; set; }
        public string LineCode { get; set; }

        public decimal? Seq { get; set; }
        public Guid ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int ProductionQty { get; set; }
        public Guid? ProductUnitID { get; set; }
        public string ProductUnitName { get; set; }
        public decimal? Weight_G { get; set; }
        public string Film { get; set; }
        public string Box { get; set; }
        public string Powder { get; set; }
        public string Oil { get; set; }
        public string FD { get; set; }
        public string Stamp { get; set; }
        public string Sticker { get; set; }
        public string Mark { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string WorkingTime { get; set; }
        public string OilType { get; set; }
        public string Formula { get; set; }
        public int? PalletQty { get; set; }
        public string Remark { get; set; }
        public TimeSpan? PeriodStartTime { get; set; }
        public TimeSpan? PeriodEndTime { get; set; }
        public Guid? UserModified { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsReceive { get; set; }
    }
}
