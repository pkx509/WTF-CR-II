using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Domain
{
    [JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public class ApiResponse
    {
        [JsonProperty("response")]
        public ApiResponseMessage ResponseMessage { get; set; }

        public bool IsSuccessResponseCode
        {
            get
            {
                if (ResponseMessage != null)
                    return ResponseMessage.StatusCode == ApiResponseCode.OK;

                return false;
            }
        }
    }
}
