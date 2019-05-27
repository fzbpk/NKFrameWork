namespace UIForm.Entity
{
    partial class ReferSetUI
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
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.Mode_Key = new System.Windows.Forms.Label();
            this.Charset_Val = new System.Windows.Forms.TextBox();
            this.Charset_Key = new System.Windows.Forms.Label();
            this.ReTry_Val = new System.Windows.Forms.NumericUpDown();
            this.ReTry_Key = new System.Windows.Forms.Label();
            this.ConnectTimeOut_Val = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.ExecTime_Val = new System.Windows.Forms.NumericUpDown();
            this.ExecTime_Key = new System.Windows.Forms.Label();
            this.WaitTime_Val = new System.Windows.Forms.NumericUpDown();
            this.WaitTime_Key = new System.Windows.Forms.Label();
            this.SendBufferSize_Val = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.SendTimeout_Val = new System.Windows.Forms.NumericUpDown();
            this.SendTimeout_Key = new System.Windows.Forms.Label();
            this.ReceiveBufferSize_Val = new System.Windows.Forms.NumericUpDown();
            this.ReceiveBufferSize_Key = new System.Windows.Forms.Label();
            this.ReceiveTimeout_Val = new System.Windows.Forms.NumericUpDown();
            this.ReceiveTimeout_Key = new System.Windows.Forms.Label();
            this.ConnPool_Val = new System.Windows.Forms.NumericUpDown();
            this.ConnPool_Key = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.ReTry_Val)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectTimeOut_Val)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExecTime_Val)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WaitTime_Val)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SendBufferSize_Val)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SendTimeout_Val)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReceiveBufferSize_Val)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReceiveTimeout_Val)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConnPool_Val)).BeginInit();
            this.SuspendLayout();
            // 
            // ConfigName_Val
            // 
            this.ConfigName_Val.Location = new System.Drawing.Point(50, 6);
            this.ConfigName_Val.Name = "ConfigName_Val";
            this.ConfigName_Val.Size = new System.Drawing.Size(160, 21);
            this.ConfigName_Val.TabIndex = 122;
            // 
            // ConfigName_Key
            // 
            this.ConfigName_Key.AutoSize = true;
            this.ConfigName_Key.Location = new System.Drawing.Point(3, 9);
            this.ConfigName_Key.Name = "ConfigName_Key";
            this.ConfigName_Key.Size = new System.Drawing.Size(41, 12);
            this.ConfigName_Key.TabIndex = 124;
            this.ConfigName_Key.Text = "配置名";
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(295, 36);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(47, 16);
            this.radioButton5.TabIndex = 133;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "File";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(230, 36);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(59, 16);
            this.radioButton4.TabIndex = 132;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "USBSet";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(168, 36);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(65, 16);
            this.radioButton3.TabIndex = 131;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "UartSet";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(103, 36);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(59, 16);
            this.radioButton2.TabIndex = 130;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "NetSet";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(50, 36);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 16);
            this.radioButton1.TabIndex = 129;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "None";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // Mode_Key
            // 
            this.Mode_Key.AutoSize = true;
            this.Mode_Key.Location = new System.Drawing.Point(3, 36);
            this.Mode_Key.Name = "Mode_Key";
            this.Mode_Key.Size = new System.Drawing.Size(29, 12);
            this.Mode_Key.TabIndex = 128;
            this.Mode_Key.Text = "类型";
            // 
            // Charset_Val
            // 
            this.Charset_Val.Location = new System.Drawing.Point(248, 165);
            this.Charset_Val.Name = "Charset_Val";
            this.Charset_Val.Size = new System.Drawing.Size(121, 21);
            this.Charset_Val.TabIndex = 149;
            // 
            // Charset_Key
            // 
            this.Charset_Key.AutoSize = true;
            this.Charset_Key.Location = new System.Drawing.Point(189, 168);
            this.Charset_Key.Name = "Charset_Key";
            this.Charset_Key.Size = new System.Drawing.Size(29, 12);
            this.Charset_Key.TabIndex = 148;
            this.Charset_Key.Text = "编码";
            // 
            // ReTry_Val
            // 
            this.ReTry_Val.Location = new System.Drawing.Point(62, 61);
            this.ReTry_Val.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.ReTry_Val.Name = "ReTry_Val";
            this.ReTry_Val.Size = new System.Drawing.Size(121, 21);
            this.ReTry_Val.TabIndex = 151;
            // 
            // ReTry_Key
            // 
            this.ReTry_Key.AutoSize = true;
            this.ReTry_Key.Location = new System.Drawing.Point(3, 63);
            this.ReTry_Key.Name = "ReTry_Key";
            this.ReTry_Key.Size = new System.Drawing.Size(29, 12);
            this.ReTry_Key.TabIndex = 150;
            this.ReTry_Key.Text = "重试";
            // 
            // ConnectTimeOut_Val
            // 
            this.ConnectTimeOut_Val.Location = new System.Drawing.Point(248, 58);
            this.ConnectTimeOut_Val.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.ConnectTimeOut_Val.Name = "ConnectTimeOut_Val";
            this.ConnectTimeOut_Val.Size = new System.Drawing.Size(121, 21);
            this.ConnectTimeOut_Val.TabIndex = 153;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(189, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 152;
            this.label1.Text = "连接超时";
            // 
            // ExecTime_Val
            // 
            this.ExecTime_Val.Location = new System.Drawing.Point(248, 84);
            this.ExecTime_Val.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.ExecTime_Val.Name = "ExecTime_Val";
            this.ExecTime_Val.Size = new System.Drawing.Size(121, 21);
            this.ExecTime_Val.TabIndex = 157;
            // 
            // ExecTime_Key
            // 
            this.ExecTime_Key.AutoSize = true;
            this.ExecTime_Key.Location = new System.Drawing.Point(189, 86);
            this.ExecTime_Key.Name = "ExecTime_Key";
            this.ExecTime_Key.Size = new System.Drawing.Size(53, 12);
            this.ExecTime_Key.TabIndex = 156;
            this.ExecTime_Key.Text = "执行超时";
            // 
            // WaitTime_Val
            // 
            this.WaitTime_Val.Location = new System.Drawing.Point(62, 84);
            this.WaitTime_Val.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.WaitTime_Val.Name = "WaitTime_Val";
            this.WaitTime_Val.Size = new System.Drawing.Size(121, 21);
            this.WaitTime_Val.TabIndex = 155;
            // 
            // WaitTime_Key
            // 
            this.WaitTime_Key.AutoSize = true;
            this.WaitTime_Key.Location = new System.Drawing.Point(3, 86);
            this.WaitTime_Key.Name = "WaitTime_Key";
            this.WaitTime_Key.Size = new System.Drawing.Size(53, 12);
            this.WaitTime_Key.TabIndex = 154;
            this.WaitTime_Key.Text = "等待时间";
            // 
            // SendBufferSize_Val
            // 
            this.SendBufferSize_Val.Location = new System.Drawing.Point(248, 111);
            this.SendBufferSize_Val.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.SendBufferSize_Val.Name = "SendBufferSize_Val";
            this.SendBufferSize_Val.Size = new System.Drawing.Size(121, 21);
            this.SendBufferSize_Val.TabIndex = 161;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(189, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 160;
            this.label4.Text = "发送缓存";
            // 
            // SendTimeout_Val
            // 
            this.SendTimeout_Val.Location = new System.Drawing.Point(62, 111);
            this.SendTimeout_Val.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.SendTimeout_Val.Name = "SendTimeout_Val";
            this.SendTimeout_Val.Size = new System.Drawing.Size(121, 21);
            this.SendTimeout_Val.TabIndex = 159;
            // 
            // SendTimeout_Key
            // 
            this.SendTimeout_Key.AutoSize = true;
            this.SendTimeout_Key.Location = new System.Drawing.Point(3, 113);
            this.SendTimeout_Key.Name = "SendTimeout_Key";
            this.SendTimeout_Key.Size = new System.Drawing.Size(53, 12);
            this.SendTimeout_Key.TabIndex = 158;
            this.SendTimeout_Key.Text = "发送超时";
            // 
            // ReceiveBufferSize_Val
            // 
            this.ReceiveBufferSize_Val.Location = new System.Drawing.Point(248, 138);
            this.ReceiveBufferSize_Val.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.ReceiveBufferSize_Val.Name = "ReceiveBufferSize_Val";
            this.ReceiveBufferSize_Val.Size = new System.Drawing.Size(121, 21);
            this.ReceiveBufferSize_Val.TabIndex = 165;
            // 
            // ReceiveBufferSize_Key
            // 
            this.ReceiveBufferSize_Key.AutoSize = true;
            this.ReceiveBufferSize_Key.Location = new System.Drawing.Point(189, 140);
            this.ReceiveBufferSize_Key.Name = "ReceiveBufferSize_Key";
            this.ReceiveBufferSize_Key.Size = new System.Drawing.Size(53, 12);
            this.ReceiveBufferSize_Key.TabIndex = 164;
            this.ReceiveBufferSize_Key.Text = "接收缓存";
            // 
            // ReceiveTimeout_Val
            // 
            this.ReceiveTimeout_Val.Location = new System.Drawing.Point(62, 138);
            this.ReceiveTimeout_Val.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.ReceiveTimeout_Val.Name = "ReceiveTimeout_Val";
            this.ReceiveTimeout_Val.Size = new System.Drawing.Size(121, 21);
            this.ReceiveTimeout_Val.TabIndex = 163;
            // 
            // ReceiveTimeout_Key
            // 
            this.ReceiveTimeout_Key.AutoSize = true;
            this.ReceiveTimeout_Key.Location = new System.Drawing.Point(3, 140);
            this.ReceiveTimeout_Key.Name = "ReceiveTimeout_Key";
            this.ReceiveTimeout_Key.Size = new System.Drawing.Size(53, 12);
            this.ReceiveTimeout_Key.TabIndex = 162;
            this.ReceiveTimeout_Key.Text = "接收超时";
            // 
            // ConnPool_Val
            // 
            this.ConnPool_Val.Location = new System.Drawing.Point(62, 166);
            this.ConnPool_Val.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.ConnPool_Val.Name = "ConnPool_Val";
            this.ConnPool_Val.Size = new System.Drawing.Size(121, 21);
            this.ConnPool_Val.TabIndex = 167;
            // 
            // ConnPool_Key
            // 
            this.ConnPool_Key.AutoSize = true;
            this.ConnPool_Key.Location = new System.Drawing.Point(3, 168);
            this.ConnPool_Key.Name = "ConnPool_Key";
            this.ConnPool_Key.Size = new System.Drawing.Size(41, 12);
            this.ConnPool_Key.TabIndex = 166;
            this.ConnPool_Key.Text = "监听数";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 168;
            this.label2.Text = "调试模式";
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(274, 200);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(47, 16);
            this.radioButton6.TabIndex = 173;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "全部";
            this.radioButton6.UseVisualStyleBackColor = true;
            this.radioButton6.CheckedChanged += new System.EventHandler(this.radioButton6_CheckedChanged);
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new System.Drawing.Point(221, 200);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(47, 16);
            this.radioButton7.TabIndex = 172;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "测试";
            this.radioButton7.UseVisualStyleBackColor = true;
            this.radioButton7.CheckedChanged += new System.EventHandler(this.radioButton7_CheckedChanged);
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(168, 200);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(47, 16);
            this.radioButton8.TabIndex = 171;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "错误";
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.CheckedChanged += new System.EventHandler(this.radioButton8_CheckedChanged);
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Location = new System.Drawing.Point(115, 200);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(47, 16);
            this.radioButton9.TabIndex = 170;
            this.radioButton9.TabStop = true;
            this.radioButton9.Text = "信息";
            this.radioButton9.UseVisualStyleBackColor = true;
            this.radioButton9.CheckedChanged += new System.EventHandler(this.radioButton9_CheckedChanged);
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Location = new System.Drawing.Point(62, 200);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(47, 16);
            this.radioButton10.TabIndex = 169;
            this.radioButton10.TabStop = true;
            this.radioButton10.Text = "None";
            this.radioButton10.UseVisualStyleBackColor = true;
            this.radioButton10.CheckedChanged += new System.EventHandler(this.radioButton10_CheckedChanged);
            // 
            // ReferSetUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioButton6);
            this.Controls.Add(this.radioButton7);
            this.Controls.Add(this.radioButton8);
            this.Controls.Add(this.radioButton9);
            this.Controls.Add(this.radioButton10);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ConnPool_Val);
            this.Controls.Add(this.ConnPool_Key);
            this.Controls.Add(this.ReceiveBufferSize_Val);
            this.Controls.Add(this.ReceiveBufferSize_Key);
            this.Controls.Add(this.ReceiveTimeout_Val);
            this.Controls.Add(this.ReceiveTimeout_Key);
            this.Controls.Add(this.SendBufferSize_Val);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SendTimeout_Val);
            this.Controls.Add(this.SendTimeout_Key);
            this.Controls.Add(this.ExecTime_Val);
            this.Controls.Add(this.ExecTime_Key);
            this.Controls.Add(this.WaitTime_Val);
            this.Controls.Add(this.WaitTime_Key);
            this.Controls.Add(this.ConnectTimeOut_Val);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ReTry_Val);
            this.Controls.Add(this.ReTry_Key);
            this.Controls.Add(this.Charset_Val);
            this.Controls.Add(this.Charset_Key);
            this.Controls.Add(this.radioButton5);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.Mode_Key);
            this.Controls.Add(this.ConfigName_Val);
            this.Controls.Add(this.ConfigName_Key);
            this.Name = "ReferSetUI";
            this.Size = new System.Drawing.Size(375, 228);
            this.Load += new System.EventHandler(this.ReferSetUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ReTry_Val)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectTimeOut_Val)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExecTime_Val)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WaitTime_Val)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SendBufferSize_Val)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SendTimeout_Val)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReceiveBufferSize_Val)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReceiveTimeout_Val)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConnPool_Val)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox ConfigName_Val;
        private System.Windows.Forms.Label ConfigName_Key;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label Mode_Key;
        private System.Windows.Forms.TextBox Charset_Val;
        private System.Windows.Forms.Label Charset_Key;
        private System.Windows.Forms.NumericUpDown ReTry_Val;
        private System.Windows.Forms.Label ReTry_Key;
        private System.Windows.Forms.NumericUpDown ConnectTimeOut_Val;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ExecTime_Val;
        private System.Windows.Forms.Label ExecTime_Key;
        private System.Windows.Forms.NumericUpDown WaitTime_Val;
        private System.Windows.Forms.Label WaitTime_Key;
        private System.Windows.Forms.NumericUpDown SendBufferSize_Val;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown SendTimeout_Val;
        private System.Windows.Forms.Label SendTimeout_Key;
        private System.Windows.Forms.NumericUpDown ReceiveBufferSize_Val;
        private System.Windows.Forms.Label ReceiveBufferSize_Key;
        private System.Windows.Forms.NumericUpDown ReceiveTimeout_Val;
        private System.Windows.Forms.Label ReceiveTimeout_Key;
        private System.Windows.Forms.NumericUpDown ConnPool_Val;
        private System.Windows.Forms.Label ConnPool_Key;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.RadioButton radioButton10;
    }
}
