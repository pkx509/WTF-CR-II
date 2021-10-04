using DITS.HILI.WMS.PutAwayModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.PutAwayConfiguration
{
    public class PutAwayJobAssignConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PutAwayJobAssign>
    {
        public PutAwayJobAssignConfiguration()
            : this("dbo")
        {
        }

        public PutAwayJobAssignConfiguration(string schema)
        {
            ToTable(schema + ".pw_putaway_assign");

            // Primary Key
            this.HasKey(t => new { t.PutAwayJobID, t.EmployeeID });

            // Properties
            // Table & Column Mappings
            this.ToTable("pw_putaway_assign");
            this.Property(t => t.PutAwayJobID).HasColumnName("PutAwayJobID");
            this.Property(t => t.EmployeeID).HasColumnName("EmployeeID");
            this.Property(x => x.Remark).HasColumnName("Remark").HasColumnType("nvarchar").HasMaxLength(250);
            this.Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            this.Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            this.Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            this.Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            this.Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            // Relationships
            this.HasRequired(t => t.PutAway)
                .WithMany(t => t.AssignJobCollection)
                .HasForeignKey(d => d.PutAwayJobID);

        }
    }
}
