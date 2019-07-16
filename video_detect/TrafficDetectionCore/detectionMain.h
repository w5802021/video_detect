#ifndef DETECTION2_H
#define DETECTION2_H

//#include "opencv2/opencv.hpp"
//#include <iostream>

//#include "DllMain.h"
//#include "CalDetectParameter.h"
//using namespace cv;
#include <iostream>
#include <opencv2/opencv.hpp>
#include "Vibe.h"
#include <vector>
#include "VirtualCoil.h"
#include "VehicleFlowDetection.h"
#include "DemarcateTransform.h"

using namespace std;


extern "C" __declspec(dllexport) double GetVechileSpeed();

extern "C" __declspec(dllexport) int GetVechileType();

extern "C" __declspec(dllexport) int GetVechileCount();

extern "C" __declspec(dllexport) void SetDemarcate( int detect_1[],int detect_2[],int detect_3[],int coilNum);

extern "C" __declspec(dllexport) int Detect(uchar* data,int width,int height,int number,int laneNum_,int inFps);

extern "C" __declspec(dllexport) int* GetVechileNum4Lanes();

extern "C" __declspec(dllexport) int* GetVechileInfo();

//���ð��߳������ȣ���Ϊ����ϵ��
extern "C" __declspec(dllexport) int SetWhiteLine(double length, double width);

//���ó������
extern "C" __declspec(dllexport) int SetLaneWidth(double width);

extern "C" __declspec(dllexport) int SetDetectParameters(float Small_Car_Length_Threshold,	//С�ͳ�����ֵ
														 int min_car_frame_num,			//��С�ĳ�βͨ��������Ȧ��֡��
														 double touchEdgeThreshold,			//�ߵ���ֵ
														 double leftAndRightBoundaryRatio,	//���ұ߽������С����
														 double similarity_scale_threshold	//���ƶ���ֵ
														 );

#endif // DETECTION_H
