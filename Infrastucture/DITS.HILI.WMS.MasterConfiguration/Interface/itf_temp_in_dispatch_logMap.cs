using DITS.HILI.WMS.MasterModel.Interface;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Interface
{
    public class itf_temp_in_dispatch_logMap : EntityTypeConfiguration<itf_temp_in_dispatch_log>
    {
        public itf_temp_in_dispatch_logMap()
            : this("dbo")
        {
        }

        public itf_temp_in_dispatch_logMap(string schema)
        {
            ToTable("itf_temp_in_dispatch_log", schema);
            HasKey(x => x.TransactionId);

            Property(x => x.Cono).HasColumnName(@"CONO").HasColumnType("decimal").IsRequired().HasPrecision(18, 2);
            Property(x => x.Orno).HasColumnName(@"ORNO").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Cuno).HasColumnName(@"CUNO").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Faci).HasColumnName(@"FACI").HasColumnType("nvarchar").IsOptional().HasMaxLength(3);
            Property(x => x.Whlo).HasColumnName(@"WHLO").HasColumnType("nvarchar").IsOptional().HasMaxLength(3);
            Property(x => x.Itno).HasColumnName(@"ITNO").HasColumnType("nvarchar").IsOptional().HasMaxLength(15);
            Property(x => x.Itds).HasColumnName(@"ITDS").HasColumnType("nvarchar").IsOptional().HasMaxLength(30);
            Property(x => x.Dwdt).HasColumnName(@"DWDT").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Orqt).HasColumnName(@"ORQT").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Alun).HasColumnName(@"ALUN").HasColumnType("nvarchar").IsOptional().HasMaxLength(3);
            Property(x => x.Sapr).HasColumnName(@"SAPR").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Spun).HasColumnName(@"SPUN").HasColumnType("nvarchar").IsOptional().HasMaxLength(3);
            Property(x => x.Dia1).HasColumnName(@"DIA1").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Dia2).HasColumnName(@"DIA2").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Dia3).HasColumnName(@"DIA3").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Dia4).HasColumnName(@"DIA4").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Dia5).HasColumnName(@"DIA5").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Dia6).HasColumnName(@"DIA6").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Dip1).HasColumnName(@"DIP1").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Dip2).HasColumnName(@"DIP2").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Dip3).HasColumnName(@"DIP3").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Dip4).HasColumnName(@"DIP4").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Dip5).HasColumnName(@"DIP5").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Dip6).HasColumnName(@"DIP6").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Cuor).HasColumnName(@"CUOR").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);
            Property(x => x.Dwdz).HasColumnName(@"DWDZ").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Dwhz).HasColumnName(@"DWHZ").HasColumnType("nvarchar").IsOptional().HasMaxLength(4);
            Property(x => x.Rscd).HasColumnName(@"RSCD").HasColumnType("nvarchar").IsOptional().HasMaxLength(3);
            Property(x => x.Bano).HasColumnName(@"BANO").HasColumnType("nvarchar").IsOptional().HasMaxLength(12);
            Property(x => x.Whsl).HasColumnName(@"WHSL").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Ortp).HasColumnName(@"ORTP").HasColumnType("nvarchar").IsOptional().HasMaxLength(3);
            Property(x => x.Rldt).HasColumnName(@"RLDT").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Modl).HasColumnName(@"MODL").HasColumnType("nvarchar").IsOptional().HasMaxLength(3);
            Property(x => x.Tedl).HasColumnName(@"TEDL").HasColumnType("nvarchar").IsOptional().HasMaxLength(3);
            Property(x => x.Yref).HasColumnName(@"YREF").HasColumnType("nvarchar").IsOptional().HasMaxLength(30);
            Property(x => x.Tepy).HasColumnName(@"TEPY").HasColumnType("nvarchar").IsOptional().HasMaxLength(3);
            Property(x => x.Pyno).HasColumnName(@"PYNO").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Adid).HasColumnName(@"ADID").HasColumnType("nvarchar").IsOptional().HasMaxLength(6);
            Property(x => x.Oref).HasColumnName(@"OREF").HasColumnType("nvarchar").IsOptional().HasMaxLength(30);
            Property(x => x.Cudt).HasColumnName(@"CUDT").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Ordt).HasColumnName(@"ORDT").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Rldz).HasColumnName(@"RLDZ").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Rlhz).HasColumnName(@"RLHZ").HasColumnType("nvarchar").IsOptional().HasMaxLength(4);
            Property(x => x.Ctst).HasColumnName(@"CTST").HasColumnType("nvarchar").IsOptional().HasMaxLength(5);
            Property(x => x.Emsg).HasColumnName(@"EMSG").HasColumnType("nvarchar").IsOptional().HasMaxLength(200);
            Property(x => x.Wmsorn).HasColumnName(@"WMSORN").HasColumnType("nvarchar").IsOptional().HasMaxLength(15);
            Property(x => x.Gdate).HasColumnName(@"GDATE").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Gtime).HasColumnName(@"GTIME").HasColumnType("nvarchar").IsOptional().HasMaxLength(6);
            Property(x => x.Gstt).HasColumnName(@"GSTT").HasColumnType("nvarchar").IsOptional().HasMaxLength(1);
            Property(x => x.Rnom3).HasColumnName(@"RNOM3").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Fdate).HasColumnName(@"FDATE").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Ftime).HasColumnName(@"FTIME").HasColumnType("nvarchar").IsOptional().HasMaxLength(6);
            Property(x => x.Fstt).HasColumnName(@"FSTT").HasColumnType("nvarchar").IsOptional().HasMaxLength(1);
            Property(x => x.SyncUnsuccessNo).HasColumnName(@"Sync_UnsuccessNo").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.SyncFlag).HasColumnName(@"Sync_Flag").HasColumnType("nvarchar").IsOptional().HasMaxLength(1);
            Property(x => x.SyncDate).HasColumnName(@"Sync_Date").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);
            Property(x => x.ProductSystemCode).HasColumnName(@"Product_System_Code").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);
            Property(x => x.ProductNameFull).HasColumnName(@"Product_Name_Full").HasColumnType("nvarchar").IsOptional().HasMaxLength(500);
            Property(x => x.DispatchDateDelivery).HasColumnName(@"Dispatch_Date_Delivery").HasColumnType("datetime").IsOptional();
            Property(x => x.DispatchDetailProductQuantity).HasColumnName(@"DispatchDetail_Product_Quantity").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.ProductPriceUomId).HasColumnName(@"Product_Price_UOM_ID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.DispatchDetailProductPrice).HasColumnName(@"DispatchDetail_Product_Price").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.DocumentNo).HasColumnName(@"Document_No").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.DispatchDateOrder).HasColumnName(@"Dispatch_Date_Order").HasColumnType("datetime").IsOptional();
            Property(x => x.SubCustCode).HasColumnName(@"SubCust_Code").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.DispatchCode).HasColumnName(@"Dispatch_Code").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);
            Property(x => x.Gedt).HasColumnName(@"GEDT").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Getm).HasColumnName(@"GETM").HasColumnType("nvarchar").IsOptional().HasMaxLength(6);
            Property(x => x.Twhl).HasColumnName(@"TWHL").HasColumnType("nvarchar").IsOptional().HasMaxLength(3);
            Property(x => x.Twsl).HasColumnName(@"TWSL").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Dlqt).HasColumnName(@"DLQT").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Alqt).HasColumnName(@"ALQT").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Ridn).HasColumnName(@"RIDN").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Ridl).HasColumnName(@"RIDL").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Ridx).HasColumnName(@"RIDX").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Ridi).HasColumnName(@"RIDI").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Plsx).HasColumnName(@"PLSX").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Dlix).HasColumnName(@"DLIX").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.Trtp).HasColumnName(@"TRTP").HasColumnType("nvarchar").IsOptional().HasMaxLength(3);
            Property(x => x.Tofp).HasColumnName(@"TOFP").HasColumnType("nvarchar").IsOptional().HasMaxLength(3);
            Property(x => x.Resp).HasColumnName(@"RESP").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Rpdt).HasColumnName(@"RPDT").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Rptm).HasColumnName(@"RPTM").HasColumnType("nvarchar").IsOptional().HasMaxLength(6);
            Property(x => x.Ituom).HasColumnName(@"ITUOM").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Dispatchtypeid).HasColumnName(@"DISPATCHTYPEID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.TransactionId).HasColumnName(@"TransactionID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.IsSentInterface).HasColumnName(@"IsSentInterface").HasColumnType("bit").IsOptional();

        }
    }
}
