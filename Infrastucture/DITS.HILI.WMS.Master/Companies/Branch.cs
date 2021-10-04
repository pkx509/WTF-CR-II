using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Companies
{
    public class Branch : BaseEntity
    {
        public Branch()
        {
            ProductOfProductOwnerCollection = new List<ProductOfProductOwner>();
            ProductOwnerCollection = new List<ProductOwner>();
        }

        public Guid BranchID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public Guid BranchTypeID { get; set; }
        public string Location { get; set; }

        public virtual BranchType BranchType { get; set; }
        public virtual ICollection<ProductOfProductOwner> ProductOfProductOwnerCollection { get; set; }
        public virtual ICollection<ProductOwner> ProductOwnerCollection { get; set; }


    }
}
