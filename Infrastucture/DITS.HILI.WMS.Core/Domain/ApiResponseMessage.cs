using Newtonsoft.Json;

namespace DITS.HILI.WMS.Core.Domain
{
    [JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public class ApiResponseMessage
    {
        #region Properties 
        //public bool IsSuccess
        //{
        //    get
        //    {
        //        return StatusCode == ApiResponseCode.OK;
        //    }
        //}

        public ApiResponseCode StatusCode { get; set; }

        [JsonProperty("responseCode")]
        public string ResponseCode { get; set; }

        [JsonProperty("responseMessage")]
        public string ResponseMessage { get; set; }

        [JsonProperty("text")]
        public string text { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        public bool IsSuccess => ResponseCode == "0" && StatusCode == ApiResponseCode.OK;
        [JsonProperty("totals")]
        public int Totals { get; set; }
        #endregion

        public T Get<T>() where T : class
        {
            if (Data != null)
            {
                return Data.ToString().JsonDeserialize<T>();
            }
            else
            {
                return default(T);
            }
        }

        public T Get<T>(T result) where T : class
        {
            if (Data != null)
            {
                return Data.ToString().JsonDeserialize(result);
            }
            else
            {
                return default(T);
            }
        }
    }
}
