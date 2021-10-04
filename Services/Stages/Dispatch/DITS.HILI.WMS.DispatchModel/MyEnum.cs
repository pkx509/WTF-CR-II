using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.DispatchModel
{
    public static class MyEnum
    {
        public static string GetDispatchEnumDescription(DispatchStatusEnum status)
        {
            Type type = typeof(DispatchStatusEnum);
            System.Reflection.MemberInfo[] memInfo = type.GetMember(status.ToString());
            object[] attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            string description = ((DescriptionAttribute)attributes[0]).Description;
            return description;
        }
    }
}
