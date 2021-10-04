using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Utility
{
    public class ISONumber : BaseEntity
    {
        public Guid ISO_Id { get; set; }
        public string ISO_Number { get; set; }
        public string ISO_EffectiveDate { get; set; }
        public string DocumentName { get; set; }
        public bool? IsReport { get; set; }
        public bool? IsForm { get; set; }
        [NotMapped]
        public DateTime? EffectiveDate { get; set; }
    }
}
