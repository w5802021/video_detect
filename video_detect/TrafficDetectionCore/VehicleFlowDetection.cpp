#include "VehicleFlowDetection.h"

VehicleFlowDetection::VehicleFlowDetection(int LNum)
{
	carNum = 0;
	laneNum = LNum;
}
int VehicleFlowDetection::reCarNum()
{
	return carNum;
}
VehicleFlowDetection::~VehicleFlowDetection()
{
}
vector<int> VehicleFlowDetection::reActiveCoils()
{
	return activeCoils;
}
vector<VirtualCoil*> VehicleFlowDetection::start(vector<VirtualCoil*> coils, Mat* frame)
{
	//**************************对每一个虚拟框中的点进行测量***************************
	for (size_t i = 0; i < coils.size(); i++)
	{
		coils[i]->compareSimilarity((*frame)(coils[i]->reLocation()));
        cout <<"第" << i << "个线圈的相似度为：" << coils[i]->reMeansSimilarityScale() << endl;
	}
	for (size_t i = 0; i < coils.size(); i++)
	{
		//cout << "************************" << "这是第" << count << "帧" <<"的第"<<i+1<<"个线圈"<< "***************" << endl;
		coils[i]->otherFrameForegroundInit(&(*frame)(coils[i]->reLocation()));
		//coils[i].imageAreaRatio();//计算当前帧所占的面积比例
		//cout << "返回触线标志位：" << coils[i].returnFlag() << endl;
		if (coils[i]->judgeCarThrough() && coils[i]->returnCarFrameNum() > DemarcateParameter::min_car_frame_num)
		{
			/*cout << "非临时线圈" << i << "检测出了车辆" << endl;
			cout << "车辆走的帧数为：" << coils[i].returnCarFrameNum() << endl;*/
			activeCoils.push_back(i);
		}
		//获取所有的正在检测的线圈
		if (coils[i]->reMeansSimilarityScale() < DemarcateParameter::similarity_scale_threshold && coils[i]->returnFlag() == true)
		{
			testingCoils.push_back(i);
		}
	}
	return coils;
}

vector<VirtualCoil*> VehicleFlowDetection::start2(vector<VirtualCoil*> coils,Mat* frame,int j)
{
	//**************************对每一个虚拟框中的点进行测量***************************
	coils[j]->compareSimilarity((*frame)(coils[j]->reLocation()));
	//coils[j]->compareSimilarity((*frame)(coils[j]->reLocation()));
        //cout <<"第" << 0 << "个线圈的相似度为：" << coils->reMeansSimilarityScale() << endl;

	//cout << "************************" << "这是第" << count << "帧" <<"的第"<<i+1<<"个线圈"<< "***************" << endl;
		coils[j]->otherFrameForegroundInit(&(*frame)(coils[j]->reLocation()));
		//coils[i].imageAreaRatio();//计算当前帧所占的面积比例
		//cout << "返回触线标志位：" << coils[i].returnFlag() << endl;
		if (coils[j]->judgeCarThrough() && coils[j]->returnCarFrameNum() > DemarcateParameter::min_car_frame_num)
		{
			/*cout << "非临时线圈" << i << "检测出了车辆" << endl;
			cout << "车辆走的帧数为：" << coils[i].returnCarFrameNum() << endl;*/
			activeCoils.push_back(j);
		}
		//获取所有的正在检测的线圈,用于跨边检测
		if (coils[j]->reMeansSimilarityScale() < DemarcateParameter::similarity_scale_threshold && coils[j]->returnFlag() == true)
		{
			coils[j]->testing=true;
			//testingCoils.push_back(j);
		}
	return coils;
}

vector<int> VehicleFlowDetection::reTestingCoils()
{
	return testingCoils;
}

vector<crossEdageCoil*> VehicleFlowDetection::cross_edage2(VirtualCoil* coil1,VirtualCoil* coil2,vector<crossEdageCoil*> crossEdageCoils)
{
	//如果正在检测的虚拟线圈小于2个，则不存在跨边车辆，直接返回
	if (!coil1->testing||!coil2->testing)
		return crossEdageCoils;

	crossEdageCoil* tempCrossEdageCoil=new crossEdageCoil();//临时的跨边线圈

	if ((coil1->leftAndRightBoundaryDetection() == "right" || coil1->leftAndRightBoundaryDetection() == "left_and_right") && (coil2->leftAndRightBoundaryDetection() == "left" || coil2->leftAndRightBoundaryDetection() == "left_and_right"))
	{
		//cout << "检测到了跨边超数的情况！4" << endl;
		if (crossEdageCoils.size() != 0)
		{
			for (size_t m = 0; m < crossEdageCoils.size(); m++)
			{
				if (crossEdageCoils[m]->start == coil1->id || crossEdageCoils[m]->end == coil1->id)
				{
					//cout << "有相等的" << endl;
					//crossEdageCoils[m].frameNum = count;
					break;
				}
				else
				{
					//cout << "没有相等的" << endl;
					tempCrossEdageCoil->start = coil1->id;
					tempCrossEdageCoil->end = coil2->id;
					//tempCrossEdageCoil.frameNum = count;
					crossEdageCoils.push_back(tempCrossEdageCoil);
				}
			}
		}
		else
		{
			//cout << "初始化" << endl;
			tempCrossEdageCoil->start = coil1->id;
			tempCrossEdageCoil->end = coil2->id;
			/*tempCrossEdageCoil.frameNum = count;*/
			crossEdageCoils.push_back(tempCrossEdageCoil);
		}
	}

	return crossEdageCoils;

}
vector<crossEdageCoil*> VehicleFlowDetection::cross_edage3(VirtualCoil* coil1,vector<crossEdageCoil*> crossEdageCoils)
{
	crossEdageCoil* tempCrossEdageCoil=new crossEdageCoil();//临时的跨边线圈

	if (coil1->leftAndRightBoundaryDetection() == "right" || coil1->leftAndRightBoundaryDetection() == "left_and_right")
	{
		if (crossEdageCoils.size() != 0)
		{
			for (size_t m = 0; m < crossEdageCoils.size(); m++)
			{
				if (crossEdageCoils[m]->start == coil1->id || crossEdageCoils[m]->end == coil1->id)
				{
					//cout << "有相等的" << endl;
					//crossEdageCoils[m].frameNum = count;
					break;
				}
				else
				{
					//cout << "没有相等的" << endl;
					tempCrossEdageCoil->start = coil1->id;
					tempCrossEdageCoil->end = -1;
					//tempCrossEdageCoil.frameNum = count;
					crossEdageCoils.push_back(tempCrossEdageCoil);
				}
			}
		}
		else
		{
			//cout << "初始化" << endl;
			tempCrossEdageCoil->start = coil1->id;
			tempCrossEdageCoil->end = -1;
			/*tempCrossEdageCoil.frameNum = count;*/
			crossEdageCoils.push_back(tempCrossEdageCoil);
		}
	}
	return crossEdageCoils;
}
void VehicleFlowDetection::cross_edage(vector<VirtualCoil*> coils, vector<crossEdageCoil*> crossEdageCoils, vector<int> testingCoils)
{
	//计算当前帧虚拟线圈的个数
	int testingCoilsNum = testingCoils.size();
	crossEdageCoil* tempCrossEdageCoil=new crossEdageCoil();//临时的跨边线圈
	//检测单车跨边的情况，要生成一个动态的虚拟线圈
	if (testingCoilsNum >= 2)
	{
		for (int i = 0; i < testingCoilsNum - 1; i++)
		{
			//不是临时线圈才开始
			//if (!coils[testingCoils[i]].reTempCoilFlag())
			{
				if ((testingCoils[i + 1] - testingCoils[i]) == 1)
				{
					if ((coils[testingCoils[i]]->leftAndRightBoundaryDetection() == "right" || coils[testingCoils[i]]->leftAndRightBoundaryDetection() == "left_and_right") && (coils[testingCoils[i + 1]]->leftAndRightBoundaryDetection() == "left" || coils[testingCoils[i + 1]]->leftAndRightBoundaryDetection() == "left_and_right"))
					{
						//cout << "检测到了跨边超数的情况！4" << endl;
						if (crossEdageCoils.size() != 0)
						{
							for (size_t m = 0; m < crossEdageCoils.size(); m++)
							{
								if (crossEdageCoils[m]->start == testingCoils[i] || crossEdageCoils[m]->end == testingCoils[i])
								{
									//cout << "有相等的" << endl;
									//crossEdageCoils[m].frameNum = count;
									break;
								}
								else
								{
									//cout << "没有相等的" << endl;
									tempCrossEdageCoil->start = testingCoils[i];
									tempCrossEdageCoil->end = testingCoils[i + 1];
									//tempCrossEdageCoil.frameNum = count;
									crossEdageCoils.push_back(tempCrossEdageCoil);
								}
							}
						}
						else
						{
							//cout << "初始化" << endl;
							tempCrossEdageCoil->start = testingCoils[i];
							tempCrossEdageCoil->end = testingCoils[i + 1];
							/*tempCrossEdageCoil.frameNum = count;*/
							crossEdageCoils.push_back(tempCrossEdageCoil);
						}

					}
				}
			}

		}

	}

	delete tempCrossEdageCoil;
	//return crossEdageCoils;
}

int VehicleFlowDetection::count2(vector<crossEdageCoil*> crossEdageCoils)
{
	//计数函数

	for (size_t i = 0; i < crossEdageCoils.size(); i++)
	{
		for (size_t j = 0; j < activeCoils.size(); j++)
		{
			//谁先出谁先入是不一定的
			if (crossEdageCoils[i]->start == activeCoils[j] && crossEdageCoils[i]->end != -1)
			{
				/*cout << "我是" << endl;*/
				crossEdageCoils[i]->start = -1;
			}
			if (crossEdageCoils[i]->end == activeCoils[j] && crossEdageCoils[i]->end != -1)
			{
				/*cout << "我是" << endl;*/
				vector<crossEdageCoil*>::iterator it1 = crossEdageCoils.begin() + i;
				crossEdageCoils.erase(it1);
				vector<int>::iterator it2 = activeCoils.begin() + j;
				activeCoils.erase(it2);
			}
			if (crossEdageCoils[i]->end == activeCoils[j] && crossEdageCoils[i]->start != -1)
			{
				/*cout << "我是" << endl;*/
				crossEdageCoils[i]->end = -1;
			}
			if (crossEdageCoils[i]->start == activeCoils[j] && crossEdageCoils[i]->start != -1)
			{
				/*cout << "我是" << endl;*/
				vector<crossEdageCoil*>::iterator it1 = crossEdageCoils.begin() + i;
				crossEdageCoils.erase(it1);
				vector<int>::iterator it2 = activeCoils.begin() + j;
				activeCoils.erase(it2);
			}
		}

	}

	if(activeCoils.empty())
		return 0;

	int activCoilNum = activeCoils.size();
	//carNum += activCoilNum;

	//如果大于车道数全清为零
	if (int(crossEdageCoils.size()) > laneNum)
	{
		vector<crossEdageCoil*>().swap(crossEdageCoils);
	}
	/*cout << "跨边线圈对的数目：" << crossEdageCoils.size() << endl;
	cout << "检测出有车辆的线圈的个数" << activeCoils.size() << endl;*/
	/*cout << "现在总的车辆数为：" << carNum << endl;*/
	
	//清空跨边线圈对
	//crossEdageCoils->clear();
	//vector<crossEdageCoil*>().swap(crossEdageCoils);
	vector<int>().swap(activeCoils);

	return activCoilNum;
}
vector<crossEdageCoil*> VehicleFlowDetection::count(vector<crossEdageCoil*> crossEdageCoils)
{
	//计数函数
	for (size_t i = 0; i < crossEdageCoils.size(); i++)
	{
		for (size_t j = 0; j < activeCoils.size(); j++)
		{
			//谁先出谁先入是不一定的
			if (crossEdageCoils[i]->start == activeCoils[j] && crossEdageCoils[i]->end != -1)
			{
				/*cout << "我是" << endl;*/
				crossEdageCoils[i]->start = -1;
			}
			if (crossEdageCoils[i]->end == activeCoils[j] && crossEdageCoils[i]->end != -1)
			{
				/*cout << "我是" << endl;*/
				vector<crossEdageCoil*>::iterator it1 = crossEdageCoils.begin() + i;
				crossEdageCoils.erase(it1);
				vector<int>::iterator it2 = activeCoils.begin() + j;
				activeCoils.erase(it2);
			}
			if (crossEdageCoils[i]->end == activeCoils[j] && crossEdageCoils[i]->start != -1)
			{
				/*cout << "我是" << endl;*/
				crossEdageCoils[i]->end = -1;
			}
			if (crossEdageCoils[i]->start == activeCoils[j] && crossEdageCoils[i]->start != -1)
			{
				/*cout << "我是" << endl;*/
				vector<crossEdageCoil*>::iterator it1 = crossEdageCoils.begin() + i;
				crossEdageCoils.erase(it1);
				vector<int>::iterator it2 = activeCoils.begin() + j;
				activeCoils.erase(it2);
			}
		}
	}
	int activCoilNum = activeCoils.size();
	carNum += activCoilNum;
	/*cout << "跨边线圈对的数目：" << crossEdageCoils.size() << endl;
	cout << "检测出有车辆的线圈的个数" << activeCoils.size() << endl;*/
	/*cout << "现在总的车辆数为：" << carNum << endl;*/
	//如果大于车道数全清为零
	if (int(crossEdageCoils.size()) > laneNum)
	{
		vector<crossEdageCoil*>().swap(crossEdageCoils);
	}
	//vector<int>().swap(activeCoils);//释放activeCoils的内存空间
	vector<int>().swap(testingCoils);//释放正在检测的虚拟线圈
	return crossEdageCoils;
}
