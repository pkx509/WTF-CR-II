using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.RegisterTruckModel.CustomModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Outbound
{
    public class ConsolidateClient
    {
        #region Consolidate
        public static async Task<ApiResponseMessage> GetConsolidateByPO(string pono, string documentNo)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("pono",pono),
                                   new KeyValuePair<string,string>("documentNo",documentNo)
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("RegisterTruck/getconsolidatebypo", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetConsolidateAll(string pono, string documentno, int? status, DateTime? datafrom, DateTime? datato, string licenseplate, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("pono",pono),
                                  new KeyValuePair<string,string>("documentno",documentno),
                                  new KeyValuePair<string,string>("status",status.ToString()),
                                  new KeyValuePair<string,string>("datafrom",datafrom.ToString()),
                                  new KeyValuePair<string,string>("datato",datato.ToString()),
                                  new KeyValuePair<string,string>("licenseplate",licenseplate),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("RegisterTruck/getconsolidateall", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> ModifyConsolidate(List<RegisterTruckConsolidateDeatilModel> entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("RegisterTruck/modifyconsolidate", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> ApproveConsolidate(List<RegisterTruckConsolidateDeatilModel> entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("RegisterTruck/approveconsolidate", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        #endregion

    }
}
