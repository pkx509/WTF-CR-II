using DITS.HILI.WMS.MasterModel.Companies;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Stock;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Contacts
{
    public class ProductOwner : BaseEntity
    {
        public Guid ProductOwnerID { get; set; }
        public Guid? BranchID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual ICollection<ProductOwnerAddress> AddressCollection { get; set; }
        public virtual ICollection<CustomerOfProductOwner> CustomerOfProductOwnerCollection { get; set; }
        public virtual ICollection<SupplierOfProductOwner> SupplierOfProductOwnerCollection { get; set; }
        public virtual ICollection<ProductGroupLevel1> ProductGroupLevel1Collection { get; set; }
        public virtual ICollection<ProductGroupLevel2> ProductGroupLevel2Collection { get; set; }
        public virtual ICollection<ProductGroupLevel3> ProductGroupLevel3Collection { get; set; }
        // public virtual ICollection<UserAccounts> UserAccountCollection { get; set; }
        public virtual ICollection<ProductOfProductOwner> ProductOfProductOwnerCollection { get; set; }
        public virtual ICollection<ProductSafetyStock> ProductSafetyStockCollection { get; set; }
        public virtual ICollection<ProductReplacement> ProductReplacementCollection { get; set; }
        public virtual ICollection<StockInfo> StockInfoCollection { get; set; }
        public ProductOwner()
        {
            AddressCollection = new List<ProductOwnerAddress>();
            CustomerOfProductOwnerCollection = new List<CustomerOfProductOwner>();
            SupplierOfProductOwnerCollection = new List<SupplierOfProductOwner>();
            ProductGroupLevel1Collection = new List<ProductGroupLevel1>();
            ProductGroupLevel2Collection = new List<ProductGroupLevel2>();
            ProductGroupLevel3Collection = new List<ProductGroupLevel3>();
            ///UserAccountCollection = new List<UserAccounts>();
            ProductOfProductOwnerCollection = new List<ProductOfProductOwner>();
            ProductReplacementCollection = new List<ProductReplacement>();
            StockInfoCollection = new List<StockInfo>();
        }
    }
}
