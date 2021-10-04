using DITS.HILI.WMS.Stock.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class StockInfoConfiguration : EntityTypeConfiguration<StockInfo>
    {
        public StockInfoConfiguration()
            : this("dbo")
        { }

        public StockInfoConfiguration(string schema)
        {
            ToTable(schema + ".stock_info");

            HasKey(x => x.StockInfoID);

            Property(x => x.StockInfoID).IsRequired().HasColumnName("StockInfoID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.ProductOwnerID).IsRequired().HasColumnName("ProductOwnerID").HasColumnType("uniqueidentifier");
            Property(x => x.SupplierID).IsRequired().HasColumnName("SupplierID").HasColumnType("uniqueidentifier");
            Property(x => x.ProductID).IsRequired().HasColumnName("ProductID").HasColumnType("uniqueidentifier");
            Property(x => x.Lot).HasColumnName("Lot").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ManufacturingDate).HasColumnName("ManufacturingDate").HasColumnType("datetime");
            Property(x => x.ExpirationDate).HasColumnName("ExpirationDate").HasColumnType("datetime");
            Property(x => x.ConversionQty).IsRequired().HasColumnName("ConversionQty").HasColumnType("decimal");
            Property(x => x.StockUnitID).IsRequired().HasColumnName("StockUnitID").HasColumnType("uniqueidentifier");
            Property(x => x.BaseUnitID).IsRequired().HasColumnName("BaseUnitID").HasColumnType("uniqueidentifier");
            Property(x => x.Price).IsOptional().HasColumnName("Price").HasColumnType("money");
            Property(x => x.UnitPriceID).IsOptional().HasColumnName("ProductUnitPriceID").HasColumnType("uniqueidentifier");
            Property(x => x.ProductStatusID).HasColumnName("ProductStatusID").HasColumnType("uniqueidentifier");
            Property(x => x.ProductSubStatusID).HasColumnName("ProductSubStatusID").HasColumnType("uniqueidentifier");
            Property(x => x.PackageWeight).IsRequired().HasColumnName("PackageWeight").HasColumnType("float");
            Property(x => x.ProductWeight).IsRequired().HasColumnName("ProductWeight").HasColumnType("float");
            Property(x => x.ProductWidth).IsRequired().HasColumnName("ProductWidth").HasColumnType("float");
            Property(x => x.ProductLength).IsRequired().HasColumnName("ProductLength").HasColumnType("float");
            Property(x => x.ProductHeight).IsRequired().HasColumnName("ProductHeight").HasColumnType("float");

            Property(x => x.Remark).HasColumnName("Remark").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            HasRequired(x => x.ProductOwner).WithMany(x => x.StockInfoCollection).HasForeignKey(x => x.ProductOwnerID).WillCascadeOnDelete(false);
            HasRequired(x => x.Supplier).WithMany(x => x.StockInfoCollection).HasForeignKey(x => x.SupplierID).WillCascadeOnDelete(false);
            HasRequired(x => x.Product).WithMany(x => x.StockInfoCollection).HasForeignKey(x => x.ProductID).WillCascadeOnDelete(false);
            HasRequired(x => x.ProductStatus).WithMany(x => x.StockInfoCollection).HasForeignKey(x => x.ProductStatusID).WillCascadeOnDelete(false);
            HasRequired(x => x.ProductSubStatus).WithMany(x => x.StockInfoCollection).HasForeignKey(x => x.ProductSubStatusID).WillCascadeOnDelete(false);

            HasRequired(x => x.ProductUOM).WithMany(x => x.StockInfoCollection).HasForeignKey(x => x.StockUnitID).WillCascadeOnDelete(false);
            HasRequired(x => x.ProductBaseUOM).WithMany(x => x.StockInfoBaseUOMCollection).HasForeignKey(x => x.BaseUnitID).WillCascadeOnDelete(false);
            HasOptional(x => x.ProductPriceUOM).WithMany(x => x.StockInfoPriceUOMCollection).HasForeignKey(x => x.UnitPriceID).WillCascadeOnDelete(false);

        }
    }
}
