using System;

namespace DITS.HILI.WMS.MasterModel.Interface
{
    public partial class itf_temp_reclassified
    {
        public decimal CONO { get; set; }
        public string GEDT { get; set; }
        public string GETM { get; set; }
        public string CTST { get; set; }
        public string EMSG { get; set; }
        public string WMSORN { get; set; }
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
        public string ITNO { get; set; }
        public string BANO { get; set; }
        public Nullable<decimal> QLQT { get; set; }
        public string QLDT { get; set; }
        public string NITN { get; set; }
        public string NBAN { get; set; }
        public string RSCD { get; set; }
        public string STAS { get; set; }
        public string RESP { get; set; }
        public string ITUOM { get; set; }
        public Nullable<decimal> NQLQT { get; set; }
        public string QA_Code { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public string Pallet_Tag { get; set; }
        public string Lot_number { get; set; }
        public Nullable<int> INS_QTY { get; set; }
        public string Product_system_code { get; set; }
        public string WHLO { get; set; }
        public Nullable<System.Guid> DMFromWarehouseID { get; set; }
        public bool? IsSentInterface { get; set; }
    }
}
