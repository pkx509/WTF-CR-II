namespace DITS.HILI.WMS.MasterModel.Interface
{
    public partial class itf_temp_in_dispatch_log
    {
        public decimal Cono { get; set; } // CONO
        public string Orno { get; set; } // ORNO (length: 10)
        public string Cuno { get; set; } // CUNO (length: 10)
        public string Faci { get; set; } // FACI (length: 3)
        public string Whlo { get; set; } // WHLO (length: 3)
        public string Itno { get; set; } // ITNO (length: 15)
        public string Itds { get; set; } // ITDS (length: 30)
        public string Dwdt { get; set; } // DWDT (length: 10)
        public decimal? Orqt { get; set; } // ORQT
        public string Alun { get; set; } // ALUN (length: 3)
        public decimal? Sapr { get; set; } // SAPR
        public string Spun { get; set; } // SPUN (length: 3)
        public decimal? Dia1 { get; set; } // DIA1
        public decimal? Dia2 { get; set; } // DIA2
        public decimal? Dia3 { get; set; } // DIA3
        public decimal? Dia4 { get; set; } // DIA4
        public decimal? Dia5 { get; set; } // DIA5
        public decimal? Dia6 { get; set; } // DIA6
        public decimal? Dip1 { get; set; } // DIP1
        public decimal? Dip2 { get; set; } // DIP2
        public decimal? Dip3 { get; set; } // DIP3
        public decimal? Dip4 { get; set; } // DIP4
        public decimal? Dip5 { get; set; } // DIP5
        public decimal? Dip6 { get; set; } // DIP6
        public string Cuor { get; set; } // CUOR (length: 20)
        public string Dwdz { get; set; } // DWDZ (length: 10)
        public string Dwhz { get; set; } // DWHZ (length: 4)
        public string Rscd { get; set; } // RSCD (length: 3)
        public string Bano { get; set; } // BANO (length: 12)
        public string Whsl { get; set; } // WHSL (length: 10)
        public string Ortp { get; set; } // ORTP (length: 3)
        public string Rldt { get; set; } // RLDT (length: 10)
        public string Modl { get; set; } // MODL (length: 3)
        public string Tedl { get; set; } // TEDL (length: 3)
        public string Yref { get; set; } // YREF (length: 30)
        public string Tepy { get; set; } // TEPY (length: 3)
        public string Pyno { get; set; } // PYNO (length: 10)
        public string Adid { get; set; } // ADID (length: 6)
        public string Oref { get; set; } // OREF (length: 30)
        public string Cudt { get; set; } // CUDT (length: 10)
        public string Ordt { get; set; } // ORDT (length: 10)
        public string Rldz { get; set; } // RLDZ (length: 10)
        public string Rlhz { get; set; } // RLHZ (length: 4)
        public string Ctst { get; set; } // CTST (length: 5)
        public string Emsg { get; set; } // EMSG (length: 200)
        public string Wmsorn { get; set; } // WMSORN (length: 15)
        public string Gdate { get; set; } // GDATE (length: 10)
        public string Gtime { get; set; } // GTIME (length: 6)
        public string Gstt { get; set; } // GSTT (length: 1)
        public string Rnom3 { get; set; } // RNOM3 (length: 10)
        public string Fdate { get; set; } // FDATE (length: 10)
        public string Ftime { get; set; } // FTIME (length: 6)
        public string Fstt { get; set; } // FSTT (length: 1)
        public decimal? SyncUnsuccessNo { get; set; } // Sync_UnsuccessNo
        public string SyncFlag { get; set; } // Sync_Flag (length: 1)
        public string SyncDate { get; set; } // Sync_Date (length: 20)
        public string ProductSystemCode { get; set; } // Product_System_Code (length: 20)
        public string ProductNameFull { get; set; } // Product_Name_Full (length: 500)
        public System.DateTime? DispatchDateDelivery { get; set; } // Dispatch_Date_Delivery
        public decimal? DispatchDetailProductQuantity { get; set; } // DispatchDetail_Product_Quantity
        public System.Guid? ProductPriceUomId { get; set; } // Product_Price_UOM_ID
        public decimal? DispatchDetailProductPrice { get; set; } // DispatchDetail_Product_Price
        public string DocumentNo { get; set; } // Document_No (length: 50)
        public System.DateTime? DispatchDateOrder { get; set; } // Dispatch_Date_Order
        public string SubCustCode { get; set; } // SubCust_Code (length: 10)
        public string DispatchCode { get; set; } // Dispatch_Code (length: 20)
        public string Gedt { get; set; } // GEDT (length: 10)
        public string Getm { get; set; } // GETM (length: 6)
        public string Twhl { get; set; } // TWHL (length: 3)
        public string Twsl { get; set; } // TWSL (length: 10)
        public decimal? Dlqt { get; set; } // DLQT
        public decimal? Alqt { get; set; } // ALQT
        public string Ridn { get; set; } // RIDN (length: 10)
        public decimal? Ridl { get; set; } // RIDL
        public decimal? Ridx { get; set; } // RIDX
        public decimal? Ridi { get; set; } // RIDI
        public decimal? Plsx { get; set; } // PLSX
        public decimal? Dlix { get; set; } // DLIX
        public string Trtp { get; set; } // TRTP (length: 3)
        public string Tofp { get; set; } // TOFP (length: 3)
        public string Resp { get; set; } // RESP (length: 10)
        public string Rpdt { get; set; } // RPDT (length: 10)
        public string Rptm { get; set; } // RPTM (length: 6)
        public string Ituom { get; set; } // ITUOM (length: 10)
        public System.Guid? Dispatchtypeid { get; set; } // DISPATCHTYPEID
        public System.Guid TransactionId { get; set; } // TransactionID (Primary key)
        public bool? IsSentInterface { get; set; } // IsSentInterface
    }
}