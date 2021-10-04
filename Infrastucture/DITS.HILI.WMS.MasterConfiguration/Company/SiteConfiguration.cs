using DITS.HILI.WMS.MasterModel.Companies;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class SiteConfiguration : EntityTypeConfiguration<Site>
    {
        public SiteConfiguration() : this("dbo")
        {
        }

        public SiteConfiguration(string schema)
        {

            // Primary Key
            HasKey(t => t.SiteID);

            // Properties
            Property(t => t.SiteName)
                .HasMaxLength(250);

            Property(t => t.SiteAdress)
                .HasMaxLength(250);

            Property(t => t.SiteRoad)
                .HasMaxLength(250);

            Property(t => t.SitePostCode)
                .HasMaxLength(50);

            Property(t => t.SiteCountry)
                .HasMaxLength(250);

            Property(t => t.SiteTel)
                .HasMaxLength(50);

            Property(t => t.SiteFax)
                .HasMaxLength(50);

            Property(t => t.SiteEmail)
                .HasMaxLength(50);

            Property(t => t.SiteURL)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("sys_site");
            Property(t => t.SiteID).IsRequired().HasColumnName("SiteID").HasColumnType("uniqueidentifier")
                                           .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.SiteName).HasColumnName("SiteName");
            Property(t => t.SiteAdress).HasColumnName("SiteAdress");
            Property(t => t.SiteRoad).HasColumnName("SiteRoad");
            Property(t => t.SiteSubDistrict_Id).HasColumnName("SiteSubDistrict_Id");
            Property(t => t.SiteDistrict_Id).HasColumnName("SiteDistrict_Id");
            Property(t => t.SiteProvince_Id).HasColumnName("SiteProvince_Id");
            Property(t => t.SitePostCode).HasColumnName("SitePostCode");
            Property(t => t.SiteCountry).HasColumnName("SiteCountry");
            Property(t => t.SiteTel).HasColumnName("SiteTel");
            Property(t => t.SiteFax).HasColumnName("SiteFax");
            Property(t => t.SiteEmail).HasColumnName("SiteEmail");
            Property(t => t.SiteURL).HasColumnName("SiteURL");
            Property(t => t.CompanyID).HasColumnName("CompanyID");

            Property(t => t.Remark).HasColumnName("Remark").HasMaxLength(250);
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");

            // Relationships
            HasOptional(t => t.Company)
                .WithMany(t => t.SiteCollection)
                .HasForeignKey(d => d.CompanyID);
        }
    }
}
