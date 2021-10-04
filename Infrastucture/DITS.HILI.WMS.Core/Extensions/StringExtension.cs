using System;
using System.ComponentModel;
using System.Globalization;

namespace DITS.WMS.Common.Extensions
{

    public static class StringExtension
    {
        public static string ToDateString(this object value)
        {
            if (value == null)
            {
                return "0000-00-00 00:00:00";
            }
            else
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        return "0000-00-00 00:00:00";
                    }

                    DateTime.TryParse(value.ToString(), out DateTime dt);
                    return dt.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static string ToDateOnlyString(this object value)
        {
            if (value == null)
            {
                return "0000-00-00 00:00:00";
            }
            else
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        return "0000-00-00 00:00:00";
                    }

                    DateTime.TryParse(value.ToString(), out DateTime dt);
                    return dt.ToString("yyyy-MM-dd 00:00:00", new CultureInfo("en-US"));
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }


        public static byte ToByte(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }
            return byte.Parse(value);
        }

        public static int ToInt(this string value)
        {
            try
            {
                if (int.TryParse(value, out int a))
                {
                    return a;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static long ToLong(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }
            return long.Parse(value);
        }

        public static bool ToBool(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            try
            {
                return bool.Parse(value);
            }
            catch
            {
                return false;
            }
        }

        public static int BoolToInt(this string value)
        {
            try
            {
                if (value.ToLower() == "true")
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static string ToValue(this string value)
        {
            try
            {
                return value.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static int ToValue(this int value)
        {
            try
            {
                return value;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static double StringToWeight(this string Item)
        {
            try
            {
                if (double.TryParse(Item, out double a))
                {

                    return Math.Round(a, 3);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static double StringToDouble(this string Item)
        {
            try
            {
                if (double.TryParse(Item, out double a))
                {
                    return a;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static decimal StringToDecimal(this string Item)
        {
            try
            {
                if (decimal.TryParse(Item, out decimal a))
                {
                    return a;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string Description(this Enum value)
        {
            // variables  
            Type enumType = value.GetType();
            System.Reflection.FieldInfo field = enumType.GetField(value.ToString());
            object[] attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // return  
            return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }

        //public static string GetStatus(this StatusCode value)
        //{
        //    var type = typeof(StatusCode);
        //    var memInfo = type.GetMember(value.ToString());
        //    var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
        //    var status = ((DescriptionAttribute)attributes[0]).Description;

        //    return status;
        //}

    }
}
