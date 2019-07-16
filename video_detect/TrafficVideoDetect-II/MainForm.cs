using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaboratoryTable
{
    public partial class MainForm : Form
    {
        public static VideoPlayer videoplayer = new VideoPlayer();

        public MainForm()
        {
            InitializeComponent();
        }       

        private void toolStripButton_Demarcate_Setting_Click(object sender, EventArgs e)
        {
            this.toolStripButton_Demarcate_Setting.BackColor = SystemColors.ActiveCaption;//图标突出颜色显示
            this.toolStripButton_Video_Detect.BackColor = SystemColors.Control;
            this.toolStripButton_Parameter_Setting.BackColor = SystemColors.Control;
            //关闭网络视频
            videoplayer.CloseVideo();
            videoplayer.Close();           

            DemarcateForm form = new DemarcateForm(); 
            ChangeChildForm(form);
        }

        private void toolStripButton_Video_Detect_Click(object sender, EventArgs e)
        {
            this.toolStripButton_Demarcate_Setting.BackColor = SystemColors.Control;
            this.toolStripButton_Video_Detect.BackColor = SystemColors.ActiveCaption;//图标突出颜色显示
            this.toolStripButton_Parameter_Setting.BackColor = SystemColors.Control;
            //关闭网络视频           
            videoplayer.CloseVideo();
            videoplayer.Close();            

            VideoDetectForm form = new VideoDetectForm();           
            ChangeChildForm(form);
        }

        private void ChangeChildForm(Form ChildForm)
        {
            ChildForm.MdiParent = this;
            //ChildForm.Size = new Size(this.ClientSize.Width - this.toolStrip_Main.Width - 5, this.ClientSize.Height - 5); //自定义调整子窗体大小，不能最大化，最大化后标题栏到顶端了
            ChildForm.Size = new Size(this.ClientSize.Width - 5, this.ClientSize.Height - this.toolStrip_Main.Height - 5); //自定义调整子窗体大小，不能最大化，最大化后标题栏到顶端了
            ChildForm.Location = new Point(0, this.toolStrip_Main.Height);
            ChildForm.StartPosition = FormStartPosition.Manual;
            ChildForm.Show();
            foreach (Form tempForm in this.MdiChildren)
            {
                if (tempForm == ChildForm)
                {
                    tempForm.Activate();
                }
                else
                {
                    tempForm.Close();
                    ChildForm.Show();
                }
            }
        }


        private void toolStripButton_Parameter_Setting_Click(object sender, EventArgs e)
        {
            this.toolStripButton_Demarcate_Setting.BackColor = SystemColors.Control;
            this.toolStripButton_Video_Detect.BackColor = SystemColors.Control;
            this.toolStripButton_Parameter_Setting.BackColor = SystemColors.ActiveCaption;//图标突出颜色显示    
            //关闭网络视频
            videoplayer.CloseVideo();
            videoplayer.Close();            

            ParameterForm form = new ParameterForm();            
            ChangeChildForm(form);
        }


    }
}
