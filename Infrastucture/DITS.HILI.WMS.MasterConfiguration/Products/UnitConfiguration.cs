using DITS.HILI.WMS.MasterModel.Products;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class UnitConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Units>
    {
        public UnitConfiguration()
            : this("dbo")
        {
        }

        public UnitConfiguration(string schema)
        {
            ToTable(schema + ".sys_units");
            HasKey(x => x.UnitID);

            Property(x => x.UnitID).IsRequired().HasColumnName("UnitID").HasColumnType("uniqueidentifier")
                               .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.ShortName).IsOptional().HasColumnName("ShortName").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

        }
    }
}
