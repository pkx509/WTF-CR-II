using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public partial class CycleCount : BaseEntity
    {
        public CycleCount()
        {
            CycCountDetail = new List<CycleCountDetail>();
        }

        public System.Guid CycleCountID { get; set; }
        public string CycleCountCode { get; set; }
        public Nullable<System.Guid> WarehounseID { get; set; }
        public Nullable<System.Guid> ZoneID { get; set; }
        public Nullable<System.DateTime> CycleCountStartDate { get; set; }
        public Nullable<System.DateTime> CycleCountCompleteDate { get; set; }
        public Nullable<System.DateTime> CycleCountAssignDate { get; set; }
        public Nullable<int> CycleCountStatus { get; set; }
        public virtual ICollection<CycleCountDetail> CycCountDetail { get; set; }
    }
}
