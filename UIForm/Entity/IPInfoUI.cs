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
    public partial class IPInfoUI : UserControl
    {
        public IPInfoUI()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.IPAddress_Val.Enabled = false;
                this.GateWay_Val.Enabled = false;
                this.SubnetMask_Val.Enabled = false;
                this.DNS_Val.Enabled = false;
            }
            else
            {
                this.IPAddress_Val.Enabled = true;
                this.GateWay_Val.Enabled = true;
                this.SubnetMask_Val.Enabled = true;
                this.DNS_Val.Enabled = true;
            }
        }

        private void IPInfoUI_Load(object sender, EventArgs e)
        {
            this.radioButton1.Checked = true;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.radioButton2.Checked = false ;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.radioButton1.Checked = false;
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
        public IPInfo Info
        {
            get
            {
                if (string.IsNullOrEmpty(this.ConfigName_Val.Text))
                {
                    MessageBox.Show("请输入" + this.ConfigName_Key.Text);
                    return null;
                }
                IPInfo info = new IPInfo();
                info.ConfigName = this.ConfigName_Val.Text;
                info.IPAddress  = this.IPAddress_Val.Text;
                info.SubnetMask = this.SubnetMask_Val.Text;
                info.GateWay = this.GateWay_Val.Text;
                info.DNS = this.DNS_Val.Text;
                info.DHCP = this.checkBox1.Checked;
                info.Enable = this.CEnabled.Checked;
                if (this.radioButton1.Checked)
                    info.Address_Family = System.Net.Sockets.AddressFamily.InterNetwork;
                else if (this.radioButton2.Checked)
                    info.Address_Family = System.Net.Sockets.AddressFamily.InterNetworkV6; 
                return info;
            }
            set
            {
                if (value != null)
                {
                    this.ConfigName_Val.Text = value.ConfigName;
                    this.IPAddress_Val.Text = value.IPAddress;
                    this.SubnetMask_Val.Text = value.SubnetMask;
                    this.GateWay_Val.Text = value.GateWay;
                    this.DNS_Val.Text = value.DNS;
                    this.checkBox1.Checked = value.DHCP; 
                    this.CEnabled.Checked = value.Enable;
                    switch (value.Address_Family)
                    {
                        case System.Net.Sockets.AddressFamily.InterNetwork:
                            this.radioButton1.Checked = true;
                            break;
                        case System.Net.Sockets.AddressFamily.InterNetworkV6:
                            this.radioButton2.Checked = true;
                            break; 
                    }
                }
            }
        }

    }
}
