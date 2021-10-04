using DITS.HILI.WMS.MasterModel.Contacts;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class SubDistrictConfiguration : EntityTypeConfiguration<SubDistrict>
    {
        public SubDistrictConfiguration()
        {
            // Primary Key
            HasKey(t => new { t.SubDistrict_Id, t.District_Id, t.SubDistrict_Code, t.Name, t.PostCode, t.UserCreated, t.DateCreated, t.IsActive });

            // Properties
            Property(t => t.SubDistrict_Code)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(250);

            Property(t => t.NameEN)
                .HasMaxLength(250);

            Property(t => t.PostCode)
                .IsRequired()
                .HasMaxLength(5);

            Property(t => t.UserCreated)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("sys_sub_district");
            Property(t => t.SubDistrict_Id).HasColumnName("SubDistrict_Id");
            Property(t => t.District_Id).HasColumnName("District_Id");
            Property(t => t.SubDistrict_Code).HasColumnName("SubDistrict_Code");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.NameEN).HasColumnName("NameEN");
            Property(t => t.PostCode).HasColumnName("PostCode");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");
            Property(t => t.IsActive).HasColumnName("IsActive");
        }
    }
}
