using System.Configuration;

namespace DITS.HILI.WMS.WebAPIs
{

    public class Configure
    {
        public static string ConnectionString()
        {

            //Get connectionstring from web.config
            return ConfigurationManager.AppSettings["ConnectionString"].ToString();

        }

        public static string ClientId => ConfigurationManager.AppSettings["clientId"].ToString();

        public static string ClientSecret => ConfigurationManager.AppSettings["clientSecret"].ToString();

        public static string AllowOrigin => ConfigurationManager.AppSettings["AllowOrigin"].ToString();

        public static int TokenLifeTime => int.Parse(ConfigurationManager.AppSettings["TokenLifeTime"].ToString());

        public static int RefreshTokenLifeTime => int.Parse(ConfigurationManager.AppSettings["RefreshTokenLifeTime"].ToString());
    }
}