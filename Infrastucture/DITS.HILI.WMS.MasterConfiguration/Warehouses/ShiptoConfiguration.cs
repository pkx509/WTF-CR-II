using DITS.HILI.WMS.MasterModel.Warehouses;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Warehouses
{
    public class ShipToConfiguration : EntityTypeConfiguration<ShippingTo>
    {
        public ShipToConfiguration()
            : this("dbo")
        {
        }

        public ShipToConfiguration(string schema)
        {
            ToTable("sys_shipto", schema);
            HasKey(x => x.ShipToId);

            Property(x => x.ShipToId).HasColumnName(@"ShipToID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(50);
            Property(x => x.ShortName).HasColumnName(@"ShortName").HasColumnType("varchar").IsRequired().IsUnicode(true).HasMaxLength(50);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("varchar").IsOptional().IsUnicode(true).HasMaxLength(255);
            Property(x => x.Address).HasColumnName(@"Address").HasColumnType("varchar").IsOptional().IsUnicode(true).HasMaxLength(255);
            Property(x => x.IsDefault).HasColumnName(@"IsDefault").HasColumnType("bit").IsOptional(); 
            Property(x => x.BusinessGroup).HasColumnName(@"BusinessGroup").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(10);
            Property(x => x.RuleId).HasColumnName(@"RuleID").HasColumnType("uniqueidentifier").IsRequired();
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");

            HasRequired(t => t.SpecialBookingRule)
             .WithMany(t => t.ShiptoCollecttion)
             .HasForeignKey(d => d.RuleId);


        }
    }
}
