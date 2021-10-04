using DITS.HILI.WMS.MasterModel.Secure;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration.Secure
{
    public class ProgramValueConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProgramValue>
    {
        public ProgramValueConfiguration() : this("dbo")
        {
        }
        public ProgramValueConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.ProgramValueID);


            // Properties


            Property(t => t.Value)
                .HasMaxLength(255);

            Property(t => t.LanguageCode)
                .HasMaxLength(10);

            // Table & Column Mappings 
            ToTable(schema + ".sys_program_value");
            Property(t => t.ProgramValueID).HasColumnName("ProgramValueID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //this.Property(t => t.ProgramValeuID).HasColumnName("ProgramValeuID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //this.Property(t => t.Code).HasColumnName("Code");
            Property(t => t.Value).HasColumnName("Value");
            Property(t => t.LanguageCode).HasColumnName("LanguageCode");
            Property(t => t.ProgramID).HasColumnName("ProgramID");

            // Relationships
            HasOptional(t => t.Program)
                .WithMany(t => t.ProgramValueCollection)
                .HasForeignKey(d => d.ProgramID);
            //HasRequired(x => x.Program).WithMany(x => x.ProgramValueCollection).HasForeignKey(x => x.Code).WillCascadeOnDelete(false);
        }
    }
}
