using System;

namespace DITS.HILI.WMS.MasterModel.Contacts
{
    public class SupplierOfProductOwner
    {
        public Guid ProductOwnerID { get; set; }
        public Guid SupplierID { get; set; }

        public virtual ProductOwner ProductOwner { get; set; }
        public virtual Contact Contact { get; set; }

        public SupplierOfProductOwner()
        {

        }
    }
}
