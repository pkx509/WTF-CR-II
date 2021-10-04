using DITS.HILI.WMS.PutAwayModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace DITS.HILI.WMS.PutAwayConfiguration
{
    public class PutAwayItemConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PutAwayItem>
    {
        public PutAwayItemConfiguration()
            : this("dbo")
        {
        }

        public PutAwayItemConfiguration(string schema)
        {
            ToTable(schema + ".pw_putaway_item");
            HasKey(x => x.PutAwayItemID);

            Property(x => x.PutAwayItemID).IsRequired().HasColumnName("PutAwayItemID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.InstanceID).HasColumnName("InstanceID").HasColumnType("uniqueidentifier");
            Property(x => x.DocumentTypeID).HasColumnName("DocumentTypeID").HasColumnType("uniqueidentifier");
            //Property(x => x.PackagePrevID).HasColumnName("PackagePrevID").HasColumnType("uniqueidentifier");
            // Property(x => x.PackageNextID).HasColumnName("PackageNextID").HasColumnType("uniqueidentifier");
            Property(x => x.ReferenceCode).HasColumnName("ReferenceCode").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.StockBalanceID).HasColumnName("StockBalanceID").HasColumnType("uniqueidentifier");

            Property(x => x.ProductOwnerID).IsRequired().HasColumnName("ProductOwnerID").HasColumnType("uniqueidentifier");
            Property(x => x.ProductID).IsRequired().HasColumnName("ProductID").HasColumnType("uniqueidentifier");
            Property(x => x.Lot).HasColumnName("Lot").HasColumnType("nvarchar").HasMaxLength(50)
                                 .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(x => x.PalletCode).HasColumnName("PalletCode").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ManufacturingDate).HasColumnName("ManufacturingDate").HasColumnType("datetime");
            Property(x => x.ExpirationDate).HasColumnName("ExpirationDate").HasColumnType("datetime");
            Property(x => x.ProductStatusID).IsRequired().HasColumnName("ProductStatusID").HasColumnType("uniqueidentifier");
            Property(x => x.ProductSubStatusID).IsRequired().HasColumnName("ProductSubStatusID").HasColumnType("uniqueidentifier");
            Property(x => x.PackageWeight).IsRequired().HasColumnName("PackageWeight").HasColumnType("float");
            Property(x => x.ProductWeight).IsRequired().HasColumnName("ProductWeight").HasColumnType("float");
            Property(x => x.ProductWidth).IsRequired().HasColumnName("ProductWidth").HasColumnType("float");
            Property(x => x.ProductLength).IsRequired().HasColumnName("ProductLength").HasColumnType("float");
            Property(x => x.ProductHeight).IsRequired().HasColumnName("ProductHeight").HasColumnType("float");

            Property(x => x.Quantity).IsRequired().HasColumnName("Quantity").HasColumnType("decimal");
            Property(x => x.BaseQuantity).IsRequired().HasColumnName("BaseQuantity").HasColumnType("decimal");
            Property(x => x.ConversionQty).IsRequired().HasColumnName("ConversionQty").HasColumnType("decimal");
            Property(x => x.StockUnitID).IsRequired().HasColumnName("StockUnitID").HasColumnType("uniqueidentifier");
            Property(x => x.BaseUnitID).IsRequired().HasColumnName("BaseUnitID").HasColumnType("uniqueidentifier");

            Property(x => x.RemainQuantity).HasColumnName("RemainQuantity").HasColumnType("decimal");

            Property(x => x.SupplierID).HasColumnName("SupplierID").HasColumnType("uniqueidentifier");
            Property(x => x.Price).HasColumnName("Price").HasColumnType("money");
            Property(x => x.ProductUnitPriceID).HasColumnName("ProductUnitPriceID").HasColumnType("uniqueidentifier");
            Property(x => x.FromLocationID).HasColumnName("FromLocationID").HasColumnType("uniqueidentifier");
            Property(x => x.SuggestionLocationID).HasColumnName("SuggestionLocationID").HasColumnType("uniqueidentifier");


            Property(x => x.Remark).HasColumnName("Remark").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

        }
    }
}
