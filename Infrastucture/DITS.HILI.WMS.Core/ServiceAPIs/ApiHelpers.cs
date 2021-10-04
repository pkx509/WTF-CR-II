using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace DITS.HILI.WMS.Core.ServiceAPIs
{

    public class ApiHelpers
    {
        public static HttpCustomHeaders Response(HttpRequestMessage request)
        {
            HttpCustomHeaders headers = new HttpCustomHeaders();
            if (request == null)
            {
                return headers;
            }

            if (!request.Headers.TryGetValues("X-PageIndex", out IEnumerable<string> result))
            {
                headers.PageIndex = null;
            }
            else
            {
                headers.PageIndex = int.Parse(result.ToList()[0].ToString());
            }

            if (!request.Headers.TryGetValues("X-PageSize", out result))
            {
                headers.PageSize = null;
            }
            else
            {
                headers.PageSize = int.Parse(result.ToList()[0].ToString());
            }

            if (!request.Headers.TryGetValues("X-TotalRecords", out result))
            {
                headers.TotalRecords = 0;
            }
            else
            {
                headers.TotalRecords = int.Parse(result.ToList()[0].ToString());
            }

            if (!request.Headers.TryGetValues("X-UserId", out result))
            {
                headers.UserID = null;
            }
            else
            {
                if (string.IsNullOrEmpty(result.ToList()[0].ToString()))
                {
                    headers.UserID = null;
                }
                else
                {
                    headers.UserID = Guid.Parse(result.ToList()[0].ToString());
                }
            }

            if (!request.Headers.TryGetValues("ProductOwnerID", out result))
            {
                headers.ProductOwnerID = null;
            }
            else
            {

                if (string.IsNullOrEmpty(result.ToList()[0].ToString()))
                {
                    headers.ProductOwnerID = null;
                }
                else
                {
                    headers.ProductOwnerID = Guid.Parse(result.ToList()[0].ToString());
                }
            }

            if (!request.Headers.TryGetValues("X-Language", out result))
            {
                headers.Language = string.Empty;
            }
            else
            {
                headers.Language = result.ToList()[0].ToString();
            }

            if (headers.PageSize == 0)
            {
                headers.PageIndex = null;
                headers.PageSize = null;
            }


            return headers;
        }
    }
}
