using DITS.HILI.WMS.MasterModel.Core;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Core
{
    public class CustomResourceConfiguration : EntityTypeConfiguration<CustomResource>
    {
        public CustomResourceConfiguration()
            : this("dbo")
        { }

        public CustomResourceConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.ResourceId);

            // Properties
            Property(t => t.ResourceKey)
                .HasMaxLength(255);

            Property(t => t.ResourceValue)
                .HasMaxLength(255);

            Property(t => t.LanguageCode)
                .HasMaxLength(10);

            // Table & Column Mappings
            ToTable("core_resource");
            Property(t => t.ResourceId).HasColumnName("ResourceId");
            Property(t => t.ResourceKey).HasColumnName("ResourceKey");
            Property(t => t.ResourceValue).HasColumnName("ResourceValue");
            Property(t => t.LanguageCode).HasColumnName("LanguageCode");
            Property(x => x.DateModified).HasColumnName("DateModified");
        }
    }
}
