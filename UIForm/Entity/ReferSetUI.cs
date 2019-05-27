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
    public partial class ReferSetUI : UserControl
    {
        public ReferSetUI()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.radioButton1.Checked = true;
            this.radioButton2.Checked = false ;
            this.radioButton3.Checked = false;
            this.radioButton4.Checked = false;
            this.radioButton5.Checked = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.radioButton1.Checked = false;
            this.radioButton2.Checked = true;
            this.radioButton3.Checked = false;
            this.radioButton4.Checked = false;
            this.radioButton5.Checked = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.radioButton1.Checked = false;
            this.radioButton2.Checked = false;
            this.radioButton3.Checked = true;
            this.radioButton4.Checked = false;
            this.radioButton5.Checked = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.radioButton1.Checked = false;
            this.radioButton2.Checked = false;
            this.radioButton3.Checked = false;
            this.radioButton4.Checked = true;
            this.radioButton5.Checked = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            this.radioButton1.Checked = false;
            this.radioButton2.Checked = false;
            this.radioButton3.Checked = false;
            this.radioButton4.Checked = false;
            this.radioButton5.Checked = true;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            this.radioButton10.Checked = true;
            this.radioButton9.Checked = false;
            this.radioButton8.Checked = false;
            this.radioButton7.Checked = false;
            this.radioButton6.Checked = false;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            this.radioButton10.Checked = false;
            this.radioButton9.Checked = true;
            this.radioButton8.Checked = false;
            this.radioButton7.Checked = false;
            this.radioButton6.Checked = false;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            this.radioButton10.Checked = false;
            this.radioButton9.Checked = false;
            this.radioButton8.Checked = true;
            this.radioButton7.Checked = false;
            this.radioButton6.Checked = false;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            this.radioButton10.Checked = false;
            this.radioButton9.Checked = false;
            this.radioButton8.Checked = false;
            this.radioButton7.Checked = true;
            this.radioButton6.Checked = false;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            this.radioButton10.Checked = false;
            this.radioButton9.Checked = false;
            this.radioButton8.Checked = false;
            this.radioButton7.Checked = false;
            this.radioButton6.Checked = true;
        }

        private void ReferSetUI_Load(object sender, EventArgs e)
        {
            this.radioButton1.Checked = true;
            this.radioButton10.Checked = true;
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
        public ReferSet Info
        {
            get
            {
                if (string.IsNullOrEmpty(this.ConfigName_Val.Text))
                {
                    MessageBox.Show("请输入" + this.ConfigName_Key.Text);
                    return null;
                }
                ReferSet info = new ReferSet();
                info.ConfigName = this.ConfigName_Val.Text;
                info.ConnectTimeOut = (int)this.ConnectTimeOut_Val.Value;
                info.WaitTime = (int)this.WaitTime_Val.Value;
                info.ReTry = (int)this.ReTry_Val.Value;
                info.ConnPool = (int)this.ConnPool_Val.Value;
                info.SendTimeout = (int)this.SendTimeout_Val.Value;
                info.SendBufferSize = (int)this.SendBufferSize_Val.Value;
                info.ReceiveTimeout = (int)this.ReceiveTimeout_Val.Value;
                info.ReceiveBufferSize = (int)this.ReceiveBufferSize_Val.Value;
                info.WaitTime = (int)this.WaitTime_Val.Value;
                info.ExecTime = (int)this.ExecTime_Val.Value;
                info.CharSet = this.Charset_Val.Text;
                if (this.radioButton1.Checked)
                    info.IMode = NK.ENum.ReferForUse.None;
                else if (this.radioButton2.Checked)
                    info.IMode = NK.ENum.ReferForUse.NetSet;
                else if (this.radioButton3.Checked)
                    info.IMode = NK.ENum.ReferForUse.UartSet;
                else if (this.radioButton4.Checked)
                    info.IMode = NK.ENum.ReferForUse.USBSet;
                else if (this.radioButton5.Checked)
                    info.IMode = NK.ENum.ReferForUse.File;
                if (this.radioButton10.Checked)
                    info.Debug = NK.ENum.Log_Type.None;
                else if (this.radioButton9.Checked)
                    info.Debug = NK.ENum.Log_Type.Infomation;
                else if (this.radioButton8.Checked)
                    info.Debug = NK.ENum.Log_Type.Error;
                else if (this.radioButton7.Checked)
                    info.Debug = NK.ENum.Log_Type.Test;
                else if (this.radioButton6.Checked)
                    info.Debug = NK.ENum.Log_Type.ALL;
                return info;
            }
            set
            {
                if (value != null)
                {
                    this.ConfigName_Val.Text = value.ConfigName;
                    this.ConnectTimeOut_Val.Value = value.ConnectTimeOut;
                    this.ReTry_Val.Value = value.ReTry;
                    this.WaitTime_Val.Value = value.WaitTime;
                    this.ExecTime_Val.Value = value.ExecTime;
                    this.ConnPool_Val.Value = value.ConnPool;
                    this.SendTimeout_Val.Value = value.SendTimeout;
                    this.SendBufferSize_Val.Value = value.SendBufferSize;
                    this.ReceiveTimeout_Val.Value = value.ReceiveTimeout;
                    this.ReceiveBufferSize_Val.Value = value.ReceiveBufferSize;
                    this.Charset_Val.Text = value.CharSet; 
                    switch (value.IMode)
                    {
                        case NK.ENum.ReferForUse.None:
                            this.radioButton1.Checked = true;
                            break;
                        case NK.ENum.ReferForUse.NetSet:
                            this.radioButton2.Checked = true;
                            break;
                        case NK.ENum.ReferForUse.UartSet:
                            this.radioButton3.Checked = true;
                            break;
                        case NK.ENum.ReferForUse.USBSet:
                            this.radioButton4.Checked = true;
                            break;
                        case NK.ENum.ReferForUse.File:
                            this.radioButton5.Checked = true;
                            break; 
                    }
                    switch (value.Debug)
                    {
                        case NK.ENum.Log_Type.None:
                            this.radioButton10.Checked = true;
                            break;
                        case NK.ENum.Log_Type.Infomation:
                            this.radioButton9.Checked = true;
                            break;
                        case NK.ENum.Log_Type.Error:
                            this.radioButton8.Checked = true;
                            break;
                        case NK.ENum.Log_Type.Test:
                            this.radioButton7.Checked = true;
                            break;
                        case NK.ENum.Log_Type.ALL:
                            this.radioButton6.Checked = true;
                            break;
                    }
                }
            }
        }

    }
}
