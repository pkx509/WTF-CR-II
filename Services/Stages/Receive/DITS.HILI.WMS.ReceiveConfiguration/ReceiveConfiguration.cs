using DITS.HILI.WMS.ReceiveModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace DITS.HILI.WMS.ReceiveConfiguration
{
    public class ReceiveConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Receive>
    {
        public ReceiveConfiguration()
            : this("dbo")
        {
        }

        public ReceiveConfiguration(string schema)
        {
            ToTable(schema + ".rcv_receive");
            HasKey(x => x.ReceiveID);

            Property(x => x.ReceiveID).IsRequired().HasColumnName("ReceiveID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ReceiveCode).IsRequired().HasColumnName("ReceiveCode").HasColumnType("nvarchar").HasMaxLength(20)
                                 .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            Property(x => x.ReceiveTypeID).IsRequired().HasColumnName("ReceiveTypeID").HasColumnType("uniqueidentifier");
            Property(x => x.ReceiveStatus).IsRequired().HasColumnName("ReceiveStatus").HasColumnType("int");
            Property(x => x.ProductOwnerID).IsRequired().HasColumnName("ProductOwnerID").HasColumnType("uniqueidentifier");
            Property(x => x.SupplierID).IsRequired().HasColumnName("SupplierID").HasColumnType("uniqueidentifier");
            Property(x => x.EstimateDate).HasColumnName("EstimateDate").HasColumnType("datetime");
            Property(x => x.ActualDate).HasColumnName("ActualDate").HasColumnType("datetime");
            Property(x => x.Reference1).HasColumnName("Reference1").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Reference2).HasColumnName("Reference2").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.CloseJobReason).HasColumnName("CloseJobReason").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.IsUrgent).HasColumnName("IsUrgent").HasColumnType("bit");
            Property(x => x.IsCrossDock).HasColumnName("IsCrossDock").HasColumnType("bit");
            Property(x => x.InvoiceNo).HasColumnName("InvoiceNo").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.PONumber).HasColumnName("PONumber").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ContainerNo).HasColumnName("ContainerNo").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.LocationID).HasColumnName("LocationID").HasColumnType("uniqueidentifier");

            Property(x => x.LineID).HasColumnName("LineID");
            Property(x => x.ReferenceID).HasColumnName("ReferenceID");
            Property(x => x.PackageID).HasColumnName("PackageID");

            Property(x => x.Remark).HasColumnName("Remark").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

        }
    }
}
