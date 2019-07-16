using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using System.Configuration;
using Utility;
using Microsoft.Office.Interop.Excel;
using UI;
using System.Threading;
using Declarations.Players;
using Declarations;
using Declarations.Media;
using Implementation;

namespace LaboratoryTable
{
    public partial class VideoDetectForm : Form
    {
        LoginForm login = new LoginForm();//登录验证
        //VideoPlayer videoplayer = new VideoPlayer();
        bool bVideoType = true;//ture为网络视频,flase为本地视频   

        Capture capture;

        //虚拟线圈列表
        public static List<System.Drawing.Rectangle> virtualCoilList = new List<System.Drawing.Rectangle>();

        //当前第N帧
        int currentFrameN = 0;

        //帧率
        int fps = 0;


        //是否在视频显示虚拟线圈
        public static bool isShowVirtualCoil = false;

        //车道数量
        int laneNum = 0;

        #region 标定参数变量

        //虚拟线圈参数
        int[] detect_1;
        //检测区域参数
        int[] detect_2;
        //补偿参数
        int[] detect_3;

        #endregion

        //视频文件路径
        string videoFileName;

        //excel保存路径
        string excelFilePath = "D:\\1.xls";

        bool isDetect = false;

        #region 海康威视网络视频变量

        //是否网络视频源
        bool isNetVideoType = false;

        IMediaPlayerFactory m_factory;
        IVideoPlayer m_player;
        IMedia m_media;
        BitmapFormat format;
        NewFrameEventHandler callback;

        #endregion

        #region 检测配置

        bool isHighway = false;

        #endregion

        int[] laneVechileNum = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };




        public VideoDetectForm()
        {
            InitializeComponent();

            #region 界面设置

            this.Button_VideoFile_Open.Enabled = true;
            this.button_Camera_Open.Enabled = true;
            this.button_Detect.Enabled = false;
            this.Button_Detect_Stop.Enabled = false;
            this.button_Detect_Close.Enabled = false;

            #endregion

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string fileName = config.AppSettings.Settings["Demarcate_Path"].Value.ToString();
            textBox_Path.Text = fileName;

        }

        private void VideoDetectForm_Load(object sender, EventArgs e)
        {

            if (MainForm.videoplayer == null)
            {
                MainForm.videoplayer = new VideoPlayer();
            }

            this.RadioButton_VirtualCoil_Show.Checked = true;
            isShowVirtualCoil = true;
        }

        #region 定时处理帧事件

        private void timer_Video_Tick(object sender, EventArgs e)
        {
            currentFrameN++;

            Image<Bgr, Byte> frame = capture.QueryFrame();

            if (frame == null)
            {
                timer_Video.Enabled = false;
                return;
            }

            Image<Bgr, Byte> frame2 = frame.Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_NN);//frame.Copy();
            IntPtr ptr = frame2.MIplImage.imageData;
            string data = ptr.ToString();

            int width = frame2.MIplImage.width;
            int height = frame2.MIplImage.height;

            if (isShowVirtualCoil)
            {
                for (int i = 0; i < virtualCoilList.Count; i++)
                    frame2.Draw(virtualCoilList[i], new Bgr(Color.Red), 1);
            }

            pictureBox1.Image = frame2.Bitmap;


            int flag2 = 0;
            try
            {
                flag2 = ExternalCall.Detect(ptr, width, height, currentFrameN, laneNum, fps);
            }
            catch (Exception)
            {

                throw;
            }
            #region  检测车道，待完成

            /*
            if (flag2 == 1)
            {
                IntPtr laneVechileNumPtr=ExternalCall.GetVechileInfo();
                Marshal.Copy(laneVechileNumPtr, laneVechileNum, 0, laneNum*3);

                int count = laneVechileNum.Count() / 3;

                for (int i = 0; i < count; i++)
                {
                    
                    int laneNo = laneVechileNum[0 + i * laneNum];
                    int speed=laneVechileNum[1+i*laneNum];
                    int typeint = laneVechileNum[1 + i * laneNum];

                    if (laneNo <= 0)
                        continue;

                    string speedStr = "";
                    if (speed == 0)
                        speedStr = "--";
                    else
                        speedStr = Convert.ToString(speed);

                    string type = "";

                    if (typeint == 0)
                        type = "小型车";
                    else if (typeint == 1)
                        type = "中型车";
                    else if (typeint == 2)
                        type = "大型车";
                    else
                        type = "--";

                    ListViewItem lvi = new ListViewItem();

                    int itemCount = listView_Vehicle_Info.Items.Count;

                    lvi.Text = Convert.ToString(itemCount + 1);

                    lvi.SubItems.Add(type);

                    lvi.SubItems.Add(speedStr);

                    this.listView_Vehicle_Info.Items.Add(lvi);

                    this.listView_Vehicle_Info.EndUpdate();

                }
            }
            */

            #endregion

            if (flag2 == 1)
            {

                int vehicleNum = ExternalCall.GetVechileCount();

                double speed = ExternalCall.GetVechileSpeed();

              

                string speedStr = "";
                if (speed == 0)
                    speedStr = "--";
                else
                    speedStr = Convert.ToString(Math.Round(speed, 0));

                string type = "";
                int typeint = ExternalCall.GetVechileType();
                if (typeint == 0)
                    type = "小型车";
                else if (typeint == 1)
                    type = "中型车";
                else if (typeint == 2)
                    type = "大型车";
                else
                    type = "--";

                ListViewItem lvi = new ListViewItem();

                int itemCount = listView_Vehicle_Info.Items.Count;

                lvi.Text = Convert.ToString(itemCount + 1);

                //lvi.SubItems.Add(type);

                lvi.SubItems.Add(speedStr);

                if (bVideoType)//是网络视频时，输出检测时间
                    lvi.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                else
                    lvi.SubItems.Add("--");



                this.listView_Vehicle_Info.Items.Add(lvi);

                this.listView_Vehicle_Info.EndUpdate();

                //IntPtr laneVechileNumPtr=ExternalCall.GetVechileNum4Lanes();
                //Marshal.Copy(laneVechileNumPtr, laneVechileNum, 0, laneNum); 

                //for(int i=0;i<laneNum;i++)
                //{
                //    ListViewItem lv = new ListViewItem();
                //    lv.SubItems.Add(Convert.ToString(i+1));
                //    lv.SubItems.Add(laneVechileNum[0].ToString());
                //    lv.SubItems.Add(laneVechileNum[1].ToString());
                //    lv.SubItems.Add(laneVechileNum[2].ToString());

                //    ListView_VechileType.Items[i] = lv;
                //}

            }


        }

        private void timer_Camera_Tick(object sender, EventArgs e)
        {
            currentFrameN++;

            Bitmap bitm = MainForm.videoplayer.SnapPicture();

            Image<Bgr, Byte> frame = new Image<Bgr, Byte>(bitm);

            if (frame == null)
            {
                timer_Video.Enabled = false;
                return;
            }

            Image<Bgr, Byte> frame2 = frame.Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_NN);//frame.Copy();
            IntPtr ptr = frame2.MIplImage.imageData;
            string data = ptr.ToString();

            int width = frame2.MIplImage.width;
            int height = frame2.MIplImage.height;

            if (isShowVirtualCoil)
            {
                for (int i = 0; i < virtualCoilList.Count; i++)
                    frame2.Draw(virtualCoilList[i], new Bgr(Color.Red), 1);
            }

            pictureBox1.Image = frame2.Bitmap;


            int flag2 = 0;
            flag2 = ExternalCall.Detect(ptr, width, height, currentFrameN, laneNum, fps);

            if (flag2 == 1)
            {

                int vehicleNum = ExternalCall.GetVechileCount();

                double speed = ExternalCall.GetVechileSpeed();

            

                string speedStr = "";
                if (speed == 0)
                    speedStr = "--";
                else
                    speedStr = Convert.ToString(Math.Round(speed, 0));

                string type = "";
                int typeint = ExternalCall.GetVechileType();
                if (typeint == 0)
                    type = "小型车";
                else if (typeint == 1)
                    type = "中型车";
                else if (typeint == 2)
                    type = "大型车";
                else
                    type = "--";

                ListViewItem lvi = new ListViewItem();

                int itemCount = listView_Vehicle_Info.Items.Count;

                lvi.Text = Convert.ToString(itemCount + 1);

                //lvi.SubItems.Add(type);

                lvi.SubItems.Add(speedStr);

                if (bVideoType)//是网络视频时，输出检测时间
                    lvi.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                else
                    lvi.SubItems.Add("--");



                this.listView_Vehicle_Info.Items.Add(lvi);

                this.listView_Vehicle_Info.EndUpdate();

                //IntPtr laneVechileNumPtr=ExternalCall.GetVechileNum4Lanes();
                //Marshal.Copy(laneVechileNumPtr, laneVechileNum, 0, laneNum); 

                //for(int i=0;i<laneNum;i++)
                //{
                //    ListViewItem lv = new ListViewItem();
                //    lv.SubItems.Add(Convert.ToString(i+1));
                //    lv.SubItems.Add(laneVechileNum[0].ToString());
                //    lv.SubItems.Add(laneVechileNum[1].ToString());
                //    lv.SubItems.Add(laneVechileNum[2].ToString());

                //    ListView_VechileType.Items[i] = lv;
                //}

            }
        }

        #endregion

        //浏览按钮
        private void Button_Demarcate_Path_Select_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择标定参数文件";
            fileDialog.Filter = "参数文件(*.ini)|*.ini";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = fileDialog.FileName;
                this.textBox_Path.Text = file;

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Demarcate_Path"].Value = this.textBox_Path.Text;
                //config.Save(ConfigurationSaveMode.Modified);
                //System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                //config.Save();
                config.Save(ConfigurationSaveMode.Modified);
                // 强制重新载入配置文件的ConnectionStrings配置节
                ConfigurationManager.RefreshSection("appSettings");
            }
        }


        #region 视频按钮操作事件

        //打开本地视频
        private void Button_VideoFile_Open_Click(object sender, EventArgs e)
        {
            bVideoType = false;//选择本地视频
            currentFrameN = 0;
            this.listView_Vehicle_Info.Items.Clear(); //清除listview中上一个视频检测的数据            

            //capture = new Capture("D:/WorkSpace/Qt_Test/TrafficDetect-II/video/20100323_142040华美达广场对面(用于内存测试).avi");

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择视频文件";
            fileDialog.Filter = "视频文件|*.avi*;*.mp4;*.rmvb";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                videoFileName = fileDialog.FileName;

                //this.textBox_Path.Text = file;
                //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //config.AppSettings.Settings["Demarcate_Path"].Value = this.textBox_Path.Text;
                //config.Save(ConfigurationSaveMode.Modified);
                //System.Configuration.ConfigurationManager.RefreshSection("appSettings");

                capture = new Capture(videoFileName);
                Image<Bgr, Byte> frame = capture.QueryFrame();
                Image<Bgr, Byte> frame2 = frame.Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_NN);//修改
                pictureBox1.Image = frame2.Bitmap;
            }
            else
            {
                return;
            }

            #region 界面设置

            //this.Button_VideoFile_Open.Enabled = false;
            this.button_Camera_Open.Enabled = false;
            this.button_Detect.Enabled = true;
            this.Button_Detect_Stop.Enabled = false;
            this.button_Detect_Close.Enabled = true;
            #endregion
        }

        //开始检测
        private void button_Detect_Click(object sender, EventArgs e)
        {
            #region 界面元素设置

            this.Button_VideoFile_Open.Enabled = false;
            this.button_Detect.Enabled = false;
            this.Button_Detect_Stop.Enabled = true;

            //this.listView_Vehicle_Info.Items.Clear();

            #endregion

            ReadDemarcateFile(textBox_Path.Text);

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //读取上次输入参数的值
            fps = Convert.ToInt32(config.AppSettings.Settings["fps"].Value);

            if (detect_1.Count() == 0 || detect_2.Count() == 0 || detect_3.Count() == 0)
            {
                ReadDemarcateFile(textBox_Path.Text);
            }

            if (virtualCoilList == null || virtualCoilList.Count == 0)
                virtualCoilList = ConvertInts2Rects(detect_1, 3);

            ExternalCall.SetDemarcate(detect_1, detect_2, detect_3, laneNum);

            #region 配置检测参数

            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            double lanesWidth = Convert.ToInt32(config.AppSettings.Settings["LanesWidth"].Value);
            double whiteLineLength = Convert.ToInt32(config.AppSettings.Settings["WhiteLineLength"].Value);
            double whiteLineWidth = Convert.ToInt32(config.AppSettings.Settings["WhiteLineWidth"].Value);

            ExternalCall.SetLaneWidth(lanesWidth);

            ExternalCall.SetWhiteLine(whiteLineLength, whiteLineWidth);

            #endregion

            #region 配置检测参数2

            float Small_Car_Length_Threshold = Convert.ToInt32(config.AppSettings.Settings["Small_Car_Length_Threshold"].Value);
            int min_car_frame_num = Convert.ToInt32(config.AppSettings.Settings["min_car_frame_num"].Value);
            double touchEdgeThreshold = Convert.ToDouble(config.AppSettings.Settings["touchEdgeThreshold"].Value);
            double leftAndRightBoundaryRatio = Convert.ToDouble(config.AppSettings.Settings["leftAndRightBoundaryRatio"].Value);
            double similarity_scale_threshold = Convert.ToDouble(config.AppSettings.Settings["similarity_scale_threshold"].Value);

            ExternalCall.SetDetectParameters(Small_Car_Length_Threshold,	//小型车长阈值
                                 min_car_frame_num,			    //最小的车尾通过虚拟线圈的帧数
                                 touchEdgeThreshold,			//边的阈值
                                 leftAndRightBoundaryRatio,	//左右边界检测的最小比率
                                 similarity_scale_threshold	//相似度阈值
                                );

            #endregion

          

            if (!bVideoType)
            {
                timer_Video.Enabled = true;
            }

            if (bVideoType)
            {
                timer_Camera.Enabled = true;

                MainForm.videoplayer.Parent = this.pictureBox1; // 子窗体的父容器
                MainForm.videoplayer.Dock = DockStyle.Fill;


            }

            /*
            if (!isNetVideoType)
            {
                capture = new Capture(videoFileName);


                timer_Video.Enabled = true;
            }
            else
            {
                isDetect = true;
            }
            */
        }

        //暂停检测
        private void Button_Detect_Stop_Click(object sender, EventArgs e)
        {
            #region 界面设置

            //this.Button_VideoFile_Open.Enabled = true;
            this.button_Detect.Enabled = true;
            this.Button_Detect_Stop.Enabled = false;

            #endregion
            if (!bVideoType)
            {
                timer_Video.Enabled = false;
            }
            if (bVideoType)
            {
                //暂停后显示最后抓拍的图片
                Bitmap bitFormImg = MainForm.videoplayer.SnapPicture();
                Image<Bgr, Byte> bgImage = new Image<Bgr, Byte>(bitFormImg);
                bgImage = bgImage.Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                MainForm.videoplayer.Parent = null;
                if (isShowVirtualCoil)
                {
                    for (int i = 0; i < virtualCoilList.Count; i++)
                        bgImage.Draw(virtualCoilList[i], new Bgr(Color.Red), 1);
                }
                pictureBox1.Image = bgImage.Bitmap;//显示抓拍的图像 

                timer_Camera.Enabled = false;
            }

        }

        //打开网络视频源
        private void button_Camera_Open_Click(object sender, EventArgs e)
        {
            try
            {

                bVideoType = true;//选择网络视频
                MainForm.videoplayer = new VideoPlayer();

                login = new LoginForm();
                login.MainFormMessageEvent += new LoginForm.MainFormMessage(VideoShow);//视频线程初始化
                login.ShowDialog();

                if (MainForm.videoplayer.IsPlaying())
                {
                    this.Button_VideoFile_Open.Enabled = false;//打开本地视频按钮
                    this.button_Detect.Enabled = true;//开始检测按钮
                    this.Button_Detect_Stop.Enabled = false;//停止检测
                    this.button_Detect_Close.Enabled = true;//关闭视频
                }

                currentFrameN = 0;
                /*
                isNetVideoType = true;
                this.timer_Video.Enabled = false;

                m_factory = new MediaPlayerFactory();
                m_player = m_factory.CreatePlayer<IVideoPlayer>();
                m_player.WindowHandle = pictureBox1.Handle;//panel1.Handle;   

                StartVideo("", "", "");

                */

            }
            catch (Exception ex)
            {
                DebugOutput.ProcessMessage("网络启动摄像头失败：" + ex.Message);
            }

        }

        //关闭视频
        private void button_Detect_Close_Click(object sender, EventArgs e)
        {
            this.button_Detect.Enabled = false;
            this.Button_Detect_Stop.Enabled = false;
            this.Button_VideoFile_Open.Enabled = true;
            this.button_Camera_Open.Enabled = true;
            this.button_Detect_Close.Enabled = false;

            if (!bVideoType)
            {
                timer_Video.Enabled = false;
                pictureBox1.Image = Properties.Resources.VideoDetecForm;
            }

            if (bVideoType)
            {
                timer_Camera.Enabled = false;
                MainForm.videoplayer.CloseVideo();//关闭视频界面
                MainForm.videoplayer.Close(); //关闭视频   
                pictureBox1.Image = Properties.Resources.VideoDetecForm;


            }
        }


        #endregion

        private void RadioButton_VirtualCoil_Show_Click(object sender, EventArgs e)
        {
            isShowVirtualCoil = true;
        }

        private void RadioButton__VirtualCoil_Hide_Click(object sender, EventArgs e)
        {
            isShowVirtualCoil = false;
        }





        //网络视频显示
        private void VideoShow(string ip, string portstring, string name, string password)
        {
            try
            {
                MainForm.videoplayer.TopLevel = false; // 这一步最重要, 去除子窗体的顶级窗体设置
                //videoplayer.Parent = this.pictureBox_BackGround; // 子窗体的父容器
                MainForm.videoplayer.FormBorderStyle = FormBorderStyle.None; // 去边框标题栏等
                MainForm.videoplayer.StartVideo(name, password, ip);//启动视频
                MainForm.videoplayer.Show();//显示界面

                //判别是否连接成功
                if (MainForm.videoplayer.IsPlaying())
                {
                    MessageBox.Show("登录成功！", "提示");
                    login.Close();

                    MainForm.videoplayer.Parent = this.pictureBox1; // 子窗体的父容器
                    MainForm.videoplayer.Dock = DockStyle.Fill;
                }
                else
                {
                    MessageBox.Show("登录失败！", "提示");
                }
            }
            catch (Exception ex)
            {
                DebugOutput.ProcessMessage("网络登录显示失败：" + ex.Message);
            }
        }

        private void button_ExportToExcel_Click(object sender, EventArgs e)
        {
            if (listView_Vehicle_Info.Items.Count <= 0)
            {
                MessageBox.Show("检测数据为空！", "提示");
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.InitialDirectory = "E:\\";
            sfd.Filter = "Excel文件(*.xls)|*.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                excelFilePath = sfd.FileName;

                ExportToExcle();
            }

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        #region 标定参数处理方法

        int[] ConvertDemarcate_1ToInts(List<System.Drawing.Rectangle> rectLists, int coilNum)
        {
            int[] detect_ints = new int[4 * coilNum];

            int j = 0;

            for (int i = 0; i < rectLists.Count; i++)
            {
                int x = rectLists[i].X;
                int y = rectLists[i].Y;
                int width = rectLists[i].Width;
                int height = rectLists[i].Height;

                detect_ints[j++] = x;
                detect_ints[j++] = y;
                detect_ints[j++] = width;
                detect_ints[j++] = height;

            }
            return detect_ints;

        }

        List<System.Drawing.Rectangle> ConvertInts2Rects(int[] virualCoilIn, int coilNum)
        {
            List<System.Drawing.Rectangle> list = new List<System.Drawing.Rectangle>();

            int j = 0;
            for (int i = 0; i < coilNum; i++)
            {
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle();

                rect.X = virualCoilIn[j++];
                rect.Y = virualCoilIn[j++];
                rect.Width = virualCoilIn[j++];
                rect.Height = virualCoilIn[j++];

                list.Add(rect);
            }

            return list;
        }

        int[] ConvertDemarcate_2ToInts(List<System.Drawing.Point> points)
        {
            int[] detect_ints = new int[8];

            int j = 0;

            for (int i = 0; i < points.Count; i++)
            {
                int x = points[i].X;
                int y = points[i].Y;

                detect_ints[j++] = x;
                detect_ints[j++] = y;
            }

            return detect_ints;
        }

        int[] ConvertDemarcate_3ToInts(List<System.Drawing.Point> points)
        {
            int[] detect_ints = new int[4];

            int j = 0;

            for (int i = 0; i < points.Count; i++)
            {
                int x = points[i].X;
                int y = points[i].Y;

                detect_ints[j++] = x;
                detect_ints[j++] = y;
            }

            return detect_ints;
        }

        //读取标定文件
        private void ReadDemarcateFile(string fileName)
        {
            string[] lines = System.IO.File.ReadAllLines(fileName);

            char[] separator = { ',' };

            String[] splitStrings1 = new String[100];
            String[] splitStrings2 = new String[100];
            String[] splitStrings3 = new String[100];
            splitStrings1 = lines[0].Split(separator);
            splitStrings2 = lines[1].Split(separator);
            splitStrings3 = lines[2].Split(separator);

            detect_1 = Array.ConvertAll<string, int>(splitStrings1, s => int.Parse(s));
            detect_2 = Array.ConvertAll<string, int>(splitStrings2, s => int.Parse(s));
            detect_3 = Array.ConvertAll<string, int>(splitStrings3, s => int.Parse(s));
            //根据虚拟线圈计算车道数
            laneNum = detect_1.Count() / 4;
        }

        #endregion

        #region  网络视频源调用函数

        //播放视频
        public void StartVideo(string name, string password, string Ip)
        {
            try
            {
                string Url = "rtsp://admin:admin123@192.168.23.64";//
                //string Url = "rtsp://" + name + ":" + password + "@" + Ip;
                m_media = m_factory.CreateMedia<IMedia>(Url);
                m_player.Open(m_media);
                format = new BitmapFormat(pictureBox1.Width, pictureBox1.Height, ChromaType.RV32);
                m_player.CustomRenderer.SetFormat(format);
                callback = new NewFrameEventHandler(display);
                m_player.CustomRenderer.SetCallback(callback);
                m_player.Play();

                if (IsPlaying())
                {
                    MessageBox.Show("网络源视频启动成功！", "提示");

                    #region

                    this.button_Detect.Enabled = true;
                    this.Button_Detect_Stop.Enabled = false;
                    this.button_Camera_Open.Enabled = false;


                    #endregion
                }
                else
                {
                    MessageBox.Show("网络源视频启动失败！", "提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void display(Bitmap bitm)
        {
            if (!isDetect)
            {
                Graphics g = pictureBox1.CreateGraphics();
                System.Drawing.Rectangle rct = new System.Drawing.Rectangle(pictureBox1.Location.X, pictureBox1.Location.Y, bitm.Width, bitm.Height);
                g.DrawImage(bitm, rct);
                g.Dispose();
                //pictureBox1.Image = bitm;
            }




            if (isDetect)
            {
                currentFrameN++;

                Image<Bgr, Byte> frame = new Image<Bgr, Byte>(bitm);



                Image<Bgr, Byte> frame2 = frame.Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_NN);//frame.Copy();
                IntPtr ptr = frame2.MIplImage.imageData;
                string data = ptr.ToString();

                int width = frame2.MIplImage.width;
                int height = frame2.MIplImage.height;

                if (isShowVirtualCoil)
                {
                    for (int i = 0; i < virtualCoilList.Count; i++)
                        frame2.Draw(virtualCoilList[i], new Bgr(Color.Red), 1);
                }

                pictureBox1.Image = frame2.Bitmap;


                int flag2 = 0;
                flag2 = ExternalCall.Detect(ptr, width, height, currentFrameN, laneNum, fps);

                if (flag2 == 1)
                {
                    double speed = ExternalCall.GetVechileSpeed();

                    string speedStr = "";
                    if (speed == 0)
                        speedStr = "--";
                    else
                        speedStr = Convert.ToString(Math.Round(speed, 0));

                    string type = "";
                    int typeint = ExternalCall.GetVechileType();
                    if (typeint == 0)
                        type = "小型车";
                    else if (typeint == 1)
                        type = "中型车";
                    else if (typeint == 2)
                        type = "大型车";

                    ListViewItem lvi = new ListViewItem();

                    int itemCount = listView_Vehicle_Info.Items.Count;

                    lvi.Text = Convert.ToString(itemCount + 1);

                    lvi.SubItems.Add(type);

                    lvi.SubItems.Add(speedStr);

                    this.listView_Vehicle_Info.Items.Add(lvi);

                    this.listView_Vehicle_Info.EndUpdate();
                }
            }
        }

        public bool IsPlaying()
        {
            System.Threading.Thread.Sleep(700);
            bool isplay = m_player.IsPlaying;
            return isplay;
        }

        public void CloseVideo()
        {
            if (m_player != null && m_player.IsPlaying)
            {
                m_player.Stop();
            }


        }
        private void VideoShow2(string ip, string portstring, string name, string password)
        {


            StartVideo(name, password, ip);
        }

        #endregion

        #region 进度条成员变量
        bool processFlag = false;           //是否打开进度条         
        Thread processThread = null;        //进度条线程

        private ProcessBarForm processBarForm = null;
        private delegate bool IncreaseHandle(int nValue);
        private IncreaseHandle myIncrease = null;



        private void ShowProcessBar()
        {
            processBarForm = new ProcessBarForm();
            processBarForm.ShowDialog();
            processBarForm = null;
        }
        private void ProcessBar_ThreadFun()
        {
            MethodInvoker mi = new MethodInvoker(ShowProcessBar);
            this.BeginInvoke(mi);

            Thread.Sleep(1000);

            bool blnIncreased = false;
            object objReturn = null;
            do
            {
                Thread.Sleep(50);
                objReturn = this.Invoke(this.myIncrease, new object[] { 2 });
                blnIncreased = (bool)objReturn;
            }
            while (blnIncreased);
        }

        #endregion

        #region 输出到EXCEL文件私有方法



        //输出到EXCEL文件
        private void ExportToExcle()
        //private bool ExportToExcle(string FilePath, System.Data.DataTable ExportTable)
        {
            //bool Flag = false;
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();//.ApplicationClass();
            if (xlApp == null)
            {
                MessageBox.Show("Excel初始化失败，请确定系统已安装Excel！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                /*this.Invoke(new MessageBox_InvokeHandler(delegate()
                {
                    MessageBox.Show("Excel初始化失败，请确定系统已安装Excel！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //new MessageBoxForm("Excel初始化失败，请确定系统已安装Excel！").ShowDialog();
                }));*/
            }
            try
            {
                #region 打开进度条提示
                /*
                processFlag = true;

                processThread = new Thread(new ThreadStart(ProcessBar_ThreadFun));
                processThread.Start();
                */
                #endregion

                //System.Data.DataTable ExportTable = (System.Data.DataTable)ExportParameter;
                int RowNumber = listView_Vehicle_Info.Items.Count;//ExportTable.Rows.Count;
                int ColumnNumber = listView_Vehicle_Info.Columns.Count;//ExportTable.Columns.Count;
                int RowIndex = 0;

                xlApp.Visible = false;
                //初始化Excel
                Microsoft.Office.Interop.Excel.Workbook wBook = xlApp.Workbooks.Add(true);
                Microsoft.Office.Interop.Excel.Worksheet wSheet = wBook.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                //加载Excel模板
                Microsoft.Office.Interop.Excel.Workbook wBookTemplate = xlApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + @"\RealTimeTemplate.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet wSheetTemplate = (Microsoft.Office.Interop.Excel.Worksheet)wBookTemplate.Sheets[1];
                //拷贝Excel模板
                wSheetTemplate.Copy(wSheet, Type.Missing);
                wSheet = wBook.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                //输入表头信息
                //wSheet.Cells[3, 2] = surveyLocation;    //TextBox_SurveyLocation.Text;
                //wSheet.Cells[3, 8] = survey;            //TextBox_Survey.Text;
                //输入统计信息
                if (RowNumber > 0)
                {
                    RowIndex = 7;
                    Range RangeCell = null;

                    /*
                    //获取数据填充区
                    RangeCell = wSheet.get_Range(wSheet.Cells[RowIndex, 1], wSheet.Cells[RowIndex, 5]);
                    //设置单元格格式
                    RangeCell.Cells.Borders.LineStyle = 1;
                    RangeCell.HorizontalAlignment = XlHAlign.xlHAlignLeft;//左对齐
                    //wSheet.Columns.AutoFit();                       //全表自动列宽
                    //填充数据
                    object[,] DataArray = (object[,])(RangeCell.Value2);
                    */
                    for (int i = 0; i < RowNumber; i++)
                    {




                        for (int j = 0; j < ColumnNumber; j++)
                        {
                            //RangeCell = wSheet.get_Range(wSheet.Cells[RowIndex, j + 1], wSheet.Cells[RowIndex, j + 1]);
                            switch (j)
                            {
                                //case 0: DataArray[i + 1, 1] = ExportTable.Rows[i][j].ToString(); break;
                                //case 1: DataArray[i + 1, 3] = ExportTable.Rows[i][j].ToString(); break;
                                //case 2: DataArray[i + 1, 5] = ExportTable.Rows[i][j].ToString(); break;
                                //case 3: DataArray[i + 1, 7] = ExportTable.Rows[i][j].ToString(); break;
                                //case 4: DataArray[i + 1, 9] = ExportTable.Rows[i][j].ToString(); break;

                                case 0: wSheet.Cells[RowIndex, 1] = listView_Vehicle_Info.Items[i].SubItems[j].Text; break;
                                //case 1: DataArray[i + 1, 2] = ExportTable.Rows[i][j].ToString(); break;
                                case 1: wSheet.Cells[RowIndex, 2] = listView_Vehicle_Info.Items[i].SubItems[j].Text; break;
                                case 2: wSheet.Cells[RowIndex, 3] = listView_Vehicle_Info.Items[i].SubItems[j].Text; break;
                                //case 4: DataArray[i + 1, 5] = ExportTable.Rows[i][j].ToString(); break;
                                default: break;
                            }
                        }

                        RowIndex++;

                    }


                    //释放资源
                    wBookTemplate.Close(false, Type.Missing, Type.Missing);
                    wBookTemplate = null;

                    //设置禁止弹出保存和覆盖的询问提示框
                    xlApp.DisplayAlerts = false;
                    xlApp.AlertBeforeOverwriting = false;
                    //保存Excel文件
                    if (excelFilePath.Length != 0)
                    {
                        wBook.SaveAs(excelFilePath, XlFileFormat.xlExcel8, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    }
                    else
                    {
                        wBook.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "\\ExportExle.xls", XlFileFormat.xlExcel8, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    }
                    //Flag = true;

                    #region 关闭进度条
                    /*
                    processFlag = false;
                    if (!processFlag)
                    {
                        processThread.Abort();
                        if(this.processBarForm!=null)
                            this.processBarForm.Close();

                        
                    }
                    */
                    #endregion

                    MessageBox.Show("数据已成功导出.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    /*
                    this.Invoke(new MessageBox_InvokeHandler(delegate()
                    {
                        MessageBox.Show("数据已成功导出.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //new MessageBoxForm("数据已成功导出！").ShowDialog();
                    }));
                     */
                }
            }
            catch (Exception ex)
            {

                Utility.DebugOutput.ProcessMessage(ex);
                #region 关闭进度条

                processFlag = false;
                if (!processFlag)
                {
                    processThread.Abort();
                    this.processBarForm.Close();
                }

                #endregion

                MessageBox.Show("数据导出Excel失败！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                /*this.Invoke(new MessageBox_InvokeHandler(delegate()
                {
                    MessageBox.Show("数据导出Excel失败！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //new MessageBoxForm("数据导出Excel失败！").ShowDialog();
                }));
                 */
            }
            finally
            {
                xlApp.Quit();
                //杀死进程	
                KillExcel(xlApp);
                //GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            //return Flag;
        }
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        public static void KillExcel(Microsoft.Office.Interop.Excel.Application excel)
        {
            IntPtr t = new IntPtr(excel.Hwnd); //得到这个句柄，具体作用是得到这块内存入口 

            int k = 0;
            GetWindowThreadProcessId(t, out k); //得到本进程唯一标志k
            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k); //得到对进程k的引用
            p.Kill(); //关闭进程k
        }

        #endregion

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


    }
}
