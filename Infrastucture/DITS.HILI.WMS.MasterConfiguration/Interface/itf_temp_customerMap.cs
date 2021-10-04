using DITS.HILI.WMS.MasterModel.Interface;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Interface
{
    public class itf_temp_customerMap : EntityTypeConfiguration<itf_temp_customer>
    {
        public itf_temp_customerMap()
        {
            // Primary Key
            HasKey(t => t.TransactionID);

            // Properties
            Property(t => t.GdateTime)
                .IsRequired()
                .HasMaxLength(20);

            Property(t => t.GSTT)
                .HasMaxLength(1);

            Property(t => t.GDATE)
                .HasMaxLength(10);

            Property(t => t.GTIME)
                .HasMaxLength(6);

            Property(t => t.FSTT)
                .HasMaxLength(1);

            Property(t => t.SubCust_Code)
                .HasMaxLength(20);

            Property(t => t.SubCust_NameTH)
                .HasMaxLength(500);

            Property(t => t.SubCust_Tel)
                .HasMaxLength(50);

            Property(t => t.SubCust_Fax)
                .HasMaxLength(50);

            Property(t => t.SubCust_Email)
                .HasMaxLength(50);

            Property(t => t.SubCust_ContractName)
                .HasMaxLength(500);

            Property(t => t.ErrorMessage)
                .HasMaxLength(250);

            // Table & Column Mappings
            ToTable("itf_temp_customer");
            Property(t => t.TransactionID).HasColumnName("TransactionID");
            Property(t => t.GdateTime).HasColumnName("GdateTime");
            Property(t => t.GSTT).HasColumnName("GSTT");
            Property(t => t.GDATE).HasColumnName("GDATE");
            Property(t => t.GTIME).HasColumnName("GTIME");
            Property(t => t.FSTT).HasColumnName("FSTT");
            Property(t => t.Company).HasColumnName("Company");
            Property(t => t.SubCust_Code).HasColumnName("SubCust_Code");
            Property(t => t.SubCust_NameTH).HasColumnName("SubCust_NameTH");
            Property(t => t.SubCust_Tel).HasColumnName("SubCust_Tel");
            Property(t => t.SubCust_Fax).HasColumnName("SubCust_Fax");
            Property(t => t.SubCust_Email).HasColumnName("SubCust_Email");
            Property(t => t.SubCust_ContractName).HasColumnName("SubCust_ContractName");
            Property(t => t.ContactID).HasColumnName("ContactID");
            Property(t => t.ContactType).HasColumnName("ContactType");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");
            Property(t => t.UpdateUserID).HasColumnName("UpdateUserID");
            Property(t => t.UpdateDateTime).HasColumnName("UpdateDateTime");
            Property(t => t.ErrorMessage).HasColumnName("ErrorMessage");
        }
    }
}
