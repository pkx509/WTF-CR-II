using DITS.HILI.WMS.MasterModel.Companies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Secure
{

    public class UserAccounts : BaseEntity
    {
        public Guid UserID { get; set; }
        public Guid EmployeeID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }

        public virtual Employee Employee { get; set; }

        [NotMapped]
        public string FirstName { get; set; }
        [NotMapped]
        public string LastName { get; set; }
        [NotMapped]
        public string Email { get; set; }

        public virtual ICollection<UserInGroup> UserInGroupCollection { get; set; }
        public virtual ICollection<UserInRole> UserInRoleCollection { get; set; }

        public UserAccounts()
        {
            UserInGroupCollection = new List<UserInGroup>();
            UserInRoleCollection = new List<UserInRole>();
        }
    }
}
