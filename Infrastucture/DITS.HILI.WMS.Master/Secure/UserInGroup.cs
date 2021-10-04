using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Secure
{
    public class UserInGroup : BaseEntity
    {
        public Guid UserID { get; set; }
        public Guid GroupID { get; set; }
        public virtual UserAccounts UserAccount { get; set; }
        public virtual UserGroups UserGroup { get; set; }

        [NotMapped]
        public bool IsGroup { get; set; }

        [NotMapped]
        public string GroupName { get; set; }
    }
}
