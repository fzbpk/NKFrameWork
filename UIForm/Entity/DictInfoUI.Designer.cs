namespace UIForm.Entity
{
    partial class DictInfoUI
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
            this.DictName_Val = new System.Windows.Forms.TextBox();
            this.DictName_Key = new System.Windows.Forms.Label();
            this.DictKey_Val = new System.Windows.Forms.TextBox();
            this.DictKey_Key = new System.Windows.Forms.Label();
            this.DictDisp_Val = new System.Windows.Forms.TextBox();
            this.DictDisp_Key = new System.Windows.Forms.Label();
            this.DictValue_Val = new System.Windows.Forms.TextBox();
            this.DictValue_Key = new System.Windows.Forms.Label();
            this.PX_Val = new System.Windows.Forms.NumericUpDown();
            this.PX_Key = new System.Windows.Forms.Label();
            this.CEnabled = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.PX_Val)).BeginInit();
            this.SuspendLayout();
            // 
            // DictName_Val
            // 
            this.DictName_Val.Location = new System.Drawing.Point(62, 6);
            this.DictName_Val.Name = "DictName_Val";
            this.DictName_Val.Size = new System.Drawing.Size(160, 21);
            this.DictName_Val.TabIndex = 122;
            // 
            // DictName_Key
            // 
            this.DictName_Key.AutoSize = true;
            this.DictName_Key.Location = new System.Drawing.Point(3, 9);
            this.DictName_Key.Name = "DictName_Key";
            this.DictName_Key.Size = new System.Drawing.Size(53, 12);
            this.DictName_Key.TabIndex = 123;
            this.DictName_Key.Text = "字典类型";
            // 
            // DictKey_Val
            // 
            this.DictKey_Val.Location = new System.Drawing.Point(62, 33);
            this.DictKey_Val.Name = "DictKey_Val";
            this.DictKey_Val.Size = new System.Drawing.Size(160, 21);
            this.DictKey_Val.TabIndex = 124;
            // 
            // DictKey_Key
            // 
            this.DictKey_Key.AutoSize = true;
            this.DictKey_Key.Location = new System.Drawing.Point(3, 36);
            this.DictKey_Key.Name = "DictKey_Key";
            this.DictKey_Key.Size = new System.Drawing.Size(53, 12);
            this.DictKey_Key.TabIndex = 125;
            this.DictKey_Key.Text = "字典键名";
            // 
            // DictDisp_Val
            // 
            this.DictDisp_Val.Location = new System.Drawing.Point(62, 59);
            this.DictDisp_Val.Name = "DictDisp_Val";
            this.DictDisp_Val.Size = new System.Drawing.Size(160, 21);
            this.DictDisp_Val.TabIndex = 126;
            // 
            // DictDisp_Key
            // 
            this.DictDisp_Key.AutoSize = true;
            this.DictDisp_Key.Location = new System.Drawing.Point(3, 62);
            this.DictDisp_Key.Name = "DictDisp_Key";
            this.DictDisp_Key.Size = new System.Drawing.Size(41, 12);
            this.DictDisp_Key.TabIndex = 127;
            this.DictDisp_Key.Text = "显示值";
            // 
            // DictValue_Val
            // 
            this.DictValue_Val.Location = new System.Drawing.Point(62, 86);
            this.DictValue_Val.Name = "DictValue_Val";
            this.DictValue_Val.Size = new System.Drawing.Size(160, 21);
            this.DictValue_Val.TabIndex = 128;
            // 
            // DictValue_Key
            // 
            this.DictValue_Key.AutoSize = true;
            this.DictValue_Key.Location = new System.Drawing.Point(3, 89);
            this.DictValue_Key.Name = "DictValue_Key";
            this.DictValue_Key.Size = new System.Drawing.Size(41, 12);
            this.DictValue_Key.TabIndex = 129;
            this.DictValue_Key.Text = "字典值";
            // 
            // PX_Val
            // 
            this.PX_Val.Location = new System.Drawing.Point(62, 113);
            this.PX_Val.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.PX_Val.Name = "PX_Val";
            this.PX_Val.Size = new System.Drawing.Size(83, 21);
            this.PX_Val.TabIndex = 147;
            // 
            // PX_Key
            // 
            this.PX_Key.AutoSize = true;
            this.PX_Key.Location = new System.Drawing.Point(3, 115);
            this.PX_Key.Name = "PX_Key";
            this.PX_Key.Size = new System.Drawing.Size(29, 12);
            this.PX_Key.TabIndex = 146;
            this.PX_Key.Text = "排序";
            // 
            // CEnabled
            // 
            this.CEnabled.AutoSize = true;
            this.CEnabled.Location = new System.Drawing.Point(156, 114);
            this.CEnabled.Name = "CEnabled";
            this.CEnabled.Size = new System.Drawing.Size(66, 16);
            this.CEnabled.TabIndex = 148;
            this.CEnabled.Text = "Enabled";
            this.CEnabled.UseVisualStyleBackColor = true;
            // 
            // DictInfoUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CEnabled);
            this.Controls.Add(this.PX_Val);
            this.Controls.Add(this.PX_Key);
            this.Controls.Add(this.DictValue_Val);
            this.Controls.Add(this.DictValue_Key);
            this.Controls.Add(this.DictDisp_Val);
            this.Controls.Add(this.DictDisp_Key);
            this.Controls.Add(this.DictKey_Val);
            this.Controls.Add(this.DictKey_Key);
            this.Controls.Add(this.DictName_Val);
            this.Controls.Add(this.DictName_Key);
            this.Name = "DictInfoUI";
            this.Size = new System.Drawing.Size(233, 139);
            this.Load += new System.EventHandler(this.DictInfoUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PX_Val)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DictName_Val;
        private System.Windows.Forms.Label DictName_Key;
        private System.Windows.Forms.TextBox DictKey_Val;
        private System.Windows.Forms.Label DictKey_Key;
        private System.Windows.Forms.TextBox DictDisp_Val;
        private System.Windows.Forms.Label DictDisp_Key;
        private System.Windows.Forms.TextBox DictValue_Val;
        private System.Windows.Forms.Label DictValue_Key;
        private System.Windows.Forms.NumericUpDown PX_Val;
        private System.Windows.Forms.Label PX_Key;
        private System.Windows.Forms.CheckBox CEnabled;
    }
}
