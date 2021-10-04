using System.Linq;

namespace DITS.HILI.WMS.Message
{
    public class MessageManger
    {
        public static string GetMessage(string code, params object[] param)
        {
            string message = code;
            int index = 0;
            param.ToList().ForEach(item =>
            {
                message += "\r\n" + item.ToString();
                index++;
            });

            return message;
        }
    }
}
