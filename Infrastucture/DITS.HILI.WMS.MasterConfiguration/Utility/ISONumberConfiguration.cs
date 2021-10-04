using DITS.HILI.WMS.MasterModel.Utility;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ISONumberConfiguration : EntityTypeConfiguration<ISONumber>
    {
        public ISONumberConfiguration()
            : this("dbo")
        {

        }

        public ISONumberConfiguration(string schema)
        {
            ToTable("sys_ISO_number", schema);

            HasKey(x => x.ISO_Id);

            Property(x => x.ISO_Id).HasColumnName(@"ISO_Id").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ISO_Number).HasColumnName(@"ISO_Number").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.ISO_EffectiveDate).HasColumnName("ISO_EffectiveDate").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.DocumentName).HasColumnName(@"DocumentName").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.IsReport).HasColumnName(@"IsReport").HasColumnType("bit").IsOptional();
            Property(x => x.IsForm).HasColumnName(@"IsForm").HasColumnType("bit").IsOptional();
        }
    }
}
