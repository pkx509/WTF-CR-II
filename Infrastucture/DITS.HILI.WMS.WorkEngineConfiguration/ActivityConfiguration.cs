using DITS.HILI.WMS.WorkEngineModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngineConfiguration
{
    public class ActivityConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Activity>
    {
        public ActivityConfiguration()
            : this("dbo")
        {
        }

        public ActivityConfiguration(string schema)
        {
            ToTable(schema + ".core_activity");
            HasKey(x => x.ActivityID);

            Property(x => x.ActivityID).IsRequired().HasColumnName("ActivityID").HasColumnType("uniqueidentifier")
                                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.ReferenceID).IsRequired().HasColumnName("ReferenceID").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ActivityDate).IsRequired().HasColumnName("ActivityDate").HasColumnType("datetime");
            Property(x => x.Source).HasColumnName("Source").HasColumnType("uniqueidentifier");
            Property(x => x.Destination).HasColumnName("Destination").HasColumnType("uniqueidentifier");
            Property(x => x.Sequence).HasColumnName("Sequence").HasColumnType("int");
            Property(x => x.IsComplete).HasColumnName("IsComplete").HasColumnType("bit");
             
        }
    }
}
