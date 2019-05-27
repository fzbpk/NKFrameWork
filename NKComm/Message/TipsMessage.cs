using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NK.ENum;
namespace NK.Message
{
    /// <summary>
    /// 提示语
    /// </summary>
     public partial class TipsMessage
    {
        /// <summary>
        /// 请选择记录
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Select(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "请选择记录";
                default:
                    return "Please select record";
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="result"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string ADD(bool result,Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "增加"+(result?"成功":"失败");
                default:
                    return "ADD " + (result ? "Success" : "Failed");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="result"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Delete(bool result, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "删除" + (result ? "成功" : "失败");
                default:
                    return "Delete " + (result ? "Success" : "Failed");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="result"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Edit(bool result, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "修改" + (result ? "成功" : "失败");
                default:
                    return "Edit " + (result ? "Success" : "Failed");
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="result"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Set(bool result, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "设置" + (result ? "成功" : "失败");
                default:
                    return "Ser " + (result ? "Success" : "Failed");
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="result"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Save(bool result, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "保存" + (result ? "成功" : "失败");
                default:
                    return "Save " + (result ? "Success" : "Failed");
            }
        }


        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="result"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Exec(bool result, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "执行" + (result ? "成功" : "失败");
                default:
                    return "Exec " + (result ? "Success" : "Failed");
            }
        }

        /// <summary>
        /// 删除确定
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string DeleteConfig( Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "该操作影响系统数据，确定删除所选的记录？";
                default:
                    return "This operation will affects the system data and determines the delete record selected";
            }

        }

        /// <summary>
        /// 编辑确定
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string EditConfig( Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "该操作影响系统数据，确定编辑所选的记录？";
                default:
                    return "This operation will affects the system data and determines the edit record selected";
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string AddConfig(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "确定添加记录";
                default:
                    return "Confirm Add this Record?";
            }
        }

        /// <summary>
        /// 没有执行权限
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string NoPower(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "没有执行权限";
                default:
                    return "No Power";
            }
        }

        /// <summary>
        /// 找不到记录
        /// </summary>
        /// <param name="message"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string NotFound(string message="",Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "找不到"+(string.IsNullOrEmpty(message)?"记录": message);
                default:
                    return (string.IsNullOrEmpty(message) ? "记录" : message)+" Not Found";
            }
        }

        /// <summary>
        /// 记录已存在
        /// </summary>
        /// <param name="message"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Found(string message = "", Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return  (string.IsNullOrEmpty(message) ? "记录" : message)+"已经存在";
                default:
                    return (string.IsNullOrEmpty(message) ? "Record" : message) + " already exists";
            }
        }

    }
}
