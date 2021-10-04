using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Extensions
{
    public static string GetResourceEnum<T>(this T enumValue)
            where T : struct
    {

        string resourceName = string.Format("{0}.{1}",
            typeof(T).Name,
            enumValue.ToString());
        string result = Resources.LocaleStringResource.ResourceManager.GetString(resourceName);

        if (String.IsNullOrEmpty(result))
            result = enumValue.ToString();

        return result;
    }
}
