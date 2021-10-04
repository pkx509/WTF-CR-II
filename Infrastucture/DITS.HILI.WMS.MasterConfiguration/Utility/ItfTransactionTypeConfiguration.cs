using DITS.HILI.WMS.MasterModel.Utility;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    //TODO: DocumentType Config
    public class ItfTransactionTypeConfiguration : EntityTypeConfiguration<ItfTransactionType>
    {
        public ItfTransactionTypeConfiguration()
            : this("dbo")
        { }

        public ItfTransactionTypeConfiguration(string schema)
        {
            ToTable("itf_transaction_type", schema);
            HasKey(x => x.InterfaceTypeId);

            Property(x => x.InterfaceTypeId).HasColumnName(@"InterfaceTypeID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Ortp).HasColumnName(@"ORTP").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(10);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            Property(x => x.UserCreated).HasColumnName(@"UserCreated").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.UserModified).HasColumnName(@"UserModified").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.DateModified).HasColumnName(@"DateModified").HasColumnType("datetime").IsRequired();


        }
    }
}
