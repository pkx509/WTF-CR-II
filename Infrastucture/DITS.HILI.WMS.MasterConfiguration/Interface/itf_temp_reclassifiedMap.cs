using DITS.HILI.WMS.MasterModel.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Interface
{
    public class itf_temp_reclassifiedMap : EntityTypeConfiguration<itf_temp_reclassified>
    {
        public itf_temp_reclassifiedMap()
        {
            // Primary Key
            HasKey(t => t.CONO);

            // Properties
            Property(t => t.CONO)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.GEDT)
                .HasMaxLength(10);

            Property(t => t.GETM)
                .HasMaxLength(6);

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

            Property(t => t.ITNO)
                .HasMaxLength(15);

            Property(t => t.BANO)
                .HasMaxLength(12);

            Property(t => t.QLDT)
                .HasMaxLength(10);

            Property(t => t.NITN)
                .HasMaxLength(15);

            Property(t => t.NBAN)
                .HasMaxLength(12);

            Property(t => t.RSCD)
                .HasMaxLength(13);

            Property(t => t.STAS)
                .HasMaxLength(1);

            Property(t => t.RESP)
                .HasMaxLength(10);

            Property(t => t.ITUOM)
                .HasMaxLength(10);

            Property(t => t.QA_Code)
                .HasMaxLength(20);

            Property(t => t.Pallet_Tag)
                .HasMaxLength(20);

            Property(t => t.Lot_number)
                .HasMaxLength(12);

            Property(t => t.Product_system_code)
                .HasMaxLength(15);

            Property(t => t.WHLO)
                .HasMaxLength(3);

            // Table & Column Mappings
            ToTable("itf_temp_reclassified");
            Property(t => t.CONO).HasColumnName("CONO");
            Property(t => t.GEDT).HasColumnName("GEDT");
            Property(t => t.GETM).HasColumnName("GETM");
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
            Property(t => t.ITNO).HasColumnName("ITNO");
            Property(t => t.BANO).HasColumnName("BANO");
            Property(t => t.QLQT).HasColumnName("QLQT");
            Property(t => t.QLDT).HasColumnName("QLDT");
            Property(t => t.NITN).HasColumnName("NITN");
            Property(t => t.NBAN).HasColumnName("NBAN");
            Property(t => t.RSCD).HasColumnName("RSCD");
            Property(t => t.STAS).HasColumnName("STAS");
            Property(t => t.RESP).HasColumnName("RESP");
            Property(t => t.ITUOM).HasColumnName("ITUOM");
            Property(t => t.NQLQT).HasColumnName("NQLQT");
            Property(t => t.QA_Code).HasColumnName("QA_Code");
            Property(t => t.ApproveDate).HasColumnName("ApproveDate");
            Property(t => t.Pallet_Tag).HasColumnName("Pallet_Tag").HasMaxLength(40);
            Property(t => t.Lot_number).HasColumnName("Lot_number");
            Property(t => t.INS_QTY).HasColumnName("INS_QTY");
            Property(t => t.Product_system_code).HasColumnName("Product_system_code");
            Property(t => t.WHLO).HasColumnName("WHLO");
            Property(t => t.DMFromWarehouseID).HasColumnName("DMFromWarehouseID");
        }
    }
}
