using DITS.HILI.WMS.MasterModel.Stock;
using System;

namespace DITS.HILI.WMS.MasterModel.CustomModel
{
    public class StockInOutModel
    {
        public Guid ProductID { get; set; }
        public Guid StockUnitID { get; set; }
        public Guid BaseUnitID { get; set; }
        public decimal? Price { get; set; }
        public Guid? ProductUnitPriceID { get; set; }
        public Guid SupplierID { get; set; }
        public Guid ProductOwnerID { get; set; }
        public Guid ProductStatusID { get; set; }
        public Guid ProductSubStatusID { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal ConversionQty { get; set; }
        public decimal Quantity { get; set; }
        public string Lot { get; set; }
        public string PalletCode { get; set; }
        public string NewPalletCode { get; set; }
        public double ProductWidth { get; set; }
        public double ProductLength { get; set; }
        public double ProductHeight { get; set; }
        public double ProductWeight { get; set; }
        public double PackageWeight { get; set; }
        public string LocationCode { get; set; }
        public Guid? LocationID { get; set; }
        public string DocumentCode { get; set; }
        public Guid DocumentID { get; set; }
        public Guid? DocumentTypeID { get; set; }
        public int PackingStatus { get; set; }
        public decimal? ReserveQuantity { get; set; }

        /// <summary>
        /// default = 0
        /// </summary>
        public StockTransactionTypeEnum StockTransTypeEnum { get; set; }
        public string Remark { get; set; }

        public string FromLocationCode { get; set; }

        public StockInOutModel()
        {
            StockTransTypeEnum = new StockTransactionTypeEnum();
        }

    }

    public class StockSearch
    {
        public Guid ProductID { get; set; }
        public Guid StockUnitID { get; set; }
        public Guid BaseUnitID { get; set; }
        public decimal ConversionQty { get; set; }
        public string Lot { get; set; }
        public Guid? ProductOwnerID { get; set; }
        public Guid ProductStatusID { get; set; }
        public Guid? SupplierID { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal QTY { get; set; }

        public Guid? LocationID { get; set; }
        public string LocationCode { get; set; }
    }

    public class StockInternalRecModel : StockInOutModel
    {
        public string Pallet_I_Code { get; set; }
    }
}
