#include <iostream>
#include <opencv2/opencv.hpp>
#include "ViBe.h"
#include <vector>
#include "VirtualCoil.h"
#include "DetectionParameter.h"

using namespace cv;
using namespace std;


class DemarcateTransform
{
public:
/************************************************************************/
/*计算虚拟线圈左边界竖线的距离（世界坐标）
 *location：虚拟线圈的位置坐标
 *BRatio：补偿率
 *points：标定点的集合
 *wPoints：世界点的坐标
 */
/************************************************************************/
static float calCoilLeftDist(Rect location, float BRatio, vector<Point2f> points, vector<Point2f> wPoints);

/************************************************************************/
/*计算虚拟线圈右边界竖线的距离（世界坐标）
 *location：虚拟线圈的位置坐标
 *BRatio：补偿率
 *points：标定点的集合
 *wPoints：世界点的坐标
 */
/************************************************************************/
static float calCoilRightDist(Rect location, float BRatio, vector<Point2f> points, vector<Point2f> wPoints);

//static vector<float> virtualCoilWDist(Rect location, float BRatio, vector<Point2f> points, vector<Point2f> wPoints);

//通过车道数计算出世界坐标
static vector<Point2f> worldPoints(int laneNum);

//计算补偿率;points是标定点的集合,wPoints是世界点的坐标
static float calculateBRatio(vector<Point2f> points, vector<Point2f> wPoints);

static float twoCoilDistance(Rect location, Rect locationsSpeed, float BRatio,vector<Point2f> points, vector<Point2f> wPoints);


private:
	static Mat calLeftMatrix4Equations(vector<Point2f> wPoints, vector<Point2f> iPoints);
	static Mat calRightMatrix4Equations(vector<Point2f> wPoints, vector<Point2f> iPoints);
	static Mat solveEquations(Mat ML,Mat MR);
	static vector<Point2f> transformToWorldCoordinate(vector<Point2f> p,Mat Coefficient);

	static float no_distanceFun(vector<Point2f> toWPoints);

	static float no_distanceFun(vector<Point2f> wPoints, vector<Point2f> iPoints,vector<Point2f> transformPoints);

	static float yes_distanceFun(float compensateRatio,float no_distance);

	static float calculCompensate(float realValue,float no_distance);

//private:
//	Mat ML;//线性方程的左矩阵
//	Mat MR;//线性方程的右矩阵
//	Mat Coefficient;//系数解
};