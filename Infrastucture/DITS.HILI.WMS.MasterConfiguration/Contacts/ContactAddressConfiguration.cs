using DITS.HILI.WMS.MasterModel.Contacts;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ContactAddressConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ContactAddress>
    {
        public ContactAddressConfiguration()
            : this("dbo")
        {
        }

        public ContactAddressConfiguration(string schema)
        {
            ToTable(schema + ".sys_contact_address");
            HasKey(x => new { x.ContactID, x.Sequence });


            Property(x => x.ContactID).IsRequired().HasColumnName("ContactID").HasColumnType("uniqueidentifier")
                                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.Sequence).IsRequired().HasColumnName("Sequence").HasColumnType("int");
            Property(x => x.Address).HasColumnName(@"Address").HasColumnType("nvarchar").HasMaxLength(300);
            Property(x => x.PostCode).HasColumnName(@"PostCode").HasColumnType("nvarchar").HasMaxLength(10);
            Property(t => t.SubDistrict_Id).HasColumnName("SubDistrict_Id").HasColumnType("uniqueidentifier");
            Property(t => t.District_Id).HasColumnName("District_Id").HasColumnType("uniqueidentifier");
            Property(t => t.Province_Id).HasColumnName("Province_Id").HasColumnType("uniqueidentifier");
            Property(x => x.Telephone).HasColumnName(@"Telephone").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Fax).HasColumnName(@"Fax").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Email).HasColumnName(@"Email").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.WebSite).HasColumnName(@"WWW").HasColumnType("nvarchar").HasMaxLength(250);

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");


            HasRequired(x => x.Contact).WithMany(x => x.AddressCollection).HasForeignKey(x => x.ContactID).WillCascadeOnDelete(false);

        }
    }
}
