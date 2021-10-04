using DITS.HILI.WMS.MasterModel.Warehouses;
using System;

namespace DITS.HILI.WMS.MasterModel.Stock
{
    public class StockTransaction : BaseEntity
    {
        public Guid StockTransactionID { get; set; }
        public Guid DocumentID { get; set; }
        public Guid? PackageID { get; set; }
        public StockTransactionTypeEnum StockTransType { get; set; }
        public Guid LocationID { get; set; }
        public string PalletCode { get; set; }
        public decimal BaseQuantity { get; set; }
        public decimal ConversionQty { get; set; }
        public Guid? StockLocationID { get; set; }
        public string DocumentCode { get; set; }
        public Guid? DocumentTypeID { get; set; }

        public bool? IsStockNonCalculate { get; set; }

        public virtual StockLocationBalance StockLocationBalance { get; set; }


        public virtual Location Location { get; set; }

    }
}
