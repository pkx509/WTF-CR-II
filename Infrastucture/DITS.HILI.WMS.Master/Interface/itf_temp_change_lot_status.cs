using System;

namespace DITS.HILI.WMS.MasterModel.Interface
{
    public partial class itf_temp_change_status
    {
        public Guid TransactionID { get; set; }

        public decimal CONO { get; set; }
        public string GEDT { get; set; }
        public string GETM { get; set; }
        public string WHLO { get; set; }
        public string WHSL { get; set; }
        public string ITNO { get; set; }
        public string BANO { get; set; }
        public decimal QLQT { get; set; }
        public string QLDT { get; set; }
        public string NITN { get; set; }
        public string NBAN { get; set; }
        public string RSCD { get; set; }
        public string STAS { get; set; }
        public string RESP { get; set; }
        public string CTST { get; set; }
        public string EMSG { get; set; }
        public string WMSORN { get; set; }
        public string ITUOM { get; set; }
        public decimal NQLQT { get; set; }
        public string GDATE { get; set; }
        public string GTIME { get; set; }
        public string GSTT { get; set; }
        public string RNOM3 { get; set; }
        public string FDATE { get; set; }
        public string FTIME { get; set; }
        public string FSTT { get; set; }
        public bool? IsSentInterface { get; set; }

        public Guid RefID { get; set; }
        public string RefType { get; set; }
    }
}
