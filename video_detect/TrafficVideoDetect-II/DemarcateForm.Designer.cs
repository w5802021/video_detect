namespace LaboratoryTable
{
    partial class DemarcateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DemarcateForm));
            this.button_Detect = new System.Windows.Forms.Button();
            this.Button_demarcate_Save = new System.Windows.Forms.Button();
            this.pictureBox_BackGround = new System.Windows.Forms.PictureBox();
            this.button_Capture = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Button_demarcate_Clear = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_BackGround)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Detect
            // 
            this.button_Detect.Location = new System.Drawing.Point(23, 23);
            this.button_Detect.Name = "button_Detect";
            this.button_Detect.Size = new System.Drawing.Size(102, 23);
            this.button_Detect.TabIndex = 6;
            this.button_Detect.Text = "打开视频";
            this.button_Detect.UseVisualStyleBackColor = true;
            this.button_Detect.Click += new System.EventHandler(this.button_Detect_Click);
            // 
            // Button_demarcate_Save
            // 
            this.Button_demarcate_Save.Enabled = false;
            this.Button_demarcate_Save.Location = new System.Drawing.Point(23, 51);
            this.Button_demarcate_Save.Name = "Button_demarcate_Save";
            this.Button_demarcate_Save.Size = new System.Drawing.Size(102, 25);
            this.Button_demarcate_Save.TabIndex = 10;
            this.Button_demarcate_Save.Text = "保存标定";
            this.Button_demarcate_Save.UseVisualStyleBackColor = true;
            this.Button_demarcate_Save.Click += new System.EventHandler(this.Button_demarcate_Save_Click);
            // 
            // pictureBox_BackGround
            // 
            this.pictureBox_BackGround.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_BackGround.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_BackGround.Image")));
            this.pictureBox_BackGround.Location = new System.Drawing.Point(8, 20);
            this.pictureBox_BackGround.Name = "pictureBox_BackGround";
            this.pictureBox_BackGround.Size = new System.Drawing.Size(500, 400);
            this.pictureBox_BackGround.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_BackGround.TabIndex = 1;
            this.pictureBox_BackGround.TabStop = false;
            this.pictureBox_BackGround.Click += new System.EventHandler(this.pictureBox_BackGround_Click);
            this.pictureBox_BackGround.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_BackGround_Paint);
            this.pictureBox_BackGround.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_BackGround_MouseDown);
            this.pictureBox_BackGround.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_BackGround_MouseMove);
            this.pictureBox_BackGround.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_BackGround_MouseUp);
            // 
            // button_Capture
            // 
            this.button_Capture.Location = new System.Drawing.Point(23, 60);
            this.button_Capture.Name = "button_Capture";
            this.button_Capture.Size = new System.Drawing.Size(102, 23);
            this.button_Capture.TabIndex = 13;
            this.button_Capture.Text = "网络视频源";
            this.button_Capture.UseVisualStyleBackColor = true;
            this.button_Capture.Click += new System.EventHandler(this.button_Capture_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button_Detect);
            this.groupBox4.Controls.Add(this.button_Capture);
            this.groupBox4.Location = new System.Drawing.Point(84, 64);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(143, 97);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "模式选择";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button3);
            this.groupBox5.Controls.Add(this.button1);
            this.groupBox5.Location = new System.Drawing.Point(84, 170);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(143, 96);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "视频控制";
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(23, 23);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(102, 23);
            this.button3.TabIndex = 19;
            this.button3.Text = "下一帧";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(23, 60);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "背景帧提取";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Button_demarcate_Clear
            // 
            this.Button_demarcate_Clear.Enabled = false;
            this.Button_demarcate_Clear.Location = new System.Drawing.Point(23, 20);
            this.Button_demarcate_Clear.Name = "Button_demarcate_Clear";
            this.Button_demarcate_Clear.Size = new System.Drawing.Size(102, 25);
            this.Button_demarcate_Clear.TabIndex = 19;
            this.Button_demarcate_Clear.Text = "重新标定";
            this.Button_demarcate_Clear.UseVisualStyleBackColor = true;
            this.Button_demarcate_Clear.Click += new System.EventHandler(this.Button_demarcate_Clear_Click);
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(23, 23);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(102, 23);
            this.button4.TabIndex = 0;
            this.button4.Text = "1.虚拟线圈标定";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(23, 63);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(102, 23);
            this.button5.TabIndex = 1;
            this.button5.Text = "2.检测区域标定";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Enabled = false;
            this.button6.Location = new System.Drawing.Point(23, 103);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(102, 23);
            this.button6.TabIndex = 2;
            this.button6.Text = "3.补偿线段标定";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Location = new System.Drawing.Point(84, 283);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(143, 129);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "标定步骤";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pictureBox_BackGround);
            this.groupBox3.Location = new System.Drawing.Point(348, 65);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(518, 429);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "背景帧显示";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Button_demarcate_Clear);
            this.groupBox2.Controls.Add(this.Button_demarcate_Save);
            this.groupBox2.Location = new System.Drawing.Point(84, 418);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(143, 85);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "标定保存";
            // 
            // DemarcateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 638);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DemarcateForm";
            this.Text = "检测标定";
            this.Load += new System.EventHandler(this.DemarcateForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_BackGround)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Detect;
        private System.Windows.Forms.Button Button_demarcate_Save;
        private System.Windows.Forms.PictureBox pictureBox_BackGround;
        private System.Windows.Forms.Button button_Capture;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button Button_demarcate_Clear;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

