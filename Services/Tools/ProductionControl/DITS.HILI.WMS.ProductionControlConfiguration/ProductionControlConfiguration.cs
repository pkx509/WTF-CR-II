using DITS.HILI.WMS.ProductionControlModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.ProductionControlConfiguration
{
    public class ProductionControlConfiguration : EntityTypeConfiguration<ProductionControl>
    {
        public ProductionControlConfiguration() : this("dbo")
        {

        }

        public ProductionControlConfiguration(string schema)
        {
            ToTable(schema + ".pc_controller");
            HasKey(t => t.ControlID);

            Property(t => t.ControlID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.ProductionDate).HasColumnType("date");
            Property(t => t.ProductionTime).HasColumnType("time");
            Property(t => t.Lot).HasMaxLength(50);
            Property(t => t.LineID);
            Property(t => t.ProductID);
            Property(t => t.ProductUnitID);
            Property(t => t.Quantity);
            Property(t => t.BaseUnitID);
            Property(t => t.ConversionQty);
            Property(t => t.StockUnitID);
            Property(t => t.OrderType).HasMaxLength(20);
            Property(t => t.OrderNo).HasMaxLength(100).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(t => t.Remark).HasMaxLength(250);
            Property(t => t.PcControlStatus);
            Property(t => t.ReferenceID);
            Property(t => t.PackageID);
            Property(t => t.StandardPalletQty);
            Property(t => t.IsActive);
            Property(t => t.UserCreated);
            Property(t => t.DateCreated);
            Property(t => t.UserModified);
            Property(t => t.DateModified);
            Property(t => t.ProductStatusID);
            Property(t => t.ProductSubStatusID);
        }
    }
}
