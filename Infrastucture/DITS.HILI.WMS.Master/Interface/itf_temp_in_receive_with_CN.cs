using System;

namespace DITS.HILI.WMS.MasterModel.Interface
{
    public class itf_temp_in_receive_with_CN
    {
        public Guid TransactionID { get; set; }
        public Guid ReferenceID { get; set; }
        public bool? IsSentInterface { get; set; }

        public decimal CONO { get; set; }
        public decimal? Sync_UnsuccessNo { get; set; }
        public decimal? ORQT { get; set; }
        public decimal? SAPR { get; set; }
        public decimal? DIA1 { get; set; }
        public decimal? DIA2 { get; set; }
        public decimal? DIA3 { get; set; }
        public decimal? DIA4 { get; set; }
        public decimal? DIA5 { get; set; }
        public decimal? DIA6 { get; set; }
        public decimal? DIP1 { get; set; }
        public decimal? DIP2 { get; set; }
        public decimal? DIP3 { get; set; }
        public decimal? DIP4 { get; set; }
        public decimal? DIP5 { get; set; }
        public decimal? DIP6 { get; set; }

        public string CUNO { get; set; }
        public string FACI { get; set; }
        public string WHLO { get; set; }
        public string ORTP { get; set; }
        public string RLDT { get; set; }
        public string MODL { get; set; }
        public string TEDL { get; set; }
        public string YREF { get; set; }
        public string TEPY { get; set; }
        public string PYNO { get; set; }
        public string ADID { get; set; }
        public string OREF { get; set; }
        public string CUOR { get; set; }
        public string CUDT { get; set; }
        public string ORDT { get; set; }
        public string RLDZ { get; set; }
        public string RLHZ { get; set; }
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
        public string Sync_Flag { get; set; }
        public string Sync_Date { get; set; }
        public string ORNO { get; set; }
        public string ITNO { get; set; }
        public string ITDS { get; set; }
        public string DWDT { get; set; }
        public string ALUN { get; set; }
        public string SPUN { get; set; }
        public string DWDZ { get; set; }
        public string DWHZ { get; set; }
        public string RSCD { get; set; }
        public string BANO { get; set; }
        public string WHSL { get; set; }
    }
}
