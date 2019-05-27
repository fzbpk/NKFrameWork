namespace UIForm.Entity
{
    partial class NetSetUI
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
            this.ConfigName_Val = new System.Windows.Forms.TextBox();
            this.ConfigName_Key = new System.Windows.Forms.Label();
            this.Port_Val = new System.Windows.Forms.NumericUpDown();
            this.Port_Key = new System.Windows.Forms.Label();
            this.Mode_Key = new System.Windows.Forms.Label();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.Address_Family_Val = new System.Windows.Forms.ComboBox();
            this.Address_Family_Key = new System.Windows.Forms.Label();
            this.Socket_Type_Val = new System.Windows.Forms.ComboBox();
            this.Socket_Type_Key = new System.Windows.Forms.Label();
            this.Protocol_Type_Val = new System.Windows.Forms.ComboBox();
            this.Protocol_Type_Key = new System.Windows.Forms.Label();
            this.IPAddress_Val = new System.Windows.Forms.TextBox();
            this.IPAddress_Key = new System.Windows.Forms.Label();
            this.DomainName_Val = new System.Windows.Forms.TextBox();
            this.DomainName_Key = new System.Windows.Forms.Label();
            this.AddrRef_Val = new System.Windows.Forms.TextBox();
            this.AddrRef_Key = new System.Windows.Forms.Label();
            this.Address_Val = new System.Windows.Forms.TextBox();
            this.Address_Key = new System.Windows.Forms.Label();
            this.CEnabled = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.Port_Val)).BeginInit();
            this.SuspendLayout();
            // 
            // ConfigName_Val
            // 
            this.ConfigName_Val.Location = new System.Drawing.Point(63, 5);
            this.ConfigName_Val.Name = "ConfigName_Val";
            this.ConfigName_Val.Size = new System.Drawing.Size(163, 21);
            this.ConfigName_Val.TabIndex = 143;
            // 
            // ConfigName_Key
            // 
            this.ConfigName_Key.AutoSize = true;
            this.ConfigName_Key.Location = new System.Drawing.Point(3, 8);
            this.ConfigName_Key.Name = "ConfigName_Key";
            this.ConfigName_Key.Size = new System.Drawing.Size(41, 12);
            this.ConfigName_Key.TabIndex = 144;
            this.ConfigName_Key.Text = "配置名";
            // 
            // Port_Val
            // 
            this.Port_Val.Location = new System.Drawing.Point(74, 152);
            this.Port_Val.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.Port_Val.Name = "Port_Val";
            this.Port_Val.Size = new System.Drawing.Size(95, 21);
            this.Port_Val.TabIndex = 180;
            // 
            // Port_Key
            // 
            this.Port_Key.AutoSize = true;
            this.Port_Key.Location = new System.Drawing.Point(15, 154);
            this.Port_Key.Name = "Port_Key";
            this.Port_Key.Size = new System.Drawing.Size(29, 12);
            this.Port_Key.TabIndex = 179;
            this.Port_Key.Text = "端口";
            // 
            // Mode_Key
            // 
            this.Mode_Key.AutoSize = true;
            this.Mode_Key.Location = new System.Drawing.Point(15, 102);
            this.Mode_Key.Name = "Mode_Key";
            this.Mode_Key.Size = new System.Drawing.Size(29, 12);
            this.Mode_Key.TabIndex = 178;
            this.Mode_Key.Text = "类型";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(179, 102);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(47, 16);
            this.radioButton3.TabIndex = 177;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "远程";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(112, 102);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 176;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "本地";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(62, 102);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(35, 16);
            this.radioButton1.TabIndex = 175;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "无";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // Address_Family_Val
            // 
            this.Address_Family_Val.FormattingEnabled = true;
            this.Address_Family_Val.Location = new System.Drawing.Point(92, 30);
            this.Address_Family_Val.Name = "Address_Family_Val";
            this.Address_Family_Val.Size = new System.Drawing.Size(121, 20);
            this.Address_Family_Val.TabIndex = 182;
            // 
            // Address_Family_Key
            // 
            this.Address_Family_Key.AutoSize = true;
            this.Address_Family_Key.Location = new System.Drawing.Point(3, 33);
            this.Address_Family_Key.Name = "Address_Family_Key";
            this.Address_Family_Key.Size = new System.Drawing.Size(83, 12);
            this.Address_Family_Key.TabIndex = 181;
            this.Address_Family_Key.Text = "AddressFamily";
            // 
            // Socket_Type_Val
            // 
            this.Socket_Type_Val.FormattingEnabled = true;
            this.Socket_Type_Val.Location = new System.Drawing.Point(92, 53);
            this.Socket_Type_Val.Name = "Socket_Type_Val";
            this.Socket_Type_Val.Size = new System.Drawing.Size(121, 20);
            this.Socket_Type_Val.TabIndex = 184;
            // 
            // Socket_Type_Key
            // 
            this.Socket_Type_Key.AutoSize = true;
            this.Socket_Type_Key.Location = new System.Drawing.Point(3, 56);
            this.Socket_Type_Key.Name = "Socket_Type_Key";
            this.Socket_Type_Key.Size = new System.Drawing.Size(65, 12);
            this.Socket_Type_Key.TabIndex = 183;
            this.Socket_Type_Key.Text = "SocketType";
            // 
            // Protocol_Type_Val
            // 
            this.Protocol_Type_Val.FormattingEnabled = true;
            this.Protocol_Type_Val.Location = new System.Drawing.Point(92, 76);
            this.Protocol_Type_Val.Name = "Protocol_Type_Val";
            this.Protocol_Type_Val.Size = new System.Drawing.Size(121, 20);
            this.Protocol_Type_Val.TabIndex = 186;
            // 
            // Protocol_Type_Key
            // 
            this.Protocol_Type_Key.AutoSize = true;
            this.Protocol_Type_Key.Location = new System.Drawing.Point(3, 79);
            this.Protocol_Type_Key.Name = "Protocol_Type_Key";
            this.Protocol_Type_Key.Size = new System.Drawing.Size(77, 12);
            this.Protocol_Type_Key.TabIndex = 185;
            this.Protocol_Type_Key.Text = "ProtocolType";
            // 
            // IPAddress_Val
            // 
            this.IPAddress_Val.Location = new System.Drawing.Point(75, 128);
            this.IPAddress_Val.Name = "IPAddress_Val";
            this.IPAddress_Val.Size = new System.Drawing.Size(151, 21);
            this.IPAddress_Val.TabIndex = 187;
            // 
            // IPAddress_Key
            // 
            this.IPAddress_Key.AutoSize = true;
            this.IPAddress_Key.Location = new System.Drawing.Point(15, 131);
            this.IPAddress_Key.Name = "IPAddress_Key";
            this.IPAddress_Key.Size = new System.Drawing.Size(41, 12);
            this.IPAddress_Key.TabIndex = 188;
            this.IPAddress_Key.Text = "IP地址";
            // 
            // DomainName_Val
            // 
            this.DomainName_Val.Location = new System.Drawing.Point(75, 176);
            this.DomainName_Val.Name = "DomainName_Val";
            this.DomainName_Val.Size = new System.Drawing.Size(151, 21);
            this.DomainName_Val.TabIndex = 189;
            // 
            // DomainName_Key
            // 
            this.DomainName_Key.AutoSize = true;
            this.DomainName_Key.Location = new System.Drawing.Point(15, 179);
            this.DomainName_Key.Name = "DomainName_Key";
            this.DomainName_Key.Size = new System.Drawing.Size(29, 12);
            this.DomainName_Key.TabIndex = 190;
            this.DomainName_Key.Text = "域名";
            // 
            // AddrRef_Val
            // 
            this.AddrRef_Val.Location = new System.Drawing.Point(75, 200);
            this.AddrRef_Val.Name = "AddrRef_Val";
            this.AddrRef_Val.Size = new System.Drawing.Size(151, 21);
            this.AddrRef_Val.TabIndex = 191;
            // 
            // AddrRef_Key
            // 
            this.AddrRef_Key.AutoSize = true;
            this.AddrRef_Key.Location = new System.Drawing.Point(15, 203);
            this.AddrRef_Key.Name = "AddrRef_Key";
            this.AddrRef_Key.Size = new System.Drawing.Size(53, 12);
            this.AddrRef_Key.TabIndex = 192;
            this.AddrRef_Key.Text = "地址参数";
            // 
            // Address_Val
            // 
            this.Address_Val.Location = new System.Drawing.Point(75, 224);
            this.Address_Val.Name = "Address_Val";
            this.Address_Val.Size = new System.Drawing.Size(151, 21);
            this.Address_Val.TabIndex = 193;
            // 
            // Address_Key
            // 
            this.Address_Key.AutoSize = true;
            this.Address_Key.Location = new System.Drawing.Point(15, 227);
            this.Address_Key.Name = "Address_Key";
            this.Address_Key.Size = new System.Drawing.Size(53, 12);
            this.Address_Key.TabIndex = 194;
            this.Address_Key.Text = "设备编址";
            // 
            // CEnabled
            // 
            this.CEnabled.AutoSize = true;
            this.CEnabled.Location = new System.Drawing.Point(5, 253);
            this.CEnabled.Name = "CEnabled";
            this.CEnabled.Size = new System.Drawing.Size(66, 16);
            this.CEnabled.TabIndex = 195;
            this.CEnabled.Text = "Enabled";
            this.CEnabled.UseVisualStyleBackColor = true;
            this.CEnabled.CheckedChanged += new System.EventHandler(this.CEnabled_CheckedChanged);
            // 
            // NetSetUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CEnabled);
            this.Controls.Add(this.Address_Val);
            this.Controls.Add(this.Address_Key);
            this.Controls.Add(this.AddrRef_Val);
            this.Controls.Add(this.AddrRef_Key);
            this.Controls.Add(this.DomainName_Val);
            this.Controls.Add(this.DomainName_Key);
            this.Controls.Add(this.IPAddress_Val);
            this.Controls.Add(this.IPAddress_Key);
            this.Controls.Add(this.Protocol_Type_Val);
            this.Controls.Add(this.Protocol_Type_Key);
            this.Controls.Add(this.Socket_Type_Val);
            this.Controls.Add(this.Socket_Type_Key);
            this.Controls.Add(this.Address_Family_Val);
            this.Controls.Add(this.Address_Family_Key);
            this.Controls.Add(this.Port_Val);
            this.Controls.Add(this.Port_Key);
            this.Controls.Add(this.Mode_Key);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.ConfigName_Val);
            this.Controls.Add(this.ConfigName_Key);
            this.Name = "NetSetUI";
            this.Size = new System.Drawing.Size(233, 276);
            this.Load += new System.EventHandler(this.NetSetUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Port_Val)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ConfigName_Val;
        private System.Windows.Forms.Label ConfigName_Key;
        private System.Windows.Forms.NumericUpDown Port_Val;
        private System.Windows.Forms.Label Port_Key;
        private System.Windows.Forms.Label Mode_Key;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.ComboBox Address_Family_Val;
        private System.Windows.Forms.Label Address_Family_Key;
        private System.Windows.Forms.ComboBox Socket_Type_Val;
        private System.Windows.Forms.Label Socket_Type_Key;
        private System.Windows.Forms.ComboBox Protocol_Type_Val;
        private System.Windows.Forms.Label Protocol_Type_Key;
        private System.Windows.Forms.TextBox IPAddress_Val;
        private System.Windows.Forms.Label IPAddress_Key;
        private System.Windows.Forms.TextBox DomainName_Val;
        private System.Windows.Forms.Label DomainName_Key;
        private System.Windows.Forms.TextBox AddrRef_Val;
        private System.Windows.Forms.Label AddrRef_Key;
        private System.Windows.Forms.TextBox Address_Val;
        private System.Windows.Forms.Label Address_Key;
        private System.Windows.Forms.CheckBox CEnabled;
    }
}
