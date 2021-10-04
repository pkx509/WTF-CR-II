using DITS.HILI.WMS.MasterModel.Contacts;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ContactInTypeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ContactInType>
    {
        public ContactInTypeConfiguration()
            : this("dbo")
        {
        }

        public ContactInTypeConfiguration(string schema)
        {
            ToTable(schema + ".sys_contact_in_type");
            HasKey(x => new { x.ContactID, x.ContactType });

            Property(x => x.CusContactName).HasColumnName(@"CusContactName").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.CusContactTel).HasColumnName(@"CusContactTel").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);
            Property(x => x.CusContactEmail).HasColumnName(@"CusContactEmail").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.CusContactMobile).HasColumnName(@"CusContactMobile").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);


            Property(x => x.ContactID).IsRequired().HasColumnName("ContactID").HasColumnType("uniqueidentifier")
                                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.ContactType).IsRequired().HasColumnName("ContactType").HasColumnType("int");

            HasRequired(x => x.Contact).WithMany(x => x.ContactInTypeCollection).HasForeignKey(x => x.ContactID).WillCascadeOnDelete(false);
        }
    }
}
