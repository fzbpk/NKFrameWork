using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Web.UI;
using NK.ENum;
using System.Web;
using System.Web.UI.WebControls;
namespace NK.UI
{
    /// <summary>
    /// 母版页
    /// </summary>
    public  class UIMasterPage:MasterPage
    {

        #region 提示框

        /// <summary>
        /// 警告框
        /// </summary>
        /// <param name="msg">提示语</param>
        /// <param name="lang">语言</param>
        public void AlertMsgbox(string msg, Language lang = Language.Chinese)
        {
            string js = "<script language=javascript>alert('{0}');</script>";
            switch (lang)
            {
                case NK.ENum.Language.Chinese:
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "警告", string.Format(js, msg));
                    break;
                default:
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "Alert", string.Format(js, msg));
                    break;
            }

        }

        /// <summary>
        /// 警告框
        /// </summary>
        /// <param name="msg">提示语</param>
        /// <param name="back">跳转</param>
        /// <param name="lang">语言</param>
        public void AlertMsgbox(string msg, int back, Language lang = Language.Chinese)
        {
            string js = "<script language=javascript>alert('{0}');history.back({1})</script>";
            switch (lang)
            {
                case NK.ENum.Language.Chinese:
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "警告", string.Format(js, msg, back.ToString()));

                    break;
                default:
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "Alert", string.Format(js, msg, back.ToString()));
                    break;
            }
        }

        /// <summary>
        /// 提示框
        /// </summary>
        /// <param name="msg">提示语</param>
        /// <param name="lang">语言</param>
        public void MessageBox(string msg, Language lang = Language.Chinese)
        {
            string js = "<script language=javascript>alert('{0}');</script>";
            switch (lang)
            {
                case NK.ENum.Language.Chinese:
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "提示", string.Format(js, msg));
                    break;
                default:
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "message", string.Format(js, msg));
                    break;
            }

        }

        /// <summary>
        /// 提示框
        /// </summary>
        /// <param name="msg">提示语</param>
        /// <param name="href">跳转</param>
        /// <param name="lang">语言</param>
        public void MessageBox(string msg, string href, Language lang = Language.Chinese)
        {
            string js = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
            switch (lang)
            {
                case NK.ENum.Language.Chinese:
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "提示", string.Format(js, msg, href));
                    break;
                default:
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "message", string.Format(js, msg, href));
                    break;
            }

        }

        /// <summary>
        /// JS函数
        /// </summary>
        /// <param name="func">函数</param>
        public void JSFunc(string func)
        {
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "Func", "<script>" + func + ";</script>");

        }

        /// <summary>
        /// 强制跳转
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="href">地址</param>
        public void Redirect(string msg, string href)
        {
            string js = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
            Response.Write(string.Format(js, msg, href));
            Response.End();
        }

        /// <summary>
        /// 刷新页面 
        /// </summary>
        public void Refresh()
        {
            string js = "<script language=javascript>window.location.reload();</script>";
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "", js);
        }

        #endregion

        #region 文件

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="CharSet">页面编码</param>
        /// <param name="Delete">是否下载后删除</param>
        /// <param name="lang">语言</param>
        public void FileDown(FileInfo file, string CharSet = "utf-8", bool Delete = false, Language lang = Language.Chinese)
        {
            if (file == null)
            {
                switch (lang)
                {
                    case NK.ENum.Language.Chinese:
                        AlertMsgbox("文件不存在", -1);
                        break;
                    default:
                        AlertMsgbox("File Is Not Exist", -1);
                        break;
                }
            }
            else if (!file.Exists)
            {
                switch (lang)
                {
                    case NK.ENum.Language.Chinese:
                        AlertMsgbox("文件不存在", -1);
                        break;
                    default:
                        AlertMsgbox("File Is Not Exist", -1);
                        break;
                }
            }
            else if (file.Exists)
            {
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding(CharSet);
                Response.WriteFile(file.FullName);
                Response.Flush();
                if (Delete)
                    file.Delete();
                Response.End();
            }
        }

        /// <summary>
        /// 相对路径转绝对路径
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public DirectoryInfo URLToPath(string Path)
        {
            return new DirectoryInfo(Server.MapPath(Path));
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="Class">上传控件</param>
        /// <param name="Path">相对路径</param>
        /// <param name="filename">保护文件</param>
        /// <returns>执行结果</returns>
        public bool UpFile(FileUpload Class, string Path, string filename)
        {

            if (Class.HasFile)
            {
                if (!System.IO.Directory.Exists(Server.MapPath(Path)))
                    System.IO.Directory.CreateDirectory(Server.MapPath(Path));
                if (System.IO.File.Exists(Server.MapPath(Path) + filename))
                    System.IO.File.Delete(Server.MapPath(Path) + filename);
                Class.SaveAs(filename);
                return true;
            }
            return false;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 返回Page属性
        /// </summary>
        public Page CurunPage
        {
            get { return this.Page; }
            set { this.Page = value; }
        }

        /// <summary>
        /// 默认语言
        /// </summary>
        /// <returns></returns>
        public List<Language> CurunLanguage
        {
            get
            {
                List<Language> res = new List<ENum.Language>();
                if (Request.UserLanguages != null)
                {
                    List<string> langs = Request.UserLanguages.ToList();
                    foreach (var tmp in langs)
                    {
                        if (tmp.ToLower().Contains("zh"))
                            res.Add(ENum.Language.Chinese);
                    }
                }
                return res;
            }
        }

        /// <summary>
        /// 获取HOST
        /// </summary>
        public string Host
        {
            get { return HttpContext.Current.Request.Url.Host; }
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public string CurunPath
        {
            get
            {
                string pp = Request.Url.LocalPath;
                if (pp.StartsWith("/"))
                    pp = pp.Substring(1);
                return pp;
            }
        }

        /// <summary>
        /// 当前类
        /// </summary>
        public string CurunClass
        {
            get
            {
                string PageModule = Request.Url.LocalPath;
                if (PageModule.StartsWith("/"))
                    PageModule = PageModule.Substring(1);
                if (PageModule.Contains("."))
                    PageModule = PageModule.Substring(0, PageModule.IndexOf(".")).Replace("/", "_");
                return PageModule;
            }
        }

        /// <summary>
        /// URL参数
        /// </summary>
        public string CurunRef
        {
            get
            {
                if (Request.QueryString != null)
                    return Request.QueryString.ToString();
                return "";
            }
        }

        /// <summary>
        /// 客户端连接
        /// </summary>
        public IPEndPoint ClientEndPoint
        {
            get
            {
                IPEndPoint Remote = new IPEndPoint(IPAddress.Any, 0);
                int port = HttpContext.Current.Request.Url.Port;
                string Ip = string.Empty;
                if (Request.ServerVariables["HTTP_VIA"] != null)
                {
                    if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
                    {
                        if (Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                            Ip = Request.ServerVariables["HTTP_CLIENT_IP"].ToString();
                        else
                            if (Request.ServerVariables["REMOTE_ADDR"] != null)
                            Ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
                        else
                            Ip = "127.0.0.1";
                    }
                    else
                        Ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else if (Request.ServerVariables["REMOTE_ADDR"] != null)
                    Ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
                else
                    Ip = "127.0.0.1";
                IPAddress IP = IPAddress.Any;
                IPAddress.TryParse(Ip, out IP);
                Remote.Address = IP;
                Remote.Port = port;

                return Remote;
            }
        }

        #endregion

    }
}
