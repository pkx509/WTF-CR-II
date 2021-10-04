using DITS.HILI.WMS.RegisterTruckModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace DITS.HILI.WMS.TruckTypeConfiguration
{
    public class RegisterTruckdDetailConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<RegisterTruckDetail>
    {
        public RegisterTruckdDetailConfiguration()
            : this("dbo")
        {
        }

        public RegisterTruckdDetailConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.ShippingDetailID);

            // Properties
            Property(t => t.ProductID);

            // Table & Column Mappings
            ToTable("reg_truck_detail");
            Property(t => t.ShippingDetailID).HasColumnName("ShippingDetailID");
            Property(t => t.ShippingID).HasColumnName("ShippingID");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.ShippingQuantity).HasColumnName("ShippingQuantity");
            Property(t => t.ShippingUnitID).HasColumnName("ShippingUnitID");
            Property(t => t.BasicQuantity).HasColumnName("BasicQuantity");
            Property(t => t.BasicUnitID).HasColumnName("BasicUnitID");
            Property(t => t.ConversionQty).HasColumnName("ConversionQty");
            Property(t => t.ReferenceID).HasColumnName("ReferenceID");
            Property(t => t.BookingID).HasColumnName("BookingID").HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(t => t.TransactionTypeID).HasColumnName("TransactionTypeID");
            Property(t => t.Shipping_DT).HasColumnName("Shipping_DT");
            Property(t => t.ConfirmQuantity).HasColumnName("ConfirmQuantity");
            Property(t => t.ConfirmUnitID).HasColumnName("ConfirmUnitID");
            Property(t => t.ConfirmBasicQuantity).HasColumnName("ConfirmBasicQuantity");
            Property(t => t.ConfirmBasicUnitID).HasColumnName("ConfirmBasicUnitID");

            // Relationships
            HasRequired(t => t.RegisterTruck)
                .WithMany(t => t.RegisterTruckDetail)
                .HasForeignKey(d => d.ShippingID);
        }

    }
}
