using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Stock.Model
{  
    public class StockBalance
    {
        public Guid StockBalanceID { get; set; }
        public Guid StockInfoID { get; set; }
        public Guid LocationID { get; set; }
        public string PalletCode { get; set; }
        public decimal BaseQuantity { get; set; }
        public decimal ReserveQuantity { get; set; }
        public decimal ConversionQty { get; set; }

        public virtual StockInfo StockInfo { get; set; }
        public virtual Location Location { get; set; }

        public ICollection<StockTransaction> StockTransactionCollection { get; set; }
        public StockBalance()
        {
            BaseQuantity = 0;
            ReserveQuantity = 0;
            ConversionQty = 0;
             StockTransactionCollection = new List<StockTransaction>();
        }
    }
}
