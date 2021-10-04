using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.MobileService.Master
{
    public class ProductStatusClient
    {
        static Ref<int> _total = new Ref<int>();
        public static async Task<ProductStatus> GetByID(Guid id)
        {
            return await HttpService.Get<ProductStatus>("ProductStatus/getbyid?id=" + id, _total, null, null, Common.User.UserID, Common.Language, Common.AccessToken);
        }
        public static async Task<List<ProductStatus>> GetByDocumentTypeID(Guid documentTypeId)
        {
            return await HttpService.Get<List<ProductStatus>>("ProductStatus/getbydocumenttypeid?documentTypeid=" + documentTypeId, _total, null, null , Common.User.UserID, Common.Language, Common.AccessToken);
        }

        public static async Task<List<ProductStatus>> Get(string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        {
            return await HttpService.Get<List<ProductStatus>>("ProductStatus/get?keyword=" + keyword, total, pageIndex, pageSize, Common.User.UserID, Common.Language, Common.AccessToken);
        }
    }
}
