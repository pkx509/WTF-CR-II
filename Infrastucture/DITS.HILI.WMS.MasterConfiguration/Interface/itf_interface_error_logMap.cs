using DITS.HILI.WMS.MasterModel.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Interface
{

    public class itf_interface_error_logMap : EntityTypeConfiguration<itf_interface_error_log>
    {
        public itf_interface_error_logMap()
        {
            // Primary Key
            HasKey(t => t.TransactionID);

            ToTable("itf_interface_error_log");
            Property(t => t.TransactionID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.APIName).HasMaxLength(50);
            Property(t => t.ErrorMessage);
            Property(t => t.TransactionDate);
            Property(t => t.Remark).HasMaxLength(500);
        }
    }
}
