using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Secure
{
    public class UserGroups : BaseEntity
    {
        public Guid GroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }


        public virtual ICollection<ProgramInGroup> ProgramInGroupCollection { get; set; }
        public virtual ICollection<UserInGroup> UserInGroupCollection { get; set; }


        public UserGroups()
        {
            ProgramInGroupCollection = new List<ProgramInGroup>();
            UserInGroupCollection = new List<UserInGroup>();
        }
    }
}
