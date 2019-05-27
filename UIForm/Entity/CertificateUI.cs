using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NK.Entity;
namespace UIForm.Entity
{
    public partial class CertificateUI : UserControl
    {
        public CertificateUI()
        {
            InitializeComponent();
        }

        private void CertificateUI_Load(object sender, EventArgs e)
        {
            this.GUIDS.Text = Guid.NewGuid().ToString();
            this.StartDateTime_Val.Value = DateTime.Now;
            this.EndDateTime_Val.Value = DateTime.Now.AddMonths(1);
            this.CEnabled.Checked = true;
        }

        /// <summary>
        /// 子控件
        /// </summary>
        public ControlCollection Containers
        {
            get { return this.Controls; }
        }

        /// <summary>
        /// 设置或获取信息
        /// </summary>
        public Certificate Info
        {
            get
            {
                if (string.IsNullOrEmpty(this.ConfigName_Val.Text))
                {
                    MessageBox.Show("请输入"+ this.ConfigName_Key.Text);
                    return null;
                }
                if (string.IsNullOrEmpty(this.SYSName_Val.Text))
                {
                    MessageBox.Show("请输入" + this.SYSName_Key.Text);
                    return null;
                }
                if (string.IsNullOrEmpty(this.ConfigName_Val.Text))
                {
                    MessageBox.Show("请输入" + this.ConfigName_Key.Text);
                    return null;
                }
                if (string.IsNullOrEmpty(this.User_Val.Text))
                {
                    MessageBox.Show("请输入" + this.User_Key.Text);
                    return null;
                }
                if (string.IsNullOrEmpty(this.Password_Val.Text))
                {
                    MessageBox.Show("请输入" + this.Password_Key.Text);
                    return null;
                }
                if (string.IsNullOrEmpty(this.APPKey_Val.Text))
                {
                    MessageBox.Show("请输入" + this.APPKey_Key.Text);
                    return null;
                }
                if (string.IsNullOrEmpty(this.Key_Val.Text))
                {
                    MessageBox.Show("请输入" + this.Key_Key.Text);
                    return null;
                }
                Certificate info = new Certificate();
                info.guid = new Guid(this.GUIDS.Text);
                info.ConfigName = this.ConfigName_Val.Text;
                info.SYSName = this.SYSName_Val.Text;
                info.Company = this.Company_Val.Text;
                info.User = this.User_Val.Text;
                info.Password = this.Password_Val.Text;
                info.Key = this.Key_Val.Text;
                info.APPKey = this.APPKey_Val.Text;
                info.StartDateTime = this.StartDateTime_Val.Value;
                info.EndDateTime = this.EndDateTime_Val.Value;
                info.Enable = this.CEnabled.Checked;
                return info;
            }
            set
            {
                if (value != null)
                {
                    this.GUIDS.Text = value.guid.ToString();
                    this.ConfigName_Val.Text = value.ConfigName;
                    this.SYSName_Val.Text = value.SYSName;
                    this.Company_Val.Text = value.Company;
                    this.User_Val.Text = value.User;
                    this.Key_Val.Text = value.Key;
                    this.APPKey_Val.Text = value.APPKey;
                    this.StartDateTime_Val.Value = value.StartDateTime;
                    this.EndDateTime_Val.Value = value.EndDateTime;
                    this.CEnabled.Checked=value.Enable;
                }
            }
        }

    }
}
