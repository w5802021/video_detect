#include "DetectionParameter.h"


float DemarcateParameter::WhiteLineLength=500;
float DemarcateParameter::WhiteLineWidth=15;
float DemarcateParameter::WhiteLineIntervalLR=375;

float DemarcateParameter::Small_Car_Length_Threshold=4;//С�ͳ�����ֵ
float DemarcateParameter::Middle_Car_Length_Threshold=8;//���ͳ�����ֵ
int DemarcateParameter::min_car_frame_num=5;//��С�ĳ�βͨ��������Ȧ��֡��
double DemarcateParameter::touchEdgeThreshold=0.298;//0.3;//�ߵ���ֵ???
double DemarcateParameter::leftAndRightBoundaryRatio=0.35;//0.35;//���ұ߽������С����
double DemarcateParameter::similarity_scale_threshold=0.93;//0.9;//���ƶ���ֵ