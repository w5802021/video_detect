using System;
using System.ComponentModel;
using System.Windows.Forms;
using Declarations;
using Declarations.Events;
using Declarations.Media;
using Declarations.Players;
using Implementation;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV;

namespace LaboratoryTable
{
    public partial class VideoPlayer : Form
    {
        IMediaPlayerFactory m_factory;
        IVideoPlayer m_player;
        IMedia m_media;
        BitmapFormat format;
        NewFrameEventHandler callback;

        //VideoDetectForm videoDetectform = new VideoDetectForm();        
        
        public VideoPlayer()
        {
            InitializeComponent();

            m_factory = new MediaPlayerFactory();
            m_player = m_factory.CreatePlayer<IVideoPlayer>();
            m_player.WindowHandle = panel1.Handle;            
        }

        //播放视频
        public void StartVideo(string name, string password, string Ip)
        {
            try
            {                
                string Url = "rtsp://" + name + ":" + password + "@" + Ip;
                m_media = m_factory.CreateMedia<IMedia>(Url);
                m_player.Open(m_media);
                format = new BitmapFormat(panel1.Width, panel1.Height, ChromaType.RV32);
                m_player.CustomRenderer.SetFormat(format);
                callback = new NewFrameEventHandler(display);
                m_player.CustomRenderer.SetCallback(callback);
                m_player.Play();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //打开
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                m_media = m_factory.CreateMedia<IMedia>(textBox1.Text);
                m_player.Open(m_media);
                format = new BitmapFormat(panel1.Width, panel1.Height, ChromaType.RV32);
                m_player.CustomRenderer.SetFormat(format);
                callback = new NewFrameEventHandler(display);
                m_player.CustomRenderer.SetCallback(callback);
                m_player.Play();
                button2.Text = "暂停";
                btnSnapShot.Enabled = true;
                btnOpen.Visible = false;
            }
            else
            {
                errorProvider1.SetError(textBox1, "Please input media path first !");
            }

        }

        void display(Bitmap bitm)
        {
            Graphics g = panel1.CreateGraphics();
            Rectangle rct = new Rectangle(panel1.AutoScrollPosition.X, panel1.AutoScrollPosition.Y, bitm.Width, bitm.Height);

            //Rectangle rt = new Rectangle(0, 0, bitm.Width, bitm.Height);
            //Graphics b = Graphics.FromImage(bitm);
            //b.DrawString("2016", new Font("宋体", 9), Brushes.Yellow, rt);

            Image<Bgr, Byte> bgImage = new Image<Bgr, Byte>(bitm);
            if (VideoDetectForm.isShowVirtualCoil)
            {
                for (int i = 0; i < VideoDetectForm.virtualCoilList.Count; i++)
                    bgImage.Draw(VideoDetectForm.virtualCoilList[i], new Bgr(Color.Red), 1);
            }
            g.DrawImage(bgImage.ToBitmap(), rct);
            g.Dispose();            
        }

        void Events_ParsedChanged(object sender, MediaParseChange e)
        {
            Console.WriteLine(e.Parsed);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            m_player.Stop();
            btnSnapShot.Enabled = false;
            btnOpen.Visible = true;
        }

        public void CloseVideo()
        {
            if (m_player.IsPlaying)
            {
                m_player.Stop();
            }           

        }

        //播放/暂停
        private void button2_Click(object sender, EventArgs e)
        {
            if (m_player.IsPlaying)
            {
                m_player.Pause();
                button2.Text = "播放";
            }
            else
            {
                m_player.Play();
                button2.Text = "暂停";
            }
        }

        public bool IsPlaying()
        {
            System.Threading.Thread.Sleep(700);
            bool isplay = m_player.IsPlaying;
            return isplay;           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }

        //截图
        private void btnSnapShot_Click(object sender, EventArgs e)
        {
            string path = @"\\Nsjt-cpp\违法管理\证据源\";
            m_player.TakeSnapShot(0, path);//保存当前桢图像
            //Bitmap bitFormImg = m_player.CustomRenderer.CurrentFrame;//获取当前桢图像
            //bitFormImg.Save("1.jpg");
        }

        public Bitmap SnapPicture()
        {
            //string path = "c:\\ProImg.bmp";
            //m_player.TakeSnapShot(0, path);//保存当前桢图像
           
            Bitmap bitFormImg = m_player.CustomRenderer.CurrentFrame;//获取当前桢图像
            return bitFormImg;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }     

        
    }
}
