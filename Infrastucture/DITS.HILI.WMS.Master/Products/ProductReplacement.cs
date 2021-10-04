using DITS.HILI.WMS.MasterModel.Contacts;
using System;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class ProductReplacement : BaseEntity
    {
        public Guid ProductID { get; set; }
        public Guid ProductOwnerID { get; set; }
        public Guid ReplaceProductID { get; set; }
        public int Sequence { get; set; }
        public double Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual Product ProductReplace { get; set; }
        public virtual ProductOwner ProductOwner { get; set; }

        public ProductReplacement()
        {
        }
    }
}
