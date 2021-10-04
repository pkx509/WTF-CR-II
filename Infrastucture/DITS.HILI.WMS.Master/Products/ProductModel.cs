using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class ProductModel
    {
        public Guid ProductID { get; set; }
        public Guid? ProductGroupLevel3ID { get; set; }
        public string ProductGroupLevel3Name { get; set; }
        public Guid? ProductGroupLevel2ID { get; set; }
        public string ProductGroupLevel2Name { get; set; }
        public Guid ProductGroupLevel1ID { get; set; }
        //public Guid ProductOwnerID { get; set; }
        public string ProductGroupLevel1Name { get; set; }
        public Guid? ProductShapeID { get; set; }
        public string ProductShapeName { get; set; }
        public Guid? ProductBrandID { get; set; }
        public string ProductBrandName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductModelName { get; set; }
        public DateTime? MFGDate { get; set; }
        public string Description { get; set; }
        public double Age { get; set; }
        public decimal? SafetyStockQTY { get; set; }
        public string URLImage { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public Guid? ProductUnitID { get; set; }
        public string ProductUnitName { get; set; }
        public List<ProductCodes> ProductCodeModel { get; set; }

        //Stock
        public string ProductLot { get; set; }
        public string ProductLocation { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? BaseQuantity { get; set; }
        public System.Guid? BaseUnitId { get; set; }
        public decimal? ConversionQty { get; set; }
        public System.Guid? ProductOwnerId { get; set; }
        public string ProductOwnerName { get; set; }
        public decimal? ProductWidth { get; set; }
        public decimal? ProductLength { get; set; }
        public decimal? ProductHeight { get; set; }
        public decimal? ProductWeight { get; set; }
        public decimal? PackageWeight { get; set; }
        public System.Guid? PriceUnitId { get; set; }
        public string PriceUnitName { get; set; }
        public decimal? Price { get; set; }
        public System.Guid? ProductStatusId { get; set; }
        public string ProductStatusName { get; set; }
        public System.Guid? ProductSubStatusId { get; set; }
        public string ProductSubStatusName { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public Guid? LocationID { get; set; }
        public string Location { get; set; }
        public string PalletCode { get; set; }
        public string WarehouseName { get; set; }
        public string OrderType { get; set; }
        public string OrderNo { get; set; }
        public string LineCode { get; set; }

        public ProductModel()
        {
            ProductCodeModel = new List<ProductCodes>();
        }

    }
}
