using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Master
{
   public class ProductSubStatusClient
    {
        static Ref<int> _total = new Ref<int>();
        public static async Task<ProductSubStatus> GetByID(Guid id)
        {
            return await HttpService.Get<ProductSubStatus>("ProductSubStatus/getbyid?id=" + id, _total, null, null, ClientHelper.User.UserID, ClientHelper.Language, ClientHelper.AccessToken);
        }

        public static async Task<List<ProductSubStatus>> Get(string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        {
            return await HttpService.Get<List<ProductSubStatus>>("ProductSubStatus/get?keyword=" + keyword, total, pageIndex, pageSize,   ClientHelper.User.UserID, ClientHelper.Language, ClientHelper.AccessToken);
        }
    }
}
