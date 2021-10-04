using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Master
{
    public class ItfInterfaceMappingClient
    {
        public static async Task<ApiResponseMessage> GetByDocument(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("InterfaceMapping/getbydocument", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
    }
}
