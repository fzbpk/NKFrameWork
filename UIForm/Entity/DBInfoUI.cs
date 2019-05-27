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
    public partial class DBInfoUI : UserControl
    {
        public DBInfoUI()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.Port_Val.Value = 0;
                this.radioButton1.Checked = true;
                this.radioButton2.Checked = false ;
                this.radioButton3.Checked = false;
                this.radioButton4.Checked = false;
                this.radioButton5.Checked = false; 
                this.radioButton7.Checked = false;
                this.radioButton8.Checked = false;
                this.radioButton9.Checked = false;
                this.radioButton10.Checked = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                this.Port_Val.Value =1433;
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = true;
                this.radioButton3.Checked = false;
                this.radioButton4.Checked = false;
                this.radioButton5.Checked = false;
                this.radioButton7.Checked = false;
                this.radioButton8.Checked = false;
                this.radioButton9.Checked = false;
                this.radioButton10.Checked = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton3.Checked)
            {
                this.Port_Val.Value = 0;
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = false;
                this.radioButton3.Checked = true;
                this.radioButton4.Checked = false;
                this.radioButton5.Checked = false;
                this.radioButton7.Checked = false;
                this.radioButton8.Checked = false;
                this.radioButton9.Checked = false;
                this.radioButton10.Checked = false;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton4.Checked)
            {
                this.Port_Val.Value = 3306;
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = false;
                this.radioButton3.Checked = false;
                this.radioButton4.Checked = true;
                this.radioButton5.Checked = false;
                this.radioButton7.Checked = false;
                this.radioButton8.Checked = false;
                this.radioButton9.Checked = false;
                this.radioButton10.Checked = false;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton5.Checked)
            {
                this.Port_Val.Value = 1521;
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = false;
                this.radioButton3.Checked = false;
                this.radioButton4.Checked = false;
                this.radioButton5.Checked = true;
                this.radioButton7.Checked = false;
                this.radioButton8.Checked = false;
                this.radioButton9.Checked = false;
                this.radioButton10.Checked = false;
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton9.Checked)
            {
                this.Port_Val.Value = 5432;
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = false;
                this.radioButton3.Checked = false;
                this.radioButton4.Checked = false;
                this.radioButton5.Checked = false;
                this.radioButton7.Checked = false;
                this.radioButton8.Checked = false;
                this.radioButton9.Checked = true;
                this.radioButton10.Checked = false;
            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton10.Checked)
            {
                this.Port_Val.Value = 0;
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = false;
                this.radioButton3.Checked = false;
                this.radioButton4.Checked = false;
                this.radioButton5.Checked = false;
                this.radioButton7.Checked = false;
                this.radioButton8.Checked = false;
                this.radioButton9.Checked = false;
                this.radioButton10.Checked = true;
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton8.Checked)
            {
                this.Port_Val.Value = 0;
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = false;
                this.radioButton3.Checked = false;
                this.radioButton4.Checked = false;
                this.radioButton5.Checked = false;
                this.radioButton7.Checked = false;
                this.radioButton8.Checked = true;
                this.radioButton9.Checked = false;
                this.radioButton10.Checked = false;
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton7.Checked)
            {
                this.Port_Val.Value = 0;
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = false;
                this.radioButton3.Checked = false;
                this.radioButton4.Checked = false;
                this.radioButton5.Checked = false;
                this.radioButton7.Checked = true;
                this.radioButton8.Checked = false;
                this.radioButton9.Checked = false;
                this.radioButton10.Checked = false;
            }
        }

        private void DBInfoUI_Load(object sender, EventArgs e)
        {
            this.Port_Val.Value = 0;
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
        public DBInfo Info
        {
            get
            {
                if (string.IsNullOrEmpty(this.ConfigName_Val.Text))
                {
                    MessageBox.Show("请输入" + this.ConfigName_Key.Text);
                    return null;
                } 
                DBInfo info = new DBInfo();
                info.ConfigName = this.ConfigName_Val.Text;
                info.Driver = this.Driver_Val.Text;
                info.Url = this.Url_Val.Text;
                info.User = this.User_Val.Text;
                info.Password = this.Password_Val.Text;
                info.DataBaseName = this.DataBaseName_Val.Text;
                info.Charset = this.Charset_Val.Text;
                info.Port = (int)this.Port_Val.Value;
                info.TimeOut = (int)this.TimeOut_Val.Value;
                info.ConnStr = this.ConnStr_Val.Text;
                if (this.radioButton1.Checked)
                    info.Mode = NK.ENum.DBType.None;
                else if (this.radioButton2.Checked)
                    info.Mode = NK.ENum.DBType.MSSQL;
                else if (this.radioButton3.Checked)
                    info.Mode = NK.ENum.DBType.Access;
                else if (this.radioButton4.Checked)
                    info.Mode = NK.ENum.DBType.MYSQL;
                else if (this.radioButton5.Checked) 
                    info.Mode = NK.ENum.DBType.Oracle;
                else if(this.radioButton9.Checked)
                    info.Mode = NK.ENum.DBType.PostgreSQL;
                else if (this.radioButton10.Checked)
                    info.Mode = NK.ENum.DBType.SQLite;
                else if (this.radioButton8.Checked)
                    info.Mode = NK.ENum.DBType.OleDB;
                else if (this.radioButton7.Checked)
                    info.Mode = NK.ENum.DBType.ODBC;
                info.Enable = this.CEnabled.Checked;
                return info;
            }
            set
            {
                if (value != null)
                {
                    this.ConfigName_Val.Text = value.ConfigName;
                    this.Driver_Val.Text = value.Driver;
                    this.Url_Val.Text = value.Url;
                    this.User_Val.Text = value.User;
                    this.Password_Val.Text = value.Password;
                    this.DataBaseName_Val.Text = value.DataBaseName;
                    this.Charset_Val.Text = value.Charset;
                    this.Port_Val.Value = value.Port;
                    this.TimeOut_Val.Value = value.TimeOut;
                    this.ConnStr_Val.Text = value.ConnStr;
                    this.CEnabled.Checked = value.Enable;
                    switch (value.Mode)
                    {
                        case NK.ENum.DBType.None:
                            this.radioButton1.Checked = true;
                            break;
                        case NK.ENum.DBType.MSSQL: 
                            this.radioButton2.Checked = true; 
                            break;
                        case NK.ENum.DBType.Access:
                            this.radioButton3.Checked = true;
                            break;
                        case NK.ENum.DBType.MYSQL:
                            this.radioButton4.Checked = true;
                            break;
                        case NK.ENum.DBType.Oracle:
                            this.radioButton5.Checked = true;
                            break;
                        case NK.ENum.DBType.PostgreSQL:
                            this.radioButton9.Checked = true;
                            break;
                        case NK.ENum.DBType.SQLite:
                            this.radioButton10.Checked = true;
                            break;
                        case NK.ENum.DBType.OleDB:
                            this.radioButton8.Checked = true;
                            break;
                        case NK.ENum.DBType.ODBC:
                            this.radioButton7.Checked = true;
                            break;
                    }
                }
            }
        }

    }
}
