using DITS.HILI.WMS.InventoryToolsModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{

    public class AdjustTypeConfiguration : EntityTypeConfiguration<AdjustType>
    {
        public AdjustTypeConfiguration() : this("dbo")
        {

        }
        public AdjustTypeConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.AdjustTypeID);

            // Properties
            Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(20);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("adj_adjust_type");
            Property(t => t.AdjustTypeID).HasColumnName("AdjustTypeID");
            Property(t => t.Value).HasColumnName("Value");
            Property(t => t.Name).HasColumnName("Name");
        }
    }
}
