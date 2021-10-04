using DITS.HILI.WMS.MasterModel.Secure;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration
{

    public class UserAccountsConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<UserAccounts>
    {
        public UserAccountsConfiguration()
            : this("dbo")
        {
        }

        public UserAccountsConfiguration(string schema)
        {
            ToTable(schema + ".sys_user_accounts");
            HasKey(x => x.UserID);

            Property(x => x.UserID).IsRequired().HasColumnName("UserID").HasColumnType("uniqueidentifier")
                               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.EmployeeID).IsRequired().HasColumnName("EmployeeID").HasColumnType("uniqueidentifier");

            Property(x => x.UserName).IsRequired().HasColumnName("UserName").HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.Password).IsRequired().HasColumnName("PasswordHash").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.PasswordSalt).IsRequired().HasColumnName("PasswordSalt").HasColumnType("nvarchar").HasMaxLength(250);

            //Property(x => x.ProductOwnerID).IsOptional().HasColumnName("ProductOwnerID").HasColumnType("uniqueidentifier");

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");



            // Relationships

            HasRequired(t => t.Employee)
                .WithMany(t => t.UserAccountCollection)
                .HasForeignKey(d => d.EmployeeID);


        }
    }
}
