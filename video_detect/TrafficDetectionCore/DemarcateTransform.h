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
/*����������Ȧ��߽����ߵľ��루�������꣩
 *location��������Ȧ��λ������
 *BRatio��������
 *points���궨��ļ���
 *wPoints������������
 */
/************************************************************************/
static float calCoilLeftDist(Rect location, float BRatio, vector<Point2f> points, vector<Point2f> wPoints);

/************************************************************************/
/*����������Ȧ�ұ߽����ߵľ��루�������꣩
 *location��������Ȧ��λ������
 *BRatio��������
 *points���궨��ļ���
 *wPoints������������
 */
/************************************************************************/
static float calCoilRightDist(Rect location, float BRatio, vector<Point2f> points, vector<Point2f> wPoints);

//static vector<float> virtualCoilWDist(Rect location, float BRatio, vector<Point2f> points, vector<Point2f> wPoints);

//ͨ���������������������
static vector<Point2f> worldPoints(int laneNum);

//���㲹����;points�Ǳ궨��ļ���,wPoints������������
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
//	Mat ML;//���Է��̵������
//	Mat MR;//���Է��̵��Ҿ���
//	Mat Coefficient;//ϵ����
};