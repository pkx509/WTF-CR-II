using DITS.HILI.WMS.PickingModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.PickingConfiguration
{
    public class PickingConfiguration : EntityTypeConfiguration<Picking>
    {
        public PickingConfiguration() : this("dbo")
        {

        }

        public PickingConfiguration(string schema)
        {
            ToTable(schema + ".pk_picking");
            HasKey(t => t.PickingID);

            Property(t => t.PickingID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.PickingCode).HasMaxLength(20);
            Property(t => t.PickingStartDate);
            Property(t => t.PickingCompleteDate);
            Property(t => t.PickingEntryDate);
            Property(t => t.PickingStatus);
            Property(t => t.PickingCloseReason).HasMaxLength(250);
            Property(t => t.ShippingCode).HasMaxLength(20).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute())); ;
            Property(t => t.EmployeeAssignID);
            Property(t => t.DispatchCode).HasMaxLength(20).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute())); ;
            Property(t => t.DocumentNo).HasMaxLength(50).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute())); ;
            Property(t => t.OrderNo).HasMaxLength(50);
            Property(t => t.PONo).HasMaxLength(50).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute())); ;
        }
    }
}
