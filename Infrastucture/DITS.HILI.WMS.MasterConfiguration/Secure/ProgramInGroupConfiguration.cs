using DITS.HILI.WMS.MasterModel.Secure;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ProgramInGroupConfiguration : EntityTypeConfiguration<ProgramInGroup>
    {
        public ProgramInGroupConfiguration()
            : this("dbo")
        { }

        public ProgramInGroupConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => new { t.GroupID, t.ProgramID });


            // Table & Column Mappings
            ToTable(schema + ".sys_program_in_groups");
            Property(t => t.GroupID).HasColumnName("GroupID");
            Property(t => t.ProgramID).HasColumnName("ProgramID");
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            // Relationships
            HasRequired(t => t.Programs)
                .WithMany(t => t.ProgramInGroupCollection)
                .HasForeignKey(d => d.ProgramID);
            HasRequired(t => t.UserGroup)
                .WithMany(t => t.ProgramInGroupCollection)
                .HasForeignKey(d => d.GroupID);

        }
    }
}
