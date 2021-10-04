using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.ProductionControlModel
{
    public class ProductionControl : BaseEntity
    {
        public Guid ControlID { get; set; }
        public DateTime ProductionDate { get; set; }
        public TimeSpan ProductionTime { get; set; }
        public string Lot { get; set; }
        public Guid LineID { get; set; }
        public Guid ProductID { get; set; }
        public Guid ProductUnitID { get; set; }
        public decimal Quantity { get; set; }
        public Guid? BaseUnitID { get; set; }
        public decimal? ConversionQty { get; set; }
        public Guid? StockUnitID { get; set; }
        public string OrderType { get; set; }
        public string OrderNo { get; set; }
        public int PcControlStatus { get; set; }
        public Guid? ReferenceID { get; set; }
        public Guid? PackageID { get; set; }
        public decimal StandardPalletQty { get; set; }
        public Guid? ProductStatusID { get; set; }
        public Guid? ProductSubStatusID { get; set; }

        public virtual ICollection<ProductionControlDetail> PCDetailCollection { get; set; }

        public ProductionControl()
        {
            PCDetailCollection = new List<ProductionControlDetail>();
        }
    }
}
