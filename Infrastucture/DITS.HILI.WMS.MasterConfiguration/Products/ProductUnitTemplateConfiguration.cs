using DITS.HILI.WMS.Master.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Configuration.Products
{
    public class ProductUnitTemplateConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductUnitTemplate>
    {
        public ProductUnitTemplateConfiguration()
            : this("dbo")
        {
        }

        public ProductUnitTemplateConfiguration(string schema)
        {
            ToTable(schema + ".sys_product_unit_template");
            HasKey(x => x.ID);
            Property(x => x.ID).IsRequired().HasColumnName("ProductUnitTemplateID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

        }

    }
}
