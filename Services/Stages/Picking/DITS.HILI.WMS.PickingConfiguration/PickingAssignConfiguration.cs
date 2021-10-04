using DITS.HILI.WMS.PickingModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.PickingConfiguration
{
    public class PickingAssignConfiguration : EntityTypeConfiguration<PickingAssign>
    {
        public PickingAssignConfiguration() : this("dbo")
        {

        }

        public PickingAssignConfiguration(string schema)
        {
            ToTable(schema + ".pk_picking_assign");
            HasKey(t => t.AssignID);

            Property(t => t.AssignID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.ShippingDetailID);
            Property(t => t.PickingID);
            Property(t => t.ProductID);
            Property(t => t.BaseQuantity);
            Property(t => t.BaseUnitID);
            Property(t => t.Barcode).HasMaxLength(40);
            Property(t => t.StockUnitID);
            Property(t => t.StockQuantity);
            Property(t => t.SuggestionLocationID);
            Property(t => t.RefLocationID);
            Property(t => t.PalletUnitID);
            Property(t => t.PalletCode).HasMaxLength(40).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(t => t.RefPalletCode).HasMaxLength(40).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(t => t.PalletQty);
            Property(t => t.PickingLot).HasMaxLength(50);
            Property(t => t.PickingUserID);
            Property(t => t.PickingDate);
            Property(t => t.OrderPick);
            Property(t => t.AssignStatus);
            Property(t => t.BookingID);
        }
    }
}
