﻿using System.Web.UI;
using System.ComponentModel;
using System.Collections.Generic;
using NK.Attribut;
using System.Collections.Specialized;
using System.Reflection;
namespace UIWeb.Entity
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [ToolboxData("<{0}:DisplayColumnAttributeUI runat='server'   ></{0}:DisplayColumnAttributeUI>")]
    public class DisplayColumnAttributeUI : Control, IPostBackDataHandler
    {

        private string Errmsg = "";
        public DisplayColumnAttribute Info { get; set; }

        #region 外观设置

        /// <summary>
        /// 行CSS
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("行CSS")]
        public virtual string LineCSS { get; set; }
        /// <summary>
        /// 标题CSS
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("标题CSS")]
        public virtual string HeadCSS { get; set; }
        /// <summary>
        /// 值单元格CSS
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("值单元格CSS")]
        public virtual string ValueCSS { get; set; }
        /// <summary>
        /// 文本框CSS
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("文本框CSS")]
        public virtual string TextBoxCSS { get; set; }
        /// <summary>
        /// 下拉列表CSS
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("下拉列表CSS")]
        public virtual string ComBoxCSS { get; set; }
        /// <summary>
        /// 时间控件CSS
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("时间控件CSS")]
        public virtual string TimeCSS { get; set; }
        /// <summary>
        /// CheckBoxCSS
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("时间控件CSS")]
        public virtual string CheckBoxCSS { get; set; }
        /// <summary>
        /// 复选框图标CSS
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("时间控件CSS")]
        public virtual string CheckBoxIconCSS { get; set; }

        #endregion

        #region 属性
        /// <summary>
        /// 获取错误信息
        /// </summary>
        public string Error
        {
            get
            {
                string err = ViewState[this.UniqueID + "_Errmsg"] != null ? (string)ViewState[this.UniqueID + "_Errmsg"] : "";
                ViewState[this.UniqueID + "_Errmsg"] = "";
                return err;
            }
        }
        /// <summary>
        /// 获取数据库配置信息
        /// </summary>
        public DisplayColumnAttribute Entity
        {
            get
            {
                string json = ViewState[this.UniqueID + "_json"] != null ? (string)ViewState[this.UniqueID + "_json"] : "";
                if (string.IsNullOrEmpty(json))
                    return null;
                else
                    return json.Deserialize<DisplayColumnAttribute>();
            }
            set
            { Info = value; }
        }

        #endregion
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Info == null)
                this.Info = new DisplayColumnAttribute();
            Dictionary<PropertyInfo, object> pls = this.Info.ToDictionary();
            string HTML = UIHelper.EntityToForm<DisplayColumnAttribute>(pls, this.UniqueID, LineCSS, HeadCSS, ValueCSS, TextBoxCSS, ComBoxCSS, TimeCSS, CheckBoxCSS);
            writer.Write(HTML);
        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            bool res = true;
            Info = UIHelper.FormToEntity<DisplayColumnAttribute>(postDataKey, postCollection, out Errmsg);
            ViewState[this.UniqueID + "_json"] = (Info == null) ? "" : Info.Serialize();
            ViewState[this.UniqueID + "_Errmsg"] = Errmsg;
            return res;
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {

        }

    }
}