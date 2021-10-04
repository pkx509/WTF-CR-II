using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Infrastructure.Assemblies;
using DITS.HILI.WMS.Core.PackagesConfiguration;
using DITS.HILI.WMS.Core.PackagesModel;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DITS.HILI.WMS.Core.Data
{
    public partial class DbContextScope : DbContext
    {
        #region [ Core ]
        public DbSet<WorkFlow> WorkFlow { get; set; }
        public DbSet<Package> Package { get; set; }
        public DbSet<PackageCategory> PackageCategory { get; set; }
        public DbSet<PackageType> PackageType { get; set; }
        public DbSet<PackageWorkFlow> PackageWorkFlow { get; set; }
        #endregion

        #region [ Property ]  
        private readonly Guid _instanceId;
        #endregion

        #region [ Contructure ] 
        public DbContextScope(string connectionString)
            : base(connectionString)
        {
            try
            {

                _instanceId = Guid.NewGuid();

                Configuration.LazyLoadingEnabled = false;
                Configuration.ProxyCreationEnabled = false;

                bool exists = Database.Exists();
                if (!exists)
                {
                    Database.SetInitializer<DbContextScope>(new CreateDatabaseIfNotExists<DbContextScope>());
                }
                else
                {
                    Database.SetInitializer<DbContextScope>(new MigrateDatabaseToLatestVersion<DbContextScope, Migrations.Configuration>(true));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ Initialize ]

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            foreach (AssembliesModel pkg in AssembliesFactory.PackageCollection)
            {
                string path = DITS.HILI.Framework.Utilities.GetCurrentDirectory();
                FileInfo file = new FileInfo(path + "\\" + pkg.ConfigurationAssembly);
                if (!file.Exists)
                {
                    throw new HILIException("SYS10001");
                }
                //throw new Exception(MessageManger.GetMessage(Message.Core.SYS10001, pkg.ConfigurationAssembly));

                Assembly assembly = Assembly.LoadFrom(string.Format("{0}\\{1}", path, pkg.ConfigurationAssembly));

                System.Collections.Generic.IEnumerable<Type> typesToRegister = assembly.GetTypes()
                                 .Where(type => !string.IsNullOrEmpty(type.Namespace))
                                 .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                                        type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

                foreach (Type type in typesToRegister)
                {
                    dynamic configurationInstance = Activator.CreateInstance(type);
                    modelBuilder.Configurations.Add(configurationInstance);
                }
            }

            modelBuilder.Configurations.Add(new PackageConfiguration());
            modelBuilder.Configurations.Add(new PackageCategoryConfiguration());
            modelBuilder.Configurations.Add(new PackageTypeConfiguration());
            modelBuilder.Configurations.Add(new PackageWorkFlowConfiguration());
            modelBuilder.Configurations.Add(new WorkFlowConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public static System.Data.Entity.DbModelBuilder CreateModel(System.Data.Entity.DbModelBuilder modelBuilder, string schema)
        {

            System.Collections.Generic.IEnumerable<Type> typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                                          .Where(type => !string.IsNullOrEmpty(type.Namespace))
                                          .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                                                 type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (Type type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            return modelBuilder;
        }
        #endregion

        public bool IsSqlParameterNull(System.Data.SqlClient.SqlParameter param)
        {
            object sqlValue = param.SqlValue;
            if (sqlValue is System.Data.SqlTypes.INullable nullableValue)
            {
                return nullableValue.IsNull;
            }

            return (sqlValue == null || sqlValue == System.DBNull.Value);
        }

    }
}
