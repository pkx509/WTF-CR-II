using DITS.HILI.WMS.MasterModel.Secure;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Secure
{
    public class PermissionInRoleConfiguration : EntityTypeConfiguration<PermissionInRole>
    {
        public PermissionInRoleConfiguration() : this("dbo")
        { }
        public PermissionInRoleConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => new { t.PermissionID, t.RoleID });


            // Table & Column Mappings
            ToTable(schema + ".sys_permission_in_roles");
            Property(t => t.PermissionID).HasColumnName("PermissionID");
            Property(t => t.RoleID).HasColumnName("RoleID");
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            // Relationships
            HasRequired(t => t.Permissions)
                .WithMany(t => t.PermissionInRoleCollection)
                .HasForeignKey(d => d.PermissionID);
            HasRequired(t => t.Role)
                .WithMany(t => t.PermissionInRoleCollection)
                .HasForeignKey(d => d.RoleID);

        }
    }
}
