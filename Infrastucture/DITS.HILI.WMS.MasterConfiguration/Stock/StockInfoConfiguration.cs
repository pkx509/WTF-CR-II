using DITS.HILI.WMS.MasterModel.Stock;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class StockInfoConfiguration : EntityTypeConfiguration<StockInfo>
    {
        public StockInfoConfiguration()
            : this("dbo")
        { }

        public StockInfoConfiguration(string schema)
        {


            // Primary Key
            HasKey(t => t.StockInfoID);

            // Properties
            Property(t => t.Lot)
                .HasMaxLength(50);

            Property(t => t.Remark)
                .HasMaxLength(250);

            // Table & Column Mappings
            ToTable(schema + ".stock_info");
            Property(t => t.StockInfoID).HasColumnName("StockInfoID");
            Property(t => t.ProductOwnerID).HasColumnName("ProductOwnerID");
            Property(t => t.SupplierID).HasColumnName("SupplierID");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.Lot).HasColumnName("Lot");
            Property(t => t.ManufacturingDate).HasColumnName("ManufacturingDate");
            Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");
            Property(t => t.ConversionQty).HasColumnName("ConversionQty");
            Property(t => t.StockUnitID).HasColumnName("StockUnitID");
            Property(t => t.BaseUnitID).HasColumnName("BaseUnitID");
            Property(t => t.Price).HasColumnName("Price");
            Property(t => t.ProductUnitPriceID).HasColumnName("ProductUnitPriceID");
            Property(t => t.ProductStatusID).HasColumnName("ProductStatusID");
            Property(t => t.ProductSubStatusID).HasColumnName("ProductSubStatusID");
            Property(t => t.PackageWeight).HasColumnName("PackageWeight");
            Property(t => t.ProductWeight).HasColumnName("ProductWeight");
            Property(t => t.ProductWidth).HasColumnName("ProductWidth");
            Property(t => t.ProductLength).HasColumnName("ProductLength");
            Property(t => t.ProductHeight).HasColumnName("ProductHeight");
            Property(t => t.Remark).HasColumnName("Remark");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");

            // Relationships
            //this.HasRequired(t => t.Supplier)
            //    .WithMany(t => t.StockInfoCollection)
            //    .HasForeignKey(d => d.SupplierID);
            //this.HasRequired(t => t.Product)
            //    .WithMany(t => t.StockInfoCollection)
            //    .HasForeignKey(d => d.ProductID);
            //this.HasRequired(t => t.ProductStatus)
            //    .WithMany(t => t.StockInfoCollection)
            //    .HasForeignKey(d => d.ProductStatusID);
            //this.HasRequired(t => t.ProductSubStatus)
            //    .WithMany(t => t.StockInfoCollection)
            //    .HasForeignKey(d => d.ProductSubStatusID);
            //this.HasRequired(t => t.ProductUOM
            //    .WithMany(t => t.StockInfoCollection)
            //    .HasForeignKey(d => d.BaseUnitID);
            //this.HasOptional(t => t.sys_product_uom1)
            //    .WithMany(t => t.stock_info1)
            //    .HasForeignKey(d => d.ProductUnitPriceID);
            //this.HasRequired(t => t.sys_product_uom2)
            //    .WithMany(t => t.stock_info2)
            //    .HasForeignKey(d => d.StockUnitID);
            //this.HasRequired(t => t.sys_productowner)
            //    .WithMany(t => t.stock_info)
            //    .HasForeignKey(d => d.ProductOwnerID);

        }
    }
}
