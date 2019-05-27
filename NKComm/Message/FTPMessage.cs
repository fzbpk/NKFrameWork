using NK.ENum;
namespace NK.Message
{
    /// <summary>
    /// RTP信息
    /// </summary>
   public partial class FTPMessage
   {
        /// <summary>
        /// 执行成功
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string CMDOK(Language language= Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "200 成功\r\n";
                default:
                    return "200 OK\r\n";
            }
        }

        /// <summary>
        /// 服务已准备好
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string MessReady(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "220 对新用户服务准备好\r\n";
                default:
                    return "220 FTP Server Ready\r\n"; 
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string SignOff(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "221 服务关闭控制连接，可以退出登录\r\n";

                default:
                    return "221 FTP Server Signing off\r\n";
            }
        }

        /// <summary>
        /// 已达最大错误数
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string MessTooManyBadCmds(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "421 已达最大错误数\r\n";
                default:
                    return "421 Too many bad Commands\r\n";
            }
        }

        /// <summary>
        /// 执行超时
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string CmdTimeOut(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "500 命令已超时\r\n";
                default:
                    return "500 Command Timeout\r\n";
            }
        }

        /// <summary>
        /// 未知错误
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string UnknownError(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "500 未知错误\r\n";
                default:
                    return "500 unkown error\r\n";
            }
        }

        /// <summary>
        /// 重复执行认证
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string AlreadyAuth(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.English:
                    return "500 你已成功登陆\r\n";
                default:
                    return "500 You are already authenticated\r\n";
            }
        }

        /// <summary>
        /// 请输入指定用户密码
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string UserButNotPass(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "500 请输入指定用户密码\r\n";
                default:
                    return "500 username is already specified, please specify password\r\n";
            }
        }

        /// <summary>
        /// 请输入用户名
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string EnterUser(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "503 请输入用户名\r\n";
                default:
                    return "503 please specify username first\r\n";
            }
        }

        /// <summary>
        /// 语法错误
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string SyntaxError(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "500 语法错误\r\n";
                default:
                    return "500 Syntax error\r\n";
            }
        }

        /// <summary>
        /// 密码已获取
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string PassOk(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "230 用户登录\r\n";
                default:
                    return "230 Password ok\r\n"; 
            }
        }

        /// <summary>
        /// 改变工作目录成功
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string CwdOk(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "250 改变工作目录成功.\r\n";
                default:
                    return "250 CWD command successful.\r\n"; 
            }
        }

        /// <summary>
        /// 删除成功
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string DeleOk(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "250 删除成功\r\n"; 
                default:
                    return "250 DELE command successful.\r\n";
            }
        }

        /// <summary>
        /// 登陆失败
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string AuthFailed(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "530 登陆失败\r\n";
                default:
                    return "530 UserName or Password is incorrect\r\n"; 
            }
        }

        /// <summary>
        /// 请输入密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string PassReq(string userName, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "331  用户名" + userName + "正确，需要口令\r\n";
                default:
                    return "331 Password required for user : '" + userName + "\r\n";
            }
        }

        /// <summary>
        /// 当前目录
        /// </summary>
        /// <param name="curdir"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Pwd( string curdir,Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "257 当前目录:\"" + curdir + "\"\r\n";
                default:
                    return "257 \"" + curdir + "\" is current directory.\r\n";
            }
        }

        /// <summary>
        /// 非当前目录
        /// </summary>
        /// <param name="curdir"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string PwdFal( string curdir, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "502 \"" + curdir + "\"非当前目录\r\n";
                default:
                    return "502 \"" + curdir + "\" is not current directory.\r\n";
            }
        }

        /// <summary>
        /// 类型设置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string TypeSet( string type, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "200 类型已转为 : " + type + ".\r\n";
                default:
                    return "200 Type is set to " + type + ".\r\n";
            }
        }

        /// <summary>
        /// 错误的类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string InvalidType(  string type, Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "500 格式错误，命令不可识别 : " + type + ".\r\n";
                default:
                    return "500 Invalid type : " + type + ".\r\n";
            }
        }

        /// <summary>
        /// 请先登陆
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string AuthReq(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "530 未登录\r\n";
                default:
                    return "530 Please authenticate first\r\n";
            }
        }

        /// <summary>
        /// 主动传输模式转换成功
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string PortCmdSuccess(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "200 主动传输模式转换成功\r\n";
                default:
                    return "200 PORT Command successful\r\n";
            }
        }

        /// <summary>
        /// 连接已打开
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string DataOpen(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "125 数据连接已打开，准备传送.\r\n";
                default:
                    return "125 Data connection open, Transfer starting.\r\n";
            }
        }

        /// <summary>
        /// 连接正在打开
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string DataOpening(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "150 文件状态良好，打开数据连接.\r\n";
                default:
                    return "150 Opening data connection.\r\n";
            }
        }

        /// <summary>
        /// 连接打开失败 
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string DataConFailed(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "425 不能打开数据连接.\r\n";
                default:
                    return "425 Can't open data connection.\r\n";
            }
        }

        /// <summary>
        /// 传输成功
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string TrComplete(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "226 传输成功.\r\n";
                default:
                    return "226 Transfer Complete.\r\n";
            }
        }

        /// <summary>
        /// 传输失败
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string TrFailed(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "426 关闭连接，中止传输.\r\n";
                default:
                    return "426 Connection closed; transfer aborted.\r\n";
            }
        }

        /// <summary>
        /// 没有权限
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string AccesDenied(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "550 没有权限\r\n";
                default:
                    return "550 Access denied or directory dosen't exist !\r\n";
            }
        }

        /// <summary>
        /// 切换被动模式成功
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string PasvCmdSuccess(string ip,Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "227 Entering Passive Mode (" + ip + ").\r\n";
                default:
                    return "227 Entering Passive Mode (" + ip + ").\r\n";
            }
        }

        /// <summary>
        /// 超时已退出
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string TimeOut(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "500  超时已退出\r\n";
                default:
                    return "500 Session timeout, OK FTP server signing off\r\n";
            }
        }

        /// <summary>
        /// NOOP
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string NOOPOK(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "200 成功\r\n";
                default:
                    return "200 OK \r\n";
            }
        }

        /// <summary>
        /// 目录创建成功
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string DirCreatedOK(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "257  目录创建成功\r\n";
                default:
                    return "257 Directory Created. \r\n";
            }
        }

        /// <summary>
        /// 请输入要修改的文件名
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string RNFRFaile(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "350  请求的文件操作需要进一步命令\r\n";
                default:
                    return "350 Please specify destination name.\r\n";
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
                    return "503  错误的命令顺序\r\n";
                default:
                    return "503 Bad sequence of commands. \r\n";
            }
        }

        /// <summary>
        /// 错误的文件名或目录名
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Errorrenameing(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "550  格式错误，命令不可识别\r\n";
                default:
                    return "550 Error renameing directory or file .\r\n";
            }
        }

        /// <summary>
        /// 目录重名
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Directoryrenamed(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "250 请求的文件操作完成\r\n";
                default:
                    return "250 Directory renamed.\r\n";
            }
        }

        /// <summary>
        /// REST参数有误
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string badargumentREST(Language language= Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "554  REST参数有误\r\n";
                default:
                    return "554 bad argument for REST\r\n";
            }
        }

        /// <summary>
        /// 返回REST
        /// </summary>
        /// <param name="OffSet"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string argumentREST(  int OffSet, Language language = Language.Chinese)
        {
            return "350 " + OffSet.ToString() + "\r\n";
        }

        /// <summary>
        /// 主目录有误
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string BadHome(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "530 未登录网络\r\n";
                default:
                    return "530 Bad Home \r\n";
            }
        }

        /// <summary>
        /// 无法打开数据连接
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string opendatafailed(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "425 无法打开数据连接\r\n";
                default:
                    return "425 Can't open data connection.\r\n";
            }
        }

        /// <summary>
        /// 命令没有执行
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Invalidcommand(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "502 命令没有执行\r\n";
                default:
                    return "502 Invalid command  \r\n";
            }
        }

        /// <summary>
        /// Noted
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Noted(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "200 \r\n";
                default:
                    return "200 Noted\r\n";
            }
        }

    }
}
