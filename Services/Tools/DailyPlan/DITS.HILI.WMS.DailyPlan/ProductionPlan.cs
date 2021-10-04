using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.DailyPlanModel
{
    public class ProductionPlan : BaseEntity
    {
        public ProductionPlan()
        {
            ProductionPlanDetail = new List<ProductionPlanDetail>();
        }

        public Guid ProductionID { get; set; }
        public DateTime ProductionDate { get; set; }
        public Guid? PeriodID { get; set; }
        public string OrderNo { get; set; }
        public string OrderType { get; set; }
        public Guid? LineID { get; set; }
        public int? DailyPlanStatus { get; set; }
        public virtual Line Line { get; set; }
        public virtual ICollection<ProductionPlanDetail> ProductionPlanDetail { get; set; }

    }
}
