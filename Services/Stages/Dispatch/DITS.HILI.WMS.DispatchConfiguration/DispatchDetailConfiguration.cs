using DITS.HILI.WMS.DispatchModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.DispatchConfiguration
{
    public class DispatchDetailConfiguration : EntityTypeConfiguration<DispatchDetail>
    {
        public DispatchDetailConfiguration()
            : this("dbo")
        {
        }

        public DispatchDetailConfiguration(string schema)
        {
            ToTable("dp_dispatch_detail", schema);
            HasKey(x => x.DispatchDetailId);

            Property(x => x.DispatchDetailId).HasColumnName(@"DispatchDetailID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.DispatchId).HasColumnName(@"DispatchID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.Sequence).HasColumnName(@"Sequence").HasColumnType("int").IsRequired();
            Property(x => x.ProductId).HasColumnName(@"ProductID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.StockUnitId).HasColumnName(@"StockUnitID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.Quantity).HasColumnName(@"Quantity").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.BaseQuantity).HasColumnName(@"BaseQuantity").HasColumnType("decimal").IsOptional().HasPrecision(18, 0);
            Property(x => x.BaseUnitId).HasColumnName(@"BaseUnitId").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.ConversionQty).HasColumnName(@"ConversionQty").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.ProductOwnerId).HasColumnName(@"ProductOwnerID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.DispatchDetailProductWidth).HasColumnName(@"DispatchDetail_Product_Width").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.DispatchDetailProductLength).HasColumnName(@"DispatchDetail_Product_Length").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.DispatchDetailProductHeight).HasColumnName(@"DispatchDetail_Product_Height").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.DispatchPriceUnitId).HasColumnName(@"DispatchPriceUnitID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.DispatchPrice).HasColumnName(@"DispatchPrice").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.DispatchDetailStatus).HasColumnName(@"DispatchDetail_Status").HasColumnType("int").IsRequired();
            Property(x => x.RuleId).HasColumnName(@"RuleID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.ProductStatusId).HasColumnName(@"ProductStatusID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.ProductSubStatusId).HasColumnName(@"ProductSubStatusID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.ReviseDateTime).HasColumnName(@"ReviseDateTime").HasColumnType("datetime").IsOptional();
            Property(x => x.ReviseReason).HasColumnName(@"ReviseReason").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.IsBackOrder).HasColumnName(@"IsBackOrder").HasColumnType("bit").IsOptional();
            Property(x => x.BackOrderQuantity).HasColumnName(@"BackOrderQuantity").HasColumnType("decimal").IsOptional();
            // Foreign keys
            HasRequired(a => a.Dispatch).WithMany(b => b.DispatchDetailCollection).HasForeignKey(c => c.DispatchId); // FK_dispatchID        }

        }
    }
}
