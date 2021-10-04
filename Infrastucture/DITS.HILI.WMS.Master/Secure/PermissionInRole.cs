using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Secure
{
    public class PermissionInRole : BaseEntity
    {
        public Guid PermissionID { get; set; }
        public Guid RoleID { get; set; }

        [NotMapped]
        public string PermissionName { get; set; }
        [NotMapped]
        public bool IsPermission { get; set; }

        public virtual Permission Permissions { get; set; }
        public virtual Roles Role { get; set; }
    }
}
