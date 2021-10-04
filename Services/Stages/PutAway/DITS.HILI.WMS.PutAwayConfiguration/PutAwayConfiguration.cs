using DITS.HILI.WMS.PutAwayModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace DITS.HILI.WMS.PutAwayConfiguration
{
    public class PutAwayConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PutAway>
    {
        public PutAwayConfiguration()
            : this("dbo")
        {
        }

        public PutAwayConfiguration(string schema)
        {
            ToTable(schema + ".pw_putaway");
            HasKey(x => x.PutAwayID);

            Property(x => x.PutAwayID).IsRequired().HasColumnName("PutAwayID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PutAwayJobCode).IsRequired().HasColumnName("PutAwayJobCode").HasColumnType("nvarchar").HasMaxLength(20)
                                 .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            Property(x => x.ProductOwnerID).IsRequired().HasColumnName("ProductOwnerID").HasColumnType("uniqueidentifier");
            Property(x => x.ProductID).IsRequired().HasColumnName("ProductID").HasColumnType("uniqueidentifier");
            Property(x => x.Lot).HasColumnName("Lot").HasColumnType("nvarchar").HasMaxLength(50)
                                 .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            Property(x => x.PalletCode).HasColumnName("PalletCode").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ManufacturingDate).HasColumnName("ManufacturingDate").HasColumnType("datetime");
            Property(x => x.ExpirationDate).HasColumnName("ExpirationDate").HasColumnType("datetime");
            Property(x => x.ProductStatusID).IsRequired().HasColumnName("ProductStatusID").HasColumnType("uniqueidentifier");
            Property(x => x.ProductSubStatusID).IsRequired().HasColumnName("ProductSubStatusID").HasColumnType("uniqueidentifier");
            Property(x => x.PackageWeight).IsRequired().HasColumnName("PackageWeight").HasColumnType("float");
            Property(x => x.ProductWeight).IsRequired().HasColumnName("ProductWeight").HasColumnType("float");
            Property(x => x.ProductWidth).IsRequired().HasColumnName("ProductWidth").HasColumnType("float");
            Property(x => x.ProductLength).IsRequired().HasColumnName("ProductLength").HasColumnType("float");
            Property(x => x.ProductHeight).IsRequired().HasColumnName("ProductHeight").HasColumnType("float");

            Property(x => x.StartDate).HasColumnName("StartDate").HasColumnType("datetime");
            Property(x => x.FinishDate).HasColumnName("FinishDate").HasColumnType("datetime");
            Property(x => x.FromLocationID).HasColumnName("FromLocationID").HasColumnType("uniqueidentifier");
            Property(x => x.SuggestionLocationID).HasColumnName("SuggestionLocationID").HasColumnType("uniqueidentifier");
            Property(x => x.PutAwayStatus).HasColumnName("PutAwayStatus").HasColumnType("int");
            Property(x => x.Price).HasColumnName("Price").HasColumnType("money");
            Property(x => x.ProductUnitPriceID).HasColumnName("ProductUnitPriceID").HasColumnType("uniqueidentifier");

            Property(x => x.PutAwayDate).HasColumnName("PutAwayDate").HasColumnType("datetime");

            Property(x => x.Remark).HasColumnName("Remark").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

        }
    }
}
