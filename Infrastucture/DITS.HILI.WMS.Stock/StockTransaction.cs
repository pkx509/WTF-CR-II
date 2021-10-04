using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Stock.Model
{ 
    public class StockTransaction : BaseEntity
    {
        public Guid StockTransID { get; set; }
        public Guid StockBalanceID { get; set; }
        public Guid DocumentID { get; set; }
        public Guid PackageID { get; set; }
        public StockTransactionTypeEnum StockTransactionType { get; set; }
        public Guid LocationID { get; set; }
        public string PalletCode { get; set; }
        public decimal BaseQuantity { get; set; } 
        public decimal ConversionQty { get; set; }

        public virtual StockBalance StockBalance { get; set; }
        public virtual Location Location { get; set; }
    }
}
