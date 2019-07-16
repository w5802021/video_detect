#pragma once
class DemarcateParameter
{
public:
	static float WhiteLineLength; //���ߵĳ���
	static float WhiteLineWidth; //���ߵĿ��
	static float WhiteLineIntervalLR;//�����������ߵļ��;�����

	static float Small_Car_Length_Threshold ;//С�ͳ�����ֵ

	static float Middle_Car_Length_Threshold ;//���ͳ�����ֵ
	
	static int min_car_frame_num ;//��С�ĳ�βͨ��������Ȧ��֡��

	static double touchEdgeThreshold ;//const double touchEdgeThreshold = 0.3;//�ߵ���ֵ???

	static double leftAndRightBoundaryRatio ;//0.35;//���ұ߽������С����

	static double similarity_scale_threshold ;//0.9;//���ƶ���ֵ

};


//{
	/*
	//�궨�г����Ĺ��
	const float WhiteLineLength = 600;//���ߵĳ���
	const float WhiteLineWidth = 15;//���ߵĿ��
	const float WhiteLineIntervalLR = 375;//�����������ߵļ��*/
	const float WhiteLineIntervalUD = 900;//�����������ߵļ��
	const float B_Line_W_Distance = 600;//�����߶ε����糤��
	const float Angle_Body_Slope_Ratio = 0.5;//�Ƕȳ�����б�ʣ����󳵳��йأ�
	//const float Small_Car_Length_Threshold = 4;//С�ͳ�����ֵ
	//const float Middle_Car_Length_Threshold = 8;//���ͳ�����ֵ

	//ԭ��VehicleFlowDetection.h����
	//const int min_car_frame_num = 5;//��С�ĳ�βͨ��������Ȧ��֡��

	//ԭ��VirtualCoil.h����
	const double areaRatioThreshold = 0.4;
	const double tempAreaRatioThreshold = 0.3;
	//const double touchEdgeThreshold = 0.298;//const double touchEdgeThreshold = 0.3;//�ߵ���ֵ???
	const float MS_TO_KMH = float(3.6 / 100);//���ٶ��е�m/sת��Ϊkm/h
	//const double leftAndRightBoundaryRatio = 0.35;//0.35;//���ұ߽������С����
	const double temp_first_diff = 0.1;//��ʱ��Ȧ�˳�����
	//const double similarity_scale_threshold = 0.93;//0.9;//���ƶ���ֵ
	

	
//};