using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NK.Entity;
using NK.ENum;
namespace NK.Data
{
    /// <summary>
    /// 连接串生成类
    /// </summary>
    public static partial class ConnectionHelper
    {
        /// <summary>
        /// 是否启用数据库配置
        /// </summary>
        /// <param name="info">数据库信息</param>
        /// <returns>是否启用数据库配置</returns>
        public static bool IsDBEnable(this DBInfo info)
        {
            if (info == null)
                return false;
            else
                return info.Enable;
        }

        /// <summary>
        ///  数据库驱动
        /// </summary>
        /// <param name="info">数据库信息</param>
        /// <returns>数据库驱动</returns>
        public static string getDbDriver(this DBInfo info)
        {
            if (info == null)
                return "";
            else
                return info.Driver;
        }

        /// <summary>
        ///  数据库地址
        /// </summary>
        /// <param name="info">数据库信息</param>
        /// <returns>数据库地址</returns>
        public static string getDbUrl(this DBInfo info)
        {
            if (info == null)
                return "";
            else
                return info.Url;
        }

        /// <summary>
        ///  数据库端口
        /// </summary>
        /// <param name="info">数据库信息</param>
        /// <returns>数据库端口</returns>
        public static int getDbPort(this DBInfo info)
        {
            if (info == null)
                return 0;
            else
                return info.Port;
        }

        /// <summary>
        ///  数据库账号
        /// </summary>
        /// <param name="info">数据库信息</param>
        /// <returns>数据库账号</returns>
        public static string getDbUser(this DBInfo info)
        {
            if (info == null)
                return "";
            else
                return info.User;
        }

        /// <summary>
        ///  数据库密码
        /// </summary>
        /// <param name="info">数据库信息</param>
        /// <returns>数据库密码</returns>
        public static string getDbPassword(this DBInfo info)
        {
            if (info == null)
                return "";
            else
                return info.Password;
        }

        /// <summary>
        ///  数据库类型
        /// </summary>
        /// <param name="info">数据库信息</param>
        /// <returns>数据库类型</returns>
        public static string getDbName(this DBInfo info)
        {
            if (info == null)
                return "";
            else
                return info.DataBaseName;
        }

        /// <summary>
        ///  数据库超时
        /// </summary>
        /// <param name="info">数据库信息</param>
        /// <returns>数据库超时</returns>
        public static int getDbTimeout(this DBInfo info)
        {
            if (info == null)
                return 0;
            else
                return info.TimeOut;
        }

        /// <summary>
        ///  数据库编码
        /// </summary>
        /// <param name="info">数据库信息</param>
        /// <returns>数据库编码</returns>
        public static System.Text.Encoding getDbCharset(this DBInfo info)
        {
            if (info == null)
                return System.Text.Encoding.Default;
            else
                return System.Text.Encoding.GetEncoding(info.Charset);
        }

        /// <summary>
        /// 数据库连接串
        /// </summary>
        /// <param name="info">数据库信息</param>
        /// <returns>连接串</returns>
        public static string ConnectionString(this DBInfo info)
        {
            if (info == null)
                return "";
            else if (!info.Enable)
                return "";
            if (!string.IsNullOrEmpty(info.ConnStr))
                return info.ConnStr;
            string connstr = "";
            if (string.IsNullOrEmpty(info.Url))
                info.Url = "(local)";
            switch (info.Mode)
            {
                case DBType.Access:
                    if (string.IsNullOrEmpty(info.User) && string.IsNullOrEmpty(info.Password))
                    {
                        if (string.IsNullOrEmpty(info.User))
                            info.User = "admin";
                        if (string.IsNullOrEmpty(info.Password))
                            connstr = "Provider=Microsoft.ACE.OLEDB.12.0;User ID=" + info.User + ";Data Source=" + (info.Url.EndsWith("\\") ? info.Url + info.DataBaseName : info.Url + "\\" + info.DataBaseName) + ";Mode=Share Deny Read|Share Deny Write;";
                        else
                            connstr = "Provider=Microsoft.ACE.OLEDB.12.0;User ID=" + info.User + ";Data Source=" + (info.Url.EndsWith("\\") ? info.Url + info.DataBaseName : info.Url + "\\" + info.DataBaseName) + ";Jet OLEDB:New Database Password=\"" + info.Password + "\";Mode=Share Deny Read|Share Deny Write;";
                    }
                    else
                        connstr = "Provider=Microsoft.ACE.OLEDB.12.0;User ID=" + info.User + ";Data Source=" + (info.Url.EndsWith("\\") ? info.Url + info.DataBaseName : info.Url + "\\" + info.DataBaseName) + ";Jet OLEDB:New Database Password=\"" + info.Password + "\";Mode=Share Deny Read|Share Deny Write;";
                    break;

                case DBType.MYSQL:
                    if (string.IsNullOrEmpty(info.Charset))
                        info.Charset = "gb2312";
                    if (info.Port == 0)
                        info.Port = 3306;
                    connstr = "Server=" + info.Url + ";Port=" + info.Port.ToString() + ";Database=" + info.DataBaseName + ";Uid=" + info.User + ";Pwd=" + info.Password + ";CharSet=" + info.Charset + ";";
                    break;

                case DBType.MSSQL:
                    if (string.IsNullOrEmpty(info.Url))
                        info.Url = ".";
                    if (info.Port > 0 && info.Port !=1433 )
                        connstr = "Data Source=" + info.Url + ":" + info.Port.ToString() + ";Initial Catalog=" + info.DataBaseName + ";Persist Security Info=True;Uid=" + info.User + ";Pwd=" + info.Password + ";";
                    else
                        connstr = "Server=" + info.Url + ";Database=" + info.DataBaseName + ";Uid=" + info.User + ";Pwd=" + info.Password + ";";
                    break;

                case DBType.Oracle:
                    if (string.IsNullOrEmpty(info.Url))
                        info.Url = "localhost";
                    if (info.Port == 0)
                        info.Port = 1522;
                    // connstr = "Persist Security Info=True;User ID=" + info.User + ";Password=" + info.Password + ";Data Source= (DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = " + info.Url + ")(PORT = " + info.Port.ToString() + ")))(CONNECT_DATA =(SERVICE_NAME = " + info.DataBaseName + ")));";
                      connstr = "Data Source="+info.Url+((info.Port==1522 || info.Port==0)?"":":"+info.Port.ToString()) + "/" + info.DataBaseName + ";User Id=" + info.User + ";Password=" + info.Password + "; ";
                    //connstr = "Password=czh;User ID=czh;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.168.211)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + info.DataBaseName + ")));";
                    break;

                case DBType.PostgreSQL:
                    if (string.IsNullOrEmpty(info.Url))
                        info.Url = "localhost";
                    if (info.Port > 0)
                        connstr = "Server=" + info.Url + ";Port=" + info.Port.ToString() + ";Database=" + info.DataBaseName + ";Uid=" + info.User + ";Pwd=" + info.Password + ";";
                    else
                        connstr = "Server=" + info.Url + ";Database=" + info.DataBaseName + ";Uid=" + info.User + ";Pwd=" + info.Password + ";";
                    break;

                case DBType.SQLite:
                    connstr = "Data Source =" + (info.Url.EndsWith("\\") ? info.Url + info.DataBaseName : info.Url + "\\" + info.DataBaseName);
                    break;

                case DBType.ODBC:
                     connstr = "Driver={"+ info.Driver + "};dbq=" + (string.IsNullOrEmpty(info.Url) ? info.DataBaseName : (info.Url.EndsWith("\\") ? info.Url + info.DataBaseName : info.Url + "\\" + info.DataBaseName)) + ";uid=" + info.User + ";pwd=" + info.Password + ")";
                    break;

                case DBType.OleDB:
                    connstr = "Provider=" + info.Driver + ";data source=" +(string.IsNullOrEmpty(info.Url)? info.DataBaseName : (info.Url.EndsWith("\\") ? info.Url + info.DataBaseName : info.Url + "\\" + info.DataBaseName))  + ";user id=" + info.User + ";password=" + info.Password + ";";
                    break;
                     
                case DBType.MongoDB:
                    if (string.IsNullOrEmpty(info.Url))
                        info.Url = "localhost";
                    connstr = "mongodb://" + (string.IsNullOrEmpty(info.User) ? "" : info.User + ":" + info.Password + "@") + info.Url + (info.Port > 0 ? ":" + info.Port.ToString() : "");
                    break;
            }
            return connstr;
        }

    }
}
