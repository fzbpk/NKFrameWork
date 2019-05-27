namespace UIForm.Entity
{
    partial class PortsSetUI
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
            this.CEnabled = new System.Windows.Forms.CheckBox();
            this.Rate_Val = new System.Windows.Forms.NumericUpDown();
            this.Rate_Key = new System.Windows.Forms.Label();
            this.DataBit_Val = new System.Windows.Forms.NumericUpDown();
            this.DataBit_Key = new System.Windows.Forms.Label();
            this.StopBit_Key = new System.Windows.Forms.Label();
            this.StopBit_Val = new System.Windows.Forms.ComboBox();
            this.Parity_Val = new System.Windows.Forms.ComboBox();
            this.Parity_Key = new System.Windows.Forms.Label();
            this.Ctrl_Val = new System.Windows.Forms.ComboBox();
            this.Ctrl_Key = new System.Windows.Forms.Label();
            this.PortType_Val = new System.Windows.Forms.ComboBox();
            this.PortType_Key = new System.Windows.Forms.Label();
            this.Mode_Key = new System.Windows.Forms.Label();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.Address_Val = new System.Windows.Forms.NumericUpDown();
            this.Address_Key = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Port_Val)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rate_Val)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataBit_Val)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address_Val)).BeginInit();
            this.SuspendLayout();
            // 
            // ConfigName_Val
            // 
            this.ConfigName_Val.Location = new System.Drawing.Point(63, 7);
            this.ConfigName_Val.Name = "ConfigName_Val";
            this.ConfigName_Val.Size = new System.Drawing.Size(178, 21);
            this.ConfigName_Val.TabIndex = 141;
            // 
            // ConfigName_Key
            // 
            this.ConfigName_Key.AutoSize = true;
            this.ConfigName_Key.Location = new System.Drawing.Point(3, 10);
            this.ConfigName_Key.Name = "ConfigName_Key";
            this.ConfigName_Key.Size = new System.Drawing.Size(41, 12);
            this.ConfigName_Key.TabIndex = 142;
            this.ConfigName_Key.Text = "配置名";
            // 
            // Port_Val
            // 
            this.Port_Val.Location = new System.Drawing.Point(63, 34);
            this.Port_Val.Name = "Port_Val";
            this.Port_Val.Size = new System.Drawing.Size(106, 21);
            this.Port_Val.TabIndex = 155;
            // 
            // Port_Key
            // 
            this.Port_Key.AutoSize = true;
            this.Port_Key.Location = new System.Drawing.Point(3, 39);
            this.Port_Key.Name = "Port_Key";
            this.Port_Key.Size = new System.Drawing.Size(29, 12);
            this.Port_Key.TabIndex = 154;
            this.Port_Key.Text = "端号";
            // 
            // CEnabled
            // 
            this.CEnabled.AutoSize = true;
            this.CEnabled.Location = new System.Drawing.Point(175, 38);
            this.CEnabled.Name = "CEnabled";
            this.CEnabled.Size = new System.Drawing.Size(66, 16);
            this.CEnabled.TabIndex = 156;
            this.CEnabled.Text = "Enabled";
            this.CEnabled.UseVisualStyleBackColor = true;
            // 
            // Rate_Val
            // 
            this.Rate_Val.Location = new System.Drawing.Point(63, 61);
            this.Rate_Val.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.Rate_Val.Name = "Rate_Val";
            this.Rate_Val.Size = new System.Drawing.Size(106, 21);
            this.Rate_Val.TabIndex = 158;
            this.Rate_Val.Value = new decimal(new int[] {
            9600,
            0,
            0,
            0});
            // 
            // Rate_Key
            // 
            this.Rate_Key.AutoSize = true;
            this.Rate_Key.Location = new System.Drawing.Point(3, 66);
            this.Rate_Key.Name = "Rate_Key";
            this.Rate_Key.Size = new System.Drawing.Size(41, 12);
            this.Rate_Key.TabIndex = 157;
            this.Rate_Key.Text = "波特率";
            // 
            // DataBit_Val
            // 
            this.DataBit_Val.Location = new System.Drawing.Point(63, 88);
            this.DataBit_Val.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.DataBit_Val.Name = "DataBit_Val";
            this.DataBit_Val.Size = new System.Drawing.Size(106, 21);
            this.DataBit_Val.TabIndex = 160;
            this.DataBit_Val.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // DataBit_Key
            // 
            this.DataBit_Key.AutoSize = true;
            this.DataBit_Key.Location = new System.Drawing.Point(3, 93);
            this.DataBit_Key.Name = "DataBit_Key";
            this.DataBit_Key.Size = new System.Drawing.Size(41, 12);
            this.DataBit_Key.TabIndex = 159;
            this.DataBit_Key.Text = "数据位";
            // 
            // StopBit_Key
            // 
            this.StopBit_Key.AutoSize = true;
            this.StopBit_Key.Location = new System.Drawing.Point(3, 118);
            this.StopBit_Key.Name = "StopBit_Key";
            this.StopBit_Key.Size = new System.Drawing.Size(41, 12);
            this.StopBit_Key.TabIndex = 161;
            this.StopBit_Key.Text = "停止位";
            // 
            // StopBit_Val
            // 
            this.StopBit_Val.FormattingEnabled = true;
            this.StopBit_Val.Items.AddRange(new object[] {
            "None",
            "One",
            "Two",
            "OnePointFive"});
            this.StopBit_Val.Location = new System.Drawing.Point(63, 115);
            this.StopBit_Val.Name = "StopBit_Val";
            this.StopBit_Val.Size = new System.Drawing.Size(121, 20);
            this.StopBit_Val.TabIndex = 162;
            this.StopBit_Val.Text = "One";
            // 
            // Parity_Val
            // 
            this.Parity_Val.FormattingEnabled = true;
            this.Parity_Val.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mar",
            "Space"});
            this.Parity_Val.Location = new System.Drawing.Point(63, 141);
            this.Parity_Val.Name = "Parity_Val";
            this.Parity_Val.Size = new System.Drawing.Size(121, 20);
            this.Parity_Val.TabIndex = 164;
            this.Parity_Val.Text = "None";
            // 
            // Parity_Key
            // 
            this.Parity_Key.AutoSize = true;
            this.Parity_Key.Location = new System.Drawing.Point(3, 144);
            this.Parity_Key.Name = "Parity_Key";
            this.Parity_Key.Size = new System.Drawing.Size(29, 12);
            this.Parity_Key.TabIndex = 163;
            this.Parity_Key.Text = "校验";
            // 
            // Ctrl_Val
            // 
            this.Ctrl_Val.FormattingEnabled = true;
            this.Ctrl_Val.Items.AddRange(new object[] {
            "None",
            "One",
            "Two",
            "OnePointFive"});
            this.Ctrl_Val.Location = new System.Drawing.Point(63, 167);
            this.Ctrl_Val.Name = "Ctrl_Val";
            this.Ctrl_Val.Size = new System.Drawing.Size(121, 20);
            this.Ctrl_Val.TabIndex = 166;
            this.Ctrl_Val.Text = "One";
            // 
            // Ctrl_Key
            // 
            this.Ctrl_Key.AutoSize = true;
            this.Ctrl_Key.Location = new System.Drawing.Point(3, 170);
            this.Ctrl_Key.Name = "Ctrl_Key";
            this.Ctrl_Key.Size = new System.Drawing.Size(29, 12);
            this.Ctrl_Key.TabIndex = 165;
            this.Ctrl_Key.Text = "流控";
            // 
            // PortType_Val
            // 
            this.PortType_Val.FormattingEnabled = true;
            this.PortType_Val.Items.AddRange(new object[] {
            "None",
            "RS232",
            "RS485",
            "RS422",
            "LPT"});
            this.PortType_Val.Location = new System.Drawing.Point(62, 193);
            this.PortType_Val.Name = "PortType_Val";
            this.PortType_Val.Size = new System.Drawing.Size(121, 20);
            this.PortType_Val.TabIndex = 168;
            this.PortType_Val.Text = "None";
            // 
            // PortType_Key
            // 
            this.PortType_Key.AutoSize = true;
            this.PortType_Key.Location = new System.Drawing.Point(3, 196);
            this.PortType_Key.Name = "PortType_Key";
            this.PortType_Key.Size = new System.Drawing.Size(53, 12);
            this.PortType_Key.TabIndex = 167;
            this.PortType_Key.Text = "通讯类型";
            // 
            // Mode_Key
            // 
            this.Mode_Key.AutoSize = true;
            this.Mode_Key.Location = new System.Drawing.Point(3, 222);
            this.Mode_Key.Name = "Mode_Key";
            this.Mode_Key.Size = new System.Drawing.Size(29, 12);
            this.Mode_Key.TabIndex = 172;
            this.Mode_Key.Text = "类型";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(167, 222);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(47, 16);
            this.radioButton3.TabIndex = 171;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "远程";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(100, 222);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 170;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "本地";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(50, 222);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(35, 16);
            this.radioButton1.TabIndex = 169;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "无";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // Address_Val
            // 
            this.Address_Val.Location = new System.Drawing.Point(62, 238);
            this.Address_Val.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.Address_Val.Name = "Address_Val";
            this.Address_Val.Size = new System.Drawing.Size(95, 21);
            this.Address_Val.TabIndex = 174;
            // 
            // Address_Key
            // 
            this.Address_Key.AutoSize = true;
            this.Address_Key.Location = new System.Drawing.Point(3, 240);
            this.Address_Key.Name = "Address_Key";
            this.Address_Key.Size = new System.Drawing.Size(29, 12);
            this.Address_Key.TabIndex = 173;
            this.Address_Key.Text = "编址";
            // 
            // PortsSetUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Address_Val);
            this.Controls.Add(this.Address_Key);
            this.Controls.Add(this.Mode_Key);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.PortType_Val);
            this.Controls.Add(this.PortType_Key);
            this.Controls.Add(this.Ctrl_Val);
            this.Controls.Add(this.Ctrl_Key);
            this.Controls.Add(this.Parity_Val);
            this.Controls.Add(this.Parity_Key);
            this.Controls.Add(this.StopBit_Val);
            this.Controls.Add(this.StopBit_Key);
            this.Controls.Add(this.DataBit_Val);
            this.Controls.Add(this.DataBit_Key);
            this.Controls.Add(this.Rate_Val);
            this.Controls.Add(this.Rate_Key);
            this.Controls.Add(this.CEnabled);
            this.Controls.Add(this.Port_Val);
            this.Controls.Add(this.Port_Key);
            this.Controls.Add(this.ConfigName_Val);
            this.Controls.Add(this.ConfigName_Key);
            this.Name = "PortsSetUI";
            this.Size = new System.Drawing.Size(243, 262);
            this.Load += new System.EventHandler(this.PortsSetUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Port_Val)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rate_Val)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataBit_Val)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address_Val)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ConfigName_Val;
        private System.Windows.Forms.Label ConfigName_Key;
        private System.Windows.Forms.NumericUpDown Port_Val;
        private System.Windows.Forms.Label Port_Key;
        private System.Windows.Forms.CheckBox CEnabled;
        private System.Windows.Forms.NumericUpDown Rate_Val;
        private System.Windows.Forms.Label Rate_Key;
        private System.Windows.Forms.NumericUpDown DataBit_Val;
        private System.Windows.Forms.Label DataBit_Key;
        private System.Windows.Forms.Label StopBit_Key;
        private System.Windows.Forms.ComboBox StopBit_Val;
        private System.Windows.Forms.ComboBox Parity_Val;
        private System.Windows.Forms.Label Parity_Key;
        private System.Windows.Forms.ComboBox Ctrl_Val;
        private System.Windows.Forms.Label Ctrl_Key;
        private System.Windows.Forms.ComboBox PortType_Val;
        private System.Windows.Forms.Label PortType_Key;
        private System.Windows.Forms.Label Mode_Key;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.NumericUpDown Address_Val;
        private System.Windows.Forms.Label Address_Key;
    }
}
