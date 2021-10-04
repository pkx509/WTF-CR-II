using System;

namespace DITS.HILI.WMS.MasterModel.Contacts
{
    public class ProductOwnerAddress : BaseAddress
    {

        public Guid ProductOwnerID { get; set; }
        public virtual ProductOwner ProductOwner { get; set; }

        public ProductOwnerAddress()
        { }
    }
}
