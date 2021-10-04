using DITS.HILI.WMS.MasterModel.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Interface
{
    public class itf_temp_in_dispatchMap : EntityTypeConfiguration<itf_temp_in_dispatch>
    {
        public itf_temp_in_dispatchMap()
        {
            // Primary Key
            HasKey(t => t.TransactionID);

            // Properties
            Property(t => t.CONO)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.ORNO)
                .HasMaxLength(10);

            Property(t => t.CUNO)
                .HasMaxLength(10);

            Property(t => t.FACI)
                .HasMaxLength(3);

            Property(t => t.WHLO)
                .HasMaxLength(3);

            Property(t => t.ITNO)
                .HasMaxLength(15);

            Property(t => t.ITDS)
                .HasMaxLength(30);

            Property(t => t.DWDT)
                .HasMaxLength(10);

            Property(t => t.ALUN)
                .HasMaxLength(3);

            Property(t => t.SPUN)
                .HasMaxLength(3);

            Property(t => t.CUOR)
                .HasMaxLength(20);

            Property(t => t.DWDZ)
                .HasMaxLength(10);

            Property(t => t.DWHZ)
                .HasMaxLength(4);

            Property(t => t.RSCD)
                .HasMaxLength(3);

            Property(t => t.BANO)
                .HasMaxLength(12);

            Property(t => t.WHSL)
                .HasMaxLength(10);

            Property(t => t.ORTP)
                .HasMaxLength(3);

            Property(t => t.RLDT)
                .HasMaxLength(10);

            Property(t => t.MODL)
                .HasMaxLength(3);

            Property(t => t.TEDL)
                .HasMaxLength(3);

            Property(t => t.YREF)
                .HasMaxLength(30);

            Property(t => t.TEPY)
                .HasMaxLength(3);

            Property(t => t.PYNO)
                .HasMaxLength(10);

            Property(t => t.ADID)
                .HasMaxLength(6);

            Property(t => t.OREF)
                .HasMaxLength(30);

            Property(t => t.CUDT)
                .HasMaxLength(10);

            Property(t => t.ORDT)
                .HasMaxLength(10);

            Property(t => t.RLDZ)
                .HasMaxLength(10);

            Property(t => t.RLHZ)
                .HasMaxLength(4);

            Property(t => t.CTST)
                .HasMaxLength(5);

            Property(t => t.EMSG)
                .HasMaxLength(200);

            Property(t => t.WMSORN)
                .HasMaxLength(15);

            Property(t => t.GDATE)
                .HasMaxLength(10);

            Property(t => t.GTIME)
                .HasMaxLength(6);

            Property(t => t.GSTT)
                .HasMaxLength(1);

            Property(t => t.RNOM3)
                .HasMaxLength(10);

            Property(t => t.FDATE)
                .HasMaxLength(10);

            Property(t => t.FTIME)
                .HasMaxLength(6);

            Property(t => t.FSTT)
                .HasMaxLength(1);

            Property(t => t.Sync_Flag)
                .HasMaxLength(1);

            Property(t => t.Sync_Date)
                .HasMaxLength(20);

            Property(t => t.Product_System_Code)
                .HasMaxLength(20);

            Property(t => t.Product_Name_Full)
                .HasMaxLength(500);

            Property(t => t.Document_No)
                .HasMaxLength(50);

            Property(t => t.SubCust_Code)
                .HasMaxLength(10);

            Property(t => t.Dispatch_Code)
                .HasMaxLength(20);

            Property(t => t.GEDT)
                .HasMaxLength(10);

            Property(t => t.GETM)
                .HasMaxLength(6);

            Property(t => t.TWHL)
                .HasMaxLength(3);

            Property(t => t.TWSL)
                .HasMaxLength(10);

            Property(t => t.RIDN)
                .HasMaxLength(10);

            Property(t => t.TRTP)
                .HasMaxLength(3);

            Property(t => t.TOFP)
                .HasMaxLength(3);

            Property(t => t.RESP)
                .HasMaxLength(10);

            Property(t => t.RPDT)
                .HasMaxLength(10);

            Property(t => t.RPTM)
                .HasMaxLength(6);

            Property(t => t.ITUOM)
                .HasMaxLength(10);

            // Table & Column Mappings
            ToTable("itf_temp_in_dispatch");
            Property(t => t.CONO).HasColumnName("CONO");
            Property(t => t.ORNO).HasColumnName("ORNO");
            Property(t => t.CUNO).HasColumnName("CUNO");
            Property(t => t.FACI).HasColumnName("FACI");
            Property(t => t.WHLO).HasColumnName("WHLO");
            Property(t => t.ITNO).HasColumnName("ITNO");
            Property(t => t.ITDS).HasColumnName("ITDS");
            Property(t => t.DWDT).HasColumnName("DWDT");
            Property(t => t.ORQT).HasColumnName("ORQT");
            Property(t => t.ALUN).HasColumnName("ALUN");
            Property(t => t.SAPR).HasColumnName("SAPR");
            Property(t => t.SPUN).HasColumnName("SPUN");
            Property(t => t.DIA1).HasColumnName("DIA1");
            Property(t => t.DIA2).HasColumnName("DIA2");
            Property(t => t.DIA3).HasColumnName("DIA3");
            Property(t => t.DIA4).HasColumnName("DIA4");
            Property(t => t.DIA5).HasColumnName("DIA5");
            Property(t => t.DIA6).HasColumnName("DIA6");
            Property(t => t.DIP1).HasColumnName("DIP1");
            Property(t => t.DIP2).HasColumnName("DIP2");
            Property(t => t.DIP3).HasColumnName("DIP3");
            Property(t => t.DIP4).HasColumnName("DIP4");
            Property(t => t.DIP5).HasColumnName("DIP5");
            Property(t => t.DIP6).HasColumnName("DIP6");
            Property(t => t.CUOR).HasColumnName("CUOR");
            Property(t => t.DWDZ).HasColumnName("DWDZ");
            Property(t => t.DWHZ).HasColumnName("DWHZ");
            Property(t => t.RSCD).HasColumnName("RSCD");
            Property(t => t.BANO).HasColumnName("BANO");
            Property(t => t.WHSL).HasColumnName("WHSL");
            Property(t => t.ORTP).HasColumnName("ORTP");
            Property(t => t.RLDT).HasColumnName("RLDT");
            Property(t => t.MODL).HasColumnName("MODL");
            Property(t => t.TEDL).HasColumnName("TEDL");
            Property(t => t.YREF).HasColumnName("YREF");
            Property(t => t.TEPY).HasColumnName("TEPY");
            Property(t => t.PYNO).HasColumnName("PYNO");
            Property(t => t.ADID).HasColumnName("ADID");
            Property(t => t.OREF).HasColumnName("OREF");
            Property(t => t.CUDT).HasColumnName("CUDT");
            Property(t => t.ORDT).HasColumnName("ORDT");
            Property(t => t.RLDZ).HasColumnName("RLDZ");
            Property(t => t.RLHZ).HasColumnName("RLHZ");
            Property(t => t.CTST).HasColumnName("CTST");
            Property(t => t.EMSG).HasColumnName("EMSG");
            Property(t => t.WMSORN).HasColumnName("WMSORN");
            Property(t => t.GDATE).HasColumnName("GDATE");
            Property(t => t.GTIME).HasColumnName("GTIME");
            Property(t => t.GSTT).HasColumnName("GSTT");
            Property(t => t.RNOM3).HasColumnName("RNOM3");
            Property(t => t.FDATE).HasColumnName("FDATE");
            Property(t => t.FTIME).HasColumnName("FTIME");
            Property(t => t.FSTT).HasColumnName("FSTT");
            Property(t => t.Sync_UnsuccessNo).HasColumnName("Sync_UnsuccessNo");
            Property(t => t.Sync_Flag).HasColumnName("Sync_Flag");
            Property(t => t.Sync_Date).HasColumnName("Sync_Date");
            Property(t => t.Product_System_Code).HasColumnName("Product_System_Code");
            Property(t => t.Product_Name_Full).HasColumnName("Product_Name_Full");
            Property(t => t.Dispatch_Date_Delivery).HasColumnName("Dispatch_Date_Delivery");
            Property(t => t.DispatchDetail_Product_Quantity).HasColumnName("DispatchDetail_Product_Quantity");
            Property(t => t.Product_Price_UOM_ID).HasColumnName("Product_Price_UOM_ID");
            Property(t => t.DispatchDetail_Product_Price).HasColumnName("DispatchDetail_Product_Price");
            Property(t => t.Document_No).HasColumnName("Document_No");
            Property(t => t.Dispatch_Date_Order).HasColumnName("Dispatch_Date_Order");
            Property(t => t.SubCust_Code).HasColumnName("SubCust_Code");
            Property(t => t.Dispatch_Code).HasColumnName("Dispatch_Code");
            Property(t => t.GEDT).HasColumnName("GEDT");
            Property(t => t.GETM).HasColumnName("GETM");
            Property(t => t.TWHL).HasColumnName("TWHL");
            Property(t => t.TWSL).HasColumnName("TWSL");
            Property(t => t.DLQT).HasColumnName("DLQT");
            Property(t => t.ALQT).HasColumnName("ALQT");
            Property(t => t.RIDN).HasColumnName("RIDN");
            Property(t => t.RIDL).HasColumnName("RIDL");
            Property(t => t.RIDX).HasColumnName("RIDX");
            Property(t => t.RIDI).HasColumnName("RIDI");
            Property(t => t.PLSX).HasColumnName("PLSX");
            Property(t => t.DLIX).HasColumnName("DLIX");
            Property(t => t.TRTP).HasColumnName("TRTP");
            Property(t => t.TOFP).HasColumnName("TOFP");
            Property(t => t.RESP).HasColumnName("RESP");
            Property(t => t.RPDT).HasColumnName("RPDT");
            Property(t => t.RPTM).HasColumnName("RPTM");
            Property(t => t.ITUOM).HasColumnName("ITUOM");
            Property(t => t.DISPATCHTYPEID).HasColumnName("DISPATCHTYPEID");
        }
    }
}
