#include "DetectionParameter.h"


float DemarcateParameter::WhiteLineLength=500;
float DemarcateParameter::WhiteLineWidth=15;
float DemarcateParameter::WhiteLineIntervalLR=375;

float DemarcateParameter::Small_Car_Length_Threshold=4;//小型车长阈值
float DemarcateParameter::Middle_Car_Length_Threshold=8;//中型车长阈值
int DemarcateParameter::min_car_frame_num=5;//最小的车尾通过虚拟线圈的帧数
double DemarcateParameter::touchEdgeThreshold=0.298;//0.3;//边的阈值???
double DemarcateParameter::leftAndRightBoundaryRatio=0.35;//0.35;//左右边界检测的最小比率
double DemarcateParameter::similarity_scale_threshold=0.93;//0.9;//相似度阈值