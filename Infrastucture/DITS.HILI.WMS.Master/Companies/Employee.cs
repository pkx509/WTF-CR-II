using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Companies
{
    public class Employee : BaseEntity
    {
        public Guid EmployeeID { get; set; }
        //public Guid? GroupID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserAccounts> UserAccountCollection { get; set; }
        public Employee()
        {
            UserAccountCollection = new List<UserAccounts>();
        }
    }
}
