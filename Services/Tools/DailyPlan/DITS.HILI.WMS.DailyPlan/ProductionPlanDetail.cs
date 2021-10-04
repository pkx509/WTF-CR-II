using DITS.HILI.WMS.MasterModel;
using System;

namespace DITS.HILI.WMS.DailyPlanModel
{
    public class ProductionPlanDetail : BaseEntity
    {
        public Guid ProductionDetailID { get; set; }
        public Guid ProductionID { get; set; }
        public decimal? Seq { get; set; }
        public Guid ProductID { get; set; }
        public int ProductionQty { get; set; }
        public Guid? ProductUnitID { get; set; }
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
        public virtual ProductionPlan ProductionPlan { get; set; }

    }
}
