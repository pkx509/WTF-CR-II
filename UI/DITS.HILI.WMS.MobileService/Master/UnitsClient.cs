using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.MobileService.Master
{
    public class UnitsClient
    {
        static Ref<int> _total = new Ref<int>();
        public static async Task<Units> GetByID(Guid id)
        {
            return await HttpService.Get<Units>("units/getbyid?id=" + id, _total, null, null, Common.User.UserID, Common.Language, Common.AccessToken);
        }

        public static async Task<List<Units>> Get(string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        {
            return await HttpService.Get<List<Units>>("units/get?keyword=" + keyword, total, pageIndex, pageSize,  Common.User.UserID, Common.Language, Common.AccessToken);
        }
    }
}
