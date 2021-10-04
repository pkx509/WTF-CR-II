using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Extensions
{
    public static class PagesExtensions
    {
        public static T GetApiResult<T>(ApiResponseMessage response, T result) where T : class
        {
            if (response.ResponseMessage == null)
                response.ResponseMessage = new List<string>();

            if (!response.IsSuccessHttpStatusCode)
            {
                 
                response.ResponseMessage.Add(response.StatusCode.GetResourceEnum());
            }
            else
            {
               
                if (response.ResponseCode !="0")
                    if (response.ResponseMessage != null && response.ResponseMessage.Count > 0)
                    {
                        
                        response.ResponseMessage.Add(response.ResponseMessage[0]);
                    }
                    else
                    {
                         
                        response.ResponseMessage.Add(ApiHttpStatusCode.UnhandledResult.GetResourceEnum());
                    }
                else
                {
                    if (response.ReturnedData != null)
                    {
                        result = response.ReturnedData.ToString().JsonDeserialize(result);

                        if (result == null)
                        {
                            controller.ModelState.AddModelError("", ApiHttpStatusCode.UnhandledResult.GetResourceEnum());
                            response.ResponseMessage.Add(ApiHttpStatusCode.UnhandledResult.GetResourceEnum());
                        }
                        else
                            return result;


                    }

                }
            }

            return default(T);
        }
        public static T GetApiResult<T>(  ApiResponseMessage response) where T : class
        {
            if (response.ResponseMessage == null)
                response.ResponseMessage = new List<string>();

            if (!response.IsSuccessHttpStatusCode)
            {
                controller.ModelState.AddModelError("", response.HttpStatusCode.GetResourceEnum());
                response.ResponseMessage.Add(response.HttpStatusCode.GetResourceEnum());
            }
            else
            {
                var config = EngineContext.Current.Resolve<PFReportConfig>();
                if (response.ResponseCode != config.ApiEpvd.SuccessResponseCode)
                    if (response.ResponseMessage != null && response.ResponseMessage.Count > 0)
                    {
                        controller.ModelState.AddModelError("", response.ResponseMessage[0]);
                        response.ResponseMessage.Add(response.ResponseMessage[0]);
                    }
                    else
                    {
                        controller.ModelState.AddModelError("", ApiHttpStatusCode.UnhandledResult.GetResourceEnum());
                        response.ResponseMessage.Add(ApiHttpStatusCode.UnhandledResult.GetResourceEnum());
                    }
                else
                {
                    if (response.ReturnedData != null)
                    {
                        var result = response.ReturnedData.ToString().JsonDeserialize<T>();

                        if (result == null)
                        {
                            controller.ModelState.AddModelError("", ApiHttpStatusCode.UnhandledResult.GetResourceEnum());
                            response.ResponseMessage.Add(ApiHttpStatusCode.UnhandledResult.GetResourceEnum());
                        }
                        else
                            return result;


                    }

                }
            }

            return default(T);

        }

        public static bool GetApiResultNoReturnedData( ApiResponseMessage response)
        {
            if (response.ResponseMessage == null)
                response.ResponseMessage = new List<string>();


            if (!response.IsSuccessHttpStatusCode)
            {
                controller.ModelState.AddModelError("", response.HttpStatusCode.GetResourceEnum());
                response.ResponseMessage.Add(response.HttpStatusCode.GetResourceEnum());
            }
            else
            {
                var config = EngineContext.Current.Resolve<PFReportConfig>();
                if (response.ResponseCode != config.ApiEpvd.SuccessResponseCode)
                    if (response.ResponseMessage != null && response.ResponseMessage.Count > 0)
                    {
                        controller.ModelState.AddModelError("", response.ResponseMessage[0]);
                        response.ResponseMessage.Add(response.ResponseMessage[0]);

                    }
                    else
                    {
                        controller.ModelState.AddModelError("", ApiHttpStatusCode.UnhandledResult.GetResourceEnum());
                        response.ResponseMessage.Add(ApiHttpStatusCode.UnhandledResult.GetResourceEnum());

                    }
                else
                    return true;

            }

            return false;

        }
    }
}
