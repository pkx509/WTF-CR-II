using DITS.HILI.WMS.MasterModel.Contacts;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ProvinceConfiguration : EntityTypeConfiguration<Province>
    {
        public ProvinceConfiguration()
        {
            // Primary Key
            HasKey(t => t.Province_Id);

            // Properties
            Property(t => t.Province_Code)
                .IsRequired()
                .HasMaxLength(10);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(250);

            Property(t => t.NameEN)
                .HasMaxLength(250);

            // Table & Column Mappings
            ToTable("sys_province");
            Property(t => t.Province_Id).HasColumnName("Province_Id");
            Property(t => t.Region_Id).HasColumnName("Region_Id");
            Property(t => t.Country_Id).HasColumnName("Country_Id");
            Property(t => t.Province_Code).HasColumnName("Province_Code");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.NameEN).HasColumnName("NameEN");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");
            Property(t => t.IsActive).HasColumnName("IsActive");
        }
    }
}
