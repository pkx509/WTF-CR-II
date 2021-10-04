using DITS.HILI.WMS.Infrastructure;
using DITS.HILI.WMS.Master;
using DITS.HILI.WMS.WorkEngineModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngine
{
    public class Engine : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool Transmit(Guid instanceID, Guid documentTypeId, List<DataTransfer> dataTransfer)
        {
            var workflow = WorkFlowRuntime.WorkFlowCollection.Where(x => x.Source == instanceID && x.DocumentTypeID == documentTypeId).OrderBy(x => x.Sequence).ToList();

            //Count = 0
            if (workflow.Count() == 0)
                return action(instanceID, dataTransfer);
            else
            {
                int _lenght = workflow.Count;
                bool[] result = new bool[_lenght];
                int nIndex = 0;
                foreach (var item in workflow)
                {
                    if (item.Destination != null)
                        result[nIndex] = action(item.Destination.Value, dataTransfer);
                    else
                        result[nIndex] = true;

                    nIndex++;
                }

                var ok = result.All(x => x == true);
                return ok;
            }
        }

        protected bool action(Guid id, List<DataTransfer> dataTransfer)
        {
            //Destination Transmit
            var _assem = AssembliesFactory.PackageCollection.FirstOrDefault(x => x.Id == id.ToString());
            var assembly = Assembly.LoadFrom(Environment.CurrentDirectory + "\\" + _assem.ActivityAssembly);
            var engine = (IWorkEngine)assembly.CreateInstance(_assem.ActivityAssembly);

            EngineContext context = new EngineContext();
            var ok = context.ActionTransmit(engine, dataTransfer);
            return ok;
        }

    }
}
