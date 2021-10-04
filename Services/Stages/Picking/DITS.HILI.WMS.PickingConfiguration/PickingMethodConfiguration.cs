using DITS.HILI.WMS.PickingModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.PickingConfiguration
{
    public class PickingMethodConfiguration : EntityTypeConfiguration<PickingMethod>
    {
        public PickingMethodConfiguration() : this("dbo")
        {

        }

        public PickingMethodConfiguration(string schema)
        {
            ToTable(schema + ".pk_picking_method_config");
            HasKey(t => t.MethodID);

            Property(t => t.MethodID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.Name).HasMaxLength(100);
            Property(t => t.Sequence);
        }
    }
}
