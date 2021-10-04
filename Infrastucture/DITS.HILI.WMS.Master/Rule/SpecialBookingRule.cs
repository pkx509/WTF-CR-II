using DITS.HILI.WMS.MasterModel.Warehouses;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Rule
{
    public class SpecialBookingRule
    {
        public System.Guid RuleId { get; set; } // RuleID (Primary key)
        public string RuleName { get; set; } // Rule_name (length: 50)
        public System.Guid? ProductId { get; set; } // ProductID
        public int Aging { get; set; } // Aging
        public string UnitAging { get; set; } // UnitAging (length: 10)
        public int? DurationNotOver { get; set; } // DurationNotOver
        public string UnitDuration { get; set; } // UnitDuration (length: 10)
        public string LotNo { get; set; } // Lot_No (length: 20)
        public int? NoMoreThanDo { get; set; } // NoMoreThanDO
        public bool IsActive { get; set; } // IsActive
        public bool? IsDefault { get; set; } // IsDefault

        public virtual ICollection<ShippingTo> ShiptoCollecttion { get; set; }


        public SpecialBookingRule()
        {
            ShiptoCollecttion = new List<ShippingTo>();

        }
    }
}
