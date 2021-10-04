using DITS.HILI.WMS.PutAwayModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.PutAwayConfiguration
{
    public class PutAwayMapConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PutAwayMap>
    {
        public PutAwayMapConfiguration()
            : this("dbo")
        {
        }

        public PutAwayMapConfiguration(string schema)
        {
            ToTable(schema + ".pw_putaway_map");

            HasKey(x => new { x.PutAwayID, x.PutAwayItemID });

            Property(x => x.PutAwayID).IsRequired().HasColumnName("PutAwayID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.PutAwayItemID).IsRequired().HasColumnName("PutAwayItemID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Remark).HasColumnName("Remark").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            //HasRequired(x => x.PutAway).WithMany(x => x.PutAwayMapCollection).HasForeignKey(x => x.PutAwayID).WillCascadeOnDelete(true);
            //HasRequired(x => x.PutAwayItem).WithMany(x => x.PutAwayCollection).HasForeignKey(x => x.PutAwayItemID).WillCascadeOnDelete(true);
        }
    }
}
