using DITS.HILI.WMS.MasterModel.Core;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Core
{
    public class LanguageConfiguration : EntityTypeConfiguration<Language>
    {
        public LanguageConfiguration()
            : this("dbo")
        { }

        public LanguageConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.LanguageCode);

            Property(t => t.LanguageCode)
                .HasMaxLength(10);

            // Properties
            Property(t => t.LanguageName)
                .HasMaxLength(255);

            Property(t => t.CultureCode)
                .HasMaxLength(50);

            Property(t => t.Flag)
                .HasMaxLength(50);

            // Table & Column Mappings 
            ToTable(schema + ".core_language");
            Property(t => t.LanguageCode).HasColumnName("LanguageCode");
            Property(t => t.LanguageName).HasColumnName("LanguageName");
            Property(t => t.CultureCode).HasColumnName("CultureCode");
            Property(t => t.Flag).HasColumnName("Flag");
            Property(t => t.IsDefault).HasColumnName("IsDefault");
            Property(x => x.DateModified).HasColumnName("DateModified");
        }
    }
}
