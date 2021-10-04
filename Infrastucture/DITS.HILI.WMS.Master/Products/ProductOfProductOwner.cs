using DITS.HILI.WMS.MasterModel.Companies;
using DITS.HILI.WMS.MasterModel.Contacts;
using System;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class ProductOfProductOwner : BaseEntity
    {
        public Guid ProductID { get; set; }
        public Guid ProductOwnerID { get; set; }
        public Guid? BranchID { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductOwner ProductOwner { get; set; }

    }
}
