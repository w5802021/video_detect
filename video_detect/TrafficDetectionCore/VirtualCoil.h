#pragma once
#include<iostream>
#include "opencv2/opencv.hpp"  
#include "Vibe.h"
//#include "Demarcate.h"
#include <exception>
#include "DetectionParameter.h"
using namespace std;
using namespace cv;

class VirtualCoil
{
public:
	VirtualCoil(Rect location, float wDistanceLeft, float wDistanceRight, bool tempCoil, Mat backImage);
	~VirtualCoil();
	float reWDistanceLeft();
	int showTouchLineFrameNum();
	float reSpeed();
	Rect reLocation();
	void secondFrameForegroundInit(Mat frame);//�ڶ�֡������ʼ��
	void otherFrameForegroundInit(Mat* frame);//����֡������ʼ��
	bool judgeCarThrough();//�ж��г�ͨ��
	int velocity(double FPS);//���㳵��
	int returnCarFrameNum();//���س��ߵ�֡��
	Mat returnMask();//���ر���ͼ
	bool returnFlag();//���ش��߱�־λ
	string leftAndRightBoundaryDetection();//���ұ߽�ļ��
	bool reCrossFlagFun();//���ؿ�߱�־
	void setCrossFlag(bool flag);//���ÿ�߱�־λ
	void setSameTempCoilFlag(int flag);//������ͬ������Ȧ��־λ(�õ�ǰ��֡����ʾ)
	int reSameTempCoilFlag();//������ͬ������Ȧ��־λ(�õ�ǰ��֡����ʾ)
	void setTempCoilStartFrame(int count);//������ʱ��Ȧ����ʼ֡��
	int reTempCoilStartFrame();//������ʱ��Ȧ����ʼ֡
	Mat reBackgroundImage();//���ر���ͼ��
	Scalar compareSimilarity(Mat b);//�Ƚ�����ͼ������ƶ�
	double reMeansSimilarityScale();//����ƽ�����ƶ�
	void setActiveFlag(bool activeF);//���ü����־λ
	bool reActiveFlag();//���ؼ����־λ
	void tempCoilFrom(int a,int b);//��ʱ��Ȧ������
	vector<int> reTempCoilFrom();//������ʱ��Ȧ������
	bool reVelocityFlag();//�ٶȱ�־λ
	void setVelocityFlag(bool flag);//�����ٶȱ�־λ
	float reDownBoundaryRatio();//�����±�Ե�ı���
	float reUpBoundaryRatio();//�����ϱ�Ե�ı���
	bool testTouchEdge(Mat binaryImage, bool upEdage);//����Ե

public:
	int id;		  //������Ȧ��ţ���1��ʼ
	bool testing; //�Ƿ����ڼ��
private:
	float wDistanceLeft;//���ֱߵ��������
	float wDistanceRight;//���ֱߵ��������
	Rect location;//������Ȧ��λ������
	ViBe_BGS Vibe_Bgs;//ViBe����
	Mat mask;//ǰ���Ķ�ֵͼ��
	double maskAreaRatio;//ǰ����ռ������Ȧ���������
	bool flag;//�Ƿ��г�ͨ���ı�־λ��������Ȧ�±�Ե����ͷ���룬��β��ȥ�������ڳ������
	int carFrameNum;//һ���仯�����ڵ�֡�����߹������֡����
	bool touchLineFlag;//���߱�־����ͷ��������ͷ���������ڳ��ټ��
	int touchLineFrameNum;//����֡�����߹�������Ȧ��֡�������ڳ��ټ��
	float speed;//ͨ���������ٶ�
	int reCarFrameNum;//���صĳ���
	//********************************************
	//�������ԣ�û����
	int newCoilNum;//��������Ȧ��֡��
	bool tempCoilFlag;//�Ƿ�Ϊ��ʱ������Ȧ
	bool tempCoilFirstFrameFlag;//��ʱ��Ȧ�ĵ�һ֡��־
	double tempCoilFirstFrameArea;//��ʱ��Ȧ��һ֡�����
	double firstFrameAreaDiff;//��ʱ��Ȧ��һ֡�����
	//********************************************
	bool crossFlag;//��߱�־λ
	int sameTempCoilFlag;//����ͬ��������Ȧ�ı�־λ(�õ�ǰ��֡����ʾ)
	int tempCoilStartFrame;//��ʱ��Ȧ����ʼ֡��
	Mat backgroundImage;//��Ȧ�ĵ�һ֡����ͼ
	double meansSimilarityScale;//��Ȧ��ƽ�����ƶ�
	bool activeFlag;//��Ȧ�Ƿ񼤻�ı�־λ
	vector<int> tempCoilFromCoil;//��ʱ��Ȧ����
	bool velocityFlag;
	float downBoundaryRatio;//�±�Ե�ı���
	float upBoundaryRatio;//�ϱ�Ե�ı���


};

class Vechile
{
public:
	//������
	int laneNo;
	//���ͣ�0��С�ͳ���1�����ͳ���2�����ͳ�
	double vechileType;
	//����
	int speed;

public:
	Vechile(int inLane,int inVechileType,double inSpeed)
	{
		laneNo=inLane;
		vechileType=inVechileType;
		speed=inSpeed;
	};
};


