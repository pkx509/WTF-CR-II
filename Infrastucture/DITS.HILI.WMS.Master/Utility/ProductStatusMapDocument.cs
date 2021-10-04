using System;

namespace DITS.HILI.WMS.MasterModel.Utility
{
    public class ProductStatusMapDocument : BaseEntity
    {
        public Guid ProductStatusID { get; set; }
        public Guid DocumentTypeID { get; set; }
        public bool IsDefault { get; set; }


        public virtual ProductStatus ProductStatus { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public ProductStatusMapDocument()
        { }
    }
}
