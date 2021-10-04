using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.ReceiveModel
{
    public class Receiving : BaseProductProfile
    {
        public Guid ReceivingID { get; set; }
        public string GRNCode { get; set; }
        public Guid ReceiveID { get; set; }
        public int Sequence { get; set; }
        public Guid ReceiveDetailID { get; set; }
        public decimal? Price { get; set; }
        public Guid? ProductUnitPriceID { get; set; }

        public decimal Quantity { get; set; }
        public decimal BaseQuantity { get; set; }
        public decimal ConversionQty { get; set; }
        public Guid StockUnitID { get; set; }
        public Guid BaseUnitID { get; set; }
        public Guid? LocationID { get; set; }
        public bool? IsSentInterface { get; set; }


        [NotMapped]
        public string ProductName { get; set; }
        [NotMapped]
        public string StockUOMName { get; set; }
        [NotMapped]
        public string BaseUOMName { get; set; }

        [NotMapped]
        public virtual ProductUnit ProductPriceUOM { get; set; }
        [NotMapped]
        public virtual ProductUnit ProductUOM { get; set; }
        [NotMapped]
        public virtual ProductUnit ProductBaseUOM { get; set; }

        [NotMapped]
        public virtual Location Location { get; set; }

        public bool IsDraft { get; set; }

        public ReceivingStatusEnum ReceivingStatus { get; set; }

        public virtual ReceiveDetail ReceiveDetail { get; set; }



        public Receiving()
        {
            Product = new Product();
            ProductPriceUOM = new ProductUnit();
            ProductUOM = new ProductUnit();
            ProductBaseUOM = new ProductUnit();
            ProductStatus = new ProductStatus();
            ProductSubStatus = new ProductSubStatus();
            Location = new Location(); ;
            ProductOwner = new ProductOwner();
        }

    }
}
