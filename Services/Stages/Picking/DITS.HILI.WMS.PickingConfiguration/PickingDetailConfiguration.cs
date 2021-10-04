using DITS.HILI.WMS.PickingModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.PickingConfiguration
{
    public class PickingDetailConfiguration : EntityTypeConfiguration<PickingDetail>
    {

        public PickingDetailConfiguration() : this("dbo")
        {

        }

        public PickingDetailConfiguration(string schema)
        {
            ToTable(schema + ".pk_picking_detail");
            HasKey(t => t.PickingDetailID);

            Property(t => t.PickingDetailID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.AssignID);
            Property(t => t.PickStockUnitID);
            Property(t => t.PickStockQty);
            Property(t => t.PickBaseUnitID);
            Property(t => t.PickBaseQty);
            Property(t => t.ConversionQty);
            Property(t => t.PickingStatus);
            Property(t => t.PickingReason).HasMaxLength(250);
            Property(t => t.LocationID);
            Property(t => t.PalletCode).HasMaxLength(50).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
        }
    }
}
