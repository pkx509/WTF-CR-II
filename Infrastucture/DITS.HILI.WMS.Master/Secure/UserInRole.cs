using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Secure
{
    public class UserInRole : BaseEntity
    {
        public Guid RoleID { get; set; }
        public Guid UserID { get; set; }
        public virtual Roles Role { get; set; }
        public virtual UserAccounts UserAccount { get; set; }

        [NotMapped]
        public string RoleName { get; set; }
        [NotMapped]
        public bool IsRole { get; set; }
    }
}
