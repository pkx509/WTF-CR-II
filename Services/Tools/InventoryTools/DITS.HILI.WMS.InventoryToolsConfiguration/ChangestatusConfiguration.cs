
using DITS.HILI.WMS.InventoryToolsModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{
    public class ChangestatusConfiguration : EntityTypeConfiguration<Changestatus>
    {
        public ChangestatusConfiguration() : this("dbo")
        {

        }
        public ChangestatusConfiguration(string schema)
        {// Primary Key
            HasKey(t => t.DamageID);

            // Properties
            Property(t => t.DamageCode)
                .HasMaxLength(20);

            Property(t => t.PalletCode)
                .HasMaxLength(40);

            Property(t => t.Lot)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable(schema + ".qa_changestatus");
            Property(t => t.DamageID).HasColumnName("DamageID");
            Property(t => t.DamageCode).HasColumnName("DamageCode");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.PalletCode).HasColumnName("PalletCode");
            Property(t => t.BaseUnitID).HasColumnName("BaseUnitID");
            Property(t => t.StockUnitID).HasColumnName("StockUnitID");
            Property(t => t.DocumentID).HasColumnName("DocumentID");
            Property(t => t.InspectionStatus).HasColumnName("InspectionStatus").HasColumnType("int"); ;
            Property(t => t.LineID).HasColumnName("LineID");
            Property(t => t.ReasonID).HasColumnName("ReasonID");
            Property(t => t.ReprocessQty).HasColumnName("ReprocessQty");
            Property(t => t.ReprocessBaseQty).HasColumnName("ReprocessBaseQty");
            Property(t => t.ConversionQty).HasColumnName("ConversionQty");
            Property(t => t.Lot).HasColumnName("Lot");
            Property(t => t.LocationID).HasColumnName("LocationID");
            Property(t => t.DMFromWarehouse).HasColumnName("DMFromWarehouse");
            Property(t => t.WorkerID).HasColumnName("WorkerID");
            Property(t => t.ProductStatusID).HasColumnName("ProductStatusID");
            Property(t => t.DamageDate).HasColumnName("DamageDate");
            Property(t => t.DamageQty).HasColumnName("DamageQty");
            Property(t => t.RejectQty).HasColumnName("RejectQty");
            Property(t => t.RejectBaseQty).HasColumnName("RejectBaseQty");
            Property(t => t.DispatchReprocessStatus).HasColumnName("DispatchReprocessStatus");
            Property(t => t.DispatchRejectStatus).HasColumnName("DispatchRejectStatus");

            Property(x => x.Remark).HasColumnName("Remark").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            // Relationships
            //this.HasOptional(t => t.sys_reason)
            //    .WithMany(t => t.qa_changestatus)
            //    .HasForeignKey(d => d.ReasonID);
        }
    }
}
