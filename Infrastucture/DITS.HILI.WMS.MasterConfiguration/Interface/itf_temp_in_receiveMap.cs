using DITS.HILI.WMS.MasterModel.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Interface
{
    public class itf_temp_in_receiveMap : EntityTypeConfiguration<itf_temp_in_receive>
    {
        public itf_temp_in_receiveMap()
        {
            // Primary Key
            HasKey(t => t.TransactionID);

            // Properties
            Property(t => t.CONO)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.WHLO)
                .HasMaxLength(3);

            Property(t => t.GEDT)
                .HasMaxLength(10);

            Property(t => t.GETM)
                .HasMaxLength(6);

            Property(t => t.Product_system_code)
                .HasMaxLength(15);

            Property(t => t.WHSL)
                .HasMaxLength(10);

            Property(t => t.TWHL)
                .HasMaxLength(3);

            Property(t => t.TWSL)
                .HasMaxLength(10);

            Property(t => t.BANO)
                .HasMaxLength(12);

            Property(t => t.RIDN)
                .HasMaxLength(10);

            Property(t => t.TRTP)
                .HasMaxLength(3);

            Property(t => t.TOFP)
                .HasMaxLength(3);

            Property(t => t.RESP)
                .HasMaxLength(10);

            Property(t => t.RSCD)
                .HasMaxLength(3);

            Property(t => t.RPDT)
                .HasMaxLength(10);

            Property(t => t.RPTM)
                .HasMaxLength(6);

            Property(t => t.WMSORN)
                .HasMaxLength(15);

            Property(t => t.ITUOM)
                .HasMaxLength(10);

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

            Property(t => t.PalletCode)
                .HasMaxLength(40);

            Property(t => t.Lot_Number)
                .HasMaxLength(20);

            Property(t => t.Product_Uom_Name)
                .HasMaxLength(50);

            Property(t => t.TranRea)
                .HasMaxLength(3);

            // Table & Column Mappings
            ToTable("itf_temp_in_receive");
            Property(t => t.TransactionID).HasColumnName("TransactionID");
            Property(t => t.CONO).HasColumnName("CONO");
            Property(t => t.WHLO).HasColumnName("WHLO");
            Property(t => t.GEDT).HasColumnName("GEDT");
            Property(t => t.GETM).HasColumnName("GETM");
            Property(t => t.Product_system_code).HasColumnName("Product_system_code");
            Property(t => t.WHSL).HasColumnName("WHSL");
            Property(t => t.TWHL).HasColumnName("TWHL");
            Property(t => t.TWSL).HasColumnName("TWSL");
            Property(t => t.BANO).HasColumnName("BANO");
            Property(t => t.ALQT).HasColumnName("ALQT");
            Property(t => t.DLQT).HasColumnName("DLQT");
            Property(t => t.RIDN).HasColumnName("RIDN");
            Property(t => t.RIDL).HasColumnName("RIDL");
            Property(t => t.RIDX).HasColumnName("RIDX");
            Property(t => t.RIDI).HasColumnName("RIDI");
            Property(t => t.PLSX).HasColumnName("PLSX");
            Property(t => t.DLIX).HasColumnName("DLIX");
            Property(t => t.TRTP).HasColumnName("TRTP");
            Property(t => t.TOFP).HasColumnName("TOFP");
            Property(t => t.RESP).HasColumnName("RESP");
            Property(t => t.RSCD).HasColumnName("RSCD");
            Property(t => t.RPDT).HasColumnName("RPDT");
            Property(t => t.RPTM).HasColumnName("RPTM");
            Property(t => t.WMSORN).HasColumnName("WMSORN");
            Property(t => t.ITUOM).HasColumnName("ITUOM");
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
            Property(t => t.PalletCode).HasColumnName("PalletCode");
            Property(t => t.ReferenceID).HasColumnName("ReferenceID");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.Lot_Number).HasColumnName("Lot_Number");
            Property(t => t.Confirm_Qty).HasColumnName("Confirm_Qty");
            Property(t => t.MFG_Date).HasColumnName("MFG_Date");
            Property(t => t.MFG_TimeEnd).HasColumnName("MFG_TimeEnd");
            Property(t => t.Product_Uom_Name).HasColumnName("Product_Uom_Name");
            Property(t => t.TranRea).HasColumnName("TranRea");
        }
    }
}

