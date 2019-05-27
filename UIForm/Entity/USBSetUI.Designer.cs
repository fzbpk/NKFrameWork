namespace UIForm.Entity
{
    partial class USBSetUI
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
            this.devPath_Key = new System.Windows.Forms.Label();
            this.CEnabled = new System.Windows.Forms.CheckBox();
            this.ConfigName_Val = new System.Windows.Forms.TextBox();
            this.ConfigName_Key = new System.Windows.Forms.Label();
            this.devPath_Val = new System.Windows.Forms.RichTextBox();
            this.PID_Val = new System.Windows.Forms.TextBox();
            this.PID_Key = new System.Windows.Forms.Label();
            this.VID_Val = new System.Windows.Forms.TextBox();
            this.VID_Key = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.Address_Key = new System.Windows.Forms.Label();
            this.Address_Val = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.Address_Val)).BeginInit();
            this.SuspendLayout();
            // 
            // devPath_Key
            // 
            this.devPath_Key.AutoSize = true;
            this.devPath_Key.Location = new System.Drawing.Point(3, 36);
            this.devPath_Key.Name = "devPath_Key";
            this.devPath_Key.Size = new System.Drawing.Size(29, 12);
            this.devPath_Key.TabIndex = 142;
            this.devPath_Key.Text = "地址";
            // 
            // CEnabled
            // 
            this.CEnabled.AutoSize = true;
            this.CEnabled.Location = new System.Drawing.Point(175, 165);
            this.CEnabled.Name = "CEnabled";
            this.CEnabled.Size = new System.Drawing.Size(66, 16);
            this.CEnabled.TabIndex = 139;
            this.CEnabled.Text = "Enabled";
            this.CEnabled.UseVisualStyleBackColor = true;
            // 
            // ConfigName_Val
            // 
            this.ConfigName_Val.Location = new System.Drawing.Point(50, 7);
            this.ConfigName_Val.Name = "ConfigName_Val";
            this.ConfigName_Val.Size = new System.Drawing.Size(191, 21);
            this.ConfigName_Val.TabIndex = 138;
            // 
            // ConfigName_Key
            // 
            this.ConfigName_Key.AutoSize = true;
            this.ConfigName_Key.Location = new System.Drawing.Point(3, 10);
            this.ConfigName_Key.Name = "ConfigName_Key";
            this.ConfigName_Key.Size = new System.Drawing.Size(41, 12);
            this.ConfigName_Key.TabIndex = 140;
            this.ConfigName_Key.Text = "配置名";
            // 
            // devPath_Val
            // 
            this.devPath_Val.Location = new System.Drawing.Point(50, 33);
            this.devPath_Val.Name = "devPath_Val";
            this.devPath_Val.Size = new System.Drawing.Size(191, 50);
            this.devPath_Val.TabIndex = 143;
            this.devPath_Val.Text = "";
            // 
            // PID_Val
            // 
            this.PID_Val.Location = new System.Drawing.Point(50, 116);
            this.PID_Val.Name = "PID_Val";
            this.PID_Val.Size = new System.Drawing.Size(191, 21);
            this.PID_Val.TabIndex = 146;
            // 
            // PID_Key
            // 
            this.PID_Key.AutoSize = true;
            this.PID_Key.Location = new System.Drawing.Point(6, 119);
            this.PID_Key.Name = "PID_Key";
            this.PID_Key.Size = new System.Drawing.Size(23, 12);
            this.PID_Key.TabIndex = 147;
            this.PID_Key.Text = "PID";
            // 
            // VID_Val
            // 
            this.VID_Val.Location = new System.Drawing.Point(50, 89);
            this.VID_Val.Name = "VID_Val";
            this.VID_Val.Size = new System.Drawing.Size(191, 21);
            this.VID_Val.TabIndex = 144;
            // 
            // VID_Key
            // 
            this.VID_Key.AutoSize = true;
            this.VID_Key.Location = new System.Drawing.Point(3, 92);
            this.VID_Key.Name = "VID_Key";
            this.VID_Key.Size = new System.Drawing.Size(23, 12);
            this.VID_Key.TabIndex = 145;
            this.VID_Key.Text = "VID";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(50, 143);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(35, 16);
            this.radioButton1.TabIndex = 148;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "无";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(100, 143);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 149;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "本地";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(167, 143);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(47, 16);
            this.radioButton3.TabIndex = 150;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "远程";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 151;
            this.label1.Text = "类型";
            // 
            // Address_Key
            // 
            this.Address_Key.AutoSize = true;
            this.Address_Key.Location = new System.Drawing.Point(3, 169);
            this.Address_Key.Name = "Address_Key";
            this.Address_Key.Size = new System.Drawing.Size(29, 12);
            this.Address_Key.TabIndex = 152;
            this.Address_Key.Text = "编址";
            // 
            // Address_Val
            // 
            this.Address_Val.Location = new System.Drawing.Point(50, 164);
            this.Address_Val.Name = "Address_Val";
            this.Address_Val.Size = new System.Drawing.Size(95, 21);
            this.Address_Val.TabIndex = 153;
            // 
            // USBSetUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Address_Val);
            this.Controls.Add(this.Address_Key);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.PID_Val);
            this.Controls.Add(this.PID_Key);
            this.Controls.Add(this.VID_Val);
            this.Controls.Add(this.VID_Key);
            this.Controls.Add(this.devPath_Val);
            this.Controls.Add(this.devPath_Key);
            this.Controls.Add(this.CEnabled);
            this.Controls.Add(this.ConfigName_Val);
            this.Controls.Add(this.ConfigName_Key);
            this.Name = "USBSetUI";
            this.Size = new System.Drawing.Size(250, 191);
            this.Load += new System.EventHandler(this.USBSetUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Address_Val)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label devPath_Key;
        private System.Windows.Forms.CheckBox CEnabled;
        private System.Windows.Forms.TextBox ConfigName_Val;
        private System.Windows.Forms.Label ConfigName_Key;
        private System.Windows.Forms.RichTextBox devPath_Val;
        private System.Windows.Forms.TextBox PID_Val;
        private System.Windows.Forms.Label PID_Key;
        private System.Windows.Forms.TextBox VID_Val;
        private System.Windows.Forms.Label VID_Key;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Address_Key;
        private System.Windows.Forms.NumericUpDown Address_Val;
    }
}
