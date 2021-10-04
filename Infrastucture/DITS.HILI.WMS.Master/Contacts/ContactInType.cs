using System;

namespace DITS.HILI.WMS.MasterModel.Contacts
{
    public class ContactInType
    {
        public Guid ContactID { get; set; }
        public ContactType ContactType { get; set; }

        public string CusContactName { get; set; } // CusContranctName (length: 50)
        public string CusContactTel { get; set; } // CusContractTel (length: 20)
        public string CusContactEmail { get; set; } // CusContractEmail (length: 50)
        public string CusContactMobile { get; set; } // CusContractMobile (length: 20)

        public virtual Contact Contact { get; set; }

        public ContactInType()
        {
        }
    }
}
