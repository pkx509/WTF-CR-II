using DITS.HILI.WMS.MasterModel.Contacts;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class DistrictConfiguration : EntityTypeConfiguration<District>
    {
        public DistrictConfiguration()
        {
            // Primary Key
            HasKey(t => t.District_Id);

            // Properties
            Property(t => t.District_Code)
                .IsRequired()
                .HasMaxLength(10);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(250);

            Property(t => t.NameEN)
                .HasMaxLength(250);

            Property(t => t.PostCode)
                .HasMaxLength(5);

            // Table & Column Mappings
            ToTable("sys_district");
            Property(t => t.District_Id).HasColumnName("District_Id");
            Property(t => t.Province_Id).HasColumnName("Province_Id");
            Property(t => t.District_Code).HasColumnName("District_Code");
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
