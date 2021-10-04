using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.ProductionControlModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.ProductionControl
{
    public class ProductionControlClient
    {
        public static async Task<ApiResponseMessage> GetAllPacking(LineTypeEnum lineType, DateTime? planDate, Guid? lineID, string keyword, int pageIndex = 0, int pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("lineType",lineType.ToString()),
                                  new KeyValuePair<string,string>("planDate",planDate.ToString()),
                                  new KeyValuePair<string,string>("lineID",lineID.ToString()),
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            return await HttpService.GetAsync("productioncontrol/getpackinglist", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> GetAllPacked(LineTypeEnum lineType, DateTime? planDate, Guid? lineID, string keyword, int pageIndex = 0, int pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("lineType",lineType.ToString()),
                                  new KeyValuePair<string,string>("planDate",planDate.ToString()),
                                  new KeyValuePair<string,string>("lineID",lineID.ToString()),
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            return await HttpService.GetAsync("productioncontrol/getpackedlist", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> GetRePrintList(Guid controlID, bool isProd)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("controlID",controlID.ToString()),
                                  new KeyValuePair<string,string>("isProduction",isProd.ToString()),
                             };

            return await HttpService.GetAsync("productioncontrol/getreprintlist", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> GetRePrintOutboundList(DateTime? MFGDate, string productName, string PONo, int pageIndex = 0, int pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("MFGDate",MFGDate.ToString()),
                                  new KeyValuePair<string,string>("productName",productName),
                                  new KeyValuePair<string,string>("PONo",PONo),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            return await HttpService.GetAsync("productioncontrol/GetRePrintOutboundList", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> GetPalletInfo(string palletCode, string oldPalletCode, Guid oldProductID, decimal orderQTY)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("palletCode", palletCode),
                                  new KeyValuePair<string,string>("oldPalletCode", oldPalletCode),
                                  new KeyValuePair<string,string>("oldProductID", oldProductID.ToString()),
                                  new KeyValuePair<string,string>("orderQTY", orderQTY.ToString()),
                             };

            return await HttpService.GetAsync("productioncontrol/getpalletinfo", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> GetByID(Guid controlID)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("controlID",controlID.ToString()),
                             };

            return await HttpService.GetAsync("productioncontrol/getbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> PrintPalletTag(PrintPalletModel model)
        {
            return await HttpService.SendAsync("productioncontrol/printpallettag", HttpMethodType.Post, model, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> GetPalletList(Guid receiveDetailId)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("receiveDetailId",receiveDetailId.ToString()),
                             };

            return await HttpService.GetAsync("productioncontrol/getpalletlist", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> CancelPallet(CancelPalletModel model)
        {
            return await HttpService.SendAsync("productioncontrol/cancelPallet", HttpMethodType.Post, model, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
    }
}
