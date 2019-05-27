using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace NK
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static partial class StringEX
    {
        #region 字符串判断

        /// <summary>
        /// 是否空字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是否空字符串</returns>
        public static bool IsNullOrEmpty(this string str)
        {
            str = string.IsNullOrEmpty(str) ? "" : str.Trim();
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 是否有空格
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是否有空格</returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 是否IP
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIPAdder(this string str)
        {
            System.Net.IPAddress val = System.Net.IPAddress.Any;
            return System.Net.IPAddress.TryParse(str, out val);
        }

        /// <summary>
        /// 是否DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDate(this string str)
        {
            DateTime  val = new DateTime();
            return DateTime.TryParse(str, out val);
        }

        /// <summary>
        /// 是否byte
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsByte(this string str)
        {
            byte val = 0;
            return byte.TryParse(str, out val);
        }

        /// <summary>
        /// 是否SHORT
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsShort(this string str)
        {
            short val = 0;
            return short.TryParse(str, out val);
        }

        /// <summary>
        /// 是否uSHORT
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsuShort(this string str)
        {
            ushort val = 0;
            return ushort.TryParse(str, out val);
        }

        /// <summary>
        /// 是否INT
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInteger(this string str)
        {
            int val =0;
            return int.TryParse(str, out val);
        }

        /// <summary>
        /// 是否UINT
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsuInt(this string str)
        {
            uint val = 0;
            return uint.TryParse(str, out val);
        }

        /// <summary>
        /// 是否long
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLong(this string str)
        {
            long val = 0;
            return long.TryParse(str, out val);
        }

        /// <summary>
        /// 是否ulong
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsuLong(this string str)
        {
            ulong val = 0;
            return ulong.TryParse(str, out val);
        }

        /// <summary>
        /// 是否float
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Isfloat(this string str)
        {
            float val = 0;
            return float.TryParse(str, out val);
        }

        /// <summary>
        /// 是否double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Isdouble(this string str)
        {
            double val = 0;
            return double.TryParse(str, out val);
        }

        /// <summary>
        /// 是否Guid
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsGuid(this string str)
        {
            Guid val = new Guid();
            return Guid.TryParse(str, out val);
        }

        #endregion

        #region 字符串转换

        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <param name="ListCap">字符串</param>
        /// <returns></returns>
        public static string ListNo(this string ListCap)
        {
            Random ra = new Random();
            if (string.IsNullOrEmpty(ListCap))
                ListCap = "";
            ListCap += DateTime.Now.ToString("yyyyMMddHHmmss") + ra.Next(1000, 9999).ToString();
            return ListCap;
        }

        /// <summary>
        /// 获取TOKEN
        /// </summary>
        /// <returns></returns>
        public static string Token(this string val)
        {
             Random rnd = new Random();
             int seed = 0;
            var rndData = new byte[4];
            rnd.NextBytes(rndData);
            var seedValue = Interlocked.Add(ref seed, 1);
            var seedData = BitConverter.GetBytes(seedValue); 
            var tokenData = rndData.Concat(seedData).OrderBy(_ => rnd.Next());
            return Convert.ToBase64String(tokenData.ToArray()).TrimEnd('=');
        }

        /// <summary>
        /// 字符串转INT
        /// </summary>
        /// <param name="obj">字符串</param>
        /// <returns></returns>
        public static int ToInteger(this string obj)
        {
            if (obj == null)
                return 0;
            int res = 0;
            int.TryParse(obj, out res);
            return res;
        }

        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="obj">字符串</param>
        /// <returns></returns>
        public static double ToDouble(this string obj)
        {
            if (obj == null)
                return 0;
            double res = 0;
            double.TryParse(obj, out res);
            return res;
        }

        /// <summary>
        /// byte转十六进制字符串
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="SpaceSplit">是否空格分割</param>
        /// <returns>十六进制字符串</returns>
        public static string ToHex(this byte[] data, bool SpaceSplit = true)
        {
            if (data == null)
                return "";
            else if (data.Length == 0)
                return "";
            string info = "";
            for (int i = 0; i < data.Length; i++)
            {
                info += " " + (data[i].ToString("X2").Length == 1 ? "0" + data[i].ToString("X2") : data[i].ToString("X2"));
            }
            info = info.TrimStart();
            if (!SpaceSplit)
                info = info.Replace(" ", "");
            return info;
        }

        /// <summary>
        /// 十六进制字符串转byte数组
        /// </summary>
        /// <param name="data">十六进制字符串</param>
        /// <returns>byte数组</returns>
        public static byte[] FromHex(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;
            data = data.Replace(" ", "");
            if ((data.Length % 2) != 0)
                data += " ";
            byte[] returnBytes = new byte[data.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(data.Substring(i * 2, 2), 16);
            return returnBytes;
        }


        /// <summary>
        /// byte数组转Base64
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>Base64</returns>
        public static string ToBase64(this byte[] data)
        {
            if (data == null)
                return "";
            else if (data.Length == 0)
                return "";
            return Convert.ToBase64String(data);
        }

        /// <summary>
        ///  BASE64转byte数组
        /// </summary>
        /// <param name="data">BASE64</param>
        /// <returns>byte数组</returns>
        public static byte[] FromBase64(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;
            return Convert.FromBase64String(data);
        }

        /// <summary>
        /// 字符串转byte[]
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="Encode">字符串编码</param>
        /// <returns>byte[]</returns>
        public static byte[] ToBytes(this string data, System.Text.Encoding Encode=null)
        {
            if (string.IsNullOrEmpty(data))
                return null;
            if (Encode == null)
                Encode = Encoding.Default;
            return Encode.GetBytes(data);
        }

        /// <summary>
        /// byte数组转字符串
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <param name="Encode">字符串编码</param>
        /// <returns>字符串</returns>
        public static string FormBytes(this byte[] data, System.Text.Encoding Encode=null)
        {
            if (data == null)
                return "";
            if (Encode == null)
                Encode = Encoding.Default;
            return Encode.GetString(data);
        }

        /// <summary>
        /// 字符串转时间
        /// </summary>
        /// <param name="data">时间字符串</param>
        /// <returns>时间</returns>
        public static DateTime FromDateTime(this string data)
        {
            DateTime DT = DateTime.Now;
            DateTime.TryParse(data, out DT);
            return DT;
        }

        /// <summary>
        /// 转换为时间字符串
        /// </summary>
        /// <param name="data">时间</param>
        /// <param name="Format">字符串格式</param>
        /// <returns>标准时间字符串</returns>
        public static string ToDateTime(this DateTime data, string Format = "")
        {
            if (string.IsNullOrEmpty(Format))
                return data.ToString();
            else
                return data.ToString(Format);
        }

        /// <summary>
        /// 字符串转GUID，空或非GUID字符串会重新生成
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string data)
        {
            Guid guid = new Guid();
            if (string.IsNullOrEmpty(data))
                return Guid.NewGuid();
            else if (Guid.TryParse(data, out guid))
                return guid;
            else
                return Guid.NewGuid();
        }

        /// <summary>
        /// 字符串转GUID，空或非GUID字符串会重新生成
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string FromGuid(this Guid data)
        {
            if (data != null)
                return data.ToString();
            else
                return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 字符串转IP
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static System .Net .IPAddress ToIPAddress(this string data)
        {
            System.Net.IPAddress val= System.Net.IPAddress.Any ;
            if(!string .IsNullOrEmpty (data))
               System.Net.IPAddress.TryParse(data,out val);
            return val;
        }

        /// <summary>
        /// IP转字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string FromIPAddress(this System.Net.IPAddress data)
        {
            if (data != null)
                return data.ToString();
            else
                return System.Net.IPAddress.Any.ToString();
        }

        /// <summary>
        /// URLENCODE
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUrlCode(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            else
                return System.Web.HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// URLDECODE
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FromUrlCode(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            else
                return System.Web.HttpUtility.UrlDecode(str);
        }

        /// <summary>
        /// HTMLENCODE
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToHTML(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            else
                return System.Web.HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// URLDECODE
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FromHTML(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            else
                return System.Web.HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="str">明码</param>
        /// <param name="Key">秘钥</param>
        /// <param name="CharSet">字符编码</param>
        /// <returns></returns>
        public static string MD5(this string str, string Key = "", Encoding CharSet = null)
        {
            if (CharSet == null)
                CharSet = Encoding.Default;
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(hashmd5.ComputeHash(CharSet.GetBytes(str + Key))).Replace("-", "");
        }

        #endregion

        #region 正则表达式

        /// <summary>
        /// 正则表达式
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="pattern">表达式</param>
        /// <returns>是否符合</returns>
        public static bool IsMatch(this string str, string pattern)
        {
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException("str");
            if (pattern.IsNullOrEmpty())
                throw new ArgumentNullException("pattern");
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 是否输入数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(this string str)
        {
            string pattern = "^[0-9]*$";
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException("str");
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 是否输入字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLetter(this string str)
        {
            string pattern = "^.[A-Za-z]+$";
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException("str");
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 是否输入小写字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLowLetter(this string str)
        {
            string pattern = "^.[a-z]+$";
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException("str");
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 是否输入大写字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUpLetter(this string str)
        {
            string pattern = "^.[A-Z]+$";
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException("str");
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 是否输入字母+数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLetterNum(this string str)
        {
            string pattern = "^.[A-Za-z0-9]+$";
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException("str");
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 是否汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsGB2312(this string str)
        {
            string pattern = "^[\u4e00-\u9fa5]{0,}$";
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException("str");
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 是否IP
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIP(this string str)
        {
            string pattern = "^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]).(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0).(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0).(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$";
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException("str");
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 是否网址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsURL(this string str)
        {
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException("str");
            else if (Regex.IsMatch(str.ToLower(), "^http:\\/\\/([\\w-]+(\\.[\\w-]+)+(\\/[\\w- .\\/\\?%&=\u4e00-\u9fa5]*)?)?$"))
                return true;
            else
                return Regex.IsMatch(str.ToLower(), "^https:\\/\\/([\\w-]+(\\.[\\w-]+)+(\\/[\\w- .\\/\\?%&=\u4e00-\u9fa5]*)?)?$");
        }

        /// <summary>
        /// 是否身份证号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIdentificationCard(this string str)
        {
            string pattern = "^[1-9]([0-9]{16}|[0-9]{13})[xX0-9]$";
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException("str");
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 是否电子邮箱
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEMail(this string str)
        {
            string pattern = "\\w+([-+.´]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException("str");
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 是否域名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDomain(this string str)
        {
            string pattern = "^[a-zA-Z0-9]+([a-zA-Z0-9\\-\\.]+)?\\.(com|org|net|cn|com.cn|edu.cn|grv.cn|)$";
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException("str");
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 是否域名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsMobile(this string str)
        {
            string pattern = "^((13[0-9])|(14[5|7])|(15([0-3]|[5-9]))|(18[0,1|5-9]))\\d{8}$";
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException("str");
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 是否域名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPostCode(this string str)
        {
            string pattern = "[1-9]{1}(\\d+){5}";
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException("str");
            return Regex.IsMatch(str, pattern);
        }

        #endregion

    }
}
