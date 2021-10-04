using DITS.HILI.WMS.MasterModel.Utility;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    //TODO: DocumentType Config
    public class ItfInterfaceMappingDocumentConfiguration : EntityTypeConfiguration<ItfInterfaceMappingDocument>
    {
        public ItfInterfaceMappingDocumentConfiguration()
            : this("dbo")
        { }

        public ItfInterfaceMappingDocumentConfiguration(string schema)
        {
            ToTable("itf_interface_mapping_document", schema);
            HasKey(x => new { x.DocumentCode });

            Property(x => x.DocumentCode).HasColumnName(@"DocumentCode").HasColumnType("nchar").IsRequired().IsFixedLength().HasMaxLength(10).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.GroupTable).HasColumnName(@"GroupTable").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nchar").IsRequired().IsFixedLength().HasMaxLength(255).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);


        }
    }
}
