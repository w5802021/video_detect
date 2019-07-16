namespace LaboratoryTable
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip_Main = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Demarcate_Setting = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Video_Detect = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Parameter_Setting = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip_Main
            // 
            this.toolStrip_Main.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip_Main.AutoSize = false;
            this.toolStrip_Main.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Demarcate_Setting,
            this.toolStripButton_Video_Detect,
            this.toolStripButton_Parameter_Setting});
            this.toolStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.toolStrip_Main.Name = "toolStrip_Main";
            this.toolStrip_Main.Size = new System.Drawing.Size(1068, 77);
            this.toolStrip_Main.TabIndex = 2;
            this.toolStrip_Main.Text = "toolStrip1";
            // 
            // toolStripButton_Demarcate_Setting
            // 
            this.toolStripButton_Demarcate_Setting.AutoSize = false;
            this.toolStripButton_Demarcate_Setting.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripButton_Demarcate_Setting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripButton_Demarcate_Setting.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.toolStripButton_Demarcate_Setting.Image = global::LaboratoryTable.Properties.Resources.DemaracteSet;
            this.toolStripButton_Demarcate_Setting.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton_Demarcate_Setting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Demarcate_Setting.Name = "toolStripButton_Demarcate_Setting";
            this.toolStripButton_Demarcate_Setting.Size = new System.Drawing.Size(70, 70);
            this.toolStripButton_Demarcate_Setting.Text = "标定设置";
            this.toolStripButton_Demarcate_Setting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_Demarcate_Setting.Click += new System.EventHandler(this.toolStripButton_Demarcate_Setting_Click);
            // 
            // toolStripButton_Video_Detect
            // 
            this.toolStripButton_Video_Detect.AutoSize = false;
            this.toolStripButton_Video_Detect.Image = global::LaboratoryTable.Properties.Resources.Video;
            this.toolStripButton_Video_Detect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton_Video_Detect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Video_Detect.Name = "toolStripButton_Video_Detect";
            this.toolStripButton_Video_Detect.Size = new System.Drawing.Size(70, 70);
            this.toolStripButton_Video_Detect.Text = "视频检测";
            this.toolStripButton_Video_Detect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_Video_Detect.Click += new System.EventHandler(this.toolStripButton_Video_Detect_Click);
            // 
            // toolStripButton_Parameter_Setting
            // 
            this.toolStripButton_Parameter_Setting.AutoSize = false;
            this.toolStripButton_Parameter_Setting.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripButton_Parameter_Setting.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripButton_Parameter_Setting.Image = global::LaboratoryTable.Properties.Resources.ParameterSettings;
            this.toolStripButton_Parameter_Setting.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton_Parameter_Setting.ImageTransparentColor = System.Drawing.Color.Indigo;
            this.toolStripButton_Parameter_Setting.Name = "toolStripButton_Parameter_Setting";
            this.toolStripButton_Parameter_Setting.Size = new System.Drawing.Size(70, 70);
            this.toolStripButton_Parameter_Setting.Text = "参数设置";
            this.toolStripButton_Parameter_Setting.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.toolStripButton_Parameter_Setting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_Parameter_Setting.ToolTipText = "\r\n参数设置";
            this.toolStripButton_Parameter_Setting.Click += new System.EventHandler(this.toolStripButton_Parameter_Setting_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1068, 721);
            this.Controls.Add(this.toolStrip_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "交通视频检测系统";
            this.toolStrip_Main.ResumeLayout(false);
            this.toolStrip_Main.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip_Main;
        private System.Windows.Forms.ToolStripButton toolStripButton_Demarcate_Setting;
        private System.Windows.Forms.ToolStripButton toolStripButton_Video_Detect;
        private System.Windows.Forms.ToolStripButton toolStripButton_Parameter_Setting;


    }
}