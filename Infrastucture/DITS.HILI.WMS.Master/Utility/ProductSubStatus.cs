using DITS.HILI.WMS.MasterModel.Stock;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Utility
{
    public class ProductSubStatus : BaseEntity
    {
        public Guid ProductSubStatusID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }

        public virtual ICollection<ProductStatusMap> ProductStatusMapCollection { get; set; }
        public virtual ICollection<StockInfo> StockInfoCollection { get; set; }

        public ProductSubStatus()
        {
            ProductStatusMapCollection = new List<ProductStatusMap>();
            StockInfoCollection = new List<StockInfo>();
        }
    }
}
