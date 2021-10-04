using DITS.HILI.WMS.MasterModel.Interface;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Interface
{
    public class itf_temp_change_lot_Map : EntityTypeConfiguration<itf_temp_change_lot>
    {
        public itf_temp_change_lot_Map()
        {
            // Primary Key
            HasKey(t => t.TransactionID);

            Property(t => t.CONO).HasPrecision(3, 0);
            Property(t => t.QLQT).HasPrecision(15, 6);
            Property(t => t.NQLQT).HasPrecision(15, 6);


            Property(t => t.GEDT).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.GETM).HasColumnType("varchar").HasMaxLength(6);
            Property(t => t.WHLO).HasColumnType("varchar").HasMaxLength(3);
            Property(t => t.WHSL).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.ITNO).HasColumnType("varchar").HasMaxLength(15);
            Property(t => t.BANO).HasColumnType("varchar").HasMaxLength(12);
            Property(t => t.QLDT).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.NITN).HasColumnType("varchar").HasMaxLength(15);
            Property(t => t.NBAN).HasColumnType("varchar").HasMaxLength(12);
            Property(t => t.RSCD).HasColumnType("varchar").HasMaxLength(13);
            Property(t => t.STAS).HasColumnType("varchar").HasMaxLength(1);
            Property(t => t.RESP).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.CTST).HasColumnType("varchar").HasMaxLength(5);
            Property(t => t.EMSG).HasColumnType("varchar").HasMaxLength(200);
            Property(t => t.WMSORN).HasColumnType("varchar").HasMaxLength(15);
            Property(t => t.ITUOM).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.GDATE).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.GTIME).HasColumnType("varchar").HasMaxLength(6);
            Property(t => t.GSTT).HasColumnType("varchar").HasMaxLength(1);
            Property(t => t.RNOM3).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.FDATE).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.FTIME).HasColumnType("varchar").HasMaxLength(6);
            Property(t => t.FSTT).HasColumnType("varchar").HasMaxLength(1);
            Property(t => t.RefType).HasColumnType("varchar").HasMaxLength(50);


            // Table & Column Mappings
            ToTable("itf_temp_change_lot");
        }
    }
}
