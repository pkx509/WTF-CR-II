using Newtonsoft.Json;

namespace DITS.HILI.WMS.Core.Domain
{
    public static class Extensions
    {
        #region Json
        public static T JsonDeserialize<T>(this string data) where T : class
        {
            if (string.IsNullOrEmpty(data))
            {
                return default(T);
            }

            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
                T item = JsonConvert.DeserializeObject<T>(data, settings);
                return item;
            }
            catch
            {
                return default(T);
            }
        }

        public static T JsonDeserialize<T>(this string data, T result) where T : class
        {
            if (string.IsNullOrEmpty(data))
            {
                return default(T);
            }

            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
                T item = JsonConvert.DeserializeAnonymousType(data, result, settings);
                return item;
            }
            catch
            {
                return default(T);
            }


        }
        #endregion

    }
}
