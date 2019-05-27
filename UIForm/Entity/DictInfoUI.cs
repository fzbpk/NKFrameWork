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
    public partial class DictInfoUI : UserControl
    {
        public DictInfoUI()
        {
            InitializeComponent();
        }

        private void DictInfoUI_Load(object sender, EventArgs e)
        {

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
        public DictInfo Info
        {
            get
            {
                if (string.IsNullOrEmpty(this.DictName_Val.Text))
                {
                    MessageBox.Show("请输入" + this.DictName_Key.Text);
                    return null;
                }
                if (string.IsNullOrEmpty(this.DictKey_Val.Text))
                {
                    MessageBox.Show("请输入" + this.DictKey_Key.Text);
                    return null;
                }
                DictInfo info = new DictInfo();
                info.Name = this.DictName_Val.Text;
                info.Key = this.DictKey_Val.Text;
                info.Display = this.DictDisp_Val.Text;
                info.Value = this.DictValue_Val.Text;
                info.Enable = this.CEnabled.Checked;
                return info;
            }
            set
            {
                if (value != null)
                {
                    this.DictName_Val.Text = value.Name;
                    this.DictKey_Val.Text = value.Key;
                    this.DictDisp_Val.Text = value.Display;
                    this.DictValue_Val.Text = value.Value; 
                    this.CEnabled.Checked = value.Enable; 
                }
            }
        }
    }
}
