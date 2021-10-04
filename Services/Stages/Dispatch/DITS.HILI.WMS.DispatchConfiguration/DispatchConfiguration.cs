using DITS.HILI.WMS.DispatchModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.DispatchConfiguration
{
    public class DispatchConfiguration : EntityTypeConfiguration<Dispatch>
    {
        public DispatchConfiguration()
            : this("dbo")
        {
        }

        public DispatchConfiguration(string schema)
        {
            ToTable("dp_dispatch", schema);
            HasKey(x => x.DispatchId);

            Property(x => x.DispatchId).HasColumnName(@"DispatchID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.DispatchCode).HasColumnName(@"DispatchCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(20).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute())); ;
            Property(x => x.DocumentId).HasColumnName(@"DocumentID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.OrderDate).HasColumnName(@"OrderDate").HasColumnType("datetime").IsOptional();
            Property(x => x.DocumentDate).HasColumnName(@"DocumentDate").HasColumnType("datetime").IsOptional();
            Property(x => x.DeliveryDate).HasColumnName(@"DeliveryDate").HasColumnType("datetime").IsOptional();
            Property(x => x.DocumentApproveDate).HasColumnName(@"DocumentApproveDate").HasColumnType("datetime").IsOptional();
            Property(x => x.Pono).HasColumnName(@"Pono").HasColumnType("nvarchar").IsOptional().HasMaxLength(50).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(x => x.OrderNo).HasColumnName(@"OrderNo").HasColumnType("nvarchar").IsOptional().HasMaxLength(50).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(x => x.ShipptoId).HasColumnName(@"ShipptoID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.IsUrgent).HasColumnName(@"IsUrgent").HasColumnType("bit").IsOptional();
            Property(x => x.IsBackOrder).HasColumnName(@"IsBackOrder").HasColumnType("bit").IsOptional();
            Property(x => x.ReferenceId).HasColumnName(@"ReferenceID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.FromwarehouseId).HasColumnName(@"FromwarehouseID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.TowarehouseId).HasColumnName(@"TowarehouseID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.DispatchStatus).HasColumnName(@"DispatchStatus").HasColumnType("int").IsOptional();
            Property(x => x.SupplierId).HasColumnName(@"SupplierID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.CustomerId).HasColumnName(@"CustomerID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.ReviseDateTime).HasColumnName(@"ReviseDateTime").HasColumnType("datetime").IsOptional();
            Property(x => x.ReviseReason).HasColumnName(@"ReviseReason").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);


        }
    }
}
