using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace LaboratoryTable
{
    public partial class ParameterForm : Form
    {
        public ParameterForm()
        {
            InitializeComponent();
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //读取上次输入参数的值
            this.numericUpDown_LanesWidth.Value = Convert.ToDecimal(config.AppSettings.Settings["LanesWidth"].Value);
            this.numericUpDown_WhiteLineLength.Value = Convert.ToDecimal(config.AppSettings.Settings["WhiteLineLength"].Value);
            this.numericUpDown_WhiteLineWidth.Value = Convert.ToDecimal(config.AppSettings.Settings["WhiteLineWidth"].Value);
            this.numericUpDown_FPS.Value = Convert.ToDecimal(config.AppSettings.Settings["fps"].Value);           
          
        }

        private void ParameterForm_Load(object sender, EventArgs e)
        {

        }      

        private void LanesNumber_TextChanged(object sender, EventArgs e)
        {

        }         

        private void button_save_Click(object sender, EventArgs e)
        {
            string[] str = new string[4];
            str[0] = this.numericUpDown_LanesWidth.Value.ToString();
            str[1] = this.numericUpDown_WhiteLineLength.Value .ToString();
            str[2] = this.numericUpDown_WhiteLineWidth.Value.ToString();
            str[3] = this.numericUpDown_FPS.Value.ToString();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //保存本次输入参数的值
            config.AppSettings.Settings["LanesWidth"].Value = str[0];
            config.AppSettings.Settings["WhiteLineLength"].Value = str[1];
            config.AppSettings.Settings["WhiteLineWidth"].Value = str[2];
            config.AppSettings.Settings["fps"].Value = str[3];
            config.Save();

            MessageBox.Show("检测参数配置成功！","提示");
        }


        private void LineLength_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown_LanesWidth_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown_FPS_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }



    }
}
