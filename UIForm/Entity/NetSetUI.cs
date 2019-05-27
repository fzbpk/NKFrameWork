using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using NK.Entity;
using NK.ENum;
namespace UIForm.Entity
{
    public partial class NetSetUI : UserControl
    {
        public NetSetUI()
        {
            InitializeComponent();
        }

        private void NetSetUI_Load(object sender, EventArgs e)
        {
            this.radioButton1.Checked = true;
            AddressFamily AddressFamilyenum= AddressFamily.InterNetwork;
            Dictionary<string, int> Address_Family = UIHelper.EnumToList(AddressFamilyenum);
            SocketType SocketTypeenum = SocketType.Stream;
            Dictionary<string, int> Socket_Type = UIHelper.EnumToList(SocketTypeenum);
            ProtocolType ProtocolTypeenum = ProtocolType.Tcp;
            Dictionary<string, int> Protocol_Type = UIHelper.EnumToList(ProtocolTypeenum);
            foreach (var di in Address_Family)
                this.Address_Family_Val.Items.Add(di.Key);
            this.Address_Family_Val.SelectedIndex = this.Address_Family_Val.Items.IndexOf("InterNetwork");
            foreach (var di in Socket_Type)
                this.Socket_Type_Val.Items.Add(di.Key);
            this.Socket_Type_Val.SelectedIndex = this.Socket_Type_Val.Items.IndexOf("Stream");
            foreach (var di in Protocol_Type)
                this.Protocol_Type_Val.Items.Add(di.Key);
            this.Protocol_Type_Val.SelectedIndex = this.Protocol_Type_Val.Items.IndexOf("Tcp");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.radioButton1.Checked = true;
                this.radioButton2.Checked = false;
                this.radioButton3.Checked = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                this.radioButton1.Checked = false;
                this.radioButton3.Checked = false;
                this.radioButton2.Checked = true;
            }

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton3.Checked)
            {
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = false;
                this.radioButton3.Checked = true;
            }

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
        public NetSet Info
        {
            get
            {
                if (string.IsNullOrEmpty(this.ConfigName_Val.Text))
                {
                    MessageBox.Show("请输入" + this.ConfigName_Key.Text);
                    return null;
                }
                NetSet info = new NetSet();
                info.ConfigName = this.ConfigName_Val.Text;
                info.DomainName = this.DomainName_Val.Text;
                info.IPAddress = this.IPAddress_Val.Text;
                info.AddrRef = this.AddrRef_Val.Text;
                info.Address = this.Address_Val.Text;
                info.Port = (int)this.Port_Val.Value; 
                Dictionary<string, int> Address_Family = UIHelper.EnumToList(info.Address_Family);
                var af = Address_Family.FirstOrDefault(c => c.Key == this.Address_Family_Val.Text);
                info.Address_Family = (AddressFamily)af.Value;
                Dictionary<string, int> Socket_Type = UIHelper.EnumToList(info.Socket_Type);
                var st = Socket_Type.FirstOrDefault(c => c.Key == this.Socket_Type_Val.Text);
                info.Socket_Type = (SocketType)st.Value;
                Dictionary<string, int> Protocol_Type = UIHelper.EnumToList(info.Protocol_Type);
                var pt = Protocol_Type.FirstOrDefault(c => c.Key == this.Protocol_Type_Val.Text);
                info.Protocol_Type = (ProtocolType)pt.Value;
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
                    this.Port_Val.Value = value.Port;
                    this.DomainName_Val.Text = value.DomainName;
                    this.IPAddress_Val.Text = value.IPAddress;
                    this.AddrRef_Val.Text = value.AddrRef ;
                    this.Address_Val.Text = value.Address;
                    this.CEnabled.Checked = value.Enable;
                    this.Address_Family_Val.SelectedIndex = this.Address_Family_Val.Items.IndexOf(Info.Address_Family);
                    this.Socket_Type_Val.SelectedIndex = this.Socket_Type_Val.Items.IndexOf(Info.Socket_Type);
                    this.Protocol_Type_Val.SelectedIndex = this.Protocol_Type_Val.Items.IndexOf(Info.Protocol_Type);
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

        private void CEnabled_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
