using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public class Adjust : BaseEntity
    {
        public Adjust()
        {
            AdjustDetail = new List<AdjustDetail>();
        }

        public System.Guid AdjustID { get; set; }
        public string AdjustCode { get; set; }
        public System.Guid AdjustTypeID { get; set; }
        public bool? IsEffect { get; set; }
        public AdjustStatusEnum AdjustStatus { get; set; }
        public DateTime? AdjustStartDate { get; set; }
        public DateTime? AdjustCompleteDate { get; set; }
        public string ReferenceDoc { get; set; }
        public virtual ICollection<AdjustDetail> AdjustDetail { get; set; }
    }
}
