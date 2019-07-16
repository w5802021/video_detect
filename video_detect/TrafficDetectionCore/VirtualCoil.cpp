#include "VirtualCoil.h"

VirtualCoil::VirtualCoil(Rect loc, float wDisLeft, float wDisRight, bool tempCoil, Mat backImage)
{
	location = loc;
	wDistanceLeft = wDisLeft;
	wDistanceRight = wDisRight;
	flag = false;
	touchLineFlag = false;
	tempCoilFlag = tempCoil;
	backgroundImage = backImage;
	velocityFlag = true;
	downBoundaryRatio = 0;
	upBoundaryRatio = 0;

	//初始化该属性为非正在检测
	testing=false;
}
Mat VirtualCoil::reBackgroundImage()
{
	return backgroundImage;
}
double VirtualCoil::reMeansSimilarityScale()
{
	return meansSimilarityScale;
}
void VirtualCoil::setActiveFlag(bool activeF)
{
	activeFlag = activeF;
}
bool VirtualCoil::reActiveFlag()
{
	return activeFlag;
}
void VirtualCoil::tempCoilFrom(int a, int b)
{
	tempCoilFromCoil.push_back(a);
	tempCoilFromCoil.push_back(b);
}
vector<int> VirtualCoil::reTempCoilFrom()
{
	return tempCoilFromCoil;
}

void VirtualCoil::setTempCoilStartFrame(int count)
{
	tempCoilStartFrame = count;
}

int VirtualCoil::reTempCoilStartFrame()
{
	return tempCoilStartFrame;
}
void VirtualCoil::setCrossFlag(bool flag)
{
	crossFlag = flag;
}
bool VirtualCoil::reCrossFlagFun()
{
	return crossFlag;
}
void VirtualCoil::setSameTempCoilFlag(int flag)
{
	sameTempCoilFlag = flag;
}
int VirtualCoil::reSameTempCoilFlag()
{
	return sameTempCoilFlag;
}

bool VirtualCoil::returnFlag()
{
	return flag;
}
float VirtualCoil::reDownBoundaryRatio()
{
	return downBoundaryRatio;
}
float VirtualCoil::reUpBoundaryRatio()
{
	return upBoundaryRatio;
}

int VirtualCoil::showTouchLineFrameNum()
{
	return touchLineFrameNum;
}

float VirtualCoil::reSpeed()
{
	return speed;
}

Mat VirtualCoil::returnMask()
{
	return mask;
}

Rect VirtualCoil::reLocation()
{
	return location;
}
VirtualCoil::~VirtualCoil()
{
}

void VirtualCoil::secondFrameForegroundInit(Mat frame)
{
	Mat gray;
	cvtColor(frame(location), gray, CV_RGB2GRAY);//将帧数图像灰度化
	Vibe_Bgs.init(&gray);
	Vibe_Bgs.processFirstFrame(&gray);
	cout << " Training GMM complete!" << endl;
	
	//delete gray;
}
//完全相似：1，有车经过：<0.9
//SSIM
Scalar VirtualCoil::compareSimilarity(Mat b)
{
	Mat i1 = backgroundImage;
	Mat i2 = b;
	const double C1 = 6.5025, C2 = 58.5225;
	int d = CV_32F;
	Mat I1, I2;
	i1.convertTo(I1, d);
	i2.convertTo(I2, d);
	Mat I2_2 = I2.mul(I2);
	Mat I1_2 = I1.mul(I1);
	Mat I1_I2 = I1.mul(I2);
	Mat mu1, mu2;
	GaussianBlur(I1, mu1, Size(11, 11), 1.5);
	GaussianBlur(I2, mu2, Size(11, 11), 1.5);
	Mat mu1_2 = mu1.mul(mu1);
	Mat mu2_2 = mu2.mul(mu2);
	Mat mu1_mu2 = mu1.mul(mu2);
	Mat sigma1_2, sigma2_2, sigma12;
	GaussianBlur(I1_2, sigma1_2, Size(11, 11), 1.5);
	sigma1_2 -= mu1_2;
	GaussianBlur(I2_2, sigma2_2, Size(11, 11), 1.5);
	sigma2_2 -= mu2_2;
	GaussianBlur(I1_I2, sigma12, Size(11, 11), 1.5);
	sigma12 -= mu1_mu2;
	Mat t1, t2, t3;
	t1 = 2 * mu1_mu2 + C1;
	t2 = 2 * sigma12 + C2;
	t3 = t1.mul(t2);
	t1 = mu1_2 + mu2_2 + C1;
	t2 = sigma1_2 + sigma2_2 + C2;
	t1 = t1.mul(t2);
	Mat ssim_map;
	divide(t3, t1, ssim_map);
	Scalar mssim = mean(ssim_map);
	meansSimilarityScale = (mssim.val[0] + mssim.val[1] + mssim.val[2]) / 3;
	return mssim;
}
void VirtualCoil::otherFrameForegroundInit(Mat* frame)
{
	Mat gray,m;
	//闭运算中的结构子
	Mat element5(5, 5, CV_8U, Scalar(1));
	//cvtColor((*frame)(location), gray, CV_RGB2GRAY);
	Vibe_Bgs.testAndUpdate(&gray);
	m = Vibe_Bgs.getMask();
	//进行闭运算将点区域连通起来
	morphologyEx(m, m, MORPH_CLOSE, element5);
	//将背景的二值图放到结构体中
	//imshow("m",m);
	mask = m;
}

int VirtualCoil::returnCarFrameNum()
{
	return reCarFrameNum;
}
bool VirtualCoil::reVelocityFlag()
{
	return velocityFlag;
}
//判断是否有车辆通过虚拟线圈（主要就车头进入、车尾出去两种情况）
bool VirtualCoil::judgeCarThrough()
{
	{
		//firstFrameAreaDiff = tempCoilFirstFrameArea - maskAreaRatio;
		if (meansSimilarityScale < DemarcateParameter::similarity_scale_threshold && flag == false)
		{
			carFrameNum = 0;
			flag = true;
		}
		if (meansSimilarityScale < DemarcateParameter::similarity_scale_threshold && flag == true)
		{
			++carFrameNum;
		}
		if (meansSimilarityScale >= DemarcateParameter::similarity_scale_threshold && flag == true)
		{
			//cout << "该车走的帧数：" << carFrameNum << endl;
			//cout << "已经进入函数了" << endl;
			reCarFrameNum = carFrameNum;
			flag = false;
			carFrameNum = 0;
			velocityFlag = true;
			return true;
		}
		return false;
	}
	
}
float VirtualCoil::reWDistanceLeft()
{
	return wDistanceLeft;
}
//只检测矩形图像的上边和下边(upEdage决定了检测上边还是下边)
bool VirtualCoil::testTouchEdge(Mat binaryImage, bool upEdage)
{
	double edgePixelsRatio;
	double edgePixels = 0;
	int beginX = 1;
	int beginY;
	bool result;
	//imshow("aa",binaryImage);
	int width = binaryImage.cols;
	beginY = upEdage ? 1 : (binaryImage.rows - 1);
	for (int i = 0; i <width - 1; i++)
	{
		if (binaryImage.at<uchar>(beginY, beginX + i)>0)
			++edgePixels;
	}
	edgePixelsRatio = edgePixels / (width - 1);

	if (upEdage)
	{
		upBoundaryRatio = float(edgePixelsRatio);
	}
	else
	{
		downBoundaryRatio = float(edgePixelsRatio);
	}
	//cout << "边的阈值为：" << edgePixelsRatio << endl;
	result = edgePixelsRatio >= DemarcateParameter::touchEdgeThreshold ? true : false;
	return result;
}
void VirtualCoil::setVelocityFlag(bool flag)
{
	velocityFlag = flag;
}
// 计算
int VirtualCoil::velocity(double FPS)
{
	float touchLineTime = 0;
	//检测到车辆的第一帧，且车辆仅碰到线圈下界
	if (testTouchEdge(mask, false) && !touchLineFlag && !testTouchEdge(mask, true))
	{
		touchLineFlag = true;
		touchLineFrameNum = 1;
	}
	//接下来车辆在线圈内，对触线帧数（车辆经过下界到经过上界）进行计数
	if (testTouchEdge(mask, false) && touchLineFlag && !testTouchEdge(mask, true))
	{
		++touchLineFrameNum;
	}
	// testTouchEdge(mask, true)当其第二个参数置为True时，说明车辆已经触碰到线圈上界
	if (touchLineFlag && testTouchEdge(mask, true))
	{
		//触线时间=触线帧数（车辆经过下界到经过上界）/帧率
		float touchLineTime = float(touchLineFrameNum / FPS);
		speed = (wDistanceRight / touchLineTime)*MS_TO_KMH;
		touchLineFlag = false;
		//velocityFlag = false;
		return true;
	}
	//返回Fasle说明当前帧没有获得车辆速度信息
	return false;
}

string VirtualCoil::leftAndRightBoundaryDetection()
{
	double edgePixelsRatioLeft, edgePixelsLeft=0;
	double edgePixelsRatioRight, edgePixelsRight = 0;
	int width = mask.cols;
	int height = mask.rows;
	//System.Runtime.InteropServices.SEHException
	int beginXLeft = 0, beginYLeft = 0;//int beginXLeft = 1, beginYLeft = 1;
	int beginXRight = width - 1;
	int beginYRight = 0;//int beginYRight = 1;
	//求左边界
	for (int i = 0; i <height; i++)
	{
		
		if (mask.at<uchar>(beginYLeft + i, beginXLeft)>0)
			++edgePixelsLeft;
	
	}
	edgePixelsRatioLeft = edgePixelsLeft / height;
	//cout << "左边界：" << edgePixelsRatioLeft << endl;
	//求右边界
	for (int i = 0; i <height; i++)
	{
		try
		{
			if (mask.at<uchar>(beginYRight+ i, beginXRight )>0)
				++edgePixelsRight;
		}		
		catch (exception* e)
		{
		}
		
	}
	edgePixelsRatioRight = edgePixelsRight / height;
	//cout << "右边界：" << edgePixelsRatioRight << endl;
	if (edgePixelsRatioRight > DemarcateParameter::leftAndRightBoundaryRatio)
	{
		if (edgePixelsRatioLeft > DemarcateParameter::leftAndRightBoundaryRatio)
		{
			//leftAndRightBoundary = "left_and_right";
			return "left_and_right";
		}
		else
		{
			//leftAndRightBoundary = "right";
			return "right";
		}
	}
	else
	{
		if (edgePixelsRatioLeft > DemarcateParameter::leftAndRightBoundaryRatio)
		{
			//leftAndRightBoundary = "left";
			return "left";
		}
		else
		{
			//leftAndRightBoundary = "none";
			return "none";
		}
	}
}
