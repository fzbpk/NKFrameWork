using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data; 
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NK.ENum;
using NK.Attribut;
namespace UIForm.Entity
{
    public partial class DisplayColumnAttributeUI : UserControl
    {
        public DisplayColumnAttributeUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 子控件
        /// </summary>
        public ControlCollection Containers
        {
            get { return this.Controls; }
        }

        private void DisplayColumnAttributeUI_Load(object sender, EventArgs e)
        {

        }



        /// <summary>
        /// 设置或获取信息
        /// </summary>
        public DisplayColumnAttribute Info
        {
            get
            {
                if (string.IsNullOrEmpty(this.Table_Val.Text))
                {
                    MessageBox.Show("请输入" + this.Table_Key.Text);
                    return null;
                }
                if (string.IsNullOrEmpty(this.Column_Val.Text))
                {
                    MessageBox.Show("请输入" + this.Column_Key.Text);
                    return null;
                }
                DisplayColumnAttribute info = new DisplayColumnAttribute();
                info.Table = this.Table_Val.Text;
                info.Column = this.Column_Val.Text;
                info.Name = this.Name_Val.Text;
                info.CSS = this.CSS_Val.Text;
                info.Format = this.Format_Val.Text;
                info.Unit = this.Unit_Val.Text;
                info.JS = this.JS_Val.Text; 
                info.Seqencing = (int)this.Seqencing_Val.Value;
                info.CanHead = this.CanHead.Checked;
                info.CanCount = this.checkBox1.Checked;
                info.CanDeitail = this.CanDeitail.Checked;
                info.CanImpExp = this.CanImpExp.Checked;
                info.CanSearch = this.CanSearch.Checked;
                info.IsUnique = this.IsUnique.Checked; 
                return info;
            }
            set
            {
                if (value != null)
                {
                    this.Table_Val.Text = value.Table;
                    this.Column_Val.Text = value.Column;
                    this.Name_Val.Text = value.Name;
                    this.CSS_Val.Text = value.CSS;
                    this.Format_Val.Text = value.Format;
                    this.Unit_Val.Text = value.Unit;
                    this.JS_Val.Text = value.JS;
                    this.Seqencing_Val.Value = value.Seqencing;
                    this.CanHead.Checked = value.CanHead;
                    this.checkBox1.Checked = value.CanCount;
                    this.CanDeitail.Checked = value.CanDeitail;
                    this.CanImpExp.Checked = value.CanImpExp;
                    this.CanSearch.Checked = value.CanSearch;
                    this.IsUnique.Checked = value.IsUnique; 
                }
            }
        }
    }
}
