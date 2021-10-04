using Autofac;
using Autofac.Integration.WebApi;
using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.Caching;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Infrastructure.Assemblies;
using DITS.HILI.WMS.Core.Infrastructure.Engine.Service;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.Stock;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace DITS.HILI.WMS.WebAPIs
{
    public static class DependencyRegistration 
    { 
        public static IContainer RegisterComponents(HttpConfiguration config) 
        {

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerLifetimeScope();
            foreach (AssembliesModel item in AssembliesFactory.PackageCollection)
            { 
                builder.RegisterApiControllers(Assembly.LoadFrom(Path.Combine(Utilities.GetCurrentDirectory(), item.WebAPIAssembly))).InstancePerLifetimeScope(); 
            }
            builder.RegisterWebApiFilterProvider(config);

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().WithParameter("connectionString", Configure.ConnectionString()).InstancePerLifetimeScope();//.SingleInstance();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance();

            //AssembliesFactory.PackageCollection.ForEach(item =>
            //{
            //    if(!File.Exists(Path.Combine(Utilities.GetCurrentDirectory(), item.ServiceAssembly)))
            //    {
            //        throw new HILIException(item.ServiceAssembly);
            //    }
            //});
            if (AssembliesFactory.PackageCollection.Any(item => !File.Exists(Path.Combine(Utilities.GetCurrentDirectory(), item.ServiceAssembly))))
            {
                throw new HILIException("SYS10001");
            }

            foreach (AssembliesModel pkg in AssembliesFactory.PackageCollection)
            { 
                Assembly assembly = Assembly.LoadFrom(Path.Combine(Utilities.GetCurrentDirectory(), pkg.ServiceAssembly)); 
                foreach (System.Type type in assembly.GetTypes())
                {
                    if (type.IsClass)
                    {
                        builder.RegisterType(type).AsImplementedInterfaces().InstancePerLifetimeScope();
                    }
                }
            }
            builder.RegisterType<MessageService>().As<IMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<StockService>().As<IStockService>().InstancePerLifetimeScope();
            builder.RegisterType<WorkflowService>().As<IWorkflowService>().InstancePerLifetimeScope();
            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container); 
            return container;
        }
    }
}