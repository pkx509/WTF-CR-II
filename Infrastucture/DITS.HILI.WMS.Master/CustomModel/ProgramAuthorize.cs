using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Secure
{
    [NotMapped]
    public class ProgramAuthorize
    {
        public Guid AppID { get; set; }
        public Guid? ParentID { get; set; }
        public Guid? NodeID { get; set; }
        public Guid? ProgramID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string Icon { get; set; }
        public int? Sequence1 { get; set; }
        public int? Sequence2 { get; set; }
        //public NodeType? NodeType { get; set; }
        public virtual Program Program { get; set; }
        public bool IsAuthorize { get; set; }
    }
}
