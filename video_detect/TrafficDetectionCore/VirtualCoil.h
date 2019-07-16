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
	void secondFrameForegroundInit(Mat frame);//第二帧背景初始化
	void otherFrameForegroundInit(Mat* frame);//其他帧背景初始化
	bool judgeCarThrough();//判断有车通过
	int velocity(double FPS);//计算车速
	int returnCarFrameNum();//返回车走的帧数
	Mat returnMask();//返回背景图
	bool returnFlag();//返回触线标志位
	string leftAndRightBoundaryDetection();//左右边界的检测
	bool reCrossFlagFun();//返回跨边标志
	void setCrossFlag(bool flag);//设置跨边标志位
	void setSameTempCoilFlag(int flag);//设置相同虚拟线圈标志位(用当前的帧数表示)
	int reSameTempCoilFlag();//返回相同虚拟线圈标志位(用当前的帧数表示)
	void setTempCoilStartFrame(int count);//设置临时线圈的起始帧数
	int reTempCoilStartFrame();//返回临时线圈的起始帧
	Mat reBackgroundImage();//返回背景图像
	Scalar compareSimilarity(Mat b);//比较两幅图像的相似度
	double reMeansSimilarityScale();//返回平均相似度
	void setActiveFlag(bool activeF);//设置激活标志位
	bool reActiveFlag();//返回激活标志位
	void tempCoilFrom(int a,int b);//临时线圈来自于
	vector<int> reTempCoilFrom();//返回临时线圈来自于
	bool reVelocityFlag();//速度标志位
	void setVelocityFlag(bool flag);//设置速度标志位
	float reDownBoundaryRatio();//返回下边缘的比率
	float reUpBoundaryRatio();//返回上边缘的比率
	bool testTouchEdge(Mat binaryImage, bool upEdage);//检测边缘

public:
	int id;		  //虚拟线圈编号，从1开始
	bool testing; //是否正在检测
private:
	float wDistanceLeft;//左手边的世界距离
	float wDistanceRight;//右手边的世界距离
	Rect location;//虚拟线圈的位置坐标
	ViBe_BGS Vibe_Bgs;//ViBe对象
	Mat mask;//前景的二值图像
	double maskAreaRatio;//前景所占虚拟线圈的面积比例
	bool flag;//是否有车通过的标志位，虚拟线圈下边缘（车头进入，车尾出去），用在车流检测
	int carFrameNum;//一个变化周期内的帧数（走过车身的帧数）
	bool touchLineFlag;//触线标志（车头进来，车头出来）用在车速检测
	int touchLineFrameNum;//触线帧数（走过虚拟线圈的帧数）用在车速检测
	float speed;//通过车辆的速度
	int reCarFrameNum;//返回的车数
	//********************************************
	//保留属性，没用上
	int newCoilNum;//新虚拟线圈测帧数
	bool tempCoilFlag;//是否为临时虚拟线圈
	bool tempCoilFirstFrameFlag;//临时线圈的第一帧标志
	double tempCoilFirstFrameArea;//临时线圈第一帧的面积
	double firstFrameAreaDiff;//临时线圈第一帧面积差
	//********************************************
	bool crossFlag;//跨边标志位
	int sameTempCoilFlag;//有相同的虚拟线圈的标志位(用当前的帧数表示)
	int tempCoilStartFrame;//临时线圈的起始帧数
	Mat backgroundImage;//线圈的第一帧背景图
	double meansSimilarityScale;//线圈的平均相似度
	bool activeFlag;//线圈是否激活的标志位
	vector<int> tempCoilFromCoil;//临时线圈来自
	bool velocityFlag;
	float downBoundaryRatio;//下边缘的比率
	float upBoundaryRatio;//上边缘的比率


};

class Vechile
{
public:
	//车道号
	int laneNo;
	//车型，0：小型车；1：中型车；2：大型车
	double vechileType;
	//车速
	int speed;

public:
	Vechile(int inLane,int inVechileType,double inSpeed)
	{
		laneNo=inLane;
		vechileType=inVechileType;
		speed=inSpeed;
	};
};


