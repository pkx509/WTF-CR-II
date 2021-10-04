using DITS.HILI.WMS.DispatchModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.DispatchConfiguration
{
    public class DispatchBookingConfiguration : EntityTypeConfiguration<DispatchBooking>
    {
        public DispatchBookingConfiguration()
            : this("dbo")
        {
        }

        public DispatchBookingConfiguration(string schema)
        {
            ToTable("bk_dispatch_booking", schema);
            HasKey(x => x.BookingId);

            Property(x => x.BookingId).HasColumnName(@"BookingID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.DispatchDetailId).HasColumnName(@"DispatchDetailID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.Sequence).HasColumnName(@"Sequence").HasColumnType("int").IsRequired();
            Property(x => x.ProductId).HasColumnName(@"ProductID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.ProductLot).HasColumnName(@"ProductLot").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.RequestQty).HasColumnName(@"RequestQty").HasColumnType("decimal").IsRequired().HasPrecision(18, 2);
            Property(x => x.RequestStockUnitId).HasColumnName(@"RequestStockUnitID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.RequestBaseUnitId).HasColumnName(@"RequestBaseUnitID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.RequestBaseQty).HasColumnName(@"RequestBaseQty").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.BookingQty).HasColumnName(@"BookingQty").HasColumnType("decimal").IsRequired().HasPrecision(18, 2);
            Property(x => x.BookingStockUnitId).HasColumnName(@"BookingStockUnitID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.BookingBaseUnitId).HasColumnName(@"BookingBaseUnitID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.BookingBaseQty).HasColumnName(@"BookingBaseQty").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.ConversionQty).HasColumnName(@"ConversionQty").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.IsBackOrder).HasColumnName(@"IsBackOrder").HasColumnType("bit").IsOptional();
            Property(x => x.BookingStatus).HasColumnName(@"BookingStatus").HasColumnType("int").IsRequired();
            Property(x => x.LocationId).HasColumnName(@"LocationID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.Mfgdate).HasColumnName(@"MFGDATE").HasColumnType("datetime").IsRequired();
            Property(x => x.ExpirationDate).HasColumnName(@"ExpirationDate").HasColumnType("datetime").IsOptional();
            Property(x => x.PalletCode).HasColumnName(@"PalletCode").HasColumnType("nvarchar").HasMaxLength(40);

            // Foreign keys
            HasRequired(a => a.DispatchDetails).WithMany(b => b.DispatchBookings).HasForeignKey(c => c.DispatchDetailId); // FK_dispatchDetailID

        }
    }
}
