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
    public partial class USBSetUI : UserControl
    {
        public USBSetUI()
        {
            InitializeComponent();
        }

        private void USBSetUI_Load(object sender, EventArgs e)
        {
            this.Address_Val.Value = 0;
            this.radioButton1.Checked = true;
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
        public USBSet Info
        {
            get
            {
                if (string.IsNullOrEmpty(this.ConfigName_Val.Text))
                {
                    MessageBox.Show("请输入" + this.ConfigName_Key.Text);
                    return null;
                }
                USBSet info = new USBSet();
                info.ConfigName = this.ConfigName_Val.Text;
                info.devPath = this.devPath_Val.Text;
                info.VID = this.VID_Val.Text;
                info.PID = this.PID_Val.Text;
                info.Address = (int)this.Address_Val.Value;
                info.Enable = this.CEnabled.Checked;
                if (this.radioButton1.Checked)
                    info.Mode = NK.ENum.Net_Mode.None;
                else if (this.radioButton2.Checked)
                    info.Mode = NK.ENum.Net_Mode.Local;
                else if (this.radioButton3.Checked)
                    info.Mode = NK.ENum.Net_Mode.Remote;
                return info;
            }
            set
            {
                if (value != null)
                {
                    this.ConfigName_Val.Text = value.ConfigName;
                    this.devPath_Val.Text = value.devPath;
                    this.VID_Val.Text = value.VID;
                    this.PID_Val.Text = value.PID;
                    this.Address_Val.Value = value.Address; 
                    this.CEnabled.Checked = value.Enable;
                    switch (value.Mode)
                    {
                        case NK.ENum.Net_Mode.None:
                            this.radioButton1.Checked = true;
                            break;
                        case NK.ENum.Net_Mode.Local:
                            this.radioButton2.Checked = true;
                            break;
                        case NK.ENum.Net_Mode.Remote:
                            this.radioButton3.Checked = true;
                            break;
                    }
                }
            }
        }
    }
}
