using DITS.HILI.WMS.MasterModel.Contacts;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ProductOwnerConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductOwner>
    {
        public ProductOwnerConfiguration()
            : this("dbo")
        {
        }

        public ProductOwnerConfiguration(string schema)
        {
            ToTable(schema + ".sys_productowner");
            HasKey(x => x.ProductOwnerID);
            Property(x => x.ProductOwnerID).IsRequired().HasColumnName("ProductOwnerID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.BranchID).HasColumnName(@"BranchID").HasColumnType("uniqueidentifier").IsOptional();

            Property(x => x.Name).IsRequired().HasColumnName("Name").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").HasMaxLength(250);

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

            HasOptional(a => a.Branch).WithMany(b => b.ProductOwnerCollection).HasForeignKey(c => c.BranchID).WillCascadeOnDelete(false);

        }
    }
}
