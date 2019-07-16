namespace LaboratoryTable
{
    partial class ParameterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_save = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown_LanesWidth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_WhiteLineLength = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_WhiteLineWidth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_FPS = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_LanesWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_WhiteLineLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_WhiteLineWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FPS)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(408, 186);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "车道宽度：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(407, 231);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "补偿线长度：";
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(488, 372);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 4;
            this.button_save.Text = "保  存";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(407, 287);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "补偿线宽度：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(614, 290);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "(厘米)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(614, 235);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "(厘米)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(614, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "(厘米)";
            // 
            // numericUpDown_LanesWidth
            // 
            this.numericUpDown_LanesWidth.Location = new System.Drawing.Point(488, 184);
            this.numericUpDown_LanesWidth.Maximum = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this.numericUpDown_LanesWidth.Name = "numericUpDown_LanesWidth";
            this.numericUpDown_LanesWidth.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown_LanesWidth.TabIndex = 10;
            this.numericUpDown_LanesWidth.ValueChanged += new System.EventHandler(this.numericUpDown_LanesWidth_ValueChanged);
            // 
            // numericUpDown_WhiteLineLength
            // 
            this.numericUpDown_WhiteLineLength.Location = new System.Drawing.Point(488, 229);
            this.numericUpDown_WhiteLineLength.Maximum = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this.numericUpDown_WhiteLineLength.Name = "numericUpDown_WhiteLineLength";
            this.numericUpDown_WhiteLineLength.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown_WhiteLineLength.TabIndex = 11;
            // 
            // numericUpDown_WhiteLineWidth
            // 
            this.numericUpDown_WhiteLineWidth.Location = new System.Drawing.Point(488, 284);
            this.numericUpDown_WhiteLineWidth.Maximum = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this.numericUpDown_WhiteLineWidth.Name = "numericUpDown_WhiteLineWidth";
            this.numericUpDown_WhiteLineWidth.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown_WhiteLineWidth.TabIndex = 12;
            // 
            // numericUpDown_FPS
            // 
            this.numericUpDown_FPS.Location = new System.Drawing.Point(488, 332);
            this.numericUpDown_FPS.Maximum = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this.numericUpDown_FPS.Name = "numericUpDown_FPS";
            this.numericUpDown_FPS.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown_FPS.TabIndex = 13;
            this.numericUpDown_FPS.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDown_FPS.ValueChanged += new System.EventHandler(this.numericUpDown_FPS_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(407, 339);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "视频源帧率：";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(612, 338);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 15;
            this.label8.Text = "(帧/秒)";
            // 
            // ParameterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1068, 638);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numericUpDown_FPS);
            this.Controls.Add(this.numericUpDown_WhiteLineWidth);
            this.Controls.Add(this.numericUpDown_WhiteLineLength);
            this.Controls.Add(this.numericUpDown_LanesWidth);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ParameterForm";
            this.Text = "ParameterForm";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.Load += new System.EventHandler(this.ParameterForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_LanesWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_WhiteLineLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_WhiteLineWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FPS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown_LanesWidth;
        private System.Windows.Forms.NumericUpDown numericUpDown_WhiteLineLength;
        private System.Windows.Forms.NumericUpDown numericUpDown_WhiteLineWidth;
        private System.Windows.Forms.NumericUpDown numericUpDown_FPS;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}