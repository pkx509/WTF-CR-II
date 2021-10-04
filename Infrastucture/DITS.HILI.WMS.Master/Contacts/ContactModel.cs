using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Contacts
{
    public class ContactModel : BaseEntity
    {
        public Guid ContactID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Sequence { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string WebSite { get; set; }
        public string Email { get; set; }
        public ContactType ContactType { get; set; }

        public ICollection<ContactTypeModel> ContactTypeCollection { get; set; }

        public string CusContactName { get; set; } // CusContranctName (length: 50)
        public string CusContactTel { get; set; } // CusContractTel (length: 20)
        public string CusContactEmail { get; set; } // CusContractEmail (length: 50)
        public string CusContactMobile { get; set; } // CusContractMobile (length: 20)

    }
}
