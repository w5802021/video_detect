#pragma once
class DemarcateParameter
{
public:
	static float WhiteLineLength; //白线的长度
	static float WhiteLineWidth; //白线的宽度
	static float WhiteLineIntervalLR;//左右两条白线的间隔;车宽度

	static float Small_Car_Length_Threshold ;//小型车长阈值

	static float Middle_Car_Length_Threshold ;//中型车长阈值
	
	static int min_car_frame_num ;//最小的车尾通过虚拟线圈的帧数

	static double touchEdgeThreshold ;//const double touchEdgeThreshold = 0.3;//边的阈值???

	static double leftAndRightBoundaryRatio ;//0.35;//左右边界检测的最小比率

	static double similarity_scale_threshold ;//0.9;//相似度阈值

};


//{
	/*
	//标定中车道的规格
	const float WhiteLineLength = 600;//白线的长度
	const float WhiteLineWidth = 15;//白线的宽度
	const float WhiteLineIntervalLR = 375;//左右两条白线的间隔*/
	const float WhiteLineIntervalUD = 900;//上下两条白线的间隔
	const float B_Line_W_Distance = 600;//补偿线段的世界长度
	const float Angle_Body_Slope_Ratio = 0.5;//角度车身倾斜率（与求车长有关）
	//const float Small_Car_Length_Threshold = 4;//小型车长阈值
	//const float Middle_Car_Length_Threshold = 8;//中型车长阈值

	//原来VehicleFlowDetection.h定义
	//const int min_car_frame_num = 5;//最小的车尾通过虚拟线圈的帧数

	//原来VirtualCoil.h定义
	const double areaRatioThreshold = 0.4;
	const double tempAreaRatioThreshold = 0.3;
	//const double touchEdgeThreshold = 0.298;//const double touchEdgeThreshold = 0.3;//边的阈值???
	const float MS_TO_KMH = float(3.6 / 100);//将速度中的m/s转化为km/h
	//const double leftAndRightBoundaryRatio = 0.35;//0.35;//左右边界检测的最小比率
	const double temp_first_diff = 0.1;//临时线圈退出条件
	//const double similarity_scale_threshold = 0.93;//0.9;//相似度阈值
	

	
//};