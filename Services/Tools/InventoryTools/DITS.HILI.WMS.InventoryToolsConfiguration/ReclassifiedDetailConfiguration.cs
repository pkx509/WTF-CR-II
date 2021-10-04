using DITS.HILI.WMS.InventoryToolsModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{
    public class ReclassifiedDetailConfiguration : EntityTypeConfiguration<ReclassifiedDetail>
    {
        public ReclassifiedDetailConfiguration() : this("dbo")
        {

        }
        public ReclassifiedDetailConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.ReclassifiedDetailID);

            // Properties
            Property(t => t.PalletCode)
                .IsRequired()
                .HasMaxLength(40);

            // Table & Column Mappings
            ToTable(schema + ".qa_reclassified_detail");
            Property(t => t.ReclassifiedDetailID).HasColumnName("ReclassifiedDetailID");
            Property(t => t.ReclassifiedID).HasColumnName("ReclassifiedID");
            Property(t => t.PalletCode).HasColumnName("PalletCode");
            Property(t => t.ReclassifiedQty).HasColumnName("ReclassifiedQty");
            Property(t => t.ReclassifiedUnitID).HasColumnName("ReclassifiedUnitID");
            Property(t => t.ReclassifiedBaseQty).HasColumnName("ReclassifiedBaseQty");
            Property(t => t.ReclassifiedBaseUnitID).HasColumnName("ReclassifiedBaseUnitID");
            Property(t => t.ConversionQty).HasColumnName("ConversionQty");
            //this.Property(t => t.ReclassifiedDetailStatus).HasColumnName("ReclassifiedDetailStatus");  


            // Relationships

            HasRequired(t => t.Reclassified)
                .WithMany(t => t.ReclassifiedDetailCollection)
                .HasForeignKey(d => d.ReclassifiedID);
        }
    }
}
