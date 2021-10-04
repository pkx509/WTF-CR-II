using DITS.HILI.WMS.MasterModel.Secure;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ProgramConfiguration : EntityTypeConfiguration<Program>
    {
        public ProgramConfiguration()
            : this("dbo")
        { }

        public ProgramConfiguration(string schema)
        {
            ToTable(schema + ".sys_program");

            HasKey(x => x.ProgramID);

            Property(x => x.ProgramID).IsRequired().HasColumnName("ProgramID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.AppID).HasColumnName("AppID").HasColumnType("uniqueidentifier");
            Property(x => x.Sequence).IsRequired().HasColumnName("Sequence").HasColumnType("int");

            Property(x => x.Code).IsRequired().HasColumnName("Code").HasColumnType("nvarchar").HasMaxLength(100);
            //Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.Icon).HasColumnName("Icon").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Url).IsRequired().HasColumnName("Url").HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.ProgramType).HasColumnName("ProgramType");
            Property(x => x.ParentID).HasColumnName("ParentID");

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            HasRequired(x => x.Application).WithMany(x => x.ProgramCollection).HasForeignKey(x => x.AppID).WillCascadeOnDelete(true);



        }
    }
}
