using DITS.HILI.WMS.MasterModel.Companies;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class BranchConfiguration : EntityTypeConfiguration<Branch>
    {
        public BranchConfiguration() : this("dbo")
        {
        }

        public BranchConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.BranchID);

            // Properties
            Property(t => t.BranchCode)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.BranchName)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.Location)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("sys_branch");
            Property(t => t.BranchID).HasColumnName("BranchID");
            Property(t => t.BranchCode).HasColumnName("BranchCode");
            Property(t => t.BranchName).HasColumnName("BranchName");
            Property(t => t.BranchTypeID).HasColumnName("BranchTypeID");
            Property(t => t.Location).HasColumnName("Location");

            Property(t => t.Remark).HasColumnName("Remark").HasMaxLength(250);
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");

            //Relationships
            HasRequired(t => t.BranchType)
                .WithMany(t => t.BranchCollection)
                .HasForeignKey(d => d.BranchTypeID);


        }
    }
}
