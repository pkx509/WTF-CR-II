using DITS.HILI.WMS.MasterModel.Utility;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class PalletTypeConfiguration : EntityTypeConfiguration<PalletType>
    {
        public PalletTypeConfiguration()
            : this("dbo")
        {

        }

        public PalletTypeConfiguration(string schema)
        {
            ToTable(schema + ".sys_pallet_type");

            HasKey(x => x.PalletTypeID);
            Property(x => x.PalletTypeID).IsRequired().HasColumnName("ID").HasColumnType("uniqueidentifier")
                                            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.Height).IsRequired().HasColumnName("Height").HasColumnType("float");
            Property(x => x.Length).IsRequired().HasColumnName("Length").HasColumnType("float");
            Property(x => x.Width).IsRequired().HasColumnName("Width").HasColumnType("float");
            Property(x => x.MaxWeight).IsRequired().HasColumnName("MaxWeight").HasColumnType("float");
            Property(x => x.IsDefault).IsRequired().HasColumnName("IsDefault").HasColumnType("bit");

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");


        }
    }
}
