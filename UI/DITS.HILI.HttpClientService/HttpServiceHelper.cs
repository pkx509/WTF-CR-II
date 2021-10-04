namespace DITS.HILI.HttpClientService
{
    public class HttpClientServiceHelper
    {

        private static string baseUrl { get; set; }
        public static string BaseUrl
        {
            get => baseUrl;
            set => baseUrl = value;
        }
        private static string shareSecret { get; set; }
        public static string ShareSecret
        {
            get => shareSecret;
            set => shareSecret = value;
        }
        private static string clientId { get; set; }
        public static string ClientId
        {
            get => clientId;
            set => clientId = value;
        }
        private static string clientSecret { get; set; }
        public static string ClientSecret
        {
            get => clientId;
            set => clientId = value;
        }

        private static int pageSize { get; set; }
        public static int PageSize
        {
            get => pageSize;
            set => pageSize = value;
        }

    }
}
