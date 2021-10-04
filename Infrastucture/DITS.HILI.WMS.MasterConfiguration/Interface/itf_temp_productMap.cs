using DITS.HILI.WMS.MasterModel.Interface;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Interface
{
    public class itf_temp_productMap : EntityTypeConfiguration<itf_temp_product>
    {
        public itf_temp_productMap()
        {
            // Primary Key
            HasKey(t => t.TransactionID);

            // Properties
            Property(t => t.Product_system_code)
                .HasMaxLength(20);

            Property(t => t.Product_Name_Full)
                .HasMaxLength(500);

            Property(t => t.Description)
                .HasMaxLength(500);

            Property(t => t.ProductBasicUnit)
                .HasMaxLength(3);

            Property(t => t.ProductCode)
                .HasMaxLength(20);

            Property(t => t.ProductGroup)
                .HasMaxLength(8);

            Property(t => t.ProductType)
                .HasMaxLength(5);

            Property(t => t.TransactionState)
                .HasMaxLength(50);

            Property(t => t.FSTT)
                .HasMaxLength(1);

            Property(t => t.FDATE)
                .HasMaxLength(10);

            Property(t => t.FTIME)
                .HasMaxLength(6);

            Property(t => t.GSTT)
                .HasMaxLength(1);

            Property(t => t.GDATE)
                .HasMaxLength(10);

            Property(t => t.GTIME)
                .HasMaxLength(6);

            Property(t => t.STAS)
                .HasMaxLength(2);

            Property(t => t.ErrorMessage)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("itf_temp_product");
            Property(t => t.TransactionID).HasColumnName("TransactionID");
            Property(t => t.Product_system_code).HasColumnName("Product_system_code");
            Property(t => t.Product_Name_Full).HasColumnName("Product_Name_Full");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.Shelf_life).HasColumnName("Shelf_life");
            Property(t => t.ProductAge).HasColumnName("ProductAge");
            Property(t => t.ProductBasicUnit).HasColumnName("ProductBasicUnit");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.ProductCode).HasColumnName("ProductCode");
            Property(t => t.ProductGroup).HasColumnName("ProductGroup");
            Property(t => t.ProductType).HasColumnName("ProductType");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateUser).HasColumnName("CreateUser");
            Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            Property(t => t.UpdateUser).HasColumnName("UpdateUser");
            Property(t => t.TransactionState).HasColumnName("TransactionState");
            Property(t => t.FSTT).HasColumnName("FSTT");
            Property(t => t.FDATE).HasColumnName("FDATE");
            Property(t => t.FTIME).HasColumnName("FTIME");
            Property(t => t.GSTT).HasColumnName("GSTT");
            Property(t => t.GDATE).HasColumnName("GDATE");
            Property(t => t.GTIME).HasColumnName("GTIME");
            Property(t => t.STAS).HasColumnName("STAS");
            Property(t => t.ErrorMessage).HasColumnName("ErrorMessage");
        }
    }
}
