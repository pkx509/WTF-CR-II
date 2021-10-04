using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DITS.HILI.Framework
{  
    public static class DateFuntion
    {
        public static string dateInterface_us(DateTime _date)
        {

            CultureInfo cultureinfo = new CultureInfo("en-US");
            return _date.ToString("yyyyMMdd", cultureinfo);
        }
    }
    public class Utilities
    {
        public static bool IsZeroGuid(Guid guid)
        {
            string format = "00000000-0000-0000-0000-000000000000";
            if (guid.ToString().Equals(format))
            {
                return true;
            }

            return false;
        }

        public static DateTime?[] GetTerm(DateTime? start, DateTime? end)
        {
            string culture = ConfigurationManager.AppSettings["CultureInfo"].ToString();
            CultureInfo cultureInfo = new CultureInfo(culture);

            start = (start == DateTime.Parse("0001-01-01", cultureInfo) ? null : start);
            end = (end == DateTime.Parse("0001-01-01", cultureInfo) ? null : end);

            DateTime? _start = null;
            DateTime? _end = null;
            if (start != null || end != null)
            {
                _start = DateTime.Parse(start.Value.ToString("yyyy/MM/dd 00:00:00"));
                _end = DateTime.Parse(end.Value.ToString("yyyy/MM/dd 23:59:59"));
            }
            DateTime?[] d = new DateTime?[] { _start, _end };
            return d;
        }

        public static string GetCurrentDirectory()
        {
            string codeBaseUrl = Assembly.GetExecutingAssembly().CodeBase;
            string filePathToCodeBase = new Uri(codeBaseUrl).LocalPath;
            string directoryPath = Path.GetDirectoryName(filePathToCodeBase);
            return directoryPath;
        }

        public static Dictionary<object, string> GetEnumDescription<T>()
        {
            List<string> items = typeof(T).GetEnumNames().ToList();
            Dictionary<object, string> Desc = new Dictionary<object, string>();
            foreach (string item in items)
            {
                int id = (int)Enum.Parse(typeof(T), item);
                string value = ((DescriptionAttribute)typeof(T).GetMember(item)[0].GetCustomAttribute(typeof(DescriptionAttribute), false)).Description;
                Desc.Add(id, value);
            }
            return Desc;
        }

        public static T ParseEnum<T>(string stringVal)
        {
            return (T)Enum.Parse(typeof(T), stringVal);
        }

        private static string GetDescription(Enum enumValue)
        {
            object[] attr = enumValue.GetType().GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attr.Length > 0
               ? ((DescriptionAttribute)attr[0]).Description
               : enumValue.ToString();
        }

        public static Dictionary<string, object> GetProductInfo<T>(Assembly assembly)
        {
            Assembly ass = typeof(T).Assembly;
            GuidAttribute attribute = (GuidAttribute)ass.GetCustomAttributes(typeof(GuidAttribute), true)[0];
            string id = attribute.Value;

            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            Dictionary<string, object> obj = new Dictionary<string, object>
            {
                { "ID", id },
                { "Version", version }
            };
            return obj;
        }

        public static T GetAssemblyAttribute<T>(Assembly assembly) where T : Attribute
        {
            if (assembly == null)
            {
                return null;
            }

            object[] attributes = assembly.GetCustomAttributes(typeof(T), true);

            if (attributes == null)
            {
                return null;
            }

            if (attributes.Length == 0)
            {
                return null;
            }

            return (T)attributes[0];
        }

        public static string ZeroPadding(int seq, int padding)
        {
            string stringSeq = seq.ToString();
            return stringSeq.PadLeft(padding, '0');
        }

    }
}
