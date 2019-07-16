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
    public partial class LoginForm : Form
    {
        //定义一个数据上传的委托(上传到MainFrom界面)
        public delegate void MainFormMessage(string ip, string port, string name, string password);
        //定义数据上传事件(上传到MainFrom界面)
        public event MainFormMessage MainFormMessageEvent;

        public LoginForm()
        {
            InitializeComponent();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //读取上次输入参数的值
            this.textBox1.Text = config.AppSettings.Settings["IP"].Value.ToString();
            this.textBox2.Text = config.AppSettings.Settings["Port"].Value.ToString();
            this.textBox3.Text = config.AppSettings.Settings["UserName"].Value.ToString();
            this.textBox4.Text = config.AppSettings.Settings["PassWord"].Value.ToString();  

        }       

        //登录
        private void button1_Click(object sender, EventArgs e)
        {            
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //保存本次输入参数的值
            config.AppSettings.Settings["IP"].Value = this.textBox1.Text.Trim().ToString();//设备IP
            config.AppSettings.Settings["Port"].Value = this.textBox2.Text.Trim().ToString();//端口;
            config.AppSettings.Settings["UserName"].Value = this.textBox3.Text.Trim().ToString();//用户名
            config.AppSettings.Settings["PassWord"].Value = this.textBox4.Text.Trim().ToString();//密码
            config.Save();
            
            string ip, port, name, password;
            ip = textBox1.Text.Trim();
            port = textBox2.Text.Trim();
            name = textBox3.Text.Trim();
            password = textBox4.Text.Trim();
            MainFormMessageEvent(ip, port, name, password);            
        }

        //退出
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
