using DITS.HILI.WMS.TruckQueueModel;
using System.Data.Entity.ModelConfiguration;
namespace DITS.HILI.WMS.TruckQueueConfiguration
{
    public class ShipFromConfiguration : EntityTypeConfiguration<ShippingFrom>
    {
        public ShipFromConfiguration()
            : this("dbo")
        {
        }

        public ShipFromConfiguration(string schema)
        {
            ToTable("queue_shipfrom", schema);
            HasKey(x => x.ShipFromId);
            Property(x => x.ShipFromId).HasColumnName(@"ShipFromId").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("varchar").IsRequired().IsUnicode(true).HasMaxLength(255);
            Property(x => x.ShortName).HasColumnName(@"ShortName").HasColumnType("varchar").IsRequired().IsUnicode(true).HasMaxLength(50);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("varchar").IsOptional().IsUnicode(true).HasMaxLength(255);
            Property(x => x.Address).HasColumnName(@"Address").HasColumnType("varchar").IsOptional().IsUnicode(true).HasMaxLength(255);
            Property(t => t.IsActive).HasColumnName(@"IsActive");
            Property(t => t.UserCreated).HasColumnName(@"UserCreated");
            Property(t => t.DateCreated).HasColumnName(@"DateCreated");
            Property(t => t.UserModified).HasColumnName(@"UserModified");
            Property(t => t.DateModified).HasColumnName(@"DateModified");  
        }
    }
}
