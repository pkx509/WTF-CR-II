using DITS.HILI.WMS.MasterModel.Secure;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Secure
{
    public class PermissionConfiguration : EntityTypeConfiguration<Permission>
    {
        public PermissionConfiguration() : this("dbo")
        { }
        public PermissionConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.PermissionID);

            // Properties 

            Property(t => t.Action)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable(schema + ".sys_permission");
            Property(t => t.PermissionID).HasColumnName("PermissionID");
            Property(t => t.Action).HasColumnName("Action");
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
        }
    }
}
