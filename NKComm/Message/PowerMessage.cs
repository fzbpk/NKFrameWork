using NK.ENum;
namespace NK.Message
{
    /// <summary>
    /// 权限账户信息
    /// </summary>
    public partial class PowerMessage
    {
        /// <summary>
        /// 用户名
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string AccountName(Language language= Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "用户名";
                default:
                    return "UserName";
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string AccountPassWord(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "密码";
                default:
                    return "PassWord";
            }
        }

        /// <summary>
        /// 号码
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Mobile(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "号码";
                default:
                    return "Mobile";
            }
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string AuthCode(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "验证码";
                default:
                    return "Auth Code";
            }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Login(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "登陆";
                default:
                    return "Login";
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Logout(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "注销";
                default:
                    return "Logout";
            }
        }

        /// <summary>
        /// 账户被禁用
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string AccountDisabled(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "账户被禁用";
                default:
                    return "Account had been disabled";
            }
        }

        /// <summary>
        /// 用户名为空
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string AccountNameNullOrEmpty(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "用户名为空";
                default:
                    return "UserName is empty";
            }
        }

        /// <summary>
        /// 密码为空
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string AccountPasswordNullOrEmpty(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "密码为空";
                default:
                    return "Password is empty";
            }
        }

        /// <summary>
        /// 找不到用户
        /// </summary>
        /// <param name="name"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string AccountNorFound(string name, Language language = Language.Chinese)
        {
            name = string.IsNullOrEmpty(name) ? "" : name;
            switch (language)
            {
                case Language.Chinese:
                    return "找不到账户"+ name;
                default:
                    return name+" can't found ";
            }
        }

        /// <summary>
        /// 找不到用户
        /// </summary>
        /// <param name="name"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string AccountFound(string name, Language language = Language.Chinese)
        {
            name = string.IsNullOrEmpty(name) ? "" : name;
            switch (language)
            {
                case Language.Chinese:
                    return  name+"已存在";
                default:
                    return name + "  already exists";
            }
        }

        /// <summary>
        /// 密码错误
        /// </summary> 
        /// <param name="language"></param>
        /// <returns></returns>
        public static string PasswordIncorrect(  Language language = Language.Chinese)
        { 
            switch (language)
            {
                case Language.Chinese:
                    return "密码错误";
                default:
                    return "Password Incorrect";
            }
        }

        /// <summary>
        /// 请登录
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string PleaseLogin(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "请登录";
                default:
                    return "Please Login";
            }
        }

        /// <summary>
        /// 请重新登录
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string PleaseReLogin(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "请重新登录";
                default:
                    return "Please Login again";
            }
        }

        /// <summary>
        /// 登录成功
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string LoginOK(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "登录成功";
                default:
                    return " Login success";
            }
        }

        /// <summary>
        /// 登录失败
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Loginfailed(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "登录失败";
                default:
                    return " Login failed";
            }
        }

        /// <summary>
        /// 登录超时
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string LoginTimeout(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "登录已超时";
                default:
                    return " Login has been time out";
            }
        }

        /// <summary>
        /// 已在其他地方登录
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string LoginKit(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "帐号已在别处登录 ，你将被强迫下线（请保管好自己的用户密码）！";
                default:
                    return " you had been login in other place,please check your account";
            }
        }

        /// <summary>
        /// RememberMe
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string RememberMe(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "记住我";
                default:
                    return "Remember Me";
            }
        }
         
        /// <summary>
        ///注册
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Regedit(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "注册";
                default:
                    return "Regedit";
            }
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string ForgoutPassword(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "忘记密码";
                default:
                    return "Forgout Password";
            }
        }

        /// <summary>
        ///注册成功
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string RegeditOK(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "注册成功";
                default:
                    return "Regedit Success";
            }
        }

        /// <summary>
        ///注册失败
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string RegeditFail(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "注册失败";
                default:
                    return "Regedit Fail";
            }
        }

    }
}
