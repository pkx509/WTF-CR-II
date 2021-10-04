using DITS.HILI.WMS.RegisterTruckModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace DITS.HILI.WMS.RegisterTruckConfiguration
{
    public class TruckTypeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<RegisterTruck>
    {
        public TruckTypeConfiguration()
            : this("dbo")
        {
        }

        public TruckTypeConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.ShippingID);

            // Properties
            Property(t => t.ShippingCode)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.TruckType)
                .HasMaxLength(20);

            Property(t => t.ShippingTruckNo)
                .HasMaxLength(50);

            Property(t => t.DriverName)
                .HasMaxLength(30);

            Property(t => t.LogisticCompany)
                .HasMaxLength(255);

            Property(t => t.OrderNo)
                .HasMaxLength(30);

            Property(t => t.Container_No)
                .HasMaxLength(30);

            Property(t => t.SealNo)
                .HasMaxLength(30);

            Property(t => t.BookingNo)
                .HasMaxLength(30);

            Property(t => t.PoNo)
                .HasMaxLength(50);

            Property(t => t.Dispatchcode)
                .HasMaxLength(20);

            Property(t => t.ShipptoCode)
                .HasMaxLength(20);

            Property(t => t.DocumentNo)
                .HasMaxLength(50);

            Property(t => t.Remark)
                .HasMaxLength(500);

            // Table & Column Mappings
            ToTable(schema + ".reg_truck");
            Property(t => t.ShippingID).HasColumnName("ShippingID");
            Property(t => t.ShippingCode).HasColumnName("ShippingCode").HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(t => t.DocumentDate).HasColumnName("DocumentDate");
            Property(t => t.RegisterTypeID).HasColumnName("RegisterTypeID");
            Property(t => t.TruckType).HasColumnName("TruckType");
            Property(t => t.DockTypeID).HasColumnName("DockTypeID");
            Property(t => t.TruckTypeID).HasColumnName("TruckTypeID");
            Property(t => t.WarehouseID).HasColumnName("WarehouseID");
            Property(t => t.ShippingTruckNo).HasColumnName("ShippingTruckNo");
            Property(t => t.DriverName).HasColumnName("DriverName");
            Property(t => t.LogisticCompany).HasColumnName("LogisticCompany");
            Property(t => t.OrderNo).HasColumnName("OrderNo");
            Property(t => t.Container_No).HasColumnName("Container_No");
            Property(t => t.SealNo).HasColumnName("SealNo");
            Property(t => t.BookingNo).HasColumnName("BookingNo").HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(t => t.PoNo).HasColumnName("PoNo").HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(t => t.ShippingStatus).HasColumnName("ShippingStatus");
            Property(t => t.Dispatchcode).HasColumnName("Dispatchcode").HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(t => t.ShiptoID).HasColumnName("ShiptoID");
            Property(t => t.ShipptoCode).HasColumnName("ShipptoCode");
            Property(t => t.DocumentNo).HasColumnName("DocumentNo");
            Property(t => t.CompleteDate).HasColumnName("CompleteDate");
            Property(t => t.CancelDate).HasColumnName("CancelDate");
            Property(t => t.Remark).HasColumnName("Remark");
        }

    }
}
