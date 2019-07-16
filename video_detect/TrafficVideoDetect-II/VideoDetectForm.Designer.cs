namespace LaboratoryTable
{
    partial class VideoDetectForm
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
            this.components = new System.ComponentModel.Container();
            this.listView_Vehicle_Info = new System.Windows.Forms.ListView();
            this.ColumnHeader_Id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Speed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_DetectTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader_Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_ExportToExcel = new System.Windows.Forms.Button();
            this.button_Detect = new System.Windows.Forms.Button();
            this.timer_Video = new System.Windows.Forms.Timer(this.components);
            this.textBox_Path = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Button_Demarcate_Path_Select = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button_Camera_Open = new System.Windows.Forms.Button();
            this.Button_VideoFile_Open = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RadioButton__VirtualCoil_Hide = new System.Windows.Forms.RadioButton();
            this.RadioButton_VirtualCoil_Show = new System.Windows.Forms.RadioButton();
            this.Button_Detect_Stop = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.button_Detect_Close = new System.Windows.Forms.Button();
            this.timer_Camera = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView_Vehicle_Info
            // 
            this.listView_Vehicle_Info.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader_Id,
            this.columnHeader_Speed,
            this.columnHeader_DetectTime,
            this.ColumnHeader_Type});
            this.listView_Vehicle_Info.FullRowSelect = true;
            this.listView_Vehicle_Info.Location = new System.Drawing.Point(6, 20);
            this.listView_Vehicle_Info.Name = "listView_Vehicle_Info";
            this.listView_Vehicle_Info.Size = new System.Drawing.Size(349, 401);
            this.listView_Vehicle_Info.TabIndex = 3;
            this.listView_Vehicle_Info.UseCompatibleStateImageBehavior = false;
            this.listView_Vehicle_Info.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader_Id
            // 
            this.ColumnHeader_Id.Text = "车流序号";
            this.ColumnHeader_Id.Width = 100;
            // 
            // columnHeader_Speed
            // 
            this.columnHeader_Speed.DisplayIndex = 2;
            this.columnHeader_Speed.Text = "车速（km/h）";
            this.columnHeader_Speed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader_Speed.Width = 85;
            // 
            // columnHeader_DetectTime
            // 
            this.columnHeader_DetectTime.DisplayIndex = 3;
            this.columnHeader_DetectTime.Text = "检测时间";
            this.columnHeader_DetectTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader_DetectTime.Width = 150;
            // 
            // ColumnHeader_Type
            // 
            this.ColumnHeader_Type.DisplayIndex = 1;
            this.ColumnHeader_Type.Text = "车型";
            this.ColumnHeader_Type.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader_Type.Width = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView_Vehicle_Info);
            this.groupBox1.Location = new System.Drawing.Point(685, 102);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(372, 439);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "实时检测信息";
            // 
            // button_ExportToExcel
            // 
            this.button_ExportToExcel.Location = new System.Drawing.Point(981, 566);
            this.button_ExportToExcel.Name = "button_ExportToExcel";
            this.button_ExportToExcel.Size = new System.Drawing.Size(75, 23);
            this.button_ExportToExcel.TabIndex = 31;
            this.button_ExportToExcel.Text = "数据导出";
            this.button_ExportToExcel.UseVisualStyleBackColor = true;
            this.button_ExportToExcel.Visible = false;
            this.button_ExportToExcel.Click += new System.EventHandler(this.button_ExportToExcel_Click);
            // 
            // button_Detect
            // 
            this.button_Detect.Location = new System.Drawing.Point(11, 29);
            this.button_Detect.Name = "button_Detect";
            this.button_Detect.Size = new System.Drawing.Size(90, 23);
            this.button_Detect.TabIndex = 6;
            this.button_Detect.Text = "开始检测";
            this.button_Detect.UseVisualStyleBackColor = true;
            this.button_Detect.Click += new System.EventHandler(this.button_Detect_Click);
            // 
            // timer_Video
            // 
            this.timer_Video.Interval = 32;
            this.timer_Video.Tick += new System.EventHandler(this.timer_Video_Tick);
            // 
            // textBox_Path
            // 
            this.textBox_Path.Location = new System.Drawing.Point(77, 14);
            this.textBox_Path.Name = "textBox_Path";
            this.textBox_Path.Size = new System.Drawing.Size(457, 21);
            this.textBox_Path.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "标定路径：";
            // 
            // Button_Demarcate_Path_Select
            // 
            this.Button_Demarcate_Path_Select.Location = new System.Drawing.Point(556, 12);
            this.Button_Demarcate_Path_Select.Name = "Button_Demarcate_Path_Select";
            this.Button_Demarcate_Path_Select.Size = new System.Drawing.Size(75, 23);
            this.Button_Demarcate_Path_Select.TabIndex = 19;
            this.Button_Demarcate_Path_Select.Text = "浏览";
            this.Button_Demarcate_Path_Select.UseVisualStyleBackColor = true;
            this.Button_Demarcate_Path_Select.Click += new System.EventHandler(this.Button_Demarcate_Path_Select_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pictureBox1);
            this.groupBox3.Location = new System.Drawing.Point(156, 102);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(523, 439);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "实时视频流显示";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = global::LaboratoryTable.Properties.Resources.VideoDetecForm;
            this.pictureBox1.Location = new System.Drawing.Point(9, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(500, 400);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.Button_Demarcate_Path_Select);
            this.groupBox4.Controls.Add(this.textBox_Path);
            this.groupBox4.Location = new System.Drawing.Point(23, 36);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(656, 45);
            this.groupBox4.TabIndex = 23;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "标定参数选择";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button_Camera_Open);
            this.groupBox5.Controls.Add(this.Button_VideoFile_Open);
            this.groupBox5.Location = new System.Drawing.Point(23, 120);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(112, 102);
            this.groupBox5.TabIndex = 24;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "模式选择";
            // 
            // button_Camera_Open
            // 
            this.button_Camera_Open.Location = new System.Drawing.Point(11, 67);
            this.button_Camera_Open.Name = "button_Camera_Open";
            this.button_Camera_Open.Size = new System.Drawing.Size(90, 23);
            this.button_Camera_Open.TabIndex = 7;
            this.button_Camera_Open.Text = "网络视频源";
            this.button_Camera_Open.UseVisualStyleBackColor = true;
            this.button_Camera_Open.Click += new System.EventHandler(this.button_Camera_Open_Click);
            // 
            // Button_VideoFile_Open
            // 
            this.Button_VideoFile_Open.Location = new System.Drawing.Point(11, 24);
            this.Button_VideoFile_Open.Name = "Button_VideoFile_Open";
            this.Button_VideoFile_Open.Size = new System.Drawing.Size(90, 23);
            this.Button_VideoFile_Open.TabIndex = 6;
            this.Button_VideoFile_Open.Text = "打开视频";
            this.Button_VideoFile_Open.UseVisualStyleBackColor = true;
            this.Button_VideoFile_Open.Click += new System.EventHandler(this.Button_VideoFile_Open_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RadioButton__VirtualCoil_Hide);
            this.groupBox2.Controls.Add(this.RadioButton_VirtualCoil_Show);
            this.groupBox2.Location = new System.Drawing.Point(21, 428);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(114, 95);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "虚拟线圈";
            // 
            // RadioButton__VirtualCoil_Hide
            // 
            this.RadioButton__VirtualCoil_Hide.AutoSize = true;
            this.RadioButton__VirtualCoil_Hide.Location = new System.Drawing.Point(11, 65);
            this.RadioButton__VirtualCoil_Hide.Name = "RadioButton__VirtualCoil_Hide";
            this.RadioButton__VirtualCoil_Hide.Size = new System.Drawing.Size(95, 16);
            this.RadioButton__VirtualCoil_Hide.TabIndex = 28;
            this.RadioButton__VirtualCoil_Hide.TabStop = true;
            this.RadioButton__VirtualCoil_Hide.Text = "隐藏虚拟线圈";
            this.RadioButton__VirtualCoil_Hide.UseVisualStyleBackColor = true;
            this.RadioButton__VirtualCoil_Hide.Click += new System.EventHandler(this.RadioButton__VirtualCoil_Hide_Click);
            // 
            // RadioButton_VirtualCoil_Show
            // 
            this.RadioButton_VirtualCoil_Show.AutoSize = true;
            this.RadioButton_VirtualCoil_Show.Location = new System.Drawing.Point(11, 29);
            this.RadioButton_VirtualCoil_Show.Name = "RadioButton_VirtualCoil_Show";
            this.RadioButton_VirtualCoil_Show.Size = new System.Drawing.Size(95, 16);
            this.RadioButton_VirtualCoil_Show.TabIndex = 27;
            this.RadioButton_VirtualCoil_Show.TabStop = true;
            this.RadioButton_VirtualCoil_Show.Text = "显示虚拟线圈";
            this.RadioButton_VirtualCoil_Show.UseVisualStyleBackColor = true;
            this.RadioButton_VirtualCoil_Show.Click += new System.EventHandler(this.RadioButton_VirtualCoil_Show_Click);
            // 
            // Button_Detect_Stop
            // 
            this.Button_Detect_Stop.Location = new System.Drawing.Point(11, 70);
            this.Button_Detect_Stop.Name = "Button_Detect_Stop";
            this.Button_Detect_Stop.Size = new System.Drawing.Size(90, 23);
            this.Button_Detect_Stop.TabIndex = 28;
            this.Button_Detect_Stop.Text = "暂停检测";
            this.Button_Detect_Stop.UseVisualStyleBackColor = true;
            this.Button_Detect_Stop.Click += new System.EventHandler(this.Button_Detect_Stop_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.button_Detect_Close);
            this.groupBox6.Controls.Add(this.button_Detect);
            this.groupBox6.Controls.Add(this.Button_Detect_Stop);
            this.groupBox6.Location = new System.Drawing.Point(23, 255);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(112, 150);
            this.groupBox6.TabIndex = 29;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "检测控制";
            // 
            // button_Detect_Close
            // 
            this.button_Detect_Close.Location = new System.Drawing.Point(11, 111);
            this.button_Detect_Close.Name = "button_Detect_Close";
            this.button_Detect_Close.Size = new System.Drawing.Size(90, 23);
            this.button_Detect_Close.TabIndex = 29;
            this.button_Detect_Close.Text = "关闭视频";
            this.button_Detect_Close.UseVisualStyleBackColor = true;
            this.button_Detect_Close.Click += new System.EventHandler(this.button_Detect_Close_Click);
            // 
            // timer_Camera
            // 
            this.timer_Camera.Interval = 40;
            this.timer_Camera.Tick += new System.EventHandler(this.timer_Camera_Tick);
            // 
            // VideoDetectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 638);
            this.Controls.Add(this.button_ExportToExcel);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VideoDetectForm";
            this.Text = "视频检测";
            this.Load += new System.EventHandler(this.VideoDetectForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListView listView_Vehicle_Info;
        private System.Windows.Forms.ColumnHeader ColumnHeader_Id;
        private System.Windows.Forms.ColumnHeader ColumnHeader_Type;
        private System.Windows.Forms.ColumnHeader columnHeader_Speed;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_Detect;
        private System.Windows.Forms.Timer timer_Video;
        private System.Windows.Forms.TextBox textBox_Path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Button_Demarcate_Path_Select;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button Button_VideoFile_Open;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton RadioButton__VirtualCoil_Hide;
        private System.Windows.Forms.RadioButton RadioButton_VirtualCoil_Show;
        private System.Windows.Forms.Button Button_Detect_Stop;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button button_Camera_Open;
        private System.Windows.Forms.Button button_Detect_Close;
        private System.Windows.Forms.Button button_ExportToExcel;
        private System.Windows.Forms.ColumnHeader columnHeader_DetectTime;
        private System.Windows.Forms.Timer timer_Camera;
    }
}

