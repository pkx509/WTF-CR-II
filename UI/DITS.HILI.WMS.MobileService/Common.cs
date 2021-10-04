using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.MasterModel.Core;

namespace DITS.HILI.WMS.MobileService
{
    public class Common
    {
        public static UserAccounts User
        {
            get
            {
                return new UserAccounts
                {
                    UserID = new Guid("917ED702-7D31-4F97-8180-B88A29CAF3A1")
                };
            }
            set { }
        }
        public static Token AccessToken { get; set; }
        public static string Language { get; set; }
    }
}