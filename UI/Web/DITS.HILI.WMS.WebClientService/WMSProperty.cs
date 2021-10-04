using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService
{
    public class WMSProperty
    {
        public static async System.Threading.Tasks.Task<List<AssembliesModel>> GetSystem()
        {
            Ref<int> total = new Ref<int>();
            return await HttpService.Get<List<AssembliesModel>>("HILIService/GetSystem", total, 0, 0, null, string.Empty, null);
        }


        public static async Task<ApiResponseMessage> GetMessage(string key)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("key",key)
                             };
            ApiResponseMessage resp = await HttpService.GetAsync("language/custommessage", parameters, null, Common.Language).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetResource(string key)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("key",key)
                             };
            ApiResponseMessage resp = await HttpService.GetAsync("language/customresource", parameters, null, Common.Language).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetLanguages()
        {
            ApiResponseMessage resp = await HttpService.GetAsync("language/language", null, null, Common.Language).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetUser(string username)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("username",username)
                             };
            ApiResponseMessage resp = await HttpService.GetAsync("UserAccounts/getuser", parameters, null, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetToken(string username, string password)
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("grant_type","password"),
                                  new KeyValuePair<string,string>("username",username),
                                  new KeyValuePair<string,string>("password",password)
                             };



                ApiResponseMessage resp = await HttpService.SendAsync("/Token", HttpMethodType.Authen, parameters).ConfigureAwait(false);

                return resp;


            }
            catch (HILIException ex)
            {
                throw ex;
            }

            //string url = HttpClientServiceHelper.BaseUrl ;
            //WebRequest request = WebRequest.Create(new Uri(String.Format("{0}/Token", url)));
            //request.Method = "POST";


            //string postString = String.Format("username={0}&password={1}&grant_type=password", HttpUtility.HtmlEncode(username), HttpUtility.HtmlEncode(password));
            //byte[] bytes = Encoding.UTF8.GetBytes(postString);
            //using (Stream requestStream = await request.GetRequestStreamAsync())
            //{
            //    requestStream.Write(bytes, 0, bytes.Length);
            //}

            //try
            //{
            //    WebResponse httpResponse = (WebResponse)(await request.GetResponseAsync());
            //    string json;
            //    using (Stream responseStream = httpResponse.GetResponseStream())
            //    {
            //        json = new StreamReader(responseStream).ReadToEnd();
            //    }
            //    Token tokenResponse = JsonConvert.DeserializeObject<Token>(json);
            //    return tokenResponse.AccessToken;
            //}
            //catch (Exception ex)
            //{
            //    throw new SecurityException("Bad credentials", ex);
            //}
        }
    }
}
