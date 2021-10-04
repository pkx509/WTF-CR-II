using DITS.HILI.WMS.MasterModel.Utility;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    //TODO: DocumentType Config
    public class ItfInterfaceMappingConfiguration : EntityTypeConfiguration<ItfInterfaceMapping>
    {
        public ItfInterfaceMappingConfiguration()
            : this("dbo")
        { }

        public ItfInterfaceMappingConfiguration(string schema)
        {
            ToTable("itf_interface_mapping", schema);
            HasKey(x => new { x.InterfaceTypeId, x.DocumentId });

            Property(x => x.InterfaceTypeId).HasColumnName(@"InterfaceTypeID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.DocumentId).HasColumnName(@"DocumentID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.IsRegistTruck).HasColumnName(@"IsRegistTruck").HasColumnType("bit").IsOptional();
            Property(x => x.IsAssign).HasColumnName(@"IsAssign").HasColumnType("bit").IsOptional();
            Property(x => x.IsMarketing).HasColumnName(@"IsMarketing").HasColumnType("bit").IsOptional();
            Property(x => x.IsCreditNote).HasColumnName(@"IsCreditNote").HasColumnType("bit").IsOptional();
            Property(x => x.ToReprocess).HasColumnName(@"ToReprocess").HasColumnType("bit").IsOptional();
            Property(x => x.FromReprocess).HasColumnName(@"FromReprocess").HasColumnType("bit").IsOptional();
            Property(x => x.ReferenceDocumentID);
            Property(x => x.IsNormal);
            Property(x => x.IsItemChange);
            Property(x => x.IsWithoutGoods);
            Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            Property(x => x.Remark).HasColumnName(@"Remark").HasColumnType("nvarchar").IsOptional().HasMaxLength(250);
            Property(x => x.UserCreated).HasColumnName(@"UserCreated").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.UserModified).HasColumnName(@"UserModified").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.DateModified).HasColumnName(@"DateModified").HasColumnType("datetime").IsRequired();

            // Foreign keys
            HasRequired(a => a.ItfTransactionType).WithMany(b => b.ItfInterfaceMappings).HasForeignKey(c => c.InterfaceTypeId); // FK_InterfaceTypeID



        }
    }
}
