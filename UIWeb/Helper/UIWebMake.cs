using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqToDB.Mapping;
using NK;
using NK.Entity;
using NK.ENum;
using NK.Attribut;
using NK.Message;
using System.Reflection;
using System.ComponentModel;
using UIWeb.Entity;
namespace NK.UI
{
    public class UIWebMake
    {
        public UIWebMake()
        {

        }

        #region 属性


        /// <summary>
        /// 语言设置
        /// </summary>
        public Language language { get; set; }
        /// <summary>
        /// 显示列
        /// </summary>
        public Dictionary<DisplayColumnAttribute, object> Column { get; set; }
        /// <summary>
        /// 当前PAGE
        /// </summary>
        public Page page { get; set; }
        /// <summary>
        /// 列表页
        /// </summary>
        public string Indexurl { get; set; }
        /// <summary>
        /// 管理页
        /// </summary>
        public string Mgrurl { get; set; }
        /// <summary>
        /// 删除页
        /// </summary>
        public string Delurl { get; set; }
        /// <summary>
        /// 详细页
        /// </summary>
        public string Viewurl { get; set; }
        /// <summary>
        /// 删除参数
        /// </summary>
        public string DelClass { get; set; }
        /// <summary>
        /// 详细参数
        /// </summary>
        public string ViewClass { get; set; }
        /// <summary>
        /// 管理参数
        /// </summary>
        public string MgrClass { get; set; }

        #endregion

        #region 事件


        /// <summary>
        /// 搜索事件
        /// </summary>
        public EventHandler SearchEven { get; set; }
        /// <summary>
        /// 保存事件
        /// </summary>
        public EventHandler SaveEven { get; set; }


        #endregion

        #region 方法


        /// <summary>
        /// 显示列名
        /// </summary>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public string UIColumnDisplay(string ClassName)
        {
            string res = "";
            if (Column == null) Column = new Dictionary<DisplayColumnAttribute, object>();
            if (Column.Found(c => c.Key.Name.ToLower().Trim() == ClassName.ToLower().Trim()))
            {
                var dic = Column.FirstOrDefault(c => c.Key.Column.ToLower().Trim() == ClassName.ToLower().Trim());
                return dic.Key.Name;
            }
            return res;
        }

        /// <summary>
        /// 显示列名
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public string UIColumnEntity(Type type, string ClassName)
        {
            string res = "";
            if (type == null)
                return res;
            string PName = ClassName;
            var prolist = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            var pro = prolist.FirstOrDefault(c => c.Name.ToUpper().Trim() == ClassName.ToUpper().Trim());
            if (pro != null)
                PName = pro.ToColumnName();
            if (Column == null) Column = new Dictionary<DisplayColumnAttribute, object>();
            if (Column.Found(c => c.Key.Column.ToLower().Trim() == ClassName.ToLower().Trim()))
            {
                var dic = Column.FirstOrDefault(c => c.Key.Column.ToLower().Trim() == ClassName.ToLower().Trim());
                return dic.Key.Name;
            }
            else if (Column.Found(c => c.Key.Column.ToLower().Trim() == PName.ToLower().Trim()))
            {
                var dic = Column.FirstOrDefault(c => c.Key.Column.ToLower().Trim() == PName.ToLower().Trim());
                return dic.Key.Name;
            }
            return res;
        }

        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public string UICaptionDisplay(string ClassName)
        {
            string res = "";
            if (Column == null) Column = new Dictionary<DisplayColumnAttribute, object>();
            if (Column.Found(c => c.Key.Name.ToLower().Trim() == ClassName.ToLower().Trim()))
            {
                var dic = Column.FirstOrDefault(c => c.Key.Column.ToLower().Trim() == ClassName.ToLower().Trim());
                return dic.Key.Caption;
            }
            return res;
        }

        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public string UICaptionEntity(Type type, string ClassName)
        {
            string res = "";
            if (type == null)
                return res;
            string PName = ClassName;
            var prolist = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            var pro = prolist.FirstOrDefault(c => c.Name.ToUpper() == ClassName.ToUpper());
            if (pro != null)
                PName = pro.ToColumnName();
            if (Column == null) Column = new Dictionary<DisplayColumnAttribute, object>();
            if (Column.Found(c => c.Key.Column.ToLower().Trim() == ClassName.ToLower().Trim()))
            {
                var dic = Column.FirstOrDefault(c => c.Key.Column.ToLower().Trim() == ClassName.ToLower().Trim());
                return dic.Key.Caption;
            }
            else if (Column.Found(c => c.Key.Column.ToLower().Trim() == PName.ToLower().Trim()))
            {
                var dic = Column.FirstOrDefault(c => c.Key.Column.ToLower().Trim() == PName.ToLower().Trim());
                return dic.Key.Caption;
            }
            return res;
        }

        /// <summary>
        /// 普遍搜索页搜索条件
        /// </summary>
        /// <param name="Add"></param>
        /// <param name="Delete"></param>
        /// <param name="Search"></param>
        /// <param name="SearchLab"></param>
        /// <param name="AddVisible"></param>
        /// <param name="DeleteVisible"></param>
        /// <param name="SearchVisible"></param>
        public void NormalIndexHead(Button Add, Button Delete, Button Search, Label SearchLab, bool AddVisible = true, bool DeleteVisible = true, bool SearchVisible = true)
        {
            if (Add != null)
            {
                Add.Text = ContorlsMessage.ADD(language);
                Add.Visible = AddVisible;
                Add.Click += Add_Click;
            }
            if (Delete != null)
            {
                Delete.Text = ContorlsMessage.Delete(language);
                Delete.Visible = DeleteVisible;
                Delete.Click += Delete_Click;
            }
            if (Search != null)
            {
                Search.Text = ContorlsMessage.Search(language);
                Search.Visible = SearchVisible;
                Search.Click += SearchEven;
            }
            if (SearchLab != null)
                SearchLab.Text = ContorlsMessage.KeyWord(language);
        }

        /// <summary>
        /// 管理业按钮控制
        /// </summary>
        /// <param name="Save"></param>
        /// <param name="Back"></param>
        /// <param name="SaveVisible"></param>
        /// <param name="BackVisible"></param>
        public void MakeDetail(Button Save, Button Back, bool SaveVisible = true, bool BackVisible = true)
        {
            if (Save != null)
            {
                Save.Text = ContorlsMessage.Save(language);
                Save.Visible = SaveVisible;
                if (SaveEven != null)
                    Save.Click += SaveEven;
            }
            if (Back != null)
            {
                Back.Text = ContorlsMessage.Back(language);
                Back.Visible = BackVisible;
                Back.Click += Back_Click;
            }
        }

        /// <summary>
        /// 标题
        /// </summary>
        /// <param name="SecTitle"></param>
        /// <param name="methord"></param>
        /// <param name="SecT"></param>
        /// <param name="Methord"></param>
        public void MakeTitle(Dictionary<Language, string> SecTitle, Dictionary<Language, string> methord, Label SecT, Label Methord)
        {
            if (SecTitle == null) SecTitle = new Dictionary<Language, string>();
            if (methord == null) methord = new Dictionary<Language, string>();
            string SecTitlestr = "", methordstr = "";
            if (SecTitle.Found(c => c.Key == this.language))
            {
                var tmp = SecTitle.FirstOrDefault(c => c.Key == this.language);
                SecTitlestr = tmp.Value;
            }
            if (methord.Found(c => c.Key == this.language))
            {
                var tmp = methord.FirstOrDefault(c => c.Key == this.language);
                methordstr = tmp.Value;
            }
            if (SecT != null)
                SecT.Text = SecTitlestr;
            if (Methord != null)
                Methord.Text = methordstr;
        }

        /// <summary>
        /// 表头操作
        /// </summary>
        /// <returns></returns>
        public string MakeOpertion()
        {
            return ContorlsMessage.Operation(language);
        }

        /// <summary>
        /// 记录操作
        /// </summary>
        /// <param name="Ref"></param>
        /// <param name="Edit"></param>
        /// <param name="Delete"></param>
        /// <param name="Read"></param>
        /// <param name="CSS"></param>
        /// <returns></returns>
        public string IndexItemLink(string Ref, bool Edit = true, bool Delete = true, bool Read = true, string CSS = "")
        {
            string Html = "<div " + (string.IsNullOrEmpty(CSS) ? "" : "class=\"" + CSS + "\"") + " >\r\n";
            if (Edit && !string.IsNullOrEmpty(Mgrurl))
                Html += " <a class=\"gree\" href=\"" + Mgrurl + "?" + MgrClass + "=" + Ref + "\" title =\"" + ContorlsMessage.Edit(language) + "\">" + ContorlsMessage.Edit(language) + "</a>\r\n";
            if (Delete && !string.IsNullOrEmpty(Delurl))
                Html += " <a class=\"gree\" href=\"" + Delurl + "?" + DelClass + "=" + Ref + "\" title =\"" + ContorlsMessage.Delete(language) + "\">" + ContorlsMessage.Delete(language) + "</a>\r\n";
            if (Read && !string.IsNullOrEmpty(Viewurl))
                Html += " <a class=\"gree\" href=\"" + Viewurl + "?" + ViewClass + "=" + Ref + "\" title =\"" + ContorlsMessage.View(language) + "\">" + ContorlsMessage.View(language) + "</a>\r\n";
            Html += "</div>\r\n";
            return Html;
        }


        #endregion

        #region 事件处理

        private void Back_Click(object sender, EventArgs e)
        {
            page.Response.Redirect(Indexurl);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            string selid = page.Request.Form[DelClass];
            selid = selid == null ? "" : selid.Trim();
            string urlprm = "?" + DelClass + "=";
            string ids = "";
            string[] aa = selid.Split(',');
            foreach (string tmp in aa)
            {
                if (!string.IsNullOrEmpty(tmp))
                {
                    if (string.IsNullOrEmpty(ids))
                        ids = tmp;
                    else
                        ids += "," + tmp;
                }
            }
            page.Response.Redirect(Delurl + urlprm + ids);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            page.Response.Redirect(Mgrurl);
        }


        #endregion



    }


}
