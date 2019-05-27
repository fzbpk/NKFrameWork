namespace UIForm.Entity
{
    partial class IPInfoUI
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.Address_Family_Key = new System.Windows.Forms.Label();
            this.CEnabled = new System.Windows.Forms.CheckBox();
            this.ConfigName_Val = new System.Windows.Forms.TextBox();
            this.ConfigName_Key = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.IPAddress_Val = new System.Windows.Forms.TextBox();
            this.IPAddress_Key = new System.Windows.Forms.Label();
            this.SubnetMask_Val = new System.Windows.Forms.TextBox();
            this.SubnetMask_Key = new System.Windows.Forms.Label();
            this.GateWay_Val = new System.Windows.Forms.TextBox();
            this.GateWay_Key = new System.Windows.Forms.Label();
            this.DNS_Val = new System.Windows.Forms.TextBox();
            this.DNS_Key = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(106, 31);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 130;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "IPV6";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(53, 31);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 16);
            this.radioButton1.TabIndex = 129;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "IPV4";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // Address_Family_Key
            // 
            this.Address_Family_Key.AutoSize = true;
            this.Address_Family_Key.Location = new System.Drawing.Point(6, 31);
            this.Address_Family_Key.Name = "Address_Family_Key";
            this.Address_Family_Key.Size = new System.Drawing.Size(29, 12);
            this.Address_Family_Key.TabIndex = 128;
            this.Address_Family_Key.Text = "类型";
            // 
            // CEnabled
            // 
            this.CEnabled.AutoSize = true;
            this.CEnabled.Location = new System.Drawing.Point(8, 160);
            this.CEnabled.Name = "CEnabled";
            this.CEnabled.Size = new System.Drawing.Size(66, 16);
            this.CEnabled.TabIndex = 126;
            this.CEnabled.Text = "Enabled";
            this.CEnabled.UseVisualStyleBackColor = true;
            // 
            // ConfigName_Val
            // 
            this.ConfigName_Val.Location = new System.Drawing.Point(53, 2);
            this.ConfigName_Val.Name = "ConfigName_Val";
            this.ConfigName_Val.Size = new System.Drawing.Size(160, 21);
            this.ConfigName_Val.TabIndex = 125;
            // 
            // ConfigName_Key
            // 
            this.ConfigName_Key.AutoSize = true;
            this.ConfigName_Key.Location = new System.Drawing.Point(6, 5);
            this.ConfigName_Key.Name = "ConfigName_Key";
            this.ConfigName_Key.Size = new System.Drawing.Size(41, 12);
            this.ConfigName_Key.TabIndex = 127;
            this.ConfigName_Key.Text = "配置名";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(200, 30);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(48, 16);
            this.checkBox1.TabIndex = 131;
            this.checkBox1.Text = "DHCP";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // IPAddress_Val
            // 
            this.IPAddress_Val.Location = new System.Drawing.Point(52, 52);
            this.IPAddress_Val.Name = "IPAddress_Val";
            this.IPAddress_Val.Size = new System.Drawing.Size(196, 21);
            this.IPAddress_Val.TabIndex = 136;
            // 
            // IPAddress_Key
            // 
            this.IPAddress_Key.AutoSize = true;
            this.IPAddress_Key.Location = new System.Drawing.Point(6, 55);
            this.IPAddress_Key.Name = "IPAddress_Key";
            this.IPAddress_Key.Size = new System.Drawing.Size(29, 12);
            this.IPAddress_Key.TabIndex = 137;
            this.IPAddress_Key.Text = "地址";
            // 
            // SubnetMask_Val
            // 
            this.SubnetMask_Val.Location = new System.Drawing.Point(53, 79);
            this.SubnetMask_Val.Name = "SubnetMask_Val";
            this.SubnetMask_Val.Size = new System.Drawing.Size(196, 21);
            this.SubnetMask_Val.TabIndex = 138;
            // 
            // SubnetMask_Key
            // 
            this.SubnetMask_Key.AutoSize = true;
            this.SubnetMask_Key.Location = new System.Drawing.Point(7, 82);
            this.SubnetMask_Key.Name = "SubnetMask_Key";
            this.SubnetMask_Key.Size = new System.Drawing.Size(29, 12);
            this.SubnetMask_Key.TabIndex = 139;
            this.SubnetMask_Key.Text = "掩码";
            // 
            // GateWay_Val
            // 
            this.GateWay_Val.Location = new System.Drawing.Point(52, 106);
            this.GateWay_Val.Name = "GateWay_Val";
            this.GateWay_Val.Size = new System.Drawing.Size(196, 21);
            this.GateWay_Val.TabIndex = 140;
            // 
            // GateWay_Key
            // 
            this.GateWay_Key.AutoSize = true;
            this.GateWay_Key.Location = new System.Drawing.Point(6, 109);
            this.GateWay_Key.Name = "GateWay_Key";
            this.GateWay_Key.Size = new System.Drawing.Size(29, 12);
            this.GateWay_Key.TabIndex = 141;
            this.GateWay_Key.Text = "网关";
            // 
            // DNS_Val
            // 
            this.DNS_Val.Location = new System.Drawing.Point(52, 133);
            this.DNS_Val.Name = "DNS_Val";
            this.DNS_Val.Size = new System.Drawing.Size(196, 21);
            this.DNS_Val.TabIndex = 142;
            // 
            // DNS_Key
            // 
            this.DNS_Key.AutoSize = true;
            this.DNS_Key.Location = new System.Drawing.Point(6, 136);
            this.DNS_Key.Name = "DNS_Key";
            this.DNS_Key.Size = new System.Drawing.Size(23, 12);
            this.DNS_Key.TabIndex = 143;
            this.DNS_Key.Text = "DNS";
            // 
            // IPInfoUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DNS_Val);
            this.Controls.Add(this.DNS_Key);
            this.Controls.Add(this.GateWay_Val);
            this.Controls.Add(this.GateWay_Key);
            this.Controls.Add(this.SubnetMask_Val);
            this.Controls.Add(this.SubnetMask_Key);
            this.Controls.Add(this.IPAddress_Val);
            this.Controls.Add(this.IPAddress_Key);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.Address_Family_Key);
            this.Controls.Add(this.CEnabled);
            this.Controls.Add(this.ConfigName_Val);
            this.Controls.Add(this.ConfigName_Key);
            this.Name = "IPInfoUI";
            this.Size = new System.Drawing.Size(252, 180);
            this.Load += new System.EventHandler(this.IPInfoUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label Address_Family_Key;
        private System.Windows.Forms.CheckBox CEnabled;
        private System.Windows.Forms.TextBox ConfigName_Val;
        private System.Windows.Forms.Label ConfigName_Key;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox IPAddress_Val;
        private System.Windows.Forms.Label IPAddress_Key;
        private System.Windows.Forms.TextBox SubnetMask_Val;
        private System.Windows.Forms.Label SubnetMask_Key;
        private System.Windows.Forms.TextBox GateWay_Val;
        private System.Windows.Forms.Label GateWay_Key;
        private System.Windows.Forms.TextBox DNS_Val;
        private System.Windows.Forms.Label DNS_Key;
    }
}
