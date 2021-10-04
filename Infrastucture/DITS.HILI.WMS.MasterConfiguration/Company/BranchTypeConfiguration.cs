using DITS.HILI.WMS.MasterModel.Companies;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class BranchTypeConfiguration : EntityTypeConfiguration<BranchType>
    {
        public BranchTypeConfiguration() : this("dbo")
        {
        }

        public BranchTypeConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.BranchTypeID);

            // Properties
            Property(t => t.BranchName)
                .HasMaxLength(250);

            // Table & Column Mappings
            ToTable("sys_branch_type");
            Property(t => t.BranchTypeID).HasColumnName("BranchTypeID");
            Property(t => t.BranchName).HasColumnName("BranchName");

            Property(t => t.Remark).HasColumnName("Remark").HasMaxLength(250);
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");

        }

    }
}
