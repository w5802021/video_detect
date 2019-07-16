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
	//**************************��ÿһ��������еĵ���в���***************************
	for (size_t i = 0; i < coils.size(); i++)
	{
		coils[i]->compareSimilarity((*frame)(coils[i]->reLocation()));
        cout <<"��" << i << "����Ȧ�����ƶ�Ϊ��" << coils[i]->reMeansSimilarityScale() << endl;
	}
	for (size_t i = 0; i < coils.size(); i++)
	{
		//cout << "************************" << "���ǵ�" << count << "֡" <<"�ĵ�"<<i+1<<"����Ȧ"<< "***************" << endl;
		coils[i]->otherFrameForegroundInit(&(*frame)(coils[i]->reLocation()));
		//coils[i].imageAreaRatio();//���㵱ǰ֡��ռ���������
		//cout << "���ش��߱�־λ��" << coils[i].returnFlag() << endl;
		if (coils[i]->judgeCarThrough() && coils[i]->returnCarFrameNum() > DemarcateParameter::min_car_frame_num)
		{
			/*cout << "����ʱ��Ȧ" << i << "�����˳���" << endl;
			cout << "�����ߵ�֡��Ϊ��" << coils[i].returnCarFrameNum() << endl;*/
			activeCoils.push_back(i);
		}
		//��ȡ���е����ڼ�����Ȧ
		if (coils[i]->reMeansSimilarityScale() < DemarcateParameter::similarity_scale_threshold && coils[i]->returnFlag() == true)
		{
			testingCoils.push_back(i);
		}
	}
	return coils;
}

vector<VirtualCoil*> VehicleFlowDetection::start2(vector<VirtualCoil*> coils,Mat* frame,int j)
{
	//**************************��ÿһ��������еĵ���в���***************************
	coils[j]->compareSimilarity((*frame)(coils[j]->reLocation()));
	//coils[j]->compareSimilarity((*frame)(coils[j]->reLocation()));
        //cout <<"��" << 0 << "����Ȧ�����ƶ�Ϊ��" << coils->reMeansSimilarityScale() << endl;

	//cout << "************************" << "���ǵ�" << count << "֡" <<"�ĵ�"<<i+1<<"����Ȧ"<< "***************" << endl;
		coils[j]->otherFrameForegroundInit(&(*frame)(coils[j]->reLocation()));
		//coils[i].imageAreaRatio();//���㵱ǰ֡��ռ���������
		//cout << "���ش��߱�־λ��" << coils[i].returnFlag() << endl;
		if (coils[j]->judgeCarThrough() && coils[j]->returnCarFrameNum() > DemarcateParameter::min_car_frame_num)
		{
			/*cout << "����ʱ��Ȧ" << i << "�����˳���" << endl;
			cout << "�����ߵ�֡��Ϊ��" << coils[i].returnCarFrameNum() << endl;*/
			activeCoils.push_back(j);
		}
		//��ȡ���е����ڼ�����Ȧ,���ڿ�߼��
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
	//������ڼ���������ȦС��2�����򲻴��ڿ�߳�����ֱ�ӷ���
	if (!coil1->testing||!coil2->testing)
		return crossEdageCoils;

	crossEdageCoil* tempCrossEdageCoil=new crossEdageCoil();//��ʱ�Ŀ����Ȧ

	if ((coil1->leftAndRightBoundaryDetection() == "right" || coil1->leftAndRightBoundaryDetection() == "left_and_right") && (coil2->leftAndRightBoundaryDetection() == "left" || coil2->leftAndRightBoundaryDetection() == "left_and_right"))
	{
		//cout << "��⵽�˿�߳����������4" << endl;
		if (crossEdageCoils.size() != 0)
		{
			for (size_t m = 0; m < crossEdageCoils.size(); m++)
			{
				if (crossEdageCoils[m]->start == coil1->id || crossEdageCoils[m]->end == coil1->id)
				{
					//cout << "����ȵ�" << endl;
					//crossEdageCoils[m].frameNum = count;
					break;
				}
				else
				{
					//cout << "û����ȵ�" << endl;
					tempCrossEdageCoil->start = coil1->id;
					tempCrossEdageCoil->end = coil2->id;
					//tempCrossEdageCoil.frameNum = count;
					crossEdageCoils.push_back(tempCrossEdageCoil);
				}
			}
		}
		else
		{
			//cout << "��ʼ��" << endl;
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
	crossEdageCoil* tempCrossEdageCoil=new crossEdageCoil();//��ʱ�Ŀ����Ȧ

	if (coil1->leftAndRightBoundaryDetection() == "right" || coil1->leftAndRightBoundaryDetection() == "left_and_right")
	{
		if (crossEdageCoils.size() != 0)
		{
			for (size_t m = 0; m < crossEdageCoils.size(); m++)
			{
				if (crossEdageCoils[m]->start == coil1->id || crossEdageCoils[m]->end == coil1->id)
				{
					//cout << "����ȵ�" << endl;
					//crossEdageCoils[m].frameNum = count;
					break;
				}
				else
				{
					//cout << "û����ȵ�" << endl;
					tempCrossEdageCoil->start = coil1->id;
					tempCrossEdageCoil->end = -1;
					//tempCrossEdageCoil.frameNum = count;
					crossEdageCoils.push_back(tempCrossEdageCoil);
				}
			}
		}
		else
		{
			//cout << "��ʼ��" << endl;
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
	//���㵱ǰ֡������Ȧ�ĸ���
	int testingCoilsNum = testingCoils.size();
	crossEdageCoil* tempCrossEdageCoil=new crossEdageCoil();//��ʱ�Ŀ����Ȧ
	//��ⵥ����ߵ������Ҫ����һ����̬��������Ȧ
	if (testingCoilsNum >= 2)
	{
		for (int i = 0; i < testingCoilsNum - 1; i++)
		{
			//������ʱ��Ȧ�ſ�ʼ
			//if (!coils[testingCoils[i]].reTempCoilFlag())
			{
				if ((testingCoils[i + 1] - testingCoils[i]) == 1)
				{
					if ((coils[testingCoils[i]]->leftAndRightBoundaryDetection() == "right" || coils[testingCoils[i]]->leftAndRightBoundaryDetection() == "left_and_right") && (coils[testingCoils[i + 1]]->leftAndRightBoundaryDetection() == "left" || coils[testingCoils[i + 1]]->leftAndRightBoundaryDetection() == "left_and_right"))
					{
						//cout << "��⵽�˿�߳����������4" << endl;
						if (crossEdageCoils.size() != 0)
						{
							for (size_t m = 0; m < crossEdageCoils.size(); m++)
							{
								if (crossEdageCoils[m]->start == testingCoils[i] || crossEdageCoils[m]->end == testingCoils[i])
								{
									//cout << "����ȵ�" << endl;
									//crossEdageCoils[m].frameNum = count;
									break;
								}
								else
								{
									//cout << "û����ȵ�" << endl;
									tempCrossEdageCoil->start = testingCoils[i];
									tempCrossEdageCoil->end = testingCoils[i + 1];
									//tempCrossEdageCoil.frameNum = count;
									crossEdageCoils.push_back(tempCrossEdageCoil);
								}
							}
						}
						else
						{
							//cout << "��ʼ��" << endl;
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
	//��������

	for (size_t i = 0; i < crossEdageCoils.size(); i++)
	{
		for (size_t j = 0; j < activeCoils.size(); j++)
		{
			//˭�ȳ�˭�����ǲ�һ����
			if (crossEdageCoils[i]->start == activeCoils[j] && crossEdageCoils[i]->end != -1)
			{
				/*cout << "����" << endl;*/
				crossEdageCoils[i]->start = -1;
			}
			if (crossEdageCoils[i]->end == activeCoils[j] && crossEdageCoils[i]->end != -1)
			{
				/*cout << "����" << endl;*/
				vector<crossEdageCoil*>::iterator it1 = crossEdageCoils.begin() + i;
				crossEdageCoils.erase(it1);
				vector<int>::iterator it2 = activeCoils.begin() + j;
				activeCoils.erase(it2);
			}
			if (crossEdageCoils[i]->end == activeCoils[j] && crossEdageCoils[i]->start != -1)
			{
				/*cout << "����" << endl;*/
				crossEdageCoils[i]->end = -1;
			}
			if (crossEdageCoils[i]->start == activeCoils[j] && crossEdageCoils[i]->start != -1)
			{
				/*cout << "����" << endl;*/
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

	//������ڳ�����ȫ��Ϊ��
	if (int(crossEdageCoils.size()) > laneNum)
	{
		vector<crossEdageCoil*>().swap(crossEdageCoils);
	}
	/*cout << "�����Ȧ�Ե���Ŀ��" << crossEdageCoils.size() << endl;
	cout << "�����г�������Ȧ�ĸ���" << activeCoils.size() << endl;*/
	/*cout << "�����ܵĳ�����Ϊ��" << carNum << endl;*/
	
	//��տ����Ȧ��
	//crossEdageCoils->clear();
	//vector<crossEdageCoil*>().swap(crossEdageCoils);
	vector<int>().swap(activeCoils);

	return activCoilNum;
}
vector<crossEdageCoil*> VehicleFlowDetection::count(vector<crossEdageCoil*> crossEdageCoils)
{
	//��������
	for (size_t i = 0; i < crossEdageCoils.size(); i++)
	{
		for (size_t j = 0; j < activeCoils.size(); j++)
		{
			//˭�ȳ�˭�����ǲ�һ����
			if (crossEdageCoils[i]->start == activeCoils[j] && crossEdageCoils[i]->end != -1)
			{
				/*cout << "����" << endl;*/
				crossEdageCoils[i]->start = -1;
			}
			if (crossEdageCoils[i]->end == activeCoils[j] && crossEdageCoils[i]->end != -1)
			{
				/*cout << "����" << endl;*/
				vector<crossEdageCoil*>::iterator it1 = crossEdageCoils.begin() + i;
				crossEdageCoils.erase(it1);
				vector<int>::iterator it2 = activeCoils.begin() + j;
				activeCoils.erase(it2);
			}
			if (crossEdageCoils[i]->end == activeCoils[j] && crossEdageCoils[i]->start != -1)
			{
				/*cout << "����" << endl;*/
				crossEdageCoils[i]->end = -1;
			}
			if (crossEdageCoils[i]->start == activeCoils[j] && crossEdageCoils[i]->start != -1)
			{
				/*cout << "����" << endl;*/
				vector<crossEdageCoil*>::iterator it1 = crossEdageCoils.begin() + i;
				crossEdageCoils.erase(it1);
				vector<int>::iterator it2 = activeCoils.begin() + j;
				activeCoils.erase(it2);
			}
		}
	}
	int activCoilNum = activeCoils.size();
	carNum += activCoilNum;
	/*cout << "�����Ȧ�Ե���Ŀ��" << crossEdageCoils.size() << endl;
	cout << "�����г�������Ȧ�ĸ���" << activeCoils.size() << endl;*/
	/*cout << "�����ܵĳ�����Ϊ��" << carNum << endl;*/
	//������ڳ�����ȫ��Ϊ��
	if (int(crossEdageCoils.size()) > laneNum)
	{
		vector<crossEdageCoil*>().swap(crossEdageCoils);
	}
	//vector<int>().swap(activeCoils);//�ͷ�activeCoils���ڴ�ռ�
	vector<int>().swap(testingCoils);//�ͷ����ڼ���������Ȧ
	return crossEdageCoils;
}
