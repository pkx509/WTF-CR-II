using DITS.HILI.WMS.MasterModel.Secure;

namespace DITS.HILI.WMS.MasterConfiguration
{

    public class UserInRoleConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<UserInRole>
    {
        public UserInRoleConfiguration()
            : this("dbo")
        {
        }

        public UserInRoleConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => new { t.RoleID, t.UserID });


            // Table & Column Mappings
            ToTable(schema + ".sys_users_in_roles");
            Property(t => t.RoleID).HasColumnName("RoleID");
            Property(t => t.UserID).HasColumnName("UserID");
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            // Relationships
            HasRequired(t => t.Role)
                .WithMany(t => t.UserInRoleCollection)
                .HasForeignKey(d => d.RoleID);

            HasRequired(t => t.UserAccount)
                .WithMany(t => t.UserInRoleCollection)
                .HasForeignKey(d => d.UserID);
        }
    }
}
