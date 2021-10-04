using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Secure
{
    public class Permission : BaseEntity
    {
        public Guid PermissionID { get; set; }
        public string Action { get; set; }
        public int Sequent { get; set; }
        public virtual ICollection<PermissionInRole> PermissionInRoleCollection { get; set; }

        public Permission()
        {
            PermissionInRoleCollection = new List<PermissionInRole>();
        }
    }
}
