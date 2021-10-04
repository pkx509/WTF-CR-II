using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Secure
{
    public class ProgramInGroup : BaseEntity
    {
        public Guid GroupID { get; set; }
        public Guid ProgramID { get; set; }
        public virtual Program Programs { get; set; }
        public virtual UserGroups UserGroup { get; set; }


        [NotMapped]
        public string Module { get; set; }

        [NotMapped]
        public bool IsCheck { get; set; }

        [NotMapped]
        public string Description { get; set; }
        [NotMapped]
        public int Sequence { get; set; }
    }
}
