using DITS.HILI.WMS.MasterModel.Core;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Core
{
    internal class CustomMessageConfiguration : EntityTypeConfiguration<CustomMessage>
    {
        public CustomMessageConfiguration()
            : this("dbo")
        { }

        public CustomMessageConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.MessageId);

            // Properties
            Property(t => t.MessageCode)
                .HasMaxLength(50);

            Property(t => t.MessageTitle)
                .HasMaxLength(255);

            Property(t => t.MessageValue)
                .HasMaxLength(255);

            Property(t => t.MessageOrgValue)
                .HasMaxLength(255);

            Property(t => t.LanguageCode)
                .HasMaxLength(10);

            // Table & Column Mappings
            ToTable("core_message");
            Property(t => t.MessageId).HasColumnName("MessageId");
            Property(t => t.MessageCode).HasColumnName("MessageCode");
            Property(t => t.MessageTitle).HasColumnName("MessageTitle");
            Property(t => t.MessageValue).HasColumnName("MessageValue");
            Property(t => t.MessageOrgValue).HasColumnName("MessageOrgValue");
            Property(t => t.LanguageCode).HasColumnName("LanguageCode");
            Property(x => x.DateModified).HasColumnName("DateModified");
        }
    }
}
