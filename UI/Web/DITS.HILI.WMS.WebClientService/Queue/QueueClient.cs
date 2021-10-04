using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.TruckQueueModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Queue
{
    public class QueueClient
    {

        public static async Task SentAnounceChange(QueueConfiguration _data)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var url = ConfigurationManager.AppSettings["QueueService"].ToString();
                    client.BaseAddress = new Uri(url);
                    var result = client.PostAsync("Home/ChangedAnounce", new
                    {
                        Message = _data.Message,
                        EnableMessage = _data.EnableMessage
                    }, new JsonMediaTypeFormatter()).Result; 
                }
            }
            catch
            {

            }
        }
        public static async Task SentCallQueue(QueueReg _data)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["QueueService"].ToString();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var result = client.PostAsync("Home/CallQueue", new
                    {
                        QueueNo = _data.QueueNo,
                        DockNo = _data.QueueDock,
                        LicensePlate = _data.TruckRegNo
                    }, new JsonMediaTypeFormatter()).Result; 
                }
            }
            catch
            {

            }
        }

        public static async Task SentCallRefreshQueue()
        {
            try
            {
                string url = ConfigurationManager.AppSettings["QueueService"].ToString();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var result = client.PostAsync("Home/RefreshQueue", "Refresh", new JsonMediaTypeFormatter()).Result; 
                }
            }
            catch
            {

            }
        }
         
        public static async Task SentCallCompletedQueue(QueueReg _data)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["QueueService"].ToString();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var result = client.PostAsync("Home/CompletedQueue", new
                    {
                        QueueNo = _data.QueueNo,
                        DockNo = _data.QueueDock,
                        LicensePlate = _data.TruckRegNo
                    }, new JsonMediaTypeFormatter()).Result; 
                }
            }
            catch
            {

            }
        }

        public static async Task<ApiResponseMessage> GetDockAll(int? status,string dockname)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string,string>("status",status.ToString()),
                                 new KeyValuePair<string,string>("dockname",dockname) 
                             };
            return await HttpService.GetAsync("truckqueue/getdockall", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> GetDockById(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string,string>("id",id.ToString()) 
                             };
            return await HttpService.GetAsync("truckqueue/getdockbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> AddDock(QueueDock entity)
        {
            return await HttpService.SendAsync("truckqueue/adddock", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> ModifyDock(QueueDock entity)
        {
            return await HttpService.SendAsync("truckqueue/modifydock", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> RemoveDock(QueueDock entity)
        {
            return await HttpService.SendAsync("truckqueue/removedock", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> GetQueueStatusAll(int? status, string statusname)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string,string>("status",status.ToString()),
                                 new KeyValuePair<string,string>("statusname",statusname)
                             };
            return await HttpService.GetAsync("truckqueue/getstatusall", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> GetQueueStatusById(int id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string,string>("id",id.ToString())
                             };
            return await HttpService.GetAsync("truckqueue/getstatusbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        public static async Task<ApiResponseMessage> GetSearchQueue(DateTime startDate, DateTime endDate, QueueStatusEnum status, string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string,string>("startDate",startDate.ToString("yyyy/MM/dd")),
                                 new KeyValuePair<string,string>("endDate",endDate.ToString("yyyy/MM/dd")),
                                 new KeyValuePair<string,string>("status",((int)status).ToString()),
                                 new KeyValuePair<string,string>("keyword",keyword),
                                 new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                 new KeyValuePair<string,string>("pageSize",pageSize.ToString())
                             };
            return await HttpService.GetAsync("truckqueue/getsearchqueue", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> GetQueueReport(DateTime startDate, DateTime endDate, Guid? shippingfrom, Guid? shippingTo, QueueStatusEnum status, Guid? dockId, string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string,string>("startDate",startDate.ToString("yyyy/MM/dd")),
                                 new KeyValuePair<string,string>("endDate",endDate.ToString("yyyy/MM/dd")),
                                 new KeyValuePair<string,string>("shippingfrom",shippingfrom.HasValue?shippingfrom.ToString():null),
                                 new KeyValuePair<string,string>("shippingTo",shippingTo.HasValue?shippingTo.ToString():null),
                                 new KeyValuePair<string,string>("status",status.ToString()),
                                 new KeyValuePair<string,string>("dockId",dockId.HasValue?dockId.ToString():null),
                                 new KeyValuePair<string,string>("keyword",keyword),
                                 new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                 new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };
            return await HttpService.GetAsync("truckqueue/getqueuereport", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> AddQueueStatus(QueueStatus entity)
        {
            return await HttpService.SendAsync("truckqueue/addqueuestatus", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> ModifyQueueStatus(QueueStatus entity)
        {
            return await HttpService.SendAsync("truckqueue/modifyqueuestatus", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> RemoveQueueStatus(QueueStatus entity)
        {
            return await HttpService.SendAsync("truckqueue/removequeuestatus", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        } 
        public static async Task<ApiResponseMessage> GetQueueTypeAll(int? status, string name)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string,string>("status",status.ToString()),
                                 new KeyValuePair<string,string>("name",name)
                             };
            return await HttpService.GetAsync("truckqueue/getqueuetypeall", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);  
        }
        public static async Task<ApiResponseMessage> GetQueueTypeById(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string,string>("id",id.ToString())
                             };
            return await HttpService.GetAsync("truckqueue/getqueuetypebyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> AddQueueType(QueueRegisterType entity)
        {
           return await HttpService.SendAsync("truckqueue/addqueuetype", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> ModifyQueueType(QueueRegisterType entity)
        {
            return await HttpService.SendAsync("truckqueue/modifyqueuetype", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> RemoveQueueType(QueueRegisterType entity)
        {
            return await HttpService.SendAsync("truckqueue/removequeuetype", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> GetConfigurationActive()
        { 
            return await HttpService.GetAsync("truckqueue/getqueueconfigurationactive", null, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        public static async Task<ApiResponseMessage> GetConfigurationAll(int? status)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string,string>("status",status.ToString()), 
                             };
            return await HttpService.GetAsync("truckqueue/getqueueconfigurationall", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> GetConfigurationById(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string,string>("id",id.ToString())
                             };
            return await HttpService.GetAsync("truckqueue/getqueueconfigurationbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> AddConfiguration(QueueConfiguration entity)
        {
            return await HttpService.SendAsync("truckqueue/addqueueconfiguration", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> ModifyConfiguration(QueueConfiguration entity)
        {
            return await HttpService.SendAsync("truckqueue/modifyqueueconfiguration", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> RemoveConfiguration(QueueConfiguration entity)
        {
            return await HttpService.SendAsync("truckqueue/removequeueconfiguration", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> GetShippingFromAll(int? status, string comname, string comshortname)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string,string>("status",status.ToString()),
                                 new KeyValuePair<string,string>("comname",comname),
                                 new KeyValuePair<string,string>("comshortname",comshortname)
                             };
            return await HttpService.GetAsync("truckqueue/getshippingfromall", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> GetShippingFromById(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string,string>("id",id.ToString())
                             };
            return await HttpService.GetAsync("truckqueue/getshippingfrombyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> AddShippingFrom(ShippingFrom entity)
        {
            return await HttpService.SendAsync("truckqueue/addshippingfrom", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> ModifyShippingFrom(ShippingFrom entity)
        {
            return await HttpService.SendAsync("truckqueue/modifyshippingfrom", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> RemoveShippingFrom(ShippingFrom entity)
        {
            return await HttpService.SendAsync("truckqueue/removeshippingfrom", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false); 
        }
        public static async Task<ApiResponseMessage> GetInQueue(int? status, string keyword)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("status",status.ToString()),
                                 new KeyValuePair<string,string>("keyword",keyword)
                             };
            return await HttpService.GetAsync("truckqueue/getqueuelist", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        public static async Task<ApiResponseMessage> GetQueueInProgress(int? status, string keyword)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string,string>("status",status.ToString()),
                                 new KeyValuePair<string,string>("keyword",keyword)
                             };
            return await HttpService.GetAsync("truckqueue/getqueueinprogress", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
       
        public static async Task<ApiResponseMessage> GetQueueAll(QueueStatusEnum status, string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             { 
                                 new KeyValuePair<string,string>("status",((int)status).ToString()), 
                                 new KeyValuePair<string,string>("keyword",keyword),
                                 new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                 new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };
            return await HttpService.GetAsync("truckqueue/getqueueall", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        public static async Task<ApiResponseMessage> GetQueue(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                 new KeyValuePair<string,string>("id",id.ToString())
                             };
            return await HttpService.GetAsync("truckqueue/getqueuebyId", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        public static async Task<ApiResponseMessage> AddQueue(QueueReg entity)
        {
            return await HttpService.SendAsync("truckqueue/addqueue", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        public static async Task<ApiResponseMessage> ChangeStatus(QueueReg entity)
        {
            return await HttpService.SendAsync("truckqueue/changequeuestatus", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        public static async Task<ApiResponseMessage> CallQueue(QueueReg entity)
        {
            return await HttpService.SendAsync("truckqueue/callqueue", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        public static async Task<ApiResponseMessage> ModifyQueue(QueueReg entity)
        {
            return await HttpService.SendAsync("truckqueue/modifyqueue", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        public static async Task<ApiResponseMessage> RemoveQueue(QueueReg entity)
        {
            return await HttpService.SendAsync("truckqueue/removequeue", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

    }
}
