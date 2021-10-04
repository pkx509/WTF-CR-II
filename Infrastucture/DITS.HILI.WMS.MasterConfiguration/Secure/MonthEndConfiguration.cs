using DITS.HILI.WMS.MasterModel.Secure;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class MonthEndConfiguration : EntityTypeConfiguration<Monthend>
    {
        public MonthEndConfiguration()
            : this("dbo")
        { }

        public MonthEndConfiguration(string schema)
        {
            ToTable("sys_monthend", schema);
            HasKey(x => x.ID);
            Property(x => x.ID).IsRequired().HasColumnName("ID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Month).HasColumnName("Month").HasColumnType("int");
            Property(x => x.Year).IsRequired().HasColumnName("Year").HasColumnType("int");
            Property(x => x.Remark).IsRequired().HasColumnName("Remark").HasColumnType("nvarchar").HasMaxLength(500);
            Property(x => x.CutOffDate).HasColumnName("CutOffDate").HasColumnType("datetime");
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
        }
    }
}
