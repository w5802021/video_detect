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
using System.IO;
using Utility;

namespace LaboratoryTable
{
    public partial class DemarcateForm : Form
    {
        LoginForm login = new LoginForm();//登录验证
        //VideoPlayer videoplayer = new VideoPlayer();
        bool bVideoType = true;//ture为网络视频,flase为本地视频
        //bool bNetConnect = false;//判别网络摄像头是否连接成功

        Capture capture;

        public Point firstPoint = new Point(0, 0);  //鼠标第一点   
        public Point secondPoint = new Point(0, 0);  //鼠标第二点   
        public bool begin = false;   //是否开始画矩形   
        //Graphics g;        

        Image<Bgr, Byte> bgImage = null;
        Image<Bgr, Byte> frame2 = null;

        private bool mouseStatus = false;//鼠标状态，false为松开     
        private Point startPoint;//鼠标按下的点
        private Point endPoint;//
        private Rectangle currRect;//当前正在绘制的矩形
        private int minStartX, minStartY, maxEndX, maxEndY;//

        List<Rectangle> rectList = new List<Rectangle>();
        List<Point> pointList = new List<Point>();
        List<Point> pointList3 = new List<Point>();

        //int[] detect;
        int demarcateStatus = 0;

        public DemarcateForm()
        {
            InitializeComponent();

           //g = this.pictureBox_BackGround.CreateGraphics();

            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //string fileName = config.AppSettings.Settings["Demarcate_Path"].Value.ToString();
            //textBox_Path.Text = fileName;
        }

        private void DemarcateForm_Load(object sender, EventArgs e)
        {
            if (MainForm.videoplayer == null)
            {
                MainForm.videoplayer = new VideoPlayer();
            }

            //Application.Idle += new EventHandler(processframe);

            //IntPtr img = getImage2();
            //Image<Bgr, Byte> im = iplimagepointtoemgucviimage(img);

            //pictureBox1.Image = im.Bitmap;
            pictureBox_BackGround.SizeMode = PictureBoxSizeMode.StretchImage;//设置pictureBox的属性

        }
        int number = 0;

        //打开本地视频
        private void button_Detect_Click(object sender, EventArgs e)
        {
            //关闭网络视频
            MainForm.videoplayer.CloseVideo();
            MainForm.videoplayer.Close();

            bVideoType = false;//选择本地视频
            
            string videoFileName = string.Empty;

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择标定参数文件";
            fileDialog.Filter = "视频文件|*.avi;*.mp4;*.rmvb";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                videoFileName = fileDialog.FileName;
            }
            else
            {
                return;
            }

            demarcateStatus = 4;
            capture = new Capture(videoFileName);

            bgImage = capture.QueryFrame();

            frame2 = bgImage.Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);//bgImage.Copy();//修改
            IntPtr ptr = frame2.MIplImage.imageData;
            string data = ptr.ToString();

            int width = bgImage.MIplImage.width;
            int height = bgImage.MIplImage.height;

            pictureBox_BackGround.Image = frame2.Bitmap;// bgImage.Bitmap;
            pictureBox_BackGround.SizeMode = PictureBoxSizeMode.Normal;//设置pictureBox的属性

            button3.Enabled = true;//下一帧按钮
            button1.Enabled = true;//背景帧提取按钮
        }


        #region 转换

        int[] ConvertDemarcate_1ToInts(List<Rectangle> rectLists, int coilNum)
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

        int[] ConvertDemarcate_2ToInts(List<Point> points)
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

        int[] ConvertDemarcate_3ToInts(List<Point> points)
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


        /// <summary>
        /// 将iplimage指针转换成emgucv中的iimage接口；
        /// 1通道对应灰度图像，3通道对应bgr图像，4通道对应bgra图像。
        /// 注意：3通道可能并非bgr图像，而是hls,hsv等图像
        /// </summary>
        /// <param name="ptr">iplimage指针</param>
        /// <returns>返回iimage接口</returns>
        public static Image<Bgr, Byte> iplimagepointtoemgucviimage(IntPtr ptr)
        {
            //int width = mi.Size.Width;
            //int height = mi.Size.Height;

            MIplImage mi = (MIplImage)Marshal.PtrToStructure(ptr, typeof(MIplImage));

            Type tcolor;
            Type tdepth;
            //string unsupporteddepth = "不支持的像素位深度ipl_depth";
            //string unsupportedchannels = "不支持的通道数（仅支持1，2，4通道）";

            tdepth = typeof(Byte);
            return new Image<Bgr, Byte>(256, 256, 768, ptr);

            if (mi.nChannels == 3)
            {
                //case 1:
                //    tcolor = typeof(Gray);
                //    switch (mi.depth)
                //    {
                //        case IPL_DEPTH.IPL_DEPTH_8U:
                //            tdepth = typeof(byte);
                //            return new Image<Gray, Byte>(mi.width, mi.height, mi.widthStep, mi.imageData);
                //        case IPL_DEPTH.ipl_depth_16u:
                //            tdepth = typeof(uint16);
                //            return new Image<Gray, UInt16>(mi.width, mi.height, mi.widthStep, mi.imageData);
                //        case IPL_DEPTH.ipl_depth_16s:
                //            tdepth = typeof(int16);
                //            return new Image<Gray, Int16>(mi.width, mi.height, mi.widthStep, mi.imageData);
                //        case IPL_DEPTH.ipl_depth_32s:
                //            tdepth = typeof(int32);
                //            return new Image<Gray, Int32>(mi.width, mi.height, mi.widthStep, mi.imagedata);
                //        case IPL_DEPTH.ipl_depth_32f:
                //            tdepth = typeof(single);
                //            return new Image<Gray, Single>(mi.width, mi.height, mi.widthStep, mi.imagedata);
                //        case IPL_DEPTH.ipl_depth_64f:
                //            tdepth = typeof(double);
                //            return new Image<gray, double>(mi.width, mi.height, mi.widthStep, mi.imagedata);
                //        //default:
                //        //    throw new notimplementedexception(unsupporteddepth);
                //    }

                //case 3:
                tcolor = typeof(Bgr);
                switch (mi.depth)
                {
                    case IPL_DEPTH.IPL_DEPTH_8U:
                        tdepth = typeof(Byte);
                        return new Image<Bgr, Byte>(mi.width, mi.height, mi.widthStep, mi.imageData);
                    //case IPL_DEPTH.ipl_depth_16u:
                    //    tdepth = typeof(uint16);
                    //    return new Image<Bgr, UInt16>(mi.width, mi.height, mi.widthStep, mi.imageData);
                    //case IPL_DEPTH.ipl_depth_16s:
                    //    tdepth = typeof(Int16);
                    //    return new Image<Bgr, Int16>(mi.width, mi.height, mi.widthStep, mi.imageData);
                    //case IPL_DEPTH.ipl_depth_32s:
                    //    tdepth = typeof(Int32);
                    //    return new Image<Bgr, Int32>(mi.width, mi.height, mi.widthStep, mi.imageData);
                    //case IPL_DEPTH.ipl_depth_32f:
                    //    tdepth = typeof(Single);
                    //    return new Image<Bgr, Single>(mi.width, mi.height, mi.widthStep, mi.imageData);
                    //case IPL_DEPTH.ipl_depth_64f:
                    //    tdepth = typeof(double);
                    //    return new Image<Bgr, double>(mi.width, mi.height, mi.widthStep, mi.imageData);
                    //default:
                    //   throw new notimplementedexception(unsupporteddepth);
                }
                //case 4:
                //    tcolor = typeof(Bgra);
                //    switch (mi.depth)
                //    {
                //        case //ipl_depth.ipl_depth_8u:
                //            tdepth = typeof(byte);
                //            return new Image<bgra, byte>(mi.width, mi.height, mi.widthStep, mi.imagedata);
                //        case IPL_DEPTH.ipl_depth_16u:
                //            tdepth = typeof(uint16);
                //            return new Image<Bgra, UInt16>(mi.width, mi.height, mi.widthStep, mi.imagedata);
                //        case IPL_DEPTH.ipl_depth_16s:
                //            tdepth = typeof(int16);
                //            return new Image<Bgra, Int16>(mi.width, mi.height, mi.widthStep, mi.imagedata);
                //        case IPL_DEPTH.ipl_depth_32s:
                //            tdepth = typeof(int32);
                //            return new Image<Bgra, Int32>(mi.width, mi.height, mi.widthStep, mi.imagedata);
                //        case IPL_DEPTH.ipl_depth_32f:
                //            tdepth = typeof(Single);
                //            return new Image<Bgra, Single>(mi.width, mi.height, mi.widthStep, mi.imagedata);
                //        case IPL_DEPTH.ipl_depth_64f:
                //            tdepth = typeof(double);
                //            return new Image<Bgra, double>(mi.width, mi.height, mi.widthStep, mi.imagedata);
                //        //default:
                //        //    throw new notimplementedexception(unsupporteddepth);
                //    }
                //default:
                //    throw new notimplementedexception(unsupportedchannels);
            }

            return null;
        }

        private void processframe(object sender, EventArgs arg)
        {
            number++;

            Image<Bgr, Byte> frame = capture.QueryFrame();

            if (frame == null)
                return;

            Image<Bgr, Byte> frame2 = frame.Copy();
            IntPtr ptr = frame2.MIplImage.imageData;
            string data = ptr.ToString();

            int width = frame.MIplImage.width;
            int height = frame.MIplImage.height;



            //Image<Gray, Byte> Ecanny = frame.Convert<Gray, Byte>();

            //CvInvoke.cvCanny(Ecanny.Ptr, Ecanny.Ptr, 50, 150, 3);

            //cvCanny是opencv中常用的函数，原本的参数应该是IplImage*类型，这里使用Intpr代替，即Ecanny.ptr

            pictureBox_BackGround.Image = frame.Bitmap;
            int a = 0;
            if (number == 25)
                a = 2;
            //int flag = ExternalCall.Detect(ptr, width, height, number, 3);

        }
        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            demarcateStatus = 1;
            button1.Enabled = false;
            button3.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            demarcateStatus = 2;
            button1.Enabled = false;
            button3.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            demarcateStatus = 3;
            button1.Enabled = false;
            button3.Enabled = false;
        }        

        private void pictureBox_BackGround_MouseMove(object sender, MouseEventArgs e)
        {
            /*
            if (begin)
            {
                g.Clear(this.BackColor);
                //获取新的右下角坐标   
                secondPoint = new Point(e.X, e.Y);
                int minX = Math.Min(firstPoint.X, secondPoint.X);
                int minY = Math.Min(firstPoint.Y, secondPoint.Y);
                int maxX = Math.Max(firstPoint.X, secondPoint.X);
                int maxY = Math.Max(firstPoint.Y, secondPoint.Y);

                //画框   
                g.DrawRectangle(new Pen(Color.Red), minX, minY, maxX - minX, maxY - minY);

            } 
             */
            if (mouseStatus && demarcateStatus == 1)
            {
                endPoint.X = e.X;
                endPoint.Y = e.Y;
                //这一段是获取要绘制矩形的上下左右的坐标，如果不这样处理的话，只有从左上开始往右下角才能画出矩形。
                //这样处理的话，可以任意方向，当然中途可以更换方向。
                //int realStartX = Math.Min(startPoint.X, endPoint.X);
                //int realStartY = Math.Min(startPoint.Y, endPoint.Y);
                //int realEndX = Math.Max(startPoint.X, endPoint.X);
                //int realEndY = Math.Max(startPoint.Y, endPoint.Y);

                //minStartX = Math.Min(minStartX, realStartX);
                //minStartY = Math.Min(minStartY, realStartY);
                //maxEndX = Math.Max(maxEndX, realEndX);
                //maxEndY = Math.Max(maxEndY, realEndY);
                //currRect = new Rectangle(realStartX, realStartY, realEndX - realStartX, realEndY - realStartY);
                currRect = new Rectangle(startPoint.X, startPoint.Y, endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);

                //bgImage.Draw(currRect, new Bgr(Color.Red), 1);
                pictureBox_BackGround.Image = frame2.Bitmap;

                this.pictureBox_BackGround.Invalidate();

            }
            else if (mouseStatus && demarcateStatus == 2)
            {
                //endPoint.X = e.X;
                //endPoint.Y = e.Y;

                //bgImage.DrawPolyline(new LineSegment2D(startPoint, endPoint), new Bgr(Color.Red), 1);

                //pictureBox_BackGround.Image = bgImage.Bitmap;
            }
        }

        private void pictureBox_BackGround_MouseDown(object sender, MouseEventArgs e)
        {
            /*
            Point pre_pt = new Point(-1, -1);//初始坐标
            Point cur_pt = new Point(-1, -1);//实时坐标
            //virtualCoil coil;
            //Rect location;
            int curCoilNum = 0;//当前所画的线圈数
            begin = true;
            firstPoint = new Point(e.X, e.Y); 
            */

            if (frame2 != null && demarcateStatus == 1)
            {
                mouseStatus = true;
                startPoint.X = e.X;
                startPoint.Y = e.Y;
                //重新一个矩形，重置最大重绘矩形的上下左右的坐标
                minStartX = e.X;
                minStartY = e.Y;
                maxEndX = e.X;
                maxEndY = e.Y;
            }
            else if (frame2 != null && demarcateStatus == 2)
            {
                mouseStatus = true;
                //startPoint.X = e.X;
                //startPoint.Y = e.Y;

                pointList.Add(new Point(e.X, e.Y));

                this.pictureBox_BackGround.Invalidate();
            }
            else if (frame2 != null && demarcateStatus == 3)
            {
                mouseStatus = true;

                pointList3.Add(new Point(e.X, e.Y));

                this.pictureBox_BackGround.Invalidate();
            }
        }

        private void pictureBox_BackGround_MouseUp(object sender, MouseEventArgs e)
        {
            if (demarcateStatus == 1)
            {
                //begin = false;  
                mouseStatus = false;
                endPoint.X = e.X;
                endPoint.Y = e.Y;

                if (currRect != null && currRect.X != 0)
                    rectList.Add(currRect);
            }
            else if (demarcateStatus == 2)
            {
                mouseStatus = false;
            }
        }

        private void pictureBox_BackGround_Paint(object sender, PaintEventArgs e)
        {
            if (frame2 == null)
                return;
            if (demarcateStatus == 4)
                return;

            Image<Bgr, Byte> bgImage2 = frame2.Copy();

            bgImage2.Draw(currRect, new Bgr(Color.Red), 1);//显示矩形框

            for (int i = 0; i < rectList.Count; i++)
                bgImage2.Draw(rectList[i], new Bgr(Color.Red), 1);

            if (pointList.Count > 0)
            {
                for (int i = 0; i < pointList.Count; i++)
                    bgImage2.Draw(new CircleF(pointList[i], 1), new Bgr(Color.Blue), 2);

                for (int i = 0; i < pointList.Count - 1; i++)
                    bgImage2.Draw(new LineSegment2D(pointList[i], pointList[i + 1]), new Bgr(Color.Green), 1);

                if (pointList.Count == 4)
                    bgImage2.Draw(new LineSegment2D(pointList[0], pointList[3]), new Bgr(Color.Green), 1);
            }

            if (pointList3.Count > 0)
            {
                for (int i = 0; i < pointList3.Count; i++)
                    bgImage2.Draw(new CircleF(pointList3[i], 1), new Bgr(Color.Blue), 2);

                for (int i = 0; i < pointList3.Count - 1; i++)
                    bgImage2.Draw(new LineSegment2D(pointList3[i], pointList3[i + 1]), new Bgr(Color.Green), 1);
            }

            pictureBox_BackGround.Image = bgImage2.Bitmap;

        }

        //清除标定
        private void Button_demarcate_Clear_Click(object sender, EventArgs e)
        {
            //button_Demarcate.BackColor = SystemColors.Control;
            //Button_Demarcate_2.BackColor = SystemColors.Control;
            //Button_Demarcate_3.BackColor = SystemColors.Control;

            //button_Demarcate.ForeColor = SystemColors.ControlText;
            //Button_Demarcate_2.ForeColor = SystemColors.ControlText;
            //Button_Demarcate_2.ForeColor = SystemColors.ControlText;
            rectList.Clear();
            pointList.Clear();
            pointList3.Clear();
            currRect.Width = 0;
            currRect.Height = 0;
            demarcateStatus = 0;
            mouseStatus = false;
            this.pictureBox_BackGround.Invalidate();            
        }

        //保存标定
        private void Button_demarcate_Save_Click(object sender, EventArgs e)
        {
            int[] detect_1 = ConvertDemarcate_1ToInts(rectList, rectList.Count());
            int[] detect_2 = ConvertDemarcate_2ToInts(pointList);
            int[] detect_3 = ConvertDemarcate_3ToInts(pointList3);

            string[] lines = new string[3];

            string detectStr1 = "";
            string detectStr2 = "";
            string detectStr3 = "";

            if (detect_1.Count() > 0)
                detectStr1 += detect_1[0];
            for (int i = 1; i < detect_1.Count(); i++)
                detectStr1 += "," + detect_1[i];

            if (detect_2.Count() > 0)
                detectStr2 += detect_2[0];
            for (int i = 1; i < detect_2.Count(); i++)
                detectStr2 += "," + detect_2[i];

            if (detect_3.Count() > 0)
                detectStr3 += detect_3[0];
            for (int i = 1; i < detect_3.Count(); i++)
                detectStr3 += "," + detect_3[i];

            lines[0] = detectStr1;
            lines[1] = detectStr2;
            lines[2] = detectStr3;            

            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.InitialDirectory = "E:\\";
            sfd.Filter = "参数文件(*.ini)|*.ini";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;

                //FileStream fs = new FileStream(fileName, FileMode.CreateNew);
                //StreamWriter sw = new StreamWriter(sfd.FileName);// (fs);
                //sw.WriteLine(detectStr1);
                //sw.WriteLine(detectStr2);
                //sw.WriteLine(detectStr3);
                //sw.Close();
                //fs.Close();

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                    System.IO.File.WriteAllLines(fileName, lines, Encoding.UTF8);

                }
                else
                    System.IO.File.WriteAllLines(fileName, lines, Encoding.UTF8);
            }

        }

        //下一帧
        private void button3_Click(object sender, EventArgs e)
        {
            Image<Bgr, Byte> frame = capture.QueryFrame();
            frame2 = frame.Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);//修改
            pictureBox_BackGround.Image = frame2.Bitmap;
            pictureBox_BackGround.SizeMode = PictureBoxSizeMode.Normal;//设置pictureBox的属性
        }

        //打开网络视频源
        private void button_Capture_Click(object sender, EventArgs e)
        {
            //关闭网络视频
            MainForm.videoplayer.CloseVideo();
            MainForm.videoplayer.Close();

            try
            {
                bVideoType = true;//选择网络视频   
                MainForm.videoplayer = new VideoPlayer();  
                              
                login = new LoginForm();                               
                login.MainFormMessageEvent += new LoginForm.MainFormMessage(VideoShow);//视频线程初始化
                login.ShowDialog();

                if (MainForm.videoplayer.IsPlaying())
                {
                    button3.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                DebugOutput.ProcessMessage("网络启动摄像头失败：" + ex.Message);
            }

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
                    button3.Enabled = false;
                    button1.Enabled = true;
                    MessageBox.Show("登录成功！", "提示");
                    login.Close();

                    MainForm.videoplayer.Parent = this.pictureBox_BackGround; // 子窗体的父容器
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

        //背景帧提取（抓拍）
        private void button1_Click(object sender, EventArgs e)
        {
            if (bVideoType)//网络视频
            {
                Bitmap bitFormImg = MainForm.videoplayer.SnapPicture();
                Image<Bgr, Byte> bgImage = new Image<Bgr, Byte>(bitFormImg);
                frame2 = bgImage.Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                MessageBox.Show("图像已抓取，请在图中标定！", "提示");
                MainForm.videoplayer.CloseVideo();//关闭视频界面
                MainForm.videoplayer.Close(); //关闭视频               
                pictureBox_BackGround.Image = frame2.Bitmap;//显示抓拍的图像
                pictureBox_BackGround.SizeMode = PictureBoxSizeMode.Normal;//设置pictureBox的属性

                button1.Enabled = false;
                //videoplayer.Parent = null;//停止显示网络视频在界面上
                //string path = "d:\\ProImg1.bmp";
                //FileInfo fi = new FileInfo(path);                
                //bitFormImg.Save(path);

            }
            else
                MessageBox.Show("图像已抓取，请在图中标定！", "提示");

            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            Button_demarcate_Save.Enabled = true;
            Button_demarcate_Clear.Enabled = true;
        }

        private void pictureBox_BackGround_Click(object sender, EventArgs e)
        {

        }       


    }
}
