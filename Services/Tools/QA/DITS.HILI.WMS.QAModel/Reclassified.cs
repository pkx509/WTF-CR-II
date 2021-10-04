using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.QAModel
{
    public class Reclassified : BaseEntity
    {
        public Reclassified()
        {
            this.ReclassifiedDetailCollection = new List<ReclassifiedDetail>();
        }

        public Guid ReclassifiedID { get; set; }
        public string ReclassifiedCode { get; set; }
        public string ReclassFromLot { get; set; }
        public string ReclassToLot { get; set; }
        public int? ReclassStatus { get; set; }
        public DateTime? ApproveDate { get; set; }
        public Guid? DamageID { get; set; }
        public string Description { get; set; }
        public TimeSpan? MFGTimeFrom { get; set; }
        public TimeSpan? MFGTimeEnd { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? LineID { get; set; }
        public DateTime? MFGDate { get; set; }
        public DateTime? EXPDate { get; set; }

        [NotMapped]
        public virtual ICollection<ReclassifiedDetail> ReclassifiedDetailCollection { get; set; }
    }
}
