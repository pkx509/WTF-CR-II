using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.ReceiveModel
{
    public class ReceiveDetail : BaseProductProfile
    {
        public Guid ReceiveDetailID { get; set; }
        public Guid ReceiveID { get; set; }
        public int Sequence { get; set; }
        public decimal? Price { get; set; }
        public Guid? ProductUnitPriceID { get; set; }

        public decimal Quantity { get; set; }
        public decimal BaseQuantity { get; set; }
        public decimal ConversionQty { get; set; }
        public Guid StockUnitID { get; set; }
        public Guid BaseUnitID { get; set; }
        public bool? IsSentInterface { get; set; }

        [NotMapped]
        public string ProductName { get; set; }
        [NotMapped]
        public string StockUOMNmae { get; set; }
        [NotMapped]
        public string BaseUOMName { get; set; }

        [NotMapped]
        public virtual ProductUnit ProductPriceUOM { get; set; }
        [NotMapped]
        public virtual ProductUnit ProductUOM { get; set; }
        [NotMapped]
        public virtual ProductUnit ProductBaseUOM { get; set; }

        public ReceiveDetailStatusEnum ReceiveDetailStatus { get; set; }

        public virtual Receive Receive { get; set; }

        public virtual ICollection<Receiving> ReceivingCollection { get; set; }

        public ReceiveDetail()
        {
            ReceivingCollection = new List<Receiving>();
            Product = new Product();
            ProductPriceUOM = new ProductUnit();
            ProductUOM = new ProductUnit();
            ProductBaseUOM = new ProductUnit();
            ProductStatus = new ProductStatus();
            ProductSubStatus = new ProductSubStatus();
        }
    }
}
