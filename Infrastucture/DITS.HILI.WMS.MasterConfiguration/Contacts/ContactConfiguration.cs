using DITS.HILI.WMS.MasterModel.Contacts;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ContactConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Contact>
    {
        public ContactConfiguration()
            : this("dbo")
        {
        }

        public ContactConfiguration(string schema)
        {
            ToTable(schema + ".sys_contact");
            HasKey(x => x.ContactID);

            Property(x => x.ContactID).IsRequired().HasColumnName("ContactID").HasColumnType("uniqueidentifier")
                               .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(x => x.Code).IsRequired().HasColumnName("Code").HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(150);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.Telephone).HasColumnName(@"Telephone").HasColumnType("nvarchar").IsOptional().HasMaxLength(16);
            Property(x => x.Fax).HasColumnName(@"Fax").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.Email).HasColumnName(@"Email").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);

            Property(x => x.Remark).HasColumnName(@"Remark").HasColumnType("nvarchar").IsOptional().HasMaxLength(250);
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

        }
    }
}
