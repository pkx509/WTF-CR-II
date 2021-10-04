using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.StockService
{
    public interface IStockService : IRepository<StockInfo>
    {
        void Incomming(List<DataTransfer> dataTrans);
        void Outgoing(List<DataTransfer> dataTrans);
        void Reserve(List<DataTransfer> dataTrans);
    }
}
