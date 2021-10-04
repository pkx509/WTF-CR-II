using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.DailyPlan
{
    public class ImportProductionClient
    {
        public static async Task<ApiResponseMessage> GetByID(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };

            ApiResponseMessage resp = await HttpService.GetAsync("dailyplan/getbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> ImportDailyPlan(List<ProductionPlanCustomModel> entity)
        {
            return await HttpService.SendAsync("dailyplan/importdailyplan", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> SendtoReceive(List<ProductionPlanCustomModel> entity)
        {
            return await HttpService.SendAsync("dailyplan/SendToReceive", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> DeletePlan(List<Guid> planDetailIds)
        {
            return await HttpService.SendAsync("dailyplan/deleteplan", HttpMethodType.Post, planDetailIds, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> SavePlan(ProductionPlanCustomModel entity)
        {
            return await HttpService.SendAsync("dailyplan/saveplan", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> ValidateImport(List<ProductionPlanCustomModel> entity)
        {
            return await HttpService.SendAsync("dailyplan/validateimportdailyplan", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> SearchPlan(DateTime sdte, DateTime edte, Guid? lineId, LineTypeEnum lineType, bool isReceive, string keyword, int pageIndex = 0, int pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("sdte",sdte.ToString()),
                                  new KeyValuePair<string,string>("edte",edte.ToString()),
                                  new KeyValuePair<string,string>("lineId",lineId.ToString()),
                                  new KeyValuePair<string,string>("lineType",lineType.ToString()),
                                  new KeyValuePair<string,string>("isReceive",isReceive.ToString()),
                                  new KeyValuePair<string,string>("keyword",keyword.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dailyplan/get", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> GetLocationByLine(Guid? lineID, LocationTypeEnum? locationType, string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("lineID",lineID.ToString()),
                                  new KeyValuePair<string,string>("locationType",locationType.ToString()),
                                  new KeyValuePair<string,string>("keyword",keyword.ToString()),
                                  new KeyValuePair<string,string>("PageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("PageSize",pageSize.ToString()),
                             };

            return await HttpService.GetAsync("dailyplan/getlocationbyline", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
    }
}
