using DITS.HILI.WMS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DITS.HILI.WMS.DailyPlanAPIs.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected ApiResponseMessage Succeed(object returnData = null)
        {
            return CreateResponse(ApiResponseCode.OK, returnData);
        }

        protected ApiResponseMessage Succeed(object returnData = null,int totals =0)
        {
            return CreateResponse(ApiResponseCode.OK, returnData, totals);
        }


        protected ApiResponseMessage Succeed(Dictionary<string, string> messages, object returnData = null)
        {
            return CreateResponse(ApiResponseCode.OK, messages, returnData);
        }

        protected ApiResponseMessage CreateResponse(ApiResponseCode statusCode, object returnData = null)
        { 
            var apiResponseMsg = new ApiResponseMessage();
            if (statusCode == ApiResponseCode.OK)
            {
                apiResponseMsg.StatusCode = statusCode;
                apiResponseMsg.ResponseCode = "0";
                apiResponseMsg.ResponseMessage = new List<string>();
                apiResponseMsg.Data = returnData;
            }
            else
            {
                apiResponseMsg.ResponseCode = "404";
                apiResponseMsg.ResponseMessage = new List<string> { "Resource not found" };
            } 

            return apiResponseMsg;
        }
        protected ApiResponseMessage CreateResponse(ApiResponseCode statusCode, object returnData = null,int totals =0)
        {
            var apiResponse = new ApiResponseMessage();
            var apiResponseMsg = new ApiResponseMessage();
            if (statusCode == ApiResponseCode.OK)
            {
                apiResponseMsg.StatusCode = statusCode;
                apiResponseMsg.ResponseCode = "0";
                apiResponseMsg.ResponseMessage = new List<string>();
                apiResponseMsg.Data = returnData;
                apiResponseMsg.Totals = totals;
            }
            else
            {
                apiResponseMsg.ResponseCode = "404";
                apiResponseMsg.ResponseMessage = new List<string> { "Resource not found" };
            } 

            return apiResponseMsg;
        }

        protected ApiResponseMessage CreateResponse(ApiResponseCode statusCode, Dictionary<string, string> messages, object returnData = null)
        {
            var apiResponse = new ApiResponseMessage();
            var apiResponseMsg = new ApiResponseMessage();

            apiResponseMsg.ResponseCode = "404";
            apiResponseMsg.ResponseMessage = new List<string> { "Resource not found" };


            return apiResponseMsg;
        }
    }
}