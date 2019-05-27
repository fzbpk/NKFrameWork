using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Runtime.Serialization.Json; 
using System.Collections.Generic; 

namespace NK
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static partial class ClassEX
    {

        #region 对象型

        /// <summary>
        /// 空时NEW
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public static T New<T>(this T obj,bool force=false ) where T:class,new()
        {
            if (obj == null || force) obj = new T();
            return obj;
        }

        /// <summary>
        /// 空时NEW
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> New<T>(this List<T> obj)
        {
            if (obj == null) obj = new List<T>();
            return obj;
        }

        /// <summary>
        /// 空时NEW
        /// </summary>
        /// <typeparam name="M">Key</typeparam>
        /// <typeparam name="N">Value</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<M,N> New<M, N>(this Dictionary<M, N> obj)
        {
            if (obj == null) obj = new Dictionary<M, N>();
            return obj;
        }

        /// <summary>
        /// 对象类型是否可空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsNullable(this object obj)
        {
            if (obj == null)
                return true;
            else if (obj.GetType()==typeof(string))
                return true;
            else
                return (obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        /// <summary>
        /// 获取可空类型的原类型，如INT?返回INT
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Type NullType(this object obj)
        {
            if (obj == null)
                return null;
            else if (obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                return obj.GetType().GetGenericArguments().Length > 0 ? obj.GetType().GetGenericArguments()[0] : obj.GetType();
            else
                return obj.GetType();
        }

        /// <summary>
        /// 强转非空字符串
        /// </summary>
        /// <param name="obj">字符串</param>
        /// <returns>非空字符串</returns>
        public static string ToStringN(this object obj)
        {
            if (obj == null)
                return "";
            else
                return obj.ToString();
        }

        /// <summary>
        /// 对象是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        { return obj == null; }

        /// <summary>
        /// 对象非空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        { return obj != null; }

        /// <summary>
        /// 是否NULL或空
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="List">列表</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IList<T> List)
        {
            if (List == null)
                return true;
            else if (List.Count <= 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否NULL或空
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="List">列表</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this List<T> List)
        {
            if (List == null)
                return true;
            else if (List.Count <= 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否NULL或空
        /// </summary>
        /// <typeparam name="M">Key</typeparam>
        /// <typeparam name="N">Value</typeparam>
        /// <param name="List">列表</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<M,N>(this Dictionary<M,N> List)
        {
            if (List == null)
                return true;
            else if (List.Count <= 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否找到索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool FoundIndex(this int index)
        {
            return index > -1;
        }

        /// <summary>
        /// 是否找到
        /// </summary>
        /// <param name="List">查询结果</param>
        /// <returns></returns>
        public static bool Found<T>(this List<T> List)
        {
            return List.Count() > 0;
        }

        /// <summary>
        /// 是否找到
        /// </summary>
        /// <param name="List">查询结果</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static bool Found<T>(this IList<T> List, Func<T, bool> where)
        {
            return List.Where(where).Count()>0;
        }

        /// <summary>
        /// 是否找到
        /// </summary>
        /// <param name="List">查询结果</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static bool Found<T>(this List<T> List, Func<T, bool> where)
        {
            return List.Where(where).Count() > 0;
        }

        /// <summary>
        /// 是否找到
        /// </summary>
        /// <param name="entity">查询结果</param>
        /// <returns></returns>
        public static bool Found<T>(this T entity)
        {
            return entity!=null;
        }

        /// <summary>
        /// 是否找到
        /// </summary>
        /// <param name="Dict">字典</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public static bool Found<M,N>(this Dictionary<M,N> Dict,Func<KeyValuePair<M,N>,bool> where)
        {
            return Dict.Where(where).Count()>0;
        }

        #endregion

        #region DATA型

        /// <summary>
        /// DataTable转JSON
        /// </summary>
        /// <param name="DT">DataTable</param>
        /// <returns>JSON</returns>
        public static string ToJSON(this DataTable DT)
        {
            return NK.ClassTransform.DATATABLE.ToJSON(DT);
        }

        /// <summary>
        /// DataTable转XML
        /// </summary>
        /// <param name="DT">DataTable</param>
        /// <returns>XML</returns>
        public static string ToXML(this DataTable DT)
        {
            return NK.ClassTransform.DATATABLE.ToXML(DT);
        }

        #endregion

        #region JSON

        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }

        /// <summary>
        ///  转换为JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T t)
        {
            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ds.WriteObject(ms, t);

            string strReturn = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();

            string p = @"\\/Date\((\d+)([-+])(\d+)\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            strReturn = reg.Replace(strReturn, matchEvaluator);

            return strReturn;
        }

     

        /// <summary>
        /// 转换为JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        public static string ToJson<T>(this List<T> jsonObject)
        {
            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(System.Collections.Generic.List<T>));
            MemoryStream ms = new MemoryStream();
            ds.WriteObject(ms, jsonObject);

            string strReturn = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();


            string p = @"\\/Date\((\d+)([-+])(\d+)\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            strReturn = reg.Replace(strReturn, matchEvaluator);

            return strReturn;
        }

        /// <summary>
        /// JSON串转类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strJson">JSON串</param>
        /// <returns>类</returns>
        public static T FromJson<T>(this string strJson) where T : class
        {
          
            string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(p);
            strJson = reg.Replace(strJson, matchEvaluator);
            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(strJson));
            return ds.ReadObject(ms) as T;
        }

        /// <summary>
        /// JSON串转类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strJson">JSON串</param>
        /// <returns>类</returns>
        public static List<T> FromJsons<T>(this string strJson) where T : class
        {
            string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(p);
            strJson = reg.Replace(strJson, matchEvaluator);
            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(List<T>));
            MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(strJson));
            return ds.ReadObject(ms) as List<T>;
        }

        /// <summary>
        /// JSON转Dictionary
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static Dictionary<string,object> FromJsonDictionary(this string strJson)
        {
            string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(p);
            strJson = reg.Replace(strJson, matchEvaluator);
            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(Dictionary<string, object>));
            MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(strJson));
            return  ds.ReadObject(ms) as Dictionary<string, object>;
        }

        #endregion

    }
}
