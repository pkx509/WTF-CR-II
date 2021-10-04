using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.MobileService.Master
{
  public   class ProductClient
    {
        public static async Task<Product> GetProduct(Guid productID)
        {
            var result = await HttpService.Get<Product>("product/getbyid?id=" + productID, Common.User.UserID, Common.Language, Common.AccessToken);
            return result;
        }
    }
}
