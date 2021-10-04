using System;

namespace DITS.HILI.WMS.MasterModel.Interface
{
    public partial class itf_temp_AddStock_adjust
    {
        public System.Guid TransactionID { get; set; }
        public decimal CONO { get; set; }
        public string WHLO { get; set; }
        public string GEDT { get; set; }
        public string GETM { get; set; }
        public string Product_system_code { get; set; }
        public string WHSL { get; set; }
        public string TWHL { get; set; }
        public string TWSL { get; set; }
        public string BANO { get; set; }
        public Nullable<decimal> ALQT { get; set; }
        public Nullable<decimal> DLQT { get; set; }
        public string RIDN { get; set; }
        public Nullable<decimal> RIDL { get; set; }
        public Nullable<decimal> RIDX { get; set; }
        public Nullable<decimal> RIDI { get; set; }
        public Nullable<decimal> PLSX { get; set; }
        public Nullable<decimal> DLIX { get; set; }
        public string TRTP { get; set; }
        public string TOFP { get; set; }
        public string RESP { get; set; }
        public string RSCD { get; set; }
        public string RPDT { get; set; }
        public string RPTM { get; set; }
        public string WMSORN { get; set; }
        public string ITUOM { get; set; }
        public string GDATE { get; set; }
        public string GTIME { get; set; }
        public string GSTT { get; set; }
        public string RNOM3 { get; set; }
        public string FDATE { get; set; }
        public string FTIME { get; set; }
        public string FSTT { get; set; }
        public Nullable<decimal> Sync_UnsuccessNo { get; set; }
        public string Sync_Flag { get; set; }
        public string Sync_Date { get; set; }
        public string PalletCode { get; set; }
        public Nullable<System.Guid> ReferenceID { get; set; }
        public Nullable<System.Guid> ProductID { get; set; }
        public string Lot_Number { get; set; }
        public Nullable<int> Confirm_Qty { get; set; }
        public Nullable<System.DateTime> MFG_Date { get; set; }
        public Nullable<System.TimeSpan> MFG_TimeEnd { get; set; }
        public string Product_Uom_Name { get; set; }
        public string TranRea { get; set; }
        public bool? IsSentInterface { get; set; }
    }
}
