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
    public partial class CertificateDetailUI : UserControl
    {
        public CertificateDetailUI()
        {
            InitializeComponent();
        }


        private void CertificateDetailUI_Load(object sender, EventArgs e)
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
        public CertificateDetail Info
        {
            get
            {
                if (string.IsNullOrEmpty(this.ModuleName_Val.Text))
                {
                    MessageBox.Show("请输入" + this.ModuleName_Key.Text);
                    return null;
                }
                if (string.IsNullOrEmpty(this.FuncName_Val.Text))
                {
                    MessageBox.Show("请输入" + this.FuncName_Key.Text);
                    return null;
                }
                CertificateDetail info = new CertificateDetail(); 
                info.ModuleName = this.ModuleName_Val.Text;
                info.FuncName = this.FuncName_Val.Text;
                info.CanUse = this.CanUse_Val.Text;
                info.CanNotUse = this.CanNotUse_Val.Text;
                info.FuncPower = (ushort)NK.ENum.Operate_Type.None;
                if(this.checkBox1.Checked)
                    info.FuncPower|= (ushort)NK.ENum.Operate_Type.Insert;
                if (this.checkBox2.Checked)
                    info.FuncPower |= (ushort)NK.ENum.Operate_Type.Update;
                if (this.checkBox3.Checked)
                    info.FuncPower |= (ushort)NK.ENum.Operate_Type.DELETE;
                if (this.checkBox4.Checked)
                    info.FuncPower |= (ushort)NK.ENum.Operate_Type.Find;
                if (this.checkBox5.Checked)
                    info.FuncPower |= (ushort)NK.ENum.Operate_Type.List;
                return info;
            }
            set
            {
                if (value != null)
                {
                    this.ModuleName_Val.Text = value.ModuleName;
                    this.FuncName_Val.Text = value.FuncName;
                    this.CanUse_Val.Text = value.CanUse;
                    this.CanNotUse_Val.Text = value.CanNotUse;
                    ushort power = value.FuncPower;
                    if ((power / 10000) > 0)
                    {
                        this.checkBox5.Checked = true;
                        power = (ushort)(power %  10000);
                    }
                    if ((power / 1000) > 0)
                    {
                        this.checkBox4.Checked = true;
                        power = (ushort)(power % 1000);
                    }
                    if ((power / 100) > 0)
                    {
                        this.checkBox3.Checked = true;
                        power = (ushort)(power % 100);
                    }
                    if ((power / 10) > 0)
                    {
                        this.checkBox2.Checked = true;
                        power = (ushort)(power % 10);
                    }
                    if (power > 0)
                        this.checkBox1.Checked = true;
                }
            }
        }

    }
}
