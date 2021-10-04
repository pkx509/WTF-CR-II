using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Secure
{
    public class Roles : BaseEntity
    {
        public Guid RoleID { get; set; }
        //public UserLevel Level { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<PermissionInRole> PermissionInRoleCollection { get; set; }
        public virtual ICollection<UserInRole> UserInRoleCollection { get; set; }
        public virtual ICollection<RoleInWarehouse> RoleInWarehouseCollection { get; set; }
        public Roles()
        {
            RoleInWarehouseCollection = new List<RoleInWarehouse>();
            PermissionInRoleCollection = new List<PermissionInRole>();
            UserInRoleCollection = new List<UserInRole>();
        }
    }


}
