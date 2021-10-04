using DITS.HILI.WMS.MasterModel.Contacts;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class SupplierOfProductOwnerConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<SupplierOfProductOwner>
    {
        public SupplierOfProductOwnerConfiguration()
            : this("dbo")
        {
        }

        public SupplierOfProductOwnerConfiguration(string schema)
        {
            ToTable(schema + ".sys_supplier_productowner");
            HasKey(x => new { x.ProductOwnerID, x.SupplierID });

            Property(x => x.ProductOwnerID).IsRequired().HasColumnName("ProductOwnerID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.SupplierID).IsRequired().HasColumnName("SupplierID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(x => x.Contact).WithMany(x => x.SupplierOfProductOwnerCollection).HasForeignKey(x => x.SupplierID).WillCascadeOnDelete(false);
            HasRequired(x => x.ProductOwner).WithMany(x => x.SupplierOfProductOwnerCollection).HasForeignKey(x => x.ProductOwnerID).WillCascadeOnDelete(false);

        }
    }
}
