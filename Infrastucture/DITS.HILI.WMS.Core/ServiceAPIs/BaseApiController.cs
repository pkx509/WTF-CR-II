using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DITS.HILI.WMS.Core.ServiceAPIs
{

    public abstract class BaseApiController : ApiController
    {


        protected ApiResponseMessage Succeed(object returnData = null, int totals = 0)
        {
            CustomMessage msg = new CustomMessage() { MessageCode = "0", MessageValue = "Success" };
            return CreateResponse(ApiResponseCode.OK, msg, returnData, totals);
        }


        protected ApiResponseMessage NotSucceed(CustomMessage msg)
        {
            return CreateResponse(ApiResponseCode.OK, msg);
        }

        protected ApiResponseMessage Error(ApiResponseCode statusCode, CustomMessage msg)
        {
            return CreateResponse(ApiResponseCode.OK, msg);
        }

        private ApiResponseMessage CreateResponse(ApiResponseCode statusCode, CustomMessage msg, object returnData = null, int totals = 0)
        {
            ApiResponseMessage apiResponseMsg = new ApiResponseMessage()
            {
                Data = returnData,
                ResponseCode = msg.MessageCode,
                ResponseMessage = msg.MessageValue,
                StatusCode = statusCode,
                Totals = totals
            };

            return apiResponseMsg;
        }


        protected Guid UserId
        {
            get
            {
                if (!Request.Headers.TryGetValues("X-UserId", out IEnumerable<string> result))
                {
                    return Guid.Empty;
                }
                else
                {
                    if (string.IsNullOrEmpty(result.ToList()[0].ToString()))
                    {
                        return Guid.Empty;
                    }
                    else
                    {
                        return Guid.Parse(result.ToList()[0].ToString());
                    }
                }
            }
        }
        protected string Language
        {
            get
            {
                if (!Request.Headers.TryGetValues("X-Language", out IEnumerable<string> result))
                {
                    return "en";
                }
                else
                {
                    return result.ToList()[0].ToString();
                }
            }
        }
    }
}
