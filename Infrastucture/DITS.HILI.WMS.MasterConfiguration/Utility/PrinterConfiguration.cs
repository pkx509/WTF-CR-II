using DITS.HILI.WMS.MasterModel.Utility;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class PrinterConfiguration : EntityTypeConfiguration<Printer>
    {
        public PrinterConfiguration()
            : this("dbo")
        {

        }

        public PrinterConfiguration(string schema)
        {
            ToTable(schema + ".sys_printers");
            HasKey(x => x.PrinterId);
            Property(x => x.PrinterId).HasColumnName(@"PrinterID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PrinterName).HasColumnName(@"PrinterName").HasColumnType("nvarchar").IsOptional().HasMaxLength(250);
            Property(x => x.PrinterLocationId);
            Property(x => x.PrinterLocation).HasColumnName(@"PrinterLocation").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.IsDefault).HasColumnName(@"IsDefault").HasColumnType("bit").IsOptional();
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

        }
    }
}
