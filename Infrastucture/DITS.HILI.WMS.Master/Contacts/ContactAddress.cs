using System;

namespace DITS.HILI.WMS.MasterModel.Contacts
{
    public class ContactAddress : BaseAddress
    {
        public Guid ContactID { get; set; }

        public virtual Contact Contact { get; set; }

        public ContactAddress()
        {

        }
    }
}
