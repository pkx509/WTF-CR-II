using DITS.HILI.WMS.MasterModel.Contacts;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class CustomerOfProductOwnerConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<CustomerOfProductOwner>
    {
        public CustomerOfProductOwnerConfiguration()
            : this("dbo")
        {
        }

        public CustomerOfProductOwnerConfiguration(string schema)
        {
            ToTable(schema + ".sys_customer_productowner");
            HasKey(x => new { x.ProductOwnerID, x.CustomerID });

            Property(x => x.ProductOwnerID).IsRequired().HasColumnName("ProductOwnerID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.CustomerID).IsRequired().HasColumnName("CustomerID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(x => x.Contact).WithMany(x => x.CustomerOfProductOwnerCollection).HasForeignKey(x => x.CustomerID).WillCascadeOnDelete(false);
            HasRequired(x => x.ProductOwner).WithMany(x => x.CustomerOfProductOwnerCollection).HasForeignKey(x => x.ProductOwnerID).WillCascadeOnDelete(false);
        }
    }
}