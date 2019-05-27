using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NK.ENum;
using NK.Entity;
namespace UIForm.Entity
{
    public partial class PortsSetUI : UserControl
    {
        public PortsSetUI()
        {
            InitializeComponent();
        }

        private void PortsSetUI_Load(object sender, EventArgs e)
        {
            this.radioButton1.Checked = true;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.radioButton1.Checked = true ;
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
        public PortsSet Info
        {
            get
            {
                if (string.IsNullOrEmpty(this.ConfigName_Val.Text))
                {
                    MessageBox.Show("请输入" + this.ConfigName_Key.Text);
                    return null;
                }
                PortsSet info = new PortsSet();
                info.ConfigName = this.ConfigName_Val.Text;
                info.Port = (int)this.Port_Val.Value;
                info.Rate = (int)this.Rate_Val.Value;
                info.DataBit = (int)this.DataBit_Val.Value;
                switch (this.StopBit_Val.Text)
                {
                    case "None":
                        info.StopBit = System.IO.Ports.StopBits.None;
                        break;
                    case "One":
                        info.StopBit = System.IO.Ports.StopBits.One;
                        break;
                    case "Two":
                        info.StopBit = System.IO.Ports.StopBits.Two;
                        break;
                    case "OnePointFive":
                        info.StopBit = System.IO.Ports.StopBits.OnePointFive;
                        break;
                }
                switch (this.Parity_Val.Text)
                {
                    case "None":
                        info.Parity = System.IO.Ports.Parity.None;
                        break;
                    case "Even":
                        info.Parity = System.IO.Ports.Parity.Even;
                        break;
                    case "Mark":
                        info.Parity = System.IO.Ports.Parity.Mark;
                        break;
                    case "Odd":
                        info.Parity = System.IO.Ports.Parity.Odd;
                        break;
                    case "Space":
                        info.Parity = System.IO.Ports.Parity.Space;
                        break;
                }
                switch (this.Ctrl_Val.Text)
                {
                    case "None":
                        info.Ctrl = System.IO.Ports.Handshake.None;
                        break;
                    case "RequestToSend":
                        info.Ctrl = System.IO.Ports.Handshake.RequestToSend;
                        break;
                    case "RequestToSendXOnXOff":
                        info.Ctrl = System.IO.Ports.Handshake.RequestToSendXOnXOff;
                        break;
                    case "XOnXOff":
                        info.Ctrl = System.IO.Ports.Handshake.XOnXOff;
                        break; 
                }
                switch (this.PortType_Val.Text)
                {
                    case "None":
                        info.PortType = Port_Mode.None;
                        break;
                    case "RS232":
                        info.PortType = Port_Mode.RS232;
                        break;
                    case "RS422":
                        info.PortType = Port_Mode.RS422;
                        break;
                    case "RS485":
                        info.PortType = Port_Mode.RS485;
                        break;
                    case "LPT":
                        info.PortType = Port_Mode.LPT;
                        break;
                }
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
                    this.Port_Val.Value = value.Port;
                    this.Rate_Val.Value = value.Rate;
                    this.DataBit_Val.Value = value.DataBit;
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
                    string StopBit = "";
                    switch (Info.StopBit)
                    {
                        case System.IO.Ports.StopBits.None:
                            StopBit= "None";
                            break;
                        case System.IO.Ports.StopBits.One:
                            StopBit = "One";
                            break;
                        case System.IO.Ports.StopBits.OnePointFive:
                            StopBit = "OnePointFive";
                            break;
                        case System.IO.Ports.StopBits.Two:
                            StopBit = "Two";
                            break;
                    }
                    this.StopBit_Val.SelectedIndex = this.StopBit_Val.Items.IndexOf(StopBit);
                    string PortType = "";
                    switch (Info.PortType)
                    {
                        case Port_Mode.None:
                            PortType = "None";
                            break;
                        case Port_Mode.RS232:
                            PortType = "RS232";
                            break;
                        case Port_Mode.RS422:
                            PortType = "RS422";
                            break;
                        case Port_Mode.RS485:
                            PortType = "RS485";
                            break;
                        case Port_Mode.LPT:
                            PortType = "LPT";
                            break;
                    }
                    this.PortType_Val.SelectedIndex = this.PortType_Val.Items.IndexOf(PortType);
                    string Parity = "";
                    switch (Info.Parity)
                    {
                        case System.IO.Ports.Parity.None:
                            Parity = "None";
                            break;
                        case System.IO.Ports.Parity.Even:
                            Parity = "Even";
                            break;
                        case System.IO.Ports.Parity.Mark:
                            Parity = "Mark";
                            break;
                        case System.IO.Ports.Parity.Odd:
                            Parity = "Odd";
                            break;
                        case System.IO.Ports.Parity.Space:
                            Parity = "Space";
                            break;
                    }
                    this.Parity_Val.SelectedIndex = this.Parity_Val.Items.IndexOf(Parity);
                    string Ctrl = "";
                    switch (Info.Ctrl)
                    {
                        case System.IO.Ports.Handshake.None:
                            Ctrl = "None";
                            break;
                        case System.IO.Ports.Handshake.RequestToSend:
                            Ctrl = "RequestToSend";
                            break;
                        case System.IO.Ports.Handshake.RequestToSendXOnXOff:
                            Ctrl = "RequestToSendXOnXOff";
                            break;
                        case System.IO.Ports.Handshake.XOnXOff:
                            Ctrl = "XOnXOff";
                            break;
                    }
                    this.Ctrl_Val.SelectedIndex = this.Ctrl_Val.Items.IndexOf(Ctrl);
                }
            }
        }

    }
}
