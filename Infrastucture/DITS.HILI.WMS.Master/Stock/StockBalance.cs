using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Stock
{
    public class StockBalance : BaseEntity
    {
        public Guid StockInfoID { get; set; }
        //public string PalletCode { get; set; }
        public decimal? StockQuantity { get; set; }
        public decimal BaseQuantity { get; set; }
        public decimal ReserveQuantity { get; set; }
        public decimal ConversionQty { get; set; }
        public Guid StockBalanceID { get; set; }


        public virtual StockInfo StockInfo { get; set; }


        public virtual ICollection<StockLocationBalance> StockLocationBalanceCollection { get; set; }

        public StockBalance()
        {
            StockLocationBalanceCollection = new List<StockLocationBalance>();

        }
    }
}
