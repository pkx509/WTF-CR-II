using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DITS.HILI.Framework
{

    public class Prefix
    {

        /// <summary>
        /// Create single prefix code
        /// </summary>
        /// <param name="Program_Code"></param>
        /// <param name="sysLastedKey"></param>
        /// <param name="sysPrefixKey"></param>
        /// <param name="sysFormatKey"></param>
        /// <param name="sysLengthKey"></param>
        /// <returns></returns>
        public static string OnCreatePrefixed(string sysLastedKey, string sysPrefixKey, string sysFormatKey, int? sysLengthKey)
        {
            try
            {

                return createPrefixed((sysLastedKey ?? ""),
                    (sysPrefixKey ?? ""), (sysFormatKey ?? ""), (sysLengthKey ?? 0));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<string> OnCreatePrefixed(string sysLastedKey, string sysPrefixKey, string sysFormatKey, int? sysLengthKey, int Count)
        {
            try
            {
                List<string> _keyNo = new List<string>();
                int _lastSequence = 0;
                string _sysPrefixkey = (sysPrefixKey ?? "");
                string _sysFormatKey = (sysFormatKey ?? "");
                string _resultFormatKey = "";

                CultureInfo cultureinfo = new CultureInfo("en-US");
                DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd", cultureinfo));
                if (!string.IsNullOrEmpty(_sysFormatKey))
                {
                    _resultFormatKey = dt.ToString(_sysFormatKey);
                }

                StringBuilder _key = new StringBuilder();
                for (int index = 0; index < sysLengthKey.Value; index++)
                {
                    _key.Append("0");
                }

                if (!string.IsNullOrWhiteSpace(sysLastedKey))
                {
                    _lastSequence = int.Parse(sysLastedKey.Substring(_sysPrefixkey.Length + _sysFormatKey.Length));
                }

                for (int index = 1; index <= Count; index++)
                {
                    string _lastKeyNo = _sysPrefixkey + _resultFormatKey + _lastSequence.ToString(_key.ToString());
                    _keyNo.Add(createPrefixed((_lastKeyNo ?? ""), (sysPrefixKey ?? ""), (sysFormatKey ?? ""), (sysLengthKey ?? 0)));
                    _lastSequence += 1;
                }

                return _keyNo;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        private static string createPrefixed(string sysLastKey, string sysPrefixKey, string sysFormatKey, int sysLengthKey)
        {
            StringBuilder key = new StringBuilder();
            string AutoNo = string.Empty;
            string resultFormatKey = string.Empty;
            string Sequence = string.Empty;
            string lastFormatKey = string.Empty;
            string NewKey;
            try
            {

                string lastKeyNo = sysLastKey;
                string prefixKey = sysPrefixKey;
                int Length = sysLengthKey;

                for (int index = 0; index < Length; index++)
                {
                    key.Append("0");
                }

                if (!string.IsNullOrEmpty(sysFormatKey))
                {
                    CultureInfo cultureinfo = new CultureInfo("en-US");
                    DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd", cultureinfo));
                    resultFormatKey = dt.ToString(sysFormatKey);

                    if (!string.IsNullOrEmpty(lastKeyNo))
                    {
                        lastFormatKey = lastKeyNo.Substring(prefixKey.Length, sysFormatKey.Length);
                        if (int.Parse(resultFormatKey.Replace("-", "")) > int.Parse(lastFormatKey.Replace("-", "")))
                        {
                            NewKey = genNewKey(prefixKey, resultFormatKey, key.ToString());
                        }
                        else
                        {
                            NewKey = genKey(prefixKey, sysLastKey, lastFormatKey, key.ToString());
                        }
                    }
                    else
                    {
                        NewKey = genNewKey(prefixKey, resultFormatKey, key.ToString());
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastKeyNo))
                    {
                        NewKey = genKey(prefixKey, sysLastKey, sysFormatKey, key.ToString());
                    }
                    else
                    {
                        NewKey = genNewKey(prefixKey, sysFormatKey, key.ToString());
                    }
                }

                return NewKey;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string genNewKey(string prefixKey, string FormatKey, string key)
        {
            int lastSequence = 1;
            string Sequence = lastSequence.ToString(key);
            string _lastKeyNo = prefixKey + FormatKey + Sequence;
            return _lastKeyNo;
        }

        private static string genKey(string prefixKey, string lastKeyNo, string FormatKey, string key)
        {
            int lastSequence = int.Parse(lastKeyNo.Substring((prefixKey + FormatKey).Length)) + 1;
            string Sequence = lastSequence.ToString(key.ToString());
            string _lastKeyNo = prefixKey + FormatKey + Sequence;
            return _lastKeyNo;
        }
    }
}
