using DITS.HILI.WMS.PutAwayModel;

namespace DITS.HILI.WMS.PutAwayConfiguration
{
    public class PutAwayConfirmConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PutAwayConfirm>
    {
        public PutAwayConfirmConfiguration()
            : this("dbo")
        {
        }

        public PutAwayConfirmConfiguration(string schema)
        {
            ToTable(schema + ".pw_putaway_confirm");
            // Primary Key
            HasKey(t => t.PutAwayConfirmID);

            // Properties
            Property(t => t.Remark)
                .HasMaxLength(250);

            Property(t => t.PutAwayConfirmID).HasColumnName("PutAwayConfirmID");
            // this.Property(t => t.PutAwayDetailID).HasColumnName("PutAwayDetailID");
            Property(t => t.Quantity).HasColumnName("Quantity");
            Property(t => t.BaseQuantity).HasColumnName("BaseQuantity");
            Property(t => t.ConversionQty).HasColumnName("ConversionQty");
            Property(t => t.StockUnitID).HasColumnName("StockUnitID");
            Property(t => t.BaseUnitID).HasColumnName("BaseUnitID");
            Property(t => t.ConfirmLocationID).HasColumnName("ConfirmLocationID");
            Property(t => t.Remark).HasColumnName("Remark");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");
            // this.Property(t => t.PutAwayReasonID).HasColumnName("PutAwayReasonID");

            // Relationships
            //this.HasOptional(t => t.PutAwayReason)
            //    .WithMany(t => t.PutAwayConfirmCollection)
            //    .HasForeignKey(d => d.PutAwayReasonID);

            //this.HasRequired(t => t.PutAwayDetail)
            //    .WithMany(t => t.PutAwayConfirmCollection)
            //    .HasForeignKey(d => d.PutAwayDetailID);

        }
    }
}
