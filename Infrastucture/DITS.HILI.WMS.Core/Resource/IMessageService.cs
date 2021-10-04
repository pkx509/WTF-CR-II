using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Core;

namespace DITS.HILI.WMS.Core.Resource
{
    public interface IMessageService : IRepository<CustomResource>
    {
        CustomMessage GetMessage(string propertyKey, string languageCode);
        CustomResource GetResource(string resourceKey, string languageCode);
    }
}
