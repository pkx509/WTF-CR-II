using DITS.HILI.WMS.MasterModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DITS.HILI.WMS.Core.Helpers
{

    public static class HttpClientHelper
    {
        public static string BuildQueryString(List<KeyValuePair<string, string>> inputs)
        {
            try
            {
                if (inputs == null)
                {
                    return "";
                }

                StringBuilder parameters = new StringBuilder();

                inputs.ForEach(x =>
                {
                    parameters.AppendFormat("{0}={1}&",
                       HttpUtility.UrlEncode(x.Key),
                       HttpUtility.UrlEncode(x.Value));
                });

                parameters.Length -= 1;

                return parameters.ToString();
            }
            catch
            {
                return "";
            }

        }

        internal static HttpClient CreateHttpClient(Guid? userId = null, string language = "", Token accessToken = null)
        {

            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromHours(1);
            httpClient.DefaultRequestHeaders.Add("X-UserId", (userId == null ? string.Empty : userId.ToString()));
            httpClient.DefaultRequestHeaders.Add("X-Language", language);
            if (accessToken != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
            }

            return httpClient;
        }

        public static async Task<HttpResponseMessage> PostAsync(string uri, List<KeyValuePair<string, string>> parameters = null, Guid? UserID = null, string language = "", Token accessToken = null)
        {
            try
            {
                using (HttpClient client = CreateHttpClient(UserID, language, accessToken))
                {

                    if (parameters == null)
                    {
                        parameters = new List<KeyValuePair<string, string>>();
                    }

                    using (FormUrlEncodedContent content = new FormUrlEncodedContent(parameters))
                    {
                        HttpResponseMessage response = await client.PostAsync(uri, content).ConfigureAwait(false);

                        return response;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<HttpResponseMessage> GetAsync(string uri, List<KeyValuePair<string, string>> parameters = null, Guid? UserID = null, string language = "", Token accessToken = null)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });

                using (HttpClient client = CreateHttpClient(UserID, language, accessToken))
                {
                    if (parameters != null)
                    {
                        string queryString = BuildQueryString(parameters);
                        uri = uri.EndsWith("?") ? uri + queryString : uri + "?" + queryString;
                    }
                    HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);

                    return response;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<HttpResponseMessage> PutAsync(string uri, List<KeyValuePair<string, string>> parameters = null, Guid? UserID = null, string language = "", Token accessToken = null)
        {
            try
            {
                using (HttpClient client = CreateHttpClient(UserID, language, accessToken))
                {

                    if (parameters == null)
                    {
                        parameters = new List<KeyValuePair<string, string>>();
                    }

                    using (FormUrlEncodedContent content = new FormUrlEncodedContent(parameters))
                    {

                        HttpResponseMessage response = await client.PutAsync(uri, content).ConfigureAwait(false);

                        return response;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<HttpResponseMessage> DeleteAsync(string uri, List<KeyValuePair<string, string>> parameters = null, Guid? UserID = null, string language = "", Token accessToken = null)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });

                using (HttpClient client = CreateHttpClient(UserID, language, accessToken))
                {
                    if (parameters != null)
                    {
                        string queryString = BuildQueryString(parameters);
                        uri = uri.EndsWith("?") ? uri + queryString : uri + "?" + queryString;
                    }
                    HttpResponseMessage response = await client.DeleteAsync(uri).ConfigureAwait(false);

                    return response;
                }
            }
            catch
            {
                return null;
            }
        }


        public static async Task<HttpResponseMessage> PutAsync<T>(string url, T obj, Guid? UserID = null, string language = "", Token accessToken = null)
        {
            try
            {
                using (HttpClient client = CreateHttpClient(UserID, language, accessToken))
                {
                    //var objContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                    //var response = await client.PostAsync(url, objContent).ConfigureAwait(false);

                    //return response;

                    string content = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                    HttpResponseMessage response = await client.PutAsync(url, new StringContent(content, Encoding.UTF8, "text/json")).ConfigureAwait(false);

                    return response;
                }
            }
            catch
            {
                return null;
            }


        }

        public static async Task<HttpResponseMessage> PostAsync<T>(string url, T obj, Guid? UserID = null, string language = "", Token accessToken = null)
        {
            try
            {

                using (HttpClient client = CreateHttpClient(UserID, language, accessToken))
                {
                    string content = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                    HttpResponseMessage response = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "text/json")).ConfigureAwait(false);

                    return response;
                }
            }
            catch
            {
                return null;
            }

        }

        public static async Task<HttpResponseMessage> DeleteAsync<T>(string url, T obj, Guid? UserID = null, string language = "", Token accessToken = null)
        {
            try
            {
                using (HttpClient client = CreateHttpClient(UserID, language, accessToken))
                {
                    StringContent objContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, objContent).ConfigureAwait(false);

                    return response;
                }
            }
            catch
            {
                return null;
            }

        }

        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            // base case: 
            //IEnumerable<IEnumerable<T>> result = new[] { Enumerable.Empty<T>() };
            //foreach (var sequence in sequences)
            //{
            //    var s = sequence; // don't close over the loop variable 
            //                      // recursive case: use SelectMany to build the new product out of the old one 
            //    result =
            //      from seq in result
            //      from item in s
            //      select seq.Concat(new[] { item });
            //}
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) =>
                    from accseq in accumulator
                    from item in sequence
                    select accseq.Concat(new[] { item })
                );

            //  return result;
        }
    }
}
