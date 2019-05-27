namespace UIForm.Entity
{
    partial class DBInfoUI
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
            this.CEnabled = new System.Windows.Forms.CheckBox();
            this.ConfigName_Val = new System.Windows.Forms.TextBox();
            this.ConfigName_Key = new System.Windows.Forms.Label();
            this.Mode_Key = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.Driver_Val = new System.Windows.Forms.TextBox();
            this.Driver_Key = new System.Windows.Forms.Label();
            this.Url_Val = new System.Windows.Forms.TextBox();
            this.Url_Key = new System.Windows.Forms.Label();
            this.User_Val = new System.Windows.Forms.TextBox();
            this.User_Key = new System.Windows.Forms.Label();
            this.Password_Val = new System.Windows.Forms.TextBox();
            this.Password_Key = new System.Windows.Forms.Label();
            this.DataBaseName_Val = new System.Windows.Forms.TextBox();
            this.DataBaseName_Key = new System.Windows.Forms.Label();
            this.Port_Key = new System.Windows.Forms.Label();
            this.Port_Val = new System.Windows.Forms.NumericUpDown();
            this.TimeOut_Key = new System.Windows.Forms.Label();
            this.TimeOut_Val = new System.Windows.Forms.NumericUpDown();
            this.Charset_Key = new System.Windows.Forms.Label();
            this.Charset_Val = new System.Windows.Forms.TextBox();
            this.ConnStr_Val = new System.Windows.Forms.RichTextBox();
            this.ConnStr_Key = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Port_Val)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeOut_Val)).BeginInit();
            this.SuspendLayout();
            // 
            // CEnabled
            // 
            this.CEnabled.AutoSize = true;
            this.CEnabled.Location = new System.Drawing.Point(239, 6);
            this.CEnabled.Name = "CEnabled";
            this.CEnabled.Size = new System.Drawing.Size(66, 16);
            this.CEnabled.TabIndex = 120;
            this.CEnabled.Text = "Enabled";
            this.CEnabled.UseVisualStyleBackColor = true;
            // 
            // ConfigName_Val
            // 
            this.ConfigName_Val.Location = new System.Drawing.Point(59, 3);
            this.ConfigName_Val.Name = "ConfigName_Val";
            this.ConfigName_Val.Size = new System.Drawing.Size(160, 21);
            this.ConfigName_Val.TabIndex = 119;
            // 
            // ConfigName_Key
            // 
            this.ConfigName_Key.AutoSize = true;
            this.ConfigName_Key.Location = new System.Drawing.Point(12, 6);
            this.ConfigName_Key.Name = "ConfigName_Key";
            this.ConfigName_Key.Size = new System.Drawing.Size(41, 12);
            this.ConfigName_Key.TabIndex = 121;
            this.ConfigName_Key.Text = "配置名";
            // 
            // Mode_Key
            // 
            this.Mode_Key.AutoSize = true;
            this.Mode_Key.Location = new System.Drawing.Point(12, 32);
            this.Mode_Key.Name = "Mode_Key";
            this.Mode_Key.Size = new System.Drawing.Size(29, 12);
            this.Mode_Key.TabIndex = 122;
            this.Mode_Key.Text = "类型";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(59, 32);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 16);
            this.radioButton1.TabIndex = 123;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "None";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(112, 32);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(53, 16);
            this.radioButton2.TabIndex = 124;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "MSSQL";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(165, 32);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(59, 16);
            this.radioButton3.TabIndex = 125;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Access";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(220, 32);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(53, 16);
            this.radioButton4.TabIndex = 126;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "MYSQL";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(279, 32);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(59, 16);
            this.radioButton5.TabIndex = 127;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Oracle";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new System.Drawing.Point(279, 54);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(47, 16);
            this.radioButton7.TabIndex = 131;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "ODBC";
            this.radioButton7.UseVisualStyleBackColor = true;
            this.radioButton7.CheckedChanged += new System.EventHandler(this.radioButton7_CheckedChanged);
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(220, 54);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(53, 16);
            this.radioButton8.TabIndex = 130;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "OleDB";
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.CheckedChanged += new System.EventHandler(this.radioButton8_CheckedChanged);
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Location = new System.Drawing.Point(59, 54);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(83, 16);
            this.radioButton9.TabIndex = 129;
            this.radioButton9.TabStop = true;
            this.radioButton9.Text = "PostgreSQL";
            this.radioButton9.UseVisualStyleBackColor = true;
            this.radioButton9.CheckedChanged += new System.EventHandler(this.radioButton9_CheckedChanged);
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Location = new System.Drawing.Point(155, 54);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(59, 16);
            this.radioButton10.TabIndex = 128;
            this.radioButton10.TabStop = true;
            this.radioButton10.Text = "SQLite";
            this.radioButton10.UseVisualStyleBackColor = true;
            this.radioButton10.CheckedChanged += new System.EventHandler(this.radioButton10_CheckedChanged);
            // 
            // Driver_Val
            // 
            this.Driver_Val.Location = new System.Drawing.Point(59, 73);
            this.Driver_Val.Name = "Driver_Val";
            this.Driver_Val.Size = new System.Drawing.Size(301, 21);
            this.Driver_Val.TabIndex = 132;
            // 
            // Driver_Key
            // 
            this.Driver_Key.AutoSize = true;
            this.Driver_Key.Location = new System.Drawing.Point(12, 76);
            this.Driver_Key.Name = "Driver_Key";
            this.Driver_Key.Size = new System.Drawing.Size(29, 12);
            this.Driver_Key.TabIndex = 133;
            this.Driver_Key.Text = "驱动";
            // 
            // Url_Val
            // 
            this.Url_Val.Location = new System.Drawing.Point(58, 100);
            this.Url_Val.Name = "Url_Val";
            this.Url_Val.Size = new System.Drawing.Size(196, 21);
            this.Url_Val.TabIndex = 134;
            // 
            // Url_Key
            // 
            this.Url_Key.AutoSize = true;
            this.Url_Key.Location = new System.Drawing.Point(12, 103);
            this.Url_Key.Name = "Url_Key";
            this.Url_Key.Size = new System.Drawing.Size(29, 12);
            this.Url_Key.TabIndex = 135;
            this.Url_Key.Text = "地址";
            // 
            // User_Val
            // 
            this.User_Val.Location = new System.Drawing.Point(59, 126);
            this.User_Val.Name = "User_Val";
            this.User_Val.Size = new System.Drawing.Size(121, 21);
            this.User_Val.TabIndex = 136;
            // 
            // User_Key
            // 
            this.User_Key.AutoSize = true;
            this.User_Key.Location = new System.Drawing.Point(12, 129);
            this.User_Key.Name = "User_Key";
            this.User_Key.Size = new System.Drawing.Size(29, 12);
            this.User_Key.TabIndex = 137;
            this.User_Key.Text = "账号";
            // 
            // Password_Val
            // 
            this.Password_Val.Location = new System.Drawing.Point(239, 126);
            this.Password_Val.Name = "Password_Val";
            this.Password_Val.Size = new System.Drawing.Size(121, 21);
            this.Password_Val.TabIndex = 138;
            // 
            // Password_Key
            // 
            this.Password_Key.AutoSize = true;
            this.Password_Key.Location = new System.Drawing.Point(195, 129);
            this.Password_Key.Name = "Password_Key";
            this.Password_Key.Size = new System.Drawing.Size(29, 12);
            this.Password_Key.TabIndex = 139;
            this.Password_Key.Text = "密码";
            // 
            // DataBaseName_Val
            // 
            this.DataBaseName_Val.Location = new System.Drawing.Point(59, 157);
            this.DataBaseName_Val.Name = "DataBaseName_Val";
            this.DataBaseName_Val.Size = new System.Drawing.Size(121, 21);
            this.DataBaseName_Val.TabIndex = 140;
            // 
            // DataBaseName_Key
            // 
            this.DataBaseName_Key.AutoSize = true;
            this.DataBaseName_Key.Location = new System.Drawing.Point(12, 160);
            this.DataBaseName_Key.Name = "DataBaseName_Key";
            this.DataBaseName_Key.Size = new System.Drawing.Size(41, 12);
            this.DataBaseName_Key.TabIndex = 141;
            this.DataBaseName_Key.Text = "数据库";
            // 
            // Port_Key
            // 
            this.Port_Key.AutoSize = true;
            this.Port_Key.Location = new System.Drawing.Point(260, 103);
            this.Port_Key.Name = "Port_Key";
            this.Port_Key.Size = new System.Drawing.Size(29, 12);
            this.Port_Key.TabIndex = 142;
            this.Port_Key.Text = "端口";
            // 
            // Port_Val
            // 
            this.Port_Val.Location = new System.Drawing.Point(296, 101);
            this.Port_Val.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.Port_Val.Name = "Port_Val";
            this.Port_Val.Size = new System.Drawing.Size(64, 21);
            this.Port_Val.TabIndex = 143;
            // 
            // TimeOut_Key
            // 
            this.TimeOut_Key.AutoSize = true;
            this.TimeOut_Key.Location = new System.Drawing.Point(195, 160);
            this.TimeOut_Key.Name = "TimeOut_Key";
            this.TimeOut_Key.Size = new System.Drawing.Size(29, 12);
            this.TimeOut_Key.TabIndex = 144;
            this.TimeOut_Key.Text = "超时";
            // 
            // TimeOut_Val
            // 
            this.TimeOut_Val.Location = new System.Drawing.Point(239, 158);
            this.TimeOut_Val.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.TimeOut_Val.Name = "TimeOut_Val";
            this.TimeOut_Val.Size = new System.Drawing.Size(121, 21);
            this.TimeOut_Val.TabIndex = 145;
            // 
            // Charset_Key
            // 
            this.Charset_Key.AutoSize = true;
            this.Charset_Key.Location = new System.Drawing.Point(12, 192);
            this.Charset_Key.Name = "Charset_Key";
            this.Charset_Key.Size = new System.Drawing.Size(29, 12);
            this.Charset_Key.TabIndex = 146;
            this.Charset_Key.Text = "编码";
            // 
            // Charset_Val
            // 
            this.Charset_Val.Location = new System.Drawing.Point(59, 189);
            this.Charset_Val.Name = "Charset_Val";
            this.Charset_Val.Size = new System.Drawing.Size(121, 21);
            this.Charset_Val.TabIndex = 147;
            // 
            // ConnStr_Val
            // 
            this.ConnStr_Val.Location = new System.Drawing.Point(59, 217);
            this.ConnStr_Val.Name = "ConnStr_Val";
            this.ConnStr_Val.Size = new System.Drawing.Size(301, 43);
            this.ConnStr_Val.TabIndex = 148;
            this.ConnStr_Val.Text = "";
            // 
            // ConnStr_Key
            // 
            this.ConnStr_Key.AutoSize = true;
            this.ConnStr_Key.Location = new System.Drawing.Point(12, 220);
            this.ConnStr_Key.Name = "ConnStr_Key";
            this.ConnStr_Key.Size = new System.Drawing.Size(41, 12);
            this.ConnStr_Key.TabIndex = 149;
            this.ConnStr_Key.Text = "连接串";
            // 
            // DBInfoUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ConnStr_Key);
            this.Controls.Add(this.ConnStr_Val);
            this.Controls.Add(this.Charset_Val);
            this.Controls.Add(this.Charset_Key);
            this.Controls.Add(this.TimeOut_Val);
            this.Controls.Add(this.TimeOut_Key);
            this.Controls.Add(this.Port_Val);
            this.Controls.Add(this.Port_Key);
            this.Controls.Add(this.DataBaseName_Val);
            this.Controls.Add(this.DataBaseName_Key);
            this.Controls.Add(this.Password_Val);
            this.Controls.Add(this.Password_Key);
            this.Controls.Add(this.User_Val);
            this.Controls.Add(this.User_Key);
            this.Controls.Add(this.Url_Val);
            this.Controls.Add(this.Url_Key);
            this.Controls.Add(this.Driver_Val);
            this.Controls.Add(this.Driver_Key);
            this.Controls.Add(this.radioButton7);
            this.Controls.Add(this.radioButton8);
            this.Controls.Add(this.radioButton9);
            this.Controls.Add(this.radioButton10);
            this.Controls.Add(this.radioButton5);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.Mode_Key);
            this.Controls.Add(this.CEnabled);
            this.Controls.Add(this.ConfigName_Val);
            this.Controls.Add(this.ConfigName_Key);
            this.Name = "DBInfoUI";
            this.Size = new System.Drawing.Size(367, 276);
            this.Load += new System.EventHandler(this.DBInfoUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Port_Val)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeOut_Val)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CEnabled;
        private System.Windows.Forms.TextBox ConfigName_Val;
        private System.Windows.Forms.Label ConfigName_Key;
        private System.Windows.Forms.Label Mode_Key;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.RadioButton radioButton10;
        private System.Windows.Forms.TextBox Driver_Val;
        private System.Windows.Forms.Label Driver_Key;
        private System.Windows.Forms.TextBox Url_Val;
        private System.Windows.Forms.Label Url_Key;
        private System.Windows.Forms.TextBox User_Val;
        private System.Windows.Forms.Label User_Key;
        private System.Windows.Forms.TextBox Password_Val;
        private System.Windows.Forms.Label Password_Key;
        private System.Windows.Forms.TextBox DataBaseName_Val;
        private System.Windows.Forms.Label DataBaseName_Key;
        private System.Windows.Forms.Label Port_Key;
        private System.Windows.Forms.NumericUpDown Port_Val;
        private System.Windows.Forms.Label TimeOut_Key;
        private System.Windows.Forms.NumericUpDown TimeOut_Val;
        private System.Windows.Forms.Label Charset_Key;
        private System.Windows.Forms.TextBox Charset_Val;
        private System.Windows.Forms.RichTextBox ConnStr_Val;
        private System.Windows.Forms.Label ConnStr_Key;
    }
}
