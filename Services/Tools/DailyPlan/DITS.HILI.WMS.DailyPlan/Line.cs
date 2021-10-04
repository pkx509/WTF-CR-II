using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.DailyPlanModel
{
    public class Line : BaseEntity
    {
        public Guid LineID { get; set; } // LineID (Primary key)
        public string LineCode { get; set; } // LineCode (length: 20)
        public Guid? WarehouseID { get; set; } // WarehouseID
        public string BoiCard { get; set; } // BOICard (length: 20)
        public string LineType { get; set; } // LineType (length: 10)

        [NotMapped]
        public string WarehouseCode { get; set; }
        [NotMapped]
        public string WarehouseName { get; set; }
        public virtual ICollection<ProductionPlan> ProductionCollection { get; set; }

    }
}
