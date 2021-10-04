using Autofac;
using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Infrastructure.Assemblies;
using DITS.HILI.WMS.Core.PackagesModel;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.Stock;
using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine
{
    public class Engine : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<bool> Transmit(List<DataTransfer> dataTransfer)
        {
            try
            {
                DataTransfer trans = dataTransfer.FirstOrDefault();
                Guid instanceID = trans.InstanceID;
                Guid? prevID = trans.PackagePrevID;
                Guid? nextID = trans.PackageNextID;
                Guid documentTypeID = trans.DocumentTypeID;
                bool start = trans.Start;
                int sequence = trans.Sequence;

                List<WorkFlow> workflow = WorkFlowRuntime.WorkFlowCollection.Where(x => x.DocumentTypeID == documentTypeID).ToList();
                if (workflow.Count == 0)
                {
                    throw new Exception("");
                }

                //Final stage return True 
                if (start)
                {
                    workflow = workflow.Where(x => x.PackagePrevID == instanceID && x.Start == start && x.Sequence == 1).ToList();
                }
                else
                {
                    sequence++;
                    workflow = workflow.Where(x => x.PackageNextID == prevID && x.Sequence == sequence).ToList();
                }

                int _lenght = workflow.Count;
                bool[] result = new bool[_lenght];
                int nIndex = 0;
                foreach (WorkFlow item in workflow)
                {
                    dataTransfer.ToList()
                      .ForEach(dataItem =>
                      {

                          dataItem.PackagePrevID = instanceID;
                          dataItem.PackageNextID = item.PackageNextID.Value;
                          dataItem.Sequence = item.Sequence;
                          dataItem.IsReserve = item.IsStockReserve;
                      });

                    result[nIndex] = action(instanceID, item.PackageNextID.Value, dataTransfer);
                    nIndex++;
                }

                bool ok = result.All(x => x == true);
                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected bool action(Guid instanceId, Guid nextInstance, List<DataTransfer> dataTransfer)
        {
            try
            {
                //Destination Transmit
                AssembliesModel _assem = AssembliesFactory.PackageCollection.FirstOrDefault(x => x.Id == nextInstance.ToString());
                Assembly assembly = Assembly.LoadFrom(Utilities.GetCurrentDirectory() + "\\" + _assem.ServiceAssembly);

                ContainerBuilder builder = new ContainerBuilder();
                builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().WithParameter("connectionString", ConfigurationManager.AppSettings["ConnectionString"].ToString());
                builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
                builder.RegisterType<StockService>().As<IStockService>().InstancePerLifetimeScope();
                builder.RegisterType<MessageService>().As<IMessageService>().InstancePerLifetimeScope();

                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsClass)
                    {
                        builder.RegisterType(type).AsImplementedInterfaces().InstancePerLifetimeScope();
                    }
                }

                IContainer contianer = builder.Build();
                using (ILifetimeScope scope = contianer.BeginLifetimeScope())
                {
                    IWorkEngine service = contianer.Resolve<IWorkEngine>();
                    EngineContext context = new EngineContext();
                    bool ok = context.ActionTransmit(service, dataTransfer);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
