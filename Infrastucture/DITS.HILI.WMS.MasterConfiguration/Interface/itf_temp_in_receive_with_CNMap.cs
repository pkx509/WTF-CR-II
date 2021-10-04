using DITS.HILI.WMS.MasterModel.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Interface
{
    public class itf_temp_in_receive_with_CNMap : EntityTypeConfiguration<itf_temp_in_receive_with_CN>
    {
        public itf_temp_in_receive_with_CNMap()
        {
            // Primary Key
            HasKey(t => t.TransactionID);

            ToTable("itf_temp_in_receive_with_CN");
            Property(t => t.TransactionID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.ReferenceID);
            Property(t => t.IsSentInterface);

            #region Varchar

            Property(t => t.CUNO).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.FACI).HasColumnType("varchar").HasMaxLength(3);
            Property(t => t.WHLO).HasColumnType("varchar").HasMaxLength(3);
            Property(t => t.ORTP).HasColumnType("varchar").HasMaxLength(3);
            Property(t => t.RLDT).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.MODL).HasColumnType("varchar").HasMaxLength(3);
            Property(t => t.TEDL).HasColumnType("varchar").HasMaxLength(3);
            Property(t => t.YREF).HasColumnType("varchar").HasMaxLength(30);
            Property(t => t.TEPY).HasColumnType("varchar").HasMaxLength(3);
            Property(t => t.PYNO).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.ADID).HasColumnType("varchar").HasMaxLength(6);
            Property(t => t.OREF).HasColumnType("varchar").HasMaxLength(30);
            Property(t => t.CUOR).HasColumnType("varchar").HasMaxLength(20);
            Property(t => t.CUDT).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.ORDT).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.RLDZ).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.RLHZ).HasColumnType("varchar").HasMaxLength(4);
            Property(t => t.CTST).HasColumnType("varchar").HasMaxLength(5);
            Property(t => t.EMSG).HasColumnType("varchar").HasMaxLength(20);
            Property(t => t.WMSORN).HasColumnType("varchar").HasMaxLength(15);
            Property(t => t.GDATE).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.GTIME).HasColumnType("varchar").HasMaxLength(6);
            Property(t => t.GSTT).HasColumnType("varchar").HasMaxLength(1);
            Property(t => t.RNOM3).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.FDATE).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.FTIME).HasColumnType("varchar").HasMaxLength(6);
            Property(t => t.FSTT).HasColumnType("varchar").HasMaxLength(1);
            Property(t => t.Sync_Flag).HasColumnType("varchar").HasMaxLength(1);
            Property(t => t.Sync_Date).HasColumnType("varchar").HasMaxLength(20);
            Property(t => t.ORNO).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.ITNO).HasColumnType("varchar").HasMaxLength(15);
            Property(t => t.ITDS).HasColumnType("varchar").HasMaxLength(30);
            Property(t => t.DWDT).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.ALUN).HasColumnType("varchar").HasMaxLength(3);
            Property(t => t.SPUN).HasColumnType("varchar").HasMaxLength(3);
            Property(t => t.DWDZ).HasColumnType("varchar").HasMaxLength(10);
            Property(t => t.DWHZ).HasColumnType("varchar").HasMaxLength(4);
            Property(t => t.RSCD).HasColumnType("varchar").HasMaxLength(3);
            Property(t => t.BANO).HasColumnType("varchar").HasMaxLength(12);
            Property(t => t.WHSL).HasColumnType("varchar").HasMaxLength(10);

            #endregion

            #region Decimal

            Property(t => t.CONO).HasPrecision(3, 0);
            Property(t => t.Sync_UnsuccessNo).HasPrecision(4, 0);
            Property(t => t.ORQT).HasPrecision(15, 6);
            Property(t => t.SAPR).HasPrecision(17, 6);
            Property(t => t.DIA1).HasPrecision(15, 2);
            Property(t => t.DIA2).HasPrecision(15, 2);
            Property(t => t.DIA3).HasPrecision(15, 2);
            Property(t => t.DIA4).HasPrecision(15, 2);
            Property(t => t.DIA5).HasPrecision(15, 2);
            Property(t => t.DIA6).HasPrecision(15, 2);
            Property(t => t.DIP1).HasPrecision(5, 2);
            Property(t => t.DIP2).HasPrecision(5, 2);
            Property(t => t.DIP3).HasPrecision(5, 2);
            Property(t => t.DIP4).HasPrecision(5, 2);
            Property(t => t.DIP5).HasPrecision(5, 2);
            Property(t => t.DIP6).HasPrecision(5, 2);

            #endregion
        }
    }
}
