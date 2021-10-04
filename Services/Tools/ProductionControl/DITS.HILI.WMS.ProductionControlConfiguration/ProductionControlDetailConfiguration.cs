using DITS.HILI.WMS.ProductionControlModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.ProductionControlConfiguration
{
    internal class ProductionControlDetailConfiguration : EntityTypeConfiguration<ProductionControlDetail>
    {
        public ProductionControlDetailConfiguration() : this("dbo")
        {

        }

        public ProductionControlDetailConfiguration(string schema)
        {
            ToTable(schema + ".pc_controller_packing_detail");
            HasKey(t => t.PackingID);



            Property(t => t.PackingID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.ControlID);
            Property(t => t.PalletCode).HasMaxLength(40).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(t => t.Sequence);
            Property(t => t.StockQuantity);
            Property(t => t.BaseQuantity);
            Property(t => t.ConversionQty);
            Property(t => t.RemainQTY);
            Property(t => t.RemainBaseQTY);
            Property(t => t.ReserveQTY);
            Property(t => t.ReserveBaseQTY);
            Property(t => t.StockUnitID);
            Property(t => t.BaseUnitID);
            Property(t => t.ProductStatusID);
            Property(t => t.ProductSubStatusID);
            Property(t => t.RemainStockUnitID);
            Property(t => t.RemainBaseUnitID);
            Property(t => t.MFGDate);
            Property(t => t.MFGTimeStart).HasColumnType("time");
            Property(t => t.MFGTimeEnd).HasColumnType("time"); ;
            Property(t => t.LocationID).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute())); ;
            Property(t => t.SugguestLocationID).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute())); ;
            Property(t => t.WarehouseID);
            Property(t => t.PackingStatus);
            Property(t => t.OptionalSuffix);
            Property(t => t.Remark).HasMaxLength(250);
            Property(t => t.IsActive);
            Property(t => t.IsNormal);
            Property(t => t.UserCreated);
            Property(t => t.DateCreated);
            Property(t => t.UserModified);
            Property(t => t.DateModified);
            Property(t => t.RefPalletCode).HasMaxLength(40);
            Property(t => t.IsNonProduction);

            HasRequired(x => x.ProductionControl).WithMany(x => x.PCDetailCollection).HasForeignKey(x => x.ControlID).WillCascadeOnDelete(false);
        }
    }
}
