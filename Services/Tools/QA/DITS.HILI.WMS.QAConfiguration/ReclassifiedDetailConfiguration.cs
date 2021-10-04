using DITS.HILI.WMS.QAModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.QAConfiguration
{
    public class ReclassifiedDetailConfiguration : EntityTypeConfiguration<ReclassifiedDetail>
    {
        public ReclassifiedDetailConfiguration() : this("dbo")
        {

        }
        public ReclassifiedDetailConfiguration(string schema)
        {
            // Primary Key
            this.HasKey(t => t.ReclassifiedDetailID);

            // Properties
            this.Property(t => t.PalletCode)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable(schema + ".qa_reclassified_detail");
            this.Property(t => t.ReclassifiedDetailID).HasColumnName("ReclassifiedDetailID");
            this.Property(t => t.ReclassifiedID).HasColumnName("ReclassifiedID");
            this.Property(t => t.PalletCode).HasColumnName("PalletCode");
            this.Property(t => t.ReclassifiedQty).HasColumnName("ReclassifiedQty");
            this.Property(t => t.ReclassifiedUnitID).HasColumnName("ReclassifiedUnitID");
            this.Property(t => t.ReclassifiedBaseQty).HasColumnName("ReclassifiedBaseQty");
            this.Property(t => t.ReclassifiedBaseUnitID).HasColumnName("ReclassifiedBaseUnitID");
            this.Property(t => t.ConversionQty).HasColumnName("ConversionQty");
            this.Property(t => t.ReclassifiedDetailStatus).HasColumnName("ReclassifiedDetailStatus");
            this.Property(t => t.DamageID).HasColumnName("DamageID");

            // Relationships

            this.HasRequired(t => t.Reclassified)
                .WithMany(t => t.ReclassifiedDetailCollection)
                .HasForeignKey(d => d.ReclassifiedID);
        }
    }
}
