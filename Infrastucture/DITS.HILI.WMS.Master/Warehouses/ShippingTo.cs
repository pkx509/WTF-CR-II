using DITS.HILI.WMS.MasterModel.Rule;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class ShippingTo : BaseEntity
    {
        public System.Guid ShipToId { get; set; } // ShipToID (Primary key)
        public string Name { get; set; }
        public string ShortName { get; set; } 
        public string Description { get; set; } 
        public string Address { get; set; } 
        public bool? IsDefault { get; set; } // IsDefault 
        public string BusinessGroup { get; set; } // BusinessGroup (length: 10)
        public System.Guid RuleId { get; set; } // RuleID 
        public virtual SpecialBookingRule SpecialBookingRule { get; set; }

    }
}
