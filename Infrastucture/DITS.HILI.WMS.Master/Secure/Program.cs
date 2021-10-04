using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Secure
{
    public class Program : BaseEntity
    {
        public Guid ProgramID { get; set; }
        public Guid? AppID { get; set; }
        public string Code { get; set; }
        //public string Name { get; set; }
        [NotMapped]
        public string Description { get; set; }


        [NotMapped]
        public string ParentDescription { get; set; }
        [NotMapped]
        public string ParenCode { get; set; }

        public int Sequence { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public ProgramType? ProgramType { get; set; }
        public Guid? ParentID { get; set; }

        public virtual ICollection<ProgramInGroup> ProgramInGroupCollection { get; set; }
        public virtual ICollection<ProgramValue> ProgramValueCollection { get; set; }

        public virtual Application Application { get; set; }
        public Program()
        {
            ProgramValueCollection = new List<ProgramValue>();
            ProgramInGroupCollection = new List<ProgramInGroup>();
        }

    }
}
