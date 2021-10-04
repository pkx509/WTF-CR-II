using DITS.HILI.WMS.MasterModel.Secure;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Secure
{
    public class UserGroupConfiguration : EntityTypeConfiguration<UserGroups>
    {
        public UserGroupConfiguration()
            : this("dbo")
        {
        }

        public UserGroupConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.GroupID);

            // Properties
            Property(t => t.GroupName)
                .HasMaxLength(50);

            Property(t => t.GroupDescription)
                .HasMaxLength(250);

            // Table & Column Mappings
            ToTable(schema + ".sys_user_groups");
            Property(t => t.GroupID).HasColumnName("GroupID");
            Property(t => t.GroupName).HasColumnName("GroupName");
            Property(t => t.GroupDescription).HasColumnName("GroupDescription");
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
        }
    }
}
