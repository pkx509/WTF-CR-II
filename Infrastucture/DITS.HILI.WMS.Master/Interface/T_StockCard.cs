using System;

namespace DITS.HILI.WMS.MasterModel.Interface
{
    public partial class T_StockCard
    {
        public System.DateTime TransactionDT { get; set; }
        public System.Guid ProductID { get; set; }
        public Nullable<System.Guid> DocumentID { get; set; }
        public int TransactionType { get; set; }
        public string ORTP { get; set; }
        public decimal BalaceBF { get; set; }
        public string DocumentNo { get; set; }
        public Nullable<decimal> ReceiveQty { get; set; }
        public Nullable<decimal> DispatchQty { get; set; }
        public decimal BalanceQty { get; set; }
        public System.Guid StockUnitID { get; set; }
        public System.Guid BaseUnitID { get; set; }
        public decimal TotalAmount { get; set; }
        public string PONO { get; set; }
        public string LotNo { get; set; }
        public Nullable<System.Guid> ShipToID { get; set; }
        public Nullable<System.Guid> LineID { get; set; }
    }
}
