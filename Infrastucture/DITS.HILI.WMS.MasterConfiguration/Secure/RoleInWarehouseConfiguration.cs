using DITS.HILI.WMS.MasterModel.Secure;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Secure
{
    public class RoleInWarehouseConfiguration : EntityTypeConfiguration<RoleInWarehouse>
    {
        public RoleInWarehouseConfiguration() : this("dbo")
        { }
        public RoleInWarehouseConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => new { t.RoleID, t.WarehouseID });

            // Properties
            // Table & Column Mappings
            ToTable(schema + ".sys_roles_in_warehouse");
            Property(t => t.RoleID).HasColumnName("RoleID");
            Property(t => t.WarehouseID).HasColumnName("WarehouseID");
            Property(t => t.Remark).HasColumnName("Remark");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");

            // Relationships
            HasRequired(t => t.Role)
                .WithMany(t => t.RoleInWarehouseCollection)
                .HasForeignKey(d => d.RoleID);
            HasRequired(t => t.Warehouses)
                .WithMany(t => t.RoleInWarehouseCollection)
                .HasForeignKey(d => d.WarehouseID);
        }
    }
}
