using DITS.HILI.WMS.MasterModel.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.MasterConfiguration.Interface
{
    public class itf_interface_mappingMap : EntityTypeConfiguration<itf_interface_mapping>
    {
        public itf_interface_mappingMap()
            : this("dbo")
        { }
        public itf_interface_mappingMap(string schema)
        {
            // Primary Key
            this.HasKey(t => new { t.InterfaceTypeID, t.DocumentID });

            // Properties
            this.Property(t => t.Remark)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable(schema+".itf_interface_mapping");
            this.Property(t => t.InterfaceTypeID).HasColumnName("InterfaceTypeID");
            this.Property(t => t.DocumentID).HasColumnName("DocumentID");
            this.Property(t => t.IsRegistTruck).HasColumnName("IsRegistTruck");
            this.Property(t => t.IsAssign).HasColumnName("IsAssign");
            this.Property(t => t.IsMarketing).HasColumnName("IsMarketing");
            this.Property(t => t.ToReprocess).HasColumnName("ToReprocess");
            this.Property(t => t.FromReprocess).HasColumnName("FromReprocess");
            this.Property(t => t.ReferenceDocumentID).HasColumnName("ReferenceDocumentID");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
            this.Property(t => t.UserModified).HasColumnName("UserModified");
            this.Property(t => t.DateModified).HasColumnName("DateModified");
            this.Property(t => t.IsCreditNote).HasColumnName("IsCreditNote");
            this.Property(t => t.IsNormal).HasColumnName("IsNormal");

            // Relationships
            this.HasRequired(t => t.itf_transaction_type)
                .WithMany(t => t.itf_interface_mapping)
                .HasForeignKey(d => d.InterfaceTypeID);
            //this.HasRequired(t => t.sys_document_type)
            //    .WithMany(t => t.itf_interface_mapping)
            //    .HasForeignKey(d => d.DocumentID);

        }
    }
}
