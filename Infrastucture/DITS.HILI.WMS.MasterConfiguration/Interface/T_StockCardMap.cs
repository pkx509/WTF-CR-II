using DITS.HILI.WMS.MasterModel.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Interface
{

    public class T_StockCardMap : EntityTypeConfiguration<T_StockCard>
    {
        public T_StockCardMap()
        {
            // Primary Key
            HasKey(t => new { t.TransactionDT, t.ProductID, t.TransactionType, t.BalaceBF, t.BalanceQty, t.StockUnitID, t.BaseUnitID, t.TotalAmount, t.LotNo });

            // Properties
            Property(t => t.TransactionType)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.ORTP)
                .HasMaxLength(10);

            Property(t => t.BalaceBF)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.DocumentNo)
                .HasMaxLength(20);

            Property(t => t.BalanceQty)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.TotalAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.PONO)
                .HasMaxLength(20);

            Property(t => t.LotNo)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            ToTable("T_StockCard");
            Property(t => t.TransactionDT).HasColumnName("TransactionDT");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.DocumentID).HasColumnName("DocumentID");
            Property(t => t.TransactionType).HasColumnName("TransactionType");
            Property(t => t.ORTP).HasColumnName("ORTP");
            Property(t => t.BalaceBF).HasColumnName("BalaceBF");
            Property(t => t.DocumentNo).HasColumnName("DocumentNo");
            Property(t => t.ReceiveQty).HasColumnName("ReceiveQty");
            Property(t => t.DispatchQty).HasColumnName("DispatchQty");
            Property(t => t.BalanceQty).HasColumnName("BalanceQty");
            Property(t => t.StockUnitID).HasColumnName("StockUnitID");
            Property(t => t.BaseUnitID).HasColumnName("BaseUnitID");
            Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            Property(t => t.PONO).HasColumnName("PONO");
            Property(t => t.LotNo).HasColumnName("LotNo");
            Property(t => t.ShipToID).HasColumnName("ShipToID");
            Property(t => t.LineID).HasColumnName("LineID");
        }
    }

}
