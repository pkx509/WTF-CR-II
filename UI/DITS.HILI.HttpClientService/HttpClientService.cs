using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Helpers;
using DITS.HILI.WMS.MasterModel.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.HttpClientService
{
    public class HttpService
    {
        public static async Task<T> Get<T>(string url, Guid? userId, string language, Token accessToken)
        {

            HttpResponseMessage response = await Host(userId, language, 0, 0, accessToken).GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string _error = JsonConvert.DeserializeObject<IDictionary<string, string>>(response.Content.ReadAsStringAsync().Result)["Message"];
                throw new Exception(_error);
            }

            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

        }

        public static async Task<T> Get<T>(string url, Ref<int> total, int? pageIndex, int? pageSize, Guid? userId, string language, Token accessToken)
        {

            HttpResponseMessage response = await Host(userId, language, pageIndex, pageSize, accessToken).GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string _error = JsonConvert.DeserializeObject<IDictionary<string, string>>(response.Content.ReadAsStringAsync().Result)["Message"];
                throw new Exception(_error);
            }
            KeyValuePair<string, IEnumerable<string>> _result = response.Headers.Where(x => x.Key == "X-TotalRecords").FirstOrDefault();
            if (_result.Key != null)
            {
                total.Value = int.Parse(_result.Value.ToList()[0].ToString());
            }

            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

        }

        public static async Task<bool> Put<T>(string url, T obj, Guid? userId, string language, Token accessToken)
        {
            StringContent objContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Host(userId, language, 0, 0, accessToken).PutAsync(url, objContent);
            if (!response.IsSuccessStatusCode)
            {
                string _error = JsonConvert.DeserializeObject<IDictionary<string, string>>(response.Content.ReadAsStringAsync().Result)["Message"];
                throw new Exception(_error);
            }

            return true;

        }

        public static async Task<T> Post<T>(string url, T obj, Guid? userId, string language, Token accessToken)
        {
            StringContent objContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Host(userId, language, 0, 0, accessToken).PostAsync(url, objContent);
            if (!response.IsSuccessStatusCode)
            {
                string _error;
                _error = JsonConvert.DeserializeObject<IDictionary<string, string>>(response.Content.ReadAsStringAsync().Result)["Message"];
                throw new Exception(_error);
            }

            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

        }

        public static async Task<bool> Delete(string url, Guid? userId, string language, Token accessToken)
        {
            HttpResponseMessage response = await Host(userId, language, 0, 0, accessToken).DeleteAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string _error;
                _error = JsonConvert.DeserializeObject<IDictionary<string, string>>(response.Content.ReadAsStringAsync().Result)["Message"];
                throw new Exception(_error);
            }
            return true;

        }

        private static HttpClient Host(Guid? userId, string language, int? pageIndex, int? pageSize, Token accessToken)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(HttpClientServiceHelper.BaseUrl)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-UserId", (userId == null ? string.Empty : userId.ToString()));
            client.DefaultRequestHeaders.Add("X-PageIndex", (pageIndex == null ? "0" : pageIndex.ToString()));
            client.DefaultRequestHeaders.Add("X-PageSize", (pageSize == null ? "0" : pageSize.ToString()));
            client.DefaultRequestHeaders.Add("X-Language", language);
            if (accessToken != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
            }

            return client;
        }

        private static async Task<ApiResponseMessage> GetApiAuthenResponseAsync(HttpResponseMessage httpResponse)
        {
            ApiResponseMessage apiResponse = null;

            if (httpResponse != null)
            {
                switch (httpResponse.StatusCode)
                {
                    case HttpStatusCode.OK:
                        string content = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        apiResponse = new ApiResponseMessage
                        {
                            Data = content.JsonDeserialize<Token>(),
                            StatusCode = ApiResponseCode.OK,
                            ResponseCode = "0"
                        };

                        break;
                    default:
                        apiResponse = new ApiResponseMessage
                        {
                            StatusCode = ApiResponseCode.Unauthorized,
                            ResponseMessage = JsonConvert.DeserializeObject<IDictionary<string, string>>(httpResponse.Content.ReadAsStringAsync().Result)["error"]
                        };
                        break;
                }
            }
            else
            {
                apiResponse = new ApiResponseMessage
                {
                    StatusCode = ApiResponseCode.CannotConnect
                };

            }

            return apiResponse;
        }

        private static async Task<ApiResponseMessage> GetApiResponseAsync(HttpResponseMessage httpResponse)
        {
            ApiResponseMessage apiResponse = null;

            if (httpResponse != null)
            {
                switch (httpResponse.StatusCode)
                {
                    case HttpStatusCode.OK:
                        string content = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        apiResponse = content.JsonDeserialize<ApiResponseMessage>();
                        if (apiResponse != null)
                        {
                            apiResponse.StatusCode = ApiResponseCode.OK;
                        }
                        else
                        {
                            apiResponse = new ApiResponseMessage
                            {
                                StatusCode = ApiResponseCode.UnhandledResult
                            };
                        }

                        break;
                    case HttpStatusCode.InternalServerError:
                        apiResponse = new ApiResponseMessage
                        {
                            StatusCode = ApiResponseCode.InternalServerError
                        };
                        break;
                    case HttpStatusCode.NotFound:
                        apiResponse = new ApiResponseMessage
                        {
                            StatusCode = ApiResponseCode.NotFound
                        };
                        break;
                    case HttpStatusCode.Unauthorized:
                        apiResponse = new ApiResponseMessage
                        {
                            StatusCode = ApiResponseCode.Unauthorized
                        };
                        break;
                    default:
                        apiResponse = new ApiResponseMessage
                        {
                            StatusCode = ApiResponseCode.UnhandledResult
                        };
                        break;
                }
            }
            else
            {
                apiResponse = new ApiResponseMessage
                {
                    StatusCode = ApiResponseCode.CannotConnect
                };

            }


            return apiResponse;
        }

        public static async Task<ApiResponseMessage> SendAsync(string uri, HttpMethodType method, List<KeyValuePair<string, string>> parameters = null, Guid? UserID = null, string language = "", Token accessToken = null)
        {
            HttpResponseMessage httpResponse = null;

            if (method == HttpMethodType.Delete)
            {
                httpResponse = await HttpClientHelper.DeleteAsync(HttpClientServiceHelper.BaseUrl + uri, parameters, UserID, language, accessToken).ConfigureAwait(false);
            }
            else if (method == HttpMethodType.Post)
            {
                httpResponse = await HttpClientHelper.PostAsync(HttpClientServiceHelper.BaseUrl + uri, parameters, UserID, language, accessToken).ConfigureAwait(false);
            }
            else if (method == HttpMethodType.Put)
            {
                httpResponse = await HttpClientHelper.PutAsync(HttpClientServiceHelper.BaseUrl + uri, parameters, UserID, language, accessToken).ConfigureAwait(false);
            }
            else if (method == HttpMethodType.Authen)
            {
                httpResponse = await HttpClientHelper.PostAsync(HttpClientServiceHelper.BaseUrl + uri, parameters, UserID, language, accessToken).ConfigureAwait(false);
            }
            else
            {
                httpResponse = await HttpClientHelper.GetAsync(HttpClientServiceHelper.BaseUrl + uri, parameters, UserID, language, accessToken).ConfigureAwait(false);
            }

            ApiResponseMessage clResponse = null;
            if (HttpMethodType.Authen == method)
            {
                clResponse = await GetApiAuthenResponseAsync(httpResponse).ConfigureAwait(false);
            }
            else
            {
                clResponse = await GetApiResponseAsync(httpResponse).ConfigureAwait(false);
            }

            return clResponse;

        }

        public static async Task<ApiResponseMessage> GetAsync(string uri, List<KeyValuePair<string, string>> parameters = null, Guid? UserID = null, string language = "", Token accessToken = null)
        {
            HttpResponseMessage httpResponse = null;

            httpResponse = await HttpClientHelper.GetAsync(HttpClientServiceHelper.BaseUrl + uri, parameters, UserID, language, accessToken).ConfigureAwait(false);


            ApiResponseMessage clResponse = await GetApiResponseAsync(httpResponse).ConfigureAwait(false);

            return clResponse;

        }

        public static async Task<ApiResponseMessage> SendAsync<T>(string uri, HttpMethodType method, T obj, Guid? UserID = null, string language = "", Token accessToken = null)
        {
            HttpResponseMessage httpResponse = null;

            if (method == HttpMethodType.Delete)
            {
                httpResponse = await HttpClientHelper.DeleteAsync(HttpClientServiceHelper.BaseUrl + uri, obj, UserID, language, accessToken).ConfigureAwait(false);
            }
            else if (method == HttpMethodType.Post)
            {
                httpResponse = await HttpClientHelper.PostAsync(HttpClientServiceHelper.BaseUrl + uri, obj, UserID, language, accessToken).ConfigureAwait(false);
            }
            else if (method == HttpMethodType.Put)
            {
                httpResponse = await HttpClientHelper.PutAsync(HttpClientServiceHelper.BaseUrl + uri, obj, UserID, language, accessToken).ConfigureAwait(false);
            }

            ApiResponseMessage clResponse = await GetApiResponseAsync(httpResponse).ConfigureAwait(false);

            return clResponse;

        }
    }
}