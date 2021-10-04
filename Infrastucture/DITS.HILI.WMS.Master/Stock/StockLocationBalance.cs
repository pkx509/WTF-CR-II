using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Stock
{
    public class StockLocationBalance : BaseEntity
    {
        public StockLocationBalance()
        {
            StockTransactionCollection = new List<StockTransaction>();
        }

        public Guid StockLocationID { get; set; }
        public Guid? StockBalanceID { get; set; }
        public Guid? WarehouseID { get; set; }
        public decimal? BaseQuantity { get; set; }
        public decimal? StockQuantity { get; set; }
        public decimal? ReserveQuantity { get; set; }
        public Guid? ZoneID { get; set; }

        public virtual StockBalance StockBalance { get; set; }


        public virtual ICollection<StockTransaction> StockTransactionCollection { get; set; }
    }
}
