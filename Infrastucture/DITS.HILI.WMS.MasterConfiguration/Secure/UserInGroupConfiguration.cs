using DITS.HILI.WMS.MasterModel.Secure;

namespace DITS.HILI.WMS.MasterConfiguration
{

    public class UserInGroupConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<UserInGroup>
    {
        public UserInGroupConfiguration()
            : this("dbo")
        {
        }

        public UserInGroupConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => new { t.UserID, t.GroupID });


            // Table & Column Mappings
            ToTable(schema + ".sys_users_in_groups");
            Property(t => t.UserID).HasColumnName("UserID");
            Property(t => t.GroupID).HasColumnName("GroupID");
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            // Relationships
            HasRequired(t => t.UserAccount)
                .WithMany(t => t.UserInGroupCollection)
                .HasForeignKey(d => d.UserID);

            HasRequired(t => t.UserGroup)
                .WithMany(t => t.UserInGroupCollection)
                .HasForeignKey(d => d.GroupID);


        }
    }
}
