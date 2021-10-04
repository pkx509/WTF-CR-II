using DITS.HILI.WMS.PutAwayModel;

namespace DITS.HILI.WMS.PutAwayConfiguration
{
    public class PutAwayDetailConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PutAwayDetail>
    {
        public PutAwayDetailConfiguration()
            : this("dbo")
        {
        }

        public PutAwayDetailConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.PutAwayDetailID);

            // Properties
            // Table & Column Mappings
            ToTable(schema + ".pw_putaway_detail");
            Property(t => t.PutAwayDetailID).HasColumnName("PutAwayDetailID");
            Property(t => t.PutAwayID).HasColumnName("PutAwayID");
            Property(t => t.StockUnitID).HasColumnName("StockUnitID");
            Property(t => t.Quantity).HasColumnName("Quantity");
            Property(t => t.BaseUnitID).HasColumnName("BaseUnitID");
            Property(t => t.BaseQuantity).HasColumnName("BaseQuantity");
            Property(t => t.ConversionQty).HasColumnName("ConversionQty");
            Property(t => t.ConfirmQty).HasColumnName("ConfirmQty");


            Property(x => x.Remark).HasColumnName("Remark").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            HasRequired(s => s.PutAway).WithMany(x => x.PutAwayDetailCollection).HasForeignKey(x => x.PutAwayID).WillCascadeOnDelete(false);

        }
    }
}
