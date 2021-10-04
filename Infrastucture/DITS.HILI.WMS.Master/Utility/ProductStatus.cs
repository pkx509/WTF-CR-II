using DITS.HILI.WMS.MasterModel.Stock;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Utility
{
    public class ProductStatus : BaseEntity
    {
        public Guid ProductStatusID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }

        public bool IsChangeStatus { get; set; }

        public bool IsInspectionReclassify { get; set; }
        public bool IsDefault { get; set; }

        public virtual ICollection<ProductStatusMap> ProductStatusMapCollection { get; set; }
        public virtual ICollection<ProductStatusMapDocument> ProductStatusMapDocumentCollection { get; set; }
        public virtual ICollection<StockInfo> StockInfoCollection { get; set; }

        public ProductStatus()
        {
            ProductStatusMapCollection = new List<ProductStatusMap>();
            ProductStatusMapDocumentCollection = new List<ProductStatusMapDocument>();
            StockInfoCollection = new List<StockInfo>();
        }
    }
}
