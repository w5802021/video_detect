#include "detectionMain.h"

//�궨����ı���
vector<Point2f> points;//�ĸ���ļ���
vector<Rect> locations;

vector<Point2f> wPoints;//������������������ĵ�
float BRatio;//������
int laneNum = 0;//������
int carNum = 0;//����
Mat tempBackground;//����
ViBe_BGS testAll;
vector<int> testingCoils;//���ڼ�����Ȧ
vector<crossEdageCoil*> crossEdageCoils;//��ߵ���Ȧ
vector<VirtualCoil*> coils;//������������Ȧ�ļ���

//�������0��û���������1����������
int isDetectResult=0;
//��ǰ�������٣�ֵΪ0��ʾ©��
double vechileSpeed=0.0;
//��ǰ֡�����ĳ��ͣ�ֵ0��ʾ©��
int vechileType=0;
Vector<int> vechileTypeInfo;
//��⵽�ĳ���
vector<Vechile> detectVechiles;
//Vector<int> vechileTypeInfo;
Vector<double> vechileSpeedInfo;
//----------------
bool isDetectSpeed=false;
bool isDetectVechile=false;


//////////////////////////////////////////////////////////////////////////
//���ñ궨����
//coilNum:������Ȧ����
//////////////////////////////////////////////////////////////////////////
void SetDemarcate( int detect_1[],int detect_2[],int detect_3[],int coilNum)
{
	int j=0;
	for(int i=0;i<coilNum;i++)
	{
		int x=detect_1[j++];
		int y=detect_1[j++];
		int width=detect_1[j++];
		int height=detect_1[j++];

		locations.push_back(Rect(x,y,width,height));
	}

	j=0;
	for(int i=0;i<4;i++)
	{
		int x=detect_2[j++];
		int y=detect_2[j++];
		points.push_back(Point(x,y));
	}

	j=0;
	for(int i=0;i<2;i++)
	{
		int x=detect_3[j++];
		int y=detect_3[j++];
		points.push_back(Point(x,y));
	}


}


//////////////////////////////////////////////////////////////////////////
//
//////////////////////////////////////////////////////////////////////////
int Detect(uchar* data,int width,int height,int currentFrameNum,int laneNum_,int inFps)
{
	 
	 double FPS = inFps;//25;//capture.get(CV_CAP_PROP_FPS);//��ȡ��Ƶ��֡��
	 laneNum=laneNum_;

	 if(!isDetectSpeed&&!isDetectVechile)//if(!isDetectSpeed&&!isDetectVechile)
	 {
		 vector<Vechile>().swap(detectVechiles);
		 carNum=0;
	 }

	 if(isDetectResult)
	 {
		 isDetectSpeed=false;
		 isDetectVechile=false;
		 vechileSpeed=0;
		 
	 }
	isDetectResult=0;

	

	cv::Mat img=Mat(width,height,CV_8UC3,data);

	Mat testAllGray;
	Mat testAllMask;

    //ViBe_BGS testAll;
    //vector<int> testingCoils;//���ڼ�����Ȧ
    //vector<crossEdageCoil> crossEdageCoils;//��ߵ���Ȧ
    //Mat frame;
    //,tempBackground;
    //float BRatio;//������
    //vector<VirtualCoil> coils;//������������Ȧ�ļ���
    //int carNum = 0;//����
    //int count=0;//֡��

    //��ֵ
    //count=currentFrameNum;
    //frame=img;

	//��һ֡���������Ȧ�����ԣ�������߸߶ȣ��������꣩....
    if (currentFrameNum==1)
    {
		//����һ֡��ͼ�񱣴�����
        img.copyTo(tempBackground);

		//�õ���׼��������
        wPoints=DemarcateTransform::worldPoints(laneNum);

        //��ⲹ����
        BRatio = DemarcateTransform::calculateBRatio(points, wPoints);
        cout << "������Ϊ��" << BRatio << endl;
        
        for (size_t i = 0; i < locations.size(); i++)
        {
			
				//����������Ȧ���ε������߳��Ⱦ��루�������꣩
				float leftDistance=DemarcateTransform::calCoilLeftDist(locations[i], BRatio, points, wPoints);
				//����������Ȧ���ε������߳��Ⱦ��루�������꣩
				float rightDistance=DemarcateTransform::calCoilRightDist(locations[i], BRatio, points, wPoints);
				cv::Mat temp2=tempBackground(locations[i]);
				VirtualCoil* coil = new VirtualCoil(locations[i], leftDistance, rightDistance, false, tempBackground(locations[i]));
				coil->setActiveFlag(true);
				coils.push_back(coil);			
        }
    }
	//����ǰ�������������VIBEģ��
    else if (currentFrameNum == 2)
    {
        //***********************ǰ��******************************
        cvtColor(img, testAllGray, CV_RGB2GRAY);
        testAll.init(&testAllGray);
        testAll.processFirstFrame(&testAllGray);
        for (size_t i = 0; i < coils.size(); i++)
        {
            //ѵ���ڶ�֡
            coils[i]->secondFrameForegroundInit(img);
        }
    }
    else
    {
//#define  _UNFINISHED_
#ifdef _UNFINISHED_   //δ���

        //***********************ǰ��******************************
		Mat gray;
		cvtColor(img, gray, CV_RGB2GRAY);//��֡��ͼ��ҶȻ�
        cvtColor(img, testAllGray, CV_RGB2GRAY);//��֡��ͼ��ҶȻ�
        testAll.testAndUpdate(&testAllGray);
        testAllMask = testAll.getMask();

		/**************************************************************************/
		VehicleFlowDetection flow = VehicleFlowDetection(laneNum);
		for(int i=0;i<coils.size();i++)
		{
			coils[i]->id=i+1;
			coils[i]->testing=false;

			Vechile vechile=Vechile(i,-1,-1);
			vechile.laneNo=i+1;
			vechile.speed=-1;
			vechile.vechileType=-1;

			Mat imgORI=img(coils[i]->reLocation());
			coils=flow.start2(coils, &imgORI,i);

			testingCoils = flow.reTestingCoils();
		}

		//////////////////////////////////////////////////////////////////////////

		//�������--------------------------------
		for(int i=1;i<coils.size();i++)
		{
			crossEdageCoils=flow.cross_edage2(coils[i-1],coils[i], crossEdageCoils);//crossEdageCoils = flow.cross_edage(coils, crossEdageCoils, testingCoils);
			int num = flow.count2(crossEdageCoils);

			if(num<=0)
				continue;

			carNum+=1;

			//���������������������©��
			/*if(carNum>detectVechiles.size()&&num==1)
			{
				Vechile vechile=Vechile(i+1,-1,-1);
				vechile.laneNo=i+1;
				vechile.speed=-1;
				vechile.vechileType=-1;
				detectVechiles.push_back(vechile);
			}
			else if(carNum-detectVechiles.size()==1&&num==2)
			{
				Vechile vechile=Vechile(i+1,-1,-1);
				vechile.laneNo=i+1;
				vechile.speed=-1;
				vechile.vechileType=-1;
				detectVechiles.push_back(vechile);
			}


			if(detectVechiles.size()>0)
			{
				cout<<"������"<<carNum<<endl;
				cout<<"������"<<detectVechiles[detectVechiles.size()-1].laneNo<<endl;
				cout<<"���ͣ�"<<detectVechiles[detectVechiles.size()-1].vechileType<<endl;
				cout<<"���٣�"<<detectVechiles[detectVechiles.size()-1].speed<<endl;
			}
			*/
			//-----����
			//carNum+=1;

			Vechile vechile=Vechile(i+1,-1,-1);
			vechile.laneNo=i+1;
			vechile.speed=-1;
			vechile.vechileType=-1;
			detectVechiles.push_back(vechile);
			//--------

			isDetectVechile=true;

		}

		if(carNum<=0)
		{
			isDetectVechile=false;
			return 0;
		}
		//////////////////////////////////////////////////////////////////////////

		//1.���ټ��/////////////////////////////////////////////////////////////////
		for(int i=0;i<coils.size();i++)
		{
			if (coils[i]->reVelocityFlag())
			{
				if (coils[i]->velocity(FPS))
				{
					cout << "���ߺ�ó��ߵ�֡����" << coils[i]->showTouchLineFrameNum() << endl;
					cout << "�ó����ٶȣ�" << coils[i]->reSpeed()<<"km/h" << endl;
					//�ó����ٶ�
					vechileSpeed=coils[i]->reSpeed();
					coils[i]->setVelocityFlag(false);

					//��ʱ����
					//carNum+=1;

					Vechile vechile=Vechile(i+1,-1,-1);
					vechile.laneNo=i+1;
					vechile.vechileType=-1;

					vechile.speed=vechileSpeed;

					//���ͼ��---------
					float carLen = float((coils[i]->returnCarFrameNum() / FPS)*coils[i]->reSpeed()*(1 / 3.6) - coils[i]->reWDistanceLeft() / 100)*Angle_Body_Slope_Ratio;

					if (carLen < Small_Car_Length_Threshold)
					{
						vechileType=0;//"С�ͳ�";
						vechileTypeInfo.push_back(1);
						cout << "С�ͳ�" << endl;

						vechile.vechileType=0;
					}
					else if (carLen < Middle_Car_Length_Threshold)
					{
						vechileType=1;//"���ͳ�";

						cout << "���ͳ�" << endl;
						vechile.vechileType=1;
					}
					else
					{
						vechileType=2;//"���ͳ�";
						vechileTypeInfo.push_back(3);
						cout << "���ͳ�" << endl;
						vechile.vechileType=2;
					}
					//******************************

					/*if(carNum-detectVechiles.size()==1)
					{
					detectVechiles[detectVechiles.size()-1].speed=vechile.speed;
					detectVechiles[detectVechiles.size()-1].vechileType=vechile.vechileType;
					}
					else
						detectVechiles.push_back(vechile);*/

					//����
					if(carNum==1)
					{
						detectVechiles[detectVechiles.size()-1].speed=vechile.speed;
						detectVechiles[detectVechiles.size()-1].vechileType=vechile.vechileType;
					}
					else if(carNum==2)
					{
						if(i==0&&detectVechiles.size()>=2)
						{
							detectVechiles[detectVechiles.size()-2].speed=vechile.speed;
							detectVechiles[detectVechiles.size()-2].vechileType=vechile.vechileType;
						}
						else if(i==1&&detectVechiles.size()>=2)
						{
							detectVechiles[detectVechiles.size()-1].speed=vechile.speed;
							detectVechiles[detectVechiles.size()-1].vechileType=vechile.vechileType;
						}
						else if(i==2&&detectVechiles.size()>=2)
						{
							detectVechiles[detectVechiles.size()-1].speed=vechile.speed;
							detectVechiles[detectVechiles.size()-1].vechileType=vechile.vechileType;
						}
					}
					else if(carNum==3)
					{
						if(i==0&&detectVechiles.size()>=3)
						{
							detectVechiles[detectVechiles.size()-3].speed=vechile.speed;
							detectVechiles[detectVechiles.size()-3].vechileType=vechile.vechileType;
						}
						else if(i==1&&detectVechiles.size()>=3)
						{
							detectVechiles[detectVechiles.size()-2].speed=vechile.speed;
							detectVechiles[detectVechiles.size()-2].vechileType=vechile.vechileType;
						}

						else if(i==2&&detectVechiles.size()>=3)
						{
							detectVechiles[detectVechiles.size()-1].speed=vechile.speed;
							detectVechiles[detectVechiles.size()-1].vechileType=vechile.vechileType;
						}
					}
					//------------------------------

					//��������
					isDetectSpeed=true;
				}
			}
		}
		
		if(isDetectSpeed&&isDetectVechile)
		{
		isDetectSpeed=false;
		isDetectVechile=false;

		isDetectResult=1;

		}
		//isDetectVechile=true;

#endif

#define  _NOT_GOOD_DELETE_ //����ÿ��������Ȧ���м��
#ifdef  _NOT_GOOD_DELETE_

		
		//***********************ǰ��******************************
		Mat gray;
		cvtColor(img, gray, CV_RGB2GRAY);//��֡��ͼ��ҶȻ�
        cvtColor(img, testAllGray, CV_RGB2GRAY);//��֡��ͼ��ҶȻ�
        testAll.testAndUpdate(&testAllGray);
        testAllMask = testAll.getMask();

		/**************************************************************************/
		VehicleFlowDetection flow = VehicleFlowDetection(laneNum);
		/*for(int i=0;i<coils.size();i++)
		{
			coils[i]->id=i+1;
			coils[i]->testing=false;

			Vechile vechile=Vechile(i,-1,-1);
			vechile.laneNo=i+1;
			vechile.speed=-1;
			vechile.vechileType=-1;

			Mat imgORI=img(coils[i]->reLocation());
			coils=flow.start2(coils, &imgORI,i);

			testingCoils = flow.reTestingCoils();
		}

		//�������--------------------------------
		for(int i=1;i<coils.size();i++)
		{
			crossEdageCoils=flow.cross_edage2(coils[i-1],coils[i], crossEdageCoils);//crossEdageCoils = flow.cross_edage(coils, crossEdageCoils, testingCoils);
			int num = flow.count2(crossEdageCoils);

			if(num<=0)
				continue;

			carNum+=1;

			
			isDetectVechile=true;

		}


		if(!isDetectVechile)
			return 0;
*/
		//1.���ټ��/////////////////////////////////////////////////////////////////
		for(int i=0;i<coils.size();i++)
		{
			bool iscoilDetec=false;

			Vechile vechile=Vechile(i+1,-1,-1);
			vechile.laneNo=i+1;
			vechile.vechileType=-1;
			vechile.speed=-1;

			//----�������1
			coils[i]->id=i+1;
			coils[i]->testing=false;


			//Mat imgORI=img(coils[i]->reLocation());
			coils=flow.start2(coils, &img,i);//coils=flow.start2(coils, &imgORI,i);

			testingCoils = flow.reTestingCoils();

			//----2
			/*if(i>0)
			crossEdageCoils=flow.cross_edage2(coils[i-1],coils[i], crossEdageCoils);

			int num = flow.count2(crossEdageCoils);

			if(num<=0)
			continue;


			carNum+=1;

			isDetectVechile=true;
			isDetectResult=true;
			*/
			

			//--------------------���ټ��

			if (coils[i]->reVelocityFlag())
			{
				if (coils[i]->velocity(FPS))
				{
					//cout << "���ߺ�ó��ߵ�֡����" << coils[i]->showTouchLineFrameNum() << endl;
					//cout << "�ó����ٶȣ�" << coils[i]->reSpeed()<<"km/h" << endl;
					//�ó����ٶ�
					vechileSpeed=coils[i]->reSpeed();
					coils[i]->setVelocityFlag(false);

					//��ʱ����
					//carNum+=1;

					
					vechileSpeedInfo.push_back(vechileSpeed);
					vechile.speed=vechileSpeed;

					//���ͼ��---------
					float carLen = float((coils[i]->returnCarFrameNum() / FPS)*coils[i]->reSpeed()*(1 / 3.6) - coils[i]->reWDistanceLeft() / 100)*Angle_Body_Slope_Ratio;

					if (carLen < DemarcateParameter::Middle_Car_Length_Threshold)//if (carLen < Small_Car_Length_Threshold)
					{
						vechileType=0;//"С�ͳ�";
						vechileTypeInfo.push_back(0);
						//cout << "С�ͳ�" << endl;

						vechile.vechileType=0;
					}
					/*else if (carLen < Middle_Car_Length_Threshold)
					{
						vechileType=1;//"���ͳ�";
						vechileTypeInfo.push_back(1);
						//cout << "���ͳ�" << endl;
						vechile.vechileType=1;
					}*/
					else
					{
						vechileType=2;//"���ͳ�";
						vechileTypeInfo.push_back(2);
						//cout << "���ͳ�" << endl;
						vechile.vechileType=2;
					}
					//******************************

					/*	if(carNum-detectVechiles.size()==1)
					{
					detectVechiles[detectVechiles.size()-1].speed=vechile.speed;
					detectVechiles[detectVechiles.size()-1].vechileType=vechile.vechileType;
					}
					else
					detectVechiles.push_back(vechile);*/

					//��������
					isDetectSpeed=true;
					iscoilDetec=true;

					detectVechiles.push_back(vechile);
				}
			}

			//----�������2
			/*coils[i]->id=i+1;
			coils[i]->testing=false;


			//Mat imgORI=img(coils[i]->reLocation());
			coils=flow.start2(coils, &img,i);//coils=flow.start2(coils, &imgORI,i);

			testingCoils = flow.reTestingCoils();

			//if(testingCoils.size()<=0)
			//	continue;
			*/
			
			if(i>0)
				crossEdageCoils=flow.cross_edage2(coils[i-1],coils[i], crossEdageCoils);

			int num = flow.count2(crossEdageCoils);
			
			//û��������ʱ
			if(!isDetectSpeed&&num>0)
			{
				vechileSpeedInfo.push_back(0);
				vechileTypeInfo.push_back(0);
			}
			else if(num<=0)
				continue;


			carNum+=1;

			isDetectVechile=true;
			
			
			
			
			//if(vechile.speed!=-1)
			//	detectVechiles.push_back(vechile);

		

		}

		if(detectVechiles.size()>0)
		{
			cout<<"������"<<detectVechiles.size()<<endl;
			cout<<"������"<<detectVechiles[detectVechiles.size()-1].laneNo<<endl;
			cout<<"���ͣ�"<<detectVechiles[detectVechiles.size()-1].vechileType<<endl;
			cout<<"���٣�"<<detectVechiles[detectVechiles.size()-1].speed<<endl;
			/*cout<<"������"<<carNum<<endl;
			if(!vechileTypeInfo.empty())
			cout<<"���ͣ�"<<vechileTypeInfo[vechileTypeInfo.size()-1]<<endl;
			cout<<"���٣�"<<vechileSpeed<<endl;*/
		}
		
		if(isDetectSpeed&&isDetectVechile)
			isDetectResult=true;
		//////////////////////////////////////////////////////////////////////////

#endif
		
//#define  _NOT_GOOD_
#ifdef  _NOT_GOOD_

		//***********************ǰ��******************************
		Mat gray;
		cvtColor(img, gray, CV_RGB2GRAY);//��֡��ͼ��ҶȻ�
        cvtColor(img, testAllGray, CV_RGB2GRAY);//��֡��ͼ��ҶȻ�
        testAll.testAndUpdate(&testAllGray);
        testAllMask = testAll.getMask();

        std::cout << "��"<<currentFrameNum<< "֡*******************************" << endl;
        //*****************�������**************************************
        VehicleFlowDetection flow = VehicleFlowDetection(laneNum);

		//Mat imgORI=img(coils[i]->reLocation());
        coils = flow.start(coils, &img);
        testingCoils = flow.reTestingCoils();
		//��߼�⣬����ȫ����Ȧ
        flow.cross_edage(coils, crossEdageCoils, testingCoils);//crossEdageCoils = flow.cross_edage(coils, crossEdageCoils, testingCoils);
        crossEdageCoils = flow.count(crossEdageCoils);
        carNum += flow.reCarNum();
        cout << "��������Ϊ��" << carNum << endl;

		if(flow.reCarNum()>0)
			isDetectResult=1;
        //*****************�������**************************************

        //vechileInfo.resize(carNum);
        bool speedDect=false;

        //*****************���ټ��**************************************
        //cout << "coils.size():" << coils.size() << endl;

        for (size_t i = 0; i < coils.size(); i++)
        {
            if (coils[i]->reVelocityFlag())
            {
                if (coils[i]->velocity(FPS))
                {
                    speedDect=true;

                    cout << "���ߺ�ó��ߵ�֡����" << coils[i]->showTouchLineFrameNum() << endl;
                    cout << "�ó����ٶȣ�" << coils[i]->reSpeed()<<"km/h" << endl;
                    //�ó����ٶ�
					vechileSpeed=coils[i]->reSpeed();
                    vechileSpeedInfo.push_back(coils[i]->reSpeed());
                    coils[i]->setVelocityFlag(false);
					//flag=1;

                }
            }
        }

        //û��������ʱ
        if(!speedDect&&flow.reCarNum()>0)
        {
            vechileSpeedInfo.push_back(0);
        }
        //*****************���ټ��**************************************

        bool typeDect=false;

        //*****************���ͼ��**************************************
        vector<int> active = flow.reActiveCoils();//��ǰ�Ļ�Ծ����ȦΪ
        for (size_t i = 0; i < active.size(); i++)
        {
            float carLen = float((coils[active[i]]->returnCarFrameNum() / FPS)*coils[active[i]]->reSpeed()*(1 / 3.6) - coils[active[i]]->reWDistanceLeft() / 100)*Angle_Body_Slope_Ratio;
			if (carLen < DemarcateParameter::Small_Car_Length_Threshold)//if (carLen < Small_Car_Length_Threshold)
            {
				vechileType=0;//"С�ͳ�";
                vechileTypeInfo.push_back(1);
                cout << "С�ͳ�" << endl;
                typeDect=true;

				//�ó�����������
				//vechileNum4Lanes[active[0]]+=1;
            }
            /*else if (carLen < Middle_Car_Length_Threshold)
            {
				vechileType=1;//"���ͳ�";
                vechileTypeInfo.push_back(2);
                cout << "���ͳ�" << endl;
                typeDect=true;

				//�ó�����������
				//vechileNum4Lanes[active[0]]+=1;
            }*/
            else
            {
				vechileType=2;//"���ͳ�";
                vechileTypeInfo.push_back(3);
                cout << "���ͳ�" << endl;
                typeDect=true;

				//�ó�����������
				//vechileNum4Lanes[active[0]]+=1;
            }

			

        }

		if(!typeDect&&flow.reCarNum()>0)
		{
			vechileTypeInfo.push_back(0);
		}
        //*****************���ͼ��**************************************
#endif
	}
	
	return isDetectResult;//return flag;
}

//int* GetVechileNum4Lanes()
//{
//	return vechileNum4Lanes;
//}

int* GetVechileInfo()
{
	int* info=new int[3*3];

	int i=0;
	for(;i<detectVechiles.size();i++)
	{
		info[i*3]=detectVechiles[i].laneNo;
		info[i*3+1]=detectVechiles[i].speed;
		info[i*3+2]=detectVechiles[i].vechileType;
	}

	if(3>detectVechiles.size())
	{
		int count=laneNum-detectVechiles.size();
		for(;i<3;i++)
		{
			info[i*3]=-1;
			info[i*3+1]=-1;
			info[i*3+2]=-1;
		}
	}

	return info;

}

//��ȡ��ǰ֡��⵽�ĳ���
double GetVechileSpeed()
{
	if(isDetectResult)
		return vechileSpeed;
	return -1;
}

//��ȡ��ǰ֡��⵽�ĳ���
int GetVechileType()
{
	if(isDetectResult)
		return vechileType;
	return -1;

}

//��ȡ��ǰ֡��⵽�ĳ�������
int GetVechileCount()
{
	return carNum;
}

int SetWhiteLine(double length,double width)
{
	DemarcateParameter::WhiteLineLength=length;
	DemarcateParameter::WhiteLineWidth=width;

	return 1;
}

int SetLaneWidth(double width)
{
	DemarcateParameter::WhiteLineIntervalLR=width;

	return 1;
}

int SetDetectParameters(float Small_Car_Length_Threshold,	//С�ͳ�����ֵ
						int min_car_frame_num,			//��С�ĳ�βͨ��������Ȧ��֡��
						double touchEdgeThreshold,			//�ߵ���ֵ
						double leftAndRightBoundaryRatio,	//���ұ߽������С����
						double similarity_scale_threshold	//���ƶ���ֵ
						)
{
	DemarcateParameter::Small_Car_Length_Threshold=Small_Car_Length_Threshold;//С�ͳ�����ֵ
	DemarcateParameter::min_car_frame_num=min_car_frame_num;//��С�ĳ�βͨ��������Ȧ��֡��
	DemarcateParameter::touchEdgeThreshold=touchEdgeThreshold;//0.3;//�ߵ���ֵ???
	DemarcateParameter::leftAndRightBoundaryRatio=leftAndRightBoundaryRatio;//0.35;//���ұ߽������С����
	DemarcateParameter::similarity_scale_threshold=similarity_scale_threshold;//0.9;//���ƶ���ֵ

	return 1;
}