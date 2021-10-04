using System;

namespace DITS.HILI.WMS.MasterModel.Contacts
{
    public class CustomerOfProductOwner
    {
        public Guid ProductOwnerID { get; set; }
        public Guid CustomerID { get; set; }

        public virtual ProductOwner ProductOwner { get; set; }
        public virtual Contact Contact { get; set; }


        public CustomerOfProductOwner()
        {
        }
    }
}
