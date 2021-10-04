using DITS.HILI.WMS.MasterModel.Products;

namespace DITS.HILI.WMS.MasterConfiguration.Products
{
    public class ProductOfProductOwnerConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductOfProductOwner>
    {
        public ProductOfProductOwnerConfiguration()
            : this("dbo")
        {
        }

        public ProductOfProductOwnerConfiguration(string schema)
        {
            ToTable(schema + ".sys_product_of_productowner");
            HasKey(x => new { x.ProductID, x.ProductOwnerID });

            Property(x => x.ProductID).IsRequired().HasColumnName("ProductID").HasColumnType("uniqueidentifier")
                            .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(x => x.ProductOwnerID).IsRequired().HasColumnName("ProductOwnerID").HasColumnType("uniqueidentifier")
                            .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(t => t.Remark).HasColumnName("Remark").HasMaxLength(250);
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");

            Property(t => t.BranchID).HasColumnName("BranchID");

            HasRequired(x => x.Product).WithMany(x => x.ProductOfProductOwnerCollection).HasForeignKey(x => x.ProductID).WillCascadeOnDelete(false);
            HasRequired(x => x.ProductOwner).WithMany(x => x.ProductOfProductOwnerCollection).HasForeignKey(x => x.ProductOwnerID).WillCascadeOnDelete(false);
            HasOptional(t => t.Branch)
               .WithMany(t => t.ProductOfProductOwnerCollection)
               .HasForeignKey(d => d.BranchID);
        }
    }
}
