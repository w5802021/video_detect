#include "DemarcateTransform.h"

/************************************************************************/
/*计算虚拟线圈左边界竖线的距离（世界坐标）
 *location：虚拟线圈的位置坐标
 *BRatio：补偿率
 *points：标定点的集合
 *wPoints：世界点的坐标
 */
/************************************************************************/
float DemarcateTransform::calCoilLeftDist(Rect location, float BRatio, vector<Point2f> points, vector<Point2f> wPoints)
{
	vector<Point2f> leftPoints(2);//虚拟线圈的左边两端点
	Point2f leftUpPoint;//虚拟线圈的左上点
	Point2f leftDownPoint;//虚拟线圈的左下点
	leftUpPoint = Point2f(float(location.x), float(location.y));
	leftDownPoint = Point2f(float(location.x), float(location.y + location.height));

	//初始化leftPoints,虚拟线圈的左边高度的两点图像坐标
	leftPoints[0] = leftUpPoint;
	leftPoints[1] = leftDownPoint;

	//Demarcate_Point yes_de_left = Demarcate_Point(wPoints, points, leftPoints);
	//float yes_distance_left = yes_de_left.yes_distanceFun(BRatio);

	float no_distance=no_distanceFun(wPoints,points,leftPoints);
	float yes_distance_left=yes_distanceFun(BRatio,no_distance);

	return yes_distance_left;

}

/************************************************************************/
/*计算虚拟线圈右边界竖线的距离（世界坐标）
 *location：虚拟线圈的位置坐标
 *BRatio：补偿率
 *points：标定点的集合
 *wPoints：世界点的坐标
 */
/************************************************************************/
float DemarcateTransform::calCoilRightDist(Rect location, float BRatio, vector<Point2f> points, vector<Point2f> wPoints)
{
	vector<Point2f> rightPoints(2);//虚拟线圈的右边两端点
	Point2f rightUpPoint;//虚拟线圈的右上点
	Point2f rightDownPoint;//虚拟线圈的右下点
	rightUpPoint = Point2f(float(location.x + location.width), float(location.y));
	rightDownPoint = Point2f(float(location.x + location.width), float(location.y + location.height));

	//初始化rightPoints
	rightPoints[0] = rightUpPoint;
	rightPoints[1] = rightDownPoint;

	//Demarcate_Point yes_de_right = Demarcate_Point(wPoints, points, rightPoints);
	//float yes_distance_right = yes_de_right.yes_distanceFun(BRatio);
	
	float no_distance=no_distanceFun(wPoints,points,rightPoints);
	float yes_distance_right=yes_distanceFun(BRatio,no_distance);

	return yes_distance_right;

}

//通过车道数计算出世界坐标
vector<Point2f> DemarcateTransform::worldPoints(int laneNum)
{
	vector<Point2f> wPoints;
	Point2f wp1, wp2, wp3, wp4;
	float x = laneNum*DemarcateParameter::WhiteLineIntervalLR + DemarcateParameter::WhiteLineWidth*(laneNum - 1);
	float y = DemarcateParameter::WhiteLineLength;
	wp1.x = 0; wp1.y = 0; wPoints.push_back(wp1); wp2.x = x; wp2.y = 0; wPoints.push_back(wp2);
	wp3.x = x; wp3.y = y; wPoints.push_back(wp3); wp4.x = 0; wp4.y = y; wPoints.push_back(wp4);

	return wPoints;
}
//计算补偿率;points是标定点的集合,wPoints是世界点的坐标
float DemarcateTransform::calculateBRatio(vector<Point2f> points, vector<Point2f> wPoints)
{
	vector<Point2f> PB;//补偿线段的两端点
	PB.push_back(points[4]);
	PB.push_back(points[5]);
	//根据转换矩阵计算补偿线段世界坐标下的估计坐标点（两点）
	/*Demarcate_Point no_de = Demarcate_Point(wPoints, points, PB);
	//根据转换矩阵计算补偿线段世界坐标下的估计长度
	float no_distance = no_de.no_distanceFun();
	cout << L"参考标定为:" << no_distance << endl;
	float a = no_de.calculCompensate(B_Line_W_Distance);*/

	//根据转换矩阵计算补偿线段世界坐标下的估计长度
	float no_distance =no_distanceFun(wPoints, points,PB);
	float a =calculCompensate(B_Line_W_Distance,no_distance);
	return a;
}

float DemarcateTransform::twoCoilDistance(Rect location, Rect locationsSpeed, float BRatio,vector<Point2f> points, vector<Point2f> wPoints)
{
	vector<Point2f> leftPoints(2);//虚拟线圈的左边两端点
	Point2f leftUpPoint;//虚拟线圈的左上点
	Point2f leftDownPoint;//虚拟线圈的左下点
	leftUpPoint = Point2f(float(location.x), float(location.y));
	leftDownPoint = Point2f(float(locationsSpeed.x), float(locationsSpeed.y));
	leftPoints[0] = leftUpPoint;
	leftPoints[1] = leftDownPoint;
	/*Demarcate_Point yes_de_left = Demarcate_Point(wPoints, points, leftPoints);
	float yes_distance_left = yes_de_left.yes_distanceFun(BRatio);*/
	float no_diatance=no_distanceFun(wPoints, points,leftPoints);
	float yes_distance_left =yes_distanceFun(BRatio,no_diatance);
	return yes_distance_left;
}

Mat DemarcateTransform::calLeftMatrix4Equations(vector<Point2f> wPoints, vector<Point2f> iPoints)
{
	Mat ML = Mat(8, 8, CV_32F);//旋转矩阵R
	float m[][8] = { iPoints[0].x, iPoints[0].y, 1, 0, 0, 0, -iPoints[0].x*wPoints[0].x, -iPoints[0].y*wPoints[0].x,
		iPoints[1].x, iPoints[1].y, 1, 0, 0, 0, -iPoints[1].x*wPoints[1].x, -iPoints[1].y*wPoints[1].x,
		iPoints[2].x, iPoints[2].y, 1, 0, 0, 0, -iPoints[2].x*wPoints[2].x, -iPoints[2].y*wPoints[2].x,
		iPoints[3].x, iPoints[3].y, 1, 0, 0, 0, -iPoints[3].x*wPoints[3].x, -iPoints[3].y*wPoints[3].x,
		0, 0, 0, iPoints[0].x, iPoints[0].y, 1, -iPoints[0].x*wPoints[0].y, -iPoints[0].y*wPoints[0].y,
		0, 0, 0, iPoints[1].x, iPoints[1].y, 1, -iPoints[1].x*wPoints[1].y, -iPoints[1].y*wPoints[1].y,
		0, 0, 0, iPoints[2].x, iPoints[2].y, 1, -iPoints[2].x*wPoints[2].y, -iPoints[2].y*wPoints[2].y,
		0, 0, 0, iPoints[3].x, iPoints[3].y, 1, -iPoints[3].x*wPoints[3].y, -iPoints[3].y*wPoints[3].y };

	//通过二维数组将8*8矩阵初始化
	for (int i = 0; i<ML.rows; i++)
		for (int j = 0; j<ML.cols; j++)
			ML.at<float>(i, j) = *(*(m + i) + j);

	return ML;
}

Mat DemarcateTransform::calRightMatrix4Equations(vector<Point2f> wPoints, vector<Point2f> iPoints)
{
	Mat MR = Mat(8, 1, CV_32F);//平移矩阵T
	float n[][1] = { wPoints[0].x, wPoints[1].x, wPoints[2].x, wPoints[3].x, wPoints[0].y, wPoints[1].y, wPoints[2].y, wPoints[3].y };

	//通过二维数组将8*1矩阵初始化
	for (int i = 0; i<MR.rows; i++)
		for (int j = 0; j<MR.cols; j++)
			MR.at<float>(i, j) = *(*(n + i) + j);

	return MR;
}

Mat DemarcateTransform::solveEquations(Mat ML,Mat MR)
{
	Mat MInverse;
	MInverse = ML.inv(DECOMP_LU);
	Mat Coefficient = MInverse*MR;
	return Coefficient;
}

vector<Point2f> DemarcateTransform::transformToWorldCoordinate(vector<Point2f> p,Mat Coefficient)
{
	vector<Point2f> toWPoints;

	Point2f mask;
	float a = Coefficient.at<float>(0, 0);
	float b = Coefficient.at<float>(1, 0);
	float c = Coefficient.at<float>(2, 0);
	float d = Coefficient.at<float>(3, 0);
	float e = Coefficient.at<float>(4, 0);
	float f = Coefficient.at<float>(5, 0);
	float g = Coefficient.at<float>(6, 0);
	float h = Coefficient.at<float>(7, 0);
	float upX = a*p[0].x + b*p[0].y + c;
	float sub = g*p[0].x + h*p[0].y + 1;
	float upY = d*p[0].x + e*p[0].y + f;
	mask.x = upX / sub;
	mask.y = upY / sub;
	toWPoints.push_back(mask);
	upX = a*p[1].x + b*p[1].y + c;
	sub = g*p[1].x + h*p[1].y + 1;
	upY = d*p[1].x + e*p[1].y + f;
	mask.x = upX / sub;
	mask.y = upY / sub;
	toWPoints.push_back(mask);
	return toWPoints;
}

float DemarcateTransform::no_distanceFun(vector<Point2f> toWPoints)
{
	float a = toWPoints[0].x - toWPoints[1].x;
	float b = toWPoints[0].y - toWPoints[1].y;
	float no_distance = sqrt(a*a + b*b);
	return no_distance;
}
float DemarcateTransform::yes_distanceFun(float compensateRatio,float no_distance)
{
	float yes_distance = (compensateRatio)*no_distance + no_distance;
	return yes_distance;
}
//补偿率计算：（真实补偿线段距离-估计距离）/真实补偿线段距离
float DemarcateTransform::calculCompensate(float realValue,float no_distance)
{
	float compensateRatio = (realValue - no_distance) / realValue;
	return compensateRatio;
}

float DemarcateTransform::no_distanceFun(vector<Point2f> wPoints, vector<Point2f> iPoints,vector<Point2f> transformPoints)
{
	Mat ML=calLeftMatrix4Equations(wPoints, iPoints);
	Mat MR=calRightMatrix4Equations(wPoints, iPoints);
	Mat coefficient=solveEquations(ML,MR);
	vector<Point2f> toWPoints =transformToWorldCoordinate(transformPoints,coefficient);
	float no_distance=no_distanceFun(toWPoints);

	return no_distance;
}

/*
//计算虚拟线圈左右边界的世界距离;location虚拟线圈的位置坐标；BRatio补偿率；points是标定点的集合,wPoints是世界点的坐标
vector<float> DemarcateTransform::virtualCoilWDist(Rect location, float BRatio, vector<Point2f> points, vector<Point2f> wPoints)
{
	vector<Point2f> leftPoints(2);//虚拟线圈的左边两端点
	vector<Point2f> rightPoints(2);//虚拟线圈的右边两端点
	Point2f leftUpPoint;//虚拟线圈的左上点
	Point2f leftDownPoint;//虚拟线圈的左下点
	Point2f rightUpPoint;//虚拟线圈的右上点
	Point2f rightDownPoint;//虚拟线圈的右下点
	vector<float> result;//结果容器
	leftUpPoint = Point2f(float(location.x), float(location.y));
	leftDownPoint = Point2f(float(location.x), float(location.y + location.height));
	rightUpPoint = Point2f(float(location.x + location.width), float(location.y));
	rightDownPoint = Point2f(float(location.x + location.width), float(location.y + location.height));
	//初始化leftPoints,虚拟线圈的左边高度的两点图像坐标
	leftPoints[0] = leftUpPoint;
	leftPoints[1] = leftDownPoint;
	//初始化rightPoints
	rightPoints[0] = rightUpPoint;
	rightPoints[1] = rightDownPoint;
	Demarcate_Point yes_de_left = Demarcate_Point(wPoints, points, leftPoints);
	float yes_distance_left = yes_de_left.yes_distanceFun(BRatio);
	result.push_back(yes_distance_left);
	Demarcate_Point yes_de_right = Demarcate_Point(wPoints, points, rightPoints);
	float yes_distance_right = yes_de_right.yes_distanceFun(BRatio);
	result.push_back(yes_distance_right);
	return result;
}
*/