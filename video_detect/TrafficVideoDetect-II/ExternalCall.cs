using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace LaboratoryTable
{
    
    class ExternalCall
    {
        [DllImport("TrafficDetectionCore.dll")]
        public extern static IntPtr getImage();

        [DllImport("TrafficDetectionCore.dll")]
        public extern static IntPtr getImage2();


        [DllImport("TrafficDetectionCore.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int Detect(IntPtr data, int width, int height, int number, int laneNum_, int inFps);

        [DllImport("TrafficDetectionCore.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static double GetVechileSpeed();

        [DllImport("TrafficDetectionCore.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetVechileType();

        [DllImport("TrafficDetectionCore.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int Demarcate(IntPtr data, int width, int height);

        [DllImport("TrafficDetectionCore.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetDemarcate(int[] detect_1, int[] detect_2, int[] detect_3, int coilNum);

        [DllImport("TrafficDetectionCore.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetVechileCount();

        [DllImport("TrafficDetectionCore.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr GetVechileNum4Lanes();

        [DllImport("TrafficDetectionCore.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr GetVechileInfo();

        [DllImport("TrafficDetectionCore.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetWhiteLine(double length, double width);

        [DllImport("TrafficDetectionCore.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetLaneWidth(double width);

        [DllImport("TrafficDetectionCore.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetDetectParameters(float Small_Car_Length_Threshold,	//小型车长阈值
                                int min_car_frame_num,			    //最小的车尾通过虚拟线圈的帧数
                                double touchEdgeThreshold,			//边的阈值
                                double leftAndRightBoundaryRatio,	//左右边界检测的最小比率
                                double similarity_scale_threshold	//相似度阈值
                                );

    }
}
