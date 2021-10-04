using DITS.HILI.WMS.ReceiveModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace DITS.HILI.WMS.ReceiveConfiguration
{
    public class ReceiveDetailConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ReceiveDetail>
    {
        public ReceiveDetailConfiguration()
            : this("dbo")
        {
        }

        public ReceiveDetailConfiguration(string schema)
        {
            ToTable(schema + ".rcv_receive_detail");
            HasKey(x => x.ReceiveDetailID);
            Property(x => x.ReceiveDetailID).IsRequired().HasColumnName("ReceiveDetailID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(x => x.ReceiveID).IsRequired().HasColumnName("ReceiveID").HasColumnType("uniqueidentifier");
            Property(x => x.ProductID).IsRequired().HasColumnName("ProductID").HasColumnType("uniqueidentifier");
            Property(x => x.Lot).HasColumnName("Lot").HasColumnType("nvarchar").HasMaxLength(50)
                                 .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            Property(x => x.ManufacturingDate).HasColumnName("ManufacturingDate").HasColumnType("datetime");
            Property(x => x.ExpirationDate).HasColumnName("ExpirationDate").HasColumnType("datetime");
            Property(x => x.Quantity).HasColumnName("Quantity").HasColumnType("decimal");
            Property(x => x.BaseQuantity).HasColumnName("BaseQuantity").HasColumnType("decimal");
            Property(x => x.ConversionQty).HasColumnName("ConversionQty").HasColumnType("decimal");
            Property(x => x.StockUnitID).HasColumnName("StockUnitID").HasColumnType("uniqueidentifier");
            Property(x => x.BaseUnitID).HasColumnName("BaseUnitID").HasColumnType("uniqueidentifier");
            Property(x => x.Price).HasColumnName("Price").HasColumnType("money");

            Property(x => x.ProductUnitPriceID).HasColumnName("ProductUnitPriceID").HasColumnType("uniqueidentifier");
            Property(x => x.ProductStatusID).HasColumnName("ProductStatusID").HasColumnType("uniqueidentifier");
            Property(x => x.ProductSubStatusID).HasColumnName("ProductSubStatusID").HasColumnType("uniqueidentifier");

            Property(x => x.PackageWeight).HasColumnName("PackageWeight").HasColumnType("float");
            Property(x => x.ProductWeight).HasColumnName("ProductWeight").HasColumnType("float");
            Property(x => x.ProductWidth).HasColumnName("ProductWidth").HasColumnType("float");
            Property(x => x.ProductLength).HasColumnName("ProductLength").HasColumnType("float");
            Property(x => x.ProductHeight).HasColumnName("ProductHeight").HasColumnType("float");
            Property(x => x.ReceiveDetailStatus).HasColumnName("ReceiveDetailStatus").HasColumnType("int");
            Property(x => x.IsSentInterface);

            Property(x => x.SupplierID).HasColumnName("SupplierID").HasColumnType("uniqueidentifier");

            Property(x => x.Remark).HasColumnName("Remark").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            HasRequired(x => x.Receive).WithMany(x => x.ReceiveDetailCollection).HasForeignKey(x => x.ReceiveID).WillCascadeOnDelete(false);

        }
    }
}
