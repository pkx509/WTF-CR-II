﻿using System;

namespace DITS.HILI.WMS.MasterModel.Interface
{
    public partial class itf_temp_in_dispatch
    {
        public System.Guid TransactionID { get; set; }
        public decimal CONO { get; set; }
        public string ORNO { get; set; }
        public string CUNO { get; set; }
        public string FACI { get; set; }
        public string WHLO { get; set; }
        public string ITNO { get; set; }
        public string ITDS { get; set; }
        public string DWDT { get; set; }
        public Nullable<decimal> ORQT { get; set; }
        public string ALUN { get; set; }
        public Nullable<decimal> SAPR { get; set; }
        public string SPUN { get; set; }
        public Nullable<decimal> DIA1 { get; set; }
        public Nullable<decimal> DIA2 { get; set; }
        public Nullable<decimal> DIA3 { get; set; }
        public Nullable<decimal> DIA4 { get; set; }
        public Nullable<decimal> DIA5 { get; set; }
        public Nullable<decimal> DIA6 { get; set; }
        public Nullable<decimal> DIP1 { get; set; }
        public Nullable<decimal> DIP2 { get; set; }
        public Nullable<decimal> DIP3 { get; set; }
        public Nullable<decimal> DIP4 { get; set; }
        public Nullable<decimal> DIP5 { get; set; }
        public Nullable<decimal> DIP6 { get; set; }
        public string CUOR { get; set; }
        public string DWDZ { get; set; }
        public string DWHZ { get; set; }
        public string RSCD { get; set; }
        public string BANO { get; set; }
        public string WHSL { get; set; }
        public string ORTP { get; set; }
        public string RLDT { get; set; }
        public string MODL { get; set; }
        public string TEDL { get; set; }
        public string YREF { get; set; }
        public string TEPY { get; set; }
        public string PYNO { get; set; }
        public string ADID { get; set; }
        public string OREF { get; set; }
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
        public Nullable<decimal> Sync_UnsuccessNo { get; set; }
        public string Sync_Flag { get; set; }
        public string Sync_Date { get; set; }
        public string Product_System_Code { get; set; }
        public string Product_Name_Full { get; set; }
        public Nullable<System.DateTime> Dispatch_Date_Delivery { get; set; }
        public Nullable<decimal> DispatchDetail_Product_Quantity { get; set; }
        public Nullable<System.Guid> Product_Price_UOM_ID { get; set; }
        public Nullable<decimal> DispatchDetail_Product_Price { get; set; }
        public string Document_No { get; set; }
        public Nullable<System.DateTime> Dispatch_Date_Order { get; set; }
        public string SubCust_Code { get; set; }
        public string Dispatch_Code { get; set; }
        public string GEDT { get; set; }
        public string GETM { get; set; }
        public string TWHL { get; set; }
        public string TWSL { get; set; }
        public Nullable<decimal> DLQT { get; set; }
        public Nullable<decimal> ALQT { get; set; }
        public string RIDN { get; set; }
        public Nullable<decimal> RIDL { get; set; }
        public Nullable<decimal> RIDX { get; set; }
        public Nullable<decimal> RIDI { get; set; }
        public Nullable<decimal> PLSX { get; set; }
        public Nullable<decimal> DLIX { get; set; }
        public string TRTP { get; set; }
        public string TOFP { get; set; }
        public string RESP { get; set; }
        public string RPDT { get; set; }
        public string RPTM { get; set; }
        public string ITUOM { get; set; }
        public Nullable<System.Guid> DISPATCHTYPEID { get; set; }
        public bool? IsSentInterface { get; set; }
    }
}