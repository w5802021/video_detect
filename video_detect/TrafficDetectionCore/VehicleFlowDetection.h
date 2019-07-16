#pragma once
#include<iostream>
#include "opencv2/opencv.hpp"
#include "VirtualCoil.h"
#include "DetectionParameter.h"

using namespace std;
using namespace cv;



struct crossEdageCoil
{
	int start;//��ʼ��Ȧ
	int end;  //������Ȧ
	int frameNum;//֡��
};

class VehicleFlowDetection
{
public:
	VehicleFlowDetection(int LNum);
	~VehicleFlowDetection();

	vector<VirtualCoil*> start(vector<VirtualCoil*> coils, Mat* frame);
	//���س���Ŀ
	int reCarNum();
	//�������ڼ�����Ȧ
	vector<int> reTestingCoils();
	/*vector<crossEdageCoil*> */ 
	void cross_edage(vector<VirtualCoil*> coils, vector<crossEdageCoil*> crossEdageCoils, vector<int> testingCoils);//���п�߼��
	//����
	vector<crossEdageCoil*> count(vector<crossEdageCoil*> crossEdageCoils);
	//���ؼ�⵽������Ȧ
	vector<int> reActiveCoils();
	vector<VirtualCoil*> start2(vector<VirtualCoil*> coils, Mat* frame,int j);
	vector<crossEdageCoil*> cross_edage2(VirtualCoil* coil1,VirtualCoil* coil2,vector<crossEdageCoil*> crossEdageCoils);
	vector<crossEdageCoil*> cross_edage3(VirtualCoil* coil1,vector<crossEdageCoil*> crossEdageCoils);
	int count2(vector<crossEdageCoil*> crossEdageCoils);
	
private:
	int carNum;//����
	int laneNum;
	vector<int> testingCoils;//���ڼ�����Ȧ
	vector<int> activeCoils;//�����г���ͨ������Ȧ
};