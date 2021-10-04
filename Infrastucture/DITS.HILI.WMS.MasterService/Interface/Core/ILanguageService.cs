using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Core;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Core
{
    public interface ILanguageService : IRepository<Language>
    {
        List<Language> GetAll();
        CustomMessage GetMessage(string propertyKey, string languageCode);
        CustomResource GetResource(string resourceKey, string languageCode);
    }
}
