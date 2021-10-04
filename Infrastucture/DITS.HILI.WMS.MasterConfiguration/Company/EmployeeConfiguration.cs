using DITS.HILI.WMS.MasterModel.Companies;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class EmployeeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Employee>
    {
        public EmployeeConfiguration()
            : this("dbo")
        {
        }

        public EmployeeConfiguration(string schema)
        {
            ToTable(schema + ".sys_employee");
            HasKey(x => x.EmployeeID);

            Property(x => x.EmployeeID).IsRequired().HasColumnName("EmployeeID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //Property(x => x.GroupID).HasColumnName(@"GroupID").HasColumnType("uniqueidentifier").IsOptional();

            Property(x => x.FirstName).IsRequired().HasColumnName("FirstName").HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.LastName).IsRequired().HasColumnName("LastName").HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.Email).HasColumnName("Email").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);

            Property(x => x.Remark).HasColumnName(@"Remark").HasColumnType("nvarchar").IsOptional().HasMaxLength(250);
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");


        }
    }
}
