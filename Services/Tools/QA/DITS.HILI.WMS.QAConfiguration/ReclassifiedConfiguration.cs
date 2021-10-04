using DITS.HILI.WMS.QAModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.QAConfiguration
{
    public class ReclassifiedConfiguration : EntityTypeConfiguration<Reclassified>
    {
        public ReclassifiedConfiguration() : this("dbo")
        {

        }
        public ReclassifiedConfiguration(string schema)
        {
            // Primary Key
            this.HasKey(t => t.ReclassifiedID);

            // Properties
            this.Property(t => t.ReclassifiedCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ReclassFromLot)
                .HasMaxLength(20);

            this.Property(t => t.ReclassToLot)
                .HasMaxLength(20);

            this.Property(t => t.Description)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable(schema + ".qa_reclassified");
            this.Property(t => t.ReclassifiedID).HasColumnName("ReclassifiedID");
            this.Property(t => t.ReclassifiedCode).HasColumnName("ReclassifiedCode");
            this.Property(t => t.ReclassFromLot).HasColumnName("ReclassFromLot");
            this.Property(t => t.ReclassToLot).HasColumnName("ReclassToLot");
            this.Property(t => t.ReclassStatus).HasColumnName("ReclassStatus");
            this.Property(t => t.ApproveDate).HasColumnName("ApproveDate");
            this.Property(t => t.DamageID).HasColumnName("DamageID");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.MFGTimeFrom).HasColumnName("MFGTimeFrom");
            this.Property(t => t.MFGTimeEnd).HasColumnName("MFGTimeEnd");
            this.Property(t => t.ProductID).HasColumnName("ProductID");
            this.Property(t => t.LineID).HasColumnName("LineID");
            this.Property(t => t.MFGDate).HasColumnName("MFGDate");
            this.Property(t => t.EXPDate).HasColumnName("EXPDate");
        }
    }
}
