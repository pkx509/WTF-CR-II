using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Stock
{
    public class StockInfo : BaseEntity
    {
        public Guid StockInfoID { get; set; }
        public Guid ProductOwnerID { get; set; }
        public Guid SupplierID { get; set; }
        public Guid ProductID { get; set; }
        public string Lot { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public decimal ConversionQty { get; set; }
        public Guid StockUnitID { get; set; }
        public Guid BaseUnitID { get; set; }
        public Guid? ProductUnitPriceID { get; set; }
        public decimal? Price { get; set; }
        public Guid ProductStatusID { get; set; }
        public Guid ProductSubStatusID { get; set; }
        public double PackageWeight { get; set; }
        public double ProductWeight { get; set; }
        public double ProductWidth { get; set; }
        public double ProductLength { get; set; }
        public double ProductHeight { get; set; }


        public virtual ICollection<StockBalance> StockBalanceCollection { get; set; }
        [NotMapped]
        public virtual Contact Supplier { get; set; }
        [NotMapped]
        public virtual Product Product { get; set; }
        [NotMapped]
        public virtual ProductStatus ProductStatus { get; set; }
        [NotMapped]
        public virtual ProductSubStatus ProductSubStatus { get; set; }
        [NotMapped]
        public virtual ProductUnit ProductUOM { get; set; }
        [NotMapped]
        public virtual ProductUnit ProductBaseUOM { get; set; }
        [NotMapped]
        public virtual ProductUnit ProductPriceUOM { get; set; }
        [NotMapped]
        public virtual ProductOwner ProductOwner { get; set; }




        public StockInfo()
        {
            StockBalanceCollection = new List<StockBalance>();
        }
    }
}
