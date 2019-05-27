using NK.ENum;
using System.Collections.Generic;
using System.Linq;
namespace NK.Message
{
    /// <summary>
    /// 公共信息
    /// </summary>
    public partial class SystemMessage
    {
        /// <summary>
        /// 自定义
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Custom(Dictionary<string, Language> Message, Language language = Language.Chinese)
        {
            if (Message == null) Message = new Dictionary<string, Language>();
            string res = "";
            if(Message.Count>0)
            {
                if (Message.Where(c => c.Value == language).Count() <= 0)
                    res = Message.First().Key;
                else
                    res = Message.FirstOrDefault(c => c.Value == language).Key;
            }
            return res;
        }

        /// <summary>
        /// 不存在
        /// </summary>
        ///  <param name="Message"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string IsNotExist(string Message,Language language = Language.Chinese)
        {
            Message = string.IsNullOrEmpty(Message) ? "" : Message;
            switch (language)
            {
                case Language.Chinese:
                    return "1 "+ Message + "不存在";
                default:
                    return "1 " + Message + " Is Not Exist";
            }
        }

        /// <summary>
        /// 存在
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string IsExist(string Message, Language language = Language.Chinese)
        {
            Message = string.IsNullOrEmpty(Message) ? "" : Message;
            switch (language)
            {
                case Language.Chinese:
                    return "1 " + Message + "已存在";
                default:
                    return "1 " + Message + " Is   Exist";
            }
        }

        /// <summary>
        /// 执行开始
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string ExecStart(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "0 执行开始";
                default:
                    return "0 Start";
            }
        }

        /// <summary>
        /// 执行结束
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string ExecStop(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "0 执行结束";
                default:
                    return "0 Stop";
            }
        }

        /// <summary>
        /// 执行成功
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string ExecOK(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "0 执行成功";
                default:
                    return "0 OK";
            }
        }

        /// <summary>
        /// 执行失败
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string ExecFail(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "1 执行失败";
                default:
                    return "1 fail";
            }
        }

        /// <summary>
        /// 连接成功
        /// </summary>
        /// <param name="key"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Connect(string key,Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "0 "+ key + "连接成功";
                default:
                    return "0 " + key + "Connected";
            }
        }

        /// <summary>
        /// 连接断开
        /// </summary>
        /// <param name="key"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string DisConnect(string key, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "0 " + key + "连接断开";
                default:
                    return "0 " + key + "DisConnect";
            }
        }

        /// <summary>
        /// 操作不支持
        /// </summary>
        /// <param name="op"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string NotSupported(string op, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "1 " + op + "操作不支持";
                default:
                    return "1 " + op + "is not supported";
            }
        }

        /// <summary>
        /// 参数为空
        /// </summary>
        /// <param name="Ref"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string RefNullOrEmpty(string Ref,Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "1 "+ Ref + "为空或NULL";
                default:
                    return "1 "+Ref+" Is  NULL OR EMPTY";
            }
        }

        /// <summary>
        /// 转换失败
        /// </summary>
        /// <param name="Ref"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string CastError(string Ref, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "1 " + Ref + "转换失败";
                default:
                    return "1 " + Ref + " cast has a error";
            }
        }

        /// <summary>
        /// 显示参数和值
        /// </summary>
        /// <param name="Ref"></param>
        /// <param name="Val"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string RefValDisp(string Ref,string Val, Language language = Language.Chinese)
        {
            if (string.IsNullOrEmpty(Val))
                Val = "";
            switch (language)
            {
                case Language.Chinese:
                    return "1 " + Ref + "的值为"+ Val ;
                default:
                    return "1 the " + Ref + " value is " + Val ;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Init(string Object, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "0 初始化" + Object ;
                default:
                    return "0 " + Object + " init";
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string SET(string Object, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "0 设置" + Object ;
                default:
                    return "0 SET " + Object ;
            }
        }

        /// <summary>
        /// 检测
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Check(string Object, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "0 检测" + Object ;
                default:
                    return "0 Check " + Object ;
            }
        }

        /// <summary>
        /// 错误的命令顺序
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Badsequencecommands(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "1  错误的命令顺序";
                default:
                    return "1 Bad sequence of commands.";
            }
        }

        /// <summary>
        /// 证书过期
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string CertificateExpired(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "1  证书过期";
                default:
                    return "1 Certificate Expired.";
            }
        }

        /// <summary>
        /// 试用期已过
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string ProbationPeriodExamples(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "1  试用期已过";
                default:
                    return "1  Probation Period Examples.";
            }
        }

    }
}
