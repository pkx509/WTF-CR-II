using DITS.HILI.WMS.RegisterTruckModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace DITS.HILI.WMS.TruckTypeConfiguration
{
    public class RegisterTruckConsolidateConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<RegisterTruckConsolidate>
    {
        public RegisterTruckConsolidateConfiguration()
            : this("dbo")
        {
        }

        public RegisterTruckConsolidateConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.DeliveryID);

            // Properties
            Property(t => t.Barcode)
                .HasMaxLength(150);
            Property(t => t.PalletCode)
                .HasMaxLength(150);

            // Table & Column Mappings
            ToTable("reg_truck_consolidate");
            Property(t => t.DeliveryID).HasColumnName("DeliveryID");
            Property(t => t.ShippingDetailID).HasColumnName("ShippingDetailID");
            Property(t => t.BaseQuantity).HasColumnName("BaseQuantity");
            Property(t => t.BaseUnitID).HasColumnName("BaseUnitID");
            Property(t => t.Barcode).HasColumnName("Barcode");
            Property(t => t.StockUnitID).HasColumnName("StockUnitID");
            Property(t => t.StockQuantity).HasColumnName("StockQuantity");
            Property(t => t.ConsolidateStatus).HasColumnName("ConsolidateStatus");
            Property(t => t.PalletCode).HasColumnName("PalletCode").HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            // Relationships
            HasOptional(t => t.RegisterTruckDetail)
                .WithMany(t => t.RegisterTruckConsolidate)
                .HasForeignKey(d => d.ShippingDetailID);
        }

    }
}
