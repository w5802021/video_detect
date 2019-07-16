#pragma once
#include<iostream>
#include "opencv2/opencv.hpp"
#include "VirtualCoil.h"
#include "DetectionParameter.h"

using namespace std;
using namespace cv;



struct crossEdageCoil
{
	int start;//起始线圈
	int end;  //结束线圈
	int frameNum;//帧数
};

class VehicleFlowDetection
{
public:
	VehicleFlowDetection(int LNum);
	~VehicleFlowDetection();

	vector<VirtualCoil*> start(vector<VirtualCoil*> coils, Mat* frame);
	//返回车数目
	int reCarNum();
	//返回正在检测的线圈
	vector<int> reTestingCoils();
	/*vector<crossEdageCoil*> */ 
	void cross_edage(vector<VirtualCoil*> coils, vector<crossEdageCoil*> crossEdageCoils, vector<int> testingCoils);//进行跨边检测
	//计数
	vector<crossEdageCoil*> count(vector<crossEdageCoil*> crossEdageCoils);
	//返回检测到车的线圈
	vector<int> reActiveCoils();
	vector<VirtualCoil*> start2(vector<VirtualCoil*> coils, Mat* frame,int j);
	vector<crossEdageCoil*> cross_edage2(VirtualCoil* coil1,VirtualCoil* coil2,vector<crossEdageCoil*> crossEdageCoils);
	vector<crossEdageCoil*> cross_edage3(VirtualCoil* coil1,vector<crossEdageCoil*> crossEdageCoils);
	int count2(vector<crossEdageCoil*> crossEdageCoils);
	
private:
	int carNum;//车数
	int laneNum;
	vector<int> testingCoils;//正在检测的线圈
	vector<int> activeCoils;//检测出有车辆通过的线圈
};