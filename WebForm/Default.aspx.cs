using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using NK;
using NK.Entity;
using UIWeb.Entity;
public partial class _Default : Page
{
    private EntityHtml html = new EntityHtml();
    protected void Page_Load(object sender, EventArgs e)
    {
        DBInfo info = new DBInfo();
        info.Mode = NK.ENum.DBType.MSSQL; 
        Dictionary<PropertyInfo, object> col = info.ToDictionary();
        html.LineCSS = "form-group";
        html.HeadCSS = "col-sm-2 control-label";
        html.ValueCSS = "col-sm-10";
        html.TextBoxCSS = "form-control";
        html.CheckBoxCSS = "checkbox";
        html.ComBoxCSS = "form-control";
        html.TimeCSS = "form-control pull-right";
        if (this.IsPostBack)
        {
           
        }
        else
        {
            
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DBInfo info = new DBInfo();
        string Table = "";
        List<PropertyInfo> col = info.ToPropertys();
        Dictionary<PropertyInfo, object> val = html.HtmlToEntity(Table, Request.Form, col);
        if (val.IsNotNull())
            info.FromDictionary(val);
        
    }
}