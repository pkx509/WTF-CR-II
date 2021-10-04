using DITS.HILI.WMS.MasterModel.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.MasterConfiguration.Interface
{
    public class itf_transaction_typeMap : EntityTypeConfiguration<itf_transaction_type>
    {
        public itf_transaction_typeMap()
        {
            // Primary Key
            this.HasKey(t => t.InterfaceTypeID);

            // Properties
            this.Property(t => t.ORTP)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Description)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("itf_transaction_type");
            this.Property(t => t.InterfaceTypeID).HasColumnName("InterfaceTypeID");
            this.Property(t => t.ORTP).HasColumnName("ORTP");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
            this.Property(t => t.UserModified).HasColumnName("UserModified");
            this.Property(t => t.DateModified).HasColumnName("DateModified");
        }
    }
}
