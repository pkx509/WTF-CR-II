using DITS.HILI.WMS.MasterModel.Stock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Contacts
{
    public class Contact : BaseEntity
    {
        public Guid ContactID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Telephone { get; set; } // Telephone (length: 16)
        public string Fax { get; set; } // Fax (length: 50)
        public string Email { get; set; } // Email (length: 50)

        [NotMapped]
        public int Sequence { get; set; }
        [NotMapped]
        public string Address { get; set; }
        [NotMapped]
        public string PostCode { get; set; }
        [NotMapped]
        public string WebSite { get; set; }
        [NotMapped]
        public ContactType ContactType { get; set; }
        [NotMapped]
        public bool IsCustomer { get; set; }
        [NotMapped]
        public bool IsSupplier { get; set; }
        [NotMapped]
        public ICollection<ContactTypeModel> ContactTypeCollection { get; set; }
        [NotMapped]
        public ICollection<ProductOwner> ProductOwnerCollection { get; set; }

        [NotMapped]
        public string CusContactName { get; set; } // CusContranctName (length: 50)
        [NotMapped]
        public string CusContactTel { get; set; } // CusContractTel (length: 20)
        [NotMapped]
        public string CusContactEmail { get; set; } // CusContractEmail (length: 50)
        [NotMapped]
        public string CusContactMobile { get; set; } // CusContractMobile (length: 20)

        [NotMapped]
        public Guid SubDistrict_Id { get; set; }
        [NotMapped]
        public string SubDistrictName { get; set; }
        [NotMapped]
        public Guid District_Id { get; set; }
        [NotMapped]
        public string DistrictName { get; set; }
        [NotMapped]
        public Guid Province_Id { get; set; }
        [NotMapped]
        public string ProvinceName { get; set; }

        [NotMapped]
        public string FullName { get; set; }

        public virtual ICollection<ContactAddress> AddressCollection { get; set; }
        public virtual ICollection<ContactInType> ContactInTypeCollection { get; set; }

        public virtual ICollection<StockInfo> StockInfoCollection { get; set; }

        public virtual ICollection<CustomerOfProductOwner> CustomerOfProductOwnerCollection { get; set; }
        public virtual ICollection<SupplierOfProductOwner> SupplierOfProductOwnerCollection { get; set; }
        public Contact()
        {
            AddressCollection = new List<ContactAddress>();
            ContactInTypeCollection = new List<ContactInType>();
            StockInfoCollection = new List<StockInfo>();
        }
    }
}
