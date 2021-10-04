using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.InventoryToolsModel;

namespace DITS.HILI.WMS.InventoryToolsService
{
    public interface IRelocationService : IRepository<RelocationModel>
    {
        RelocationModel GetAll(string ScanPallet);
        bool UpdateLocation(string NewLocation, string LotNumber);
    }
}
