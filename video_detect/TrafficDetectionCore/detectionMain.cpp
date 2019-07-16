#include "detectionMain.h"

//标定所需的变量
vector<Point2f> points;//四个点的集合
vector<Rect> locations;

vector<Point2f> wPoints;//整个车道世界坐标的四点
float BRatio;//补偿率
int laneNum = 0;//车道数
int carNum = 0;//车数
Mat tempBackground;//背景
ViBe_BGS testAll;
vector<int> testingCoils;//正在检测的线圈
vector<crossEdageCoil*> crossEdageCoils;//跨边的线圈
vector<VirtualCoil*> coils;//所布的虚拟线圈的集合

//检测结果；0：没检测出结果；1：检测出车辆
int isDetectResult=0;
//当前检测出车速，值为0表示漏检
double vechileSpeed=0.0;
//当前帧检测出的车型；值0表示漏检
int vechileType=0;
Vector<int> vechileTypeInfo;
//检测到的车辆
vector<Vechile> detectVechiles;
//Vector<int> vechileTypeInfo;
Vector<double> vechileSpeedInfo;
//----------------
bool isDetectSpeed=false;
bool isDetectVechile=false;


//////////////////////////////////////////////////////////////////////////
//设置标定参数
//coilNum:虚拟线圈数量
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
	 
	 double FPS = inFps;//25;//capture.get(CV_CAP_PROP_FPS);//获取视频的帧率
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
    //vector<int> testingCoils;//正在检测的线圈
    //vector<crossEdageCoil> crossEdageCoils;//跨边的线圈
    //Mat frame;
    //,tempBackground;
    //float BRatio;//补偿率
    //vector<VirtualCoil> coils;//所布的虚拟线圈的集合
    //int carNum = 0;//车数
    //int count=0;//帧数

    //赋值
    //count=currentFrameNum;
    //frame=img;

	//第一帧算出虚拟线圈的属性，包括左边高度（世界坐标）....
    if (currentFrameNum==1)
    {
		//将第一帧的图像保存下来
        img.copyTo(tempBackground);

		//得到标准世界坐标
        wPoints=DemarcateTransform::worldPoints(laneNum);

        //求解补偿率
        BRatio = DemarcateTransform::calculateBRatio(points, wPoints);
        cout << "补偿率为：" << BRatio << endl;
        
        for (size_t i = 0; i < locations.size(); i++)
        {
			
				//计算虚拟线圈矩形的左竖线长度距离（世界坐标）
				float leftDistance=DemarcateTransform::calCoilLeftDist(locations[i], BRatio, points, wPoints);
				//计算虚拟线圈矩形的右竖线长度距离（世界坐标）
				float rightDistance=DemarcateTransform::calCoilRightDist(locations[i], BRatio, points, wPoints);
				cv::Mat temp2=tempBackground(locations[i]);
				VirtualCoil* coil = new VirtualCoil(locations[i], leftDistance, rightDistance, false, tempBackground(locations[i]));
				coil->setActiveFlag(true);
				coils.push_back(coil);			
        }
    }
	//分离前景、背景，算出VIBE模型
    else if (currentFrameNum == 2)
    {
        //***********************前景******************************
        cvtColor(img, testAllGray, CV_RGB2GRAY);
        testAll.init(&testAllGray);
        testAll.processFirstFrame(&testAllGray);
        for (size_t i = 0; i < coils.size(); i++)
        {
            //训练第二帧
            coils[i]->secondFrameForegroundInit(img);
        }
    }
    else
    {
//#define  _UNFINISHED_
#ifdef _UNFINISHED_   //未完成

        //***********************前景******************************
		Mat gray;
		cvtColor(img, gray, CV_RGB2GRAY);//将帧数图像灰度化
        cvtColor(img, testAllGray, CV_RGB2GRAY);//将帧数图像灰度化
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

		//车流检测--------------------------------
		for(int i=1;i<coils.size();i++)
		{
			crossEdageCoils=flow.cross_edage2(coils[i-1],coils[i], crossEdageCoils);//crossEdageCoils = flow.cross_edage(coils, crossEdageCoils, testingCoils);
			int num = flow.count2(crossEdageCoils);

			if(num<=0)
				continue;

			carNum+=1;

			//如果车流检测出车辆，车速漏检
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
				cout<<"车流："<<carNum<<endl;
				cout<<"车道："<<detectVechiles[detectVechiles.size()-1].laneNo<<endl;
				cout<<"车型："<<detectVechiles[detectVechiles.size()-1].vechileType<<endl;
				cout<<"车速："<<detectVechiles[detectVechiles.size()-1].speed<<endl;
			}
			*/
			//-----测试
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

		//1.车速检测/////////////////////////////////////////////////////////////////
		for(int i=0;i<coils.size();i++)
		{
			if (coils[i]->reVelocityFlag())
			{
				if (coils[i]->velocity(FPS))
				{
					cout << "触线后该车走的帧数：" << coils[i]->showTouchLineFrameNum() << endl;
					cout << "该车的速度：" << coils[i]->reSpeed()<<"km/h" << endl;
					//该车的速度
					vechileSpeed=coils[i]->reSpeed();
					coils[i]->setVelocityFlag(false);

					//临时变量
					//carNum+=1;

					Vechile vechile=Vechile(i+1,-1,-1);
					vechile.laneNo=i+1;
					vechile.vechileType=-1;

					vechile.speed=vechileSpeed;

					//车型检测---------
					float carLen = float((coils[i]->returnCarFrameNum() / FPS)*coils[i]->reSpeed()*(1 / 3.6) - coils[i]->reWDistanceLeft() / 100)*Angle_Body_Slope_Ratio;

					if (carLen < Small_Car_Length_Threshold)
					{
						vechileType=0;//"小型车";
						vechileTypeInfo.push_back(1);
						cout << "小型车" << endl;

						vechile.vechileType=0;
					}
					else if (carLen < Middle_Car_Length_Threshold)
					{
						vechileType=1;//"中型车";

						cout << "中型车" << endl;
						vechile.vechileType=1;
					}
					else
					{
						vechileType=2;//"大型车";
						vechileTypeInfo.push_back(3);
						cout << "大型车" << endl;
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

					//测试
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

					//检测出车辆
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

#define  _NOT_GOOD_DELETE_ //遍历每个虚拟线圈进行检测
#ifdef  _NOT_GOOD_DELETE_

		
		//***********************前景******************************
		Mat gray;
		cvtColor(img, gray, CV_RGB2GRAY);//将帧数图像灰度化
        cvtColor(img, testAllGray, CV_RGB2GRAY);//将帧数图像灰度化
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

		//车流检测--------------------------------
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
		//1.车速检测/////////////////////////////////////////////////////////////////
		for(int i=0;i<coils.size();i++)
		{
			bool iscoilDetec=false;

			Vechile vechile=Vechile(i+1,-1,-1);
			vechile.laneNo=i+1;
			vechile.vechileType=-1;
			vechile.speed=-1;

			//----车流检测1
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
			

			//--------------------车速检测

			if (coils[i]->reVelocityFlag())
			{
				if (coils[i]->velocity(FPS))
				{
					//cout << "触线后该车走的帧数：" << coils[i]->showTouchLineFrameNum() << endl;
					//cout << "该车的速度：" << coils[i]->reSpeed()<<"km/h" << endl;
					//该车的速度
					vechileSpeed=coils[i]->reSpeed();
					coils[i]->setVelocityFlag(false);

					//临时变量
					//carNum+=1;

					
					vechileSpeedInfo.push_back(vechileSpeed);
					vechile.speed=vechileSpeed;

					//车型检测---------
					float carLen = float((coils[i]->returnCarFrameNum() / FPS)*coils[i]->reSpeed()*(1 / 3.6) - coils[i]->reWDistanceLeft() / 100)*Angle_Body_Slope_Ratio;

					if (carLen < DemarcateParameter::Middle_Car_Length_Threshold)//if (carLen < Small_Car_Length_Threshold)
					{
						vechileType=0;//"小型车";
						vechileTypeInfo.push_back(0);
						//cout << "小型车" << endl;

						vechile.vechileType=0;
					}
					/*else if (carLen < Middle_Car_Length_Threshold)
					{
						vechileType=1;//"中型车";
						vechileTypeInfo.push_back(1);
						//cout << "中型车" << endl;
						vechile.vechileType=1;
					}*/
					else
					{
						vechileType=2;//"大型车";
						vechileTypeInfo.push_back(2);
						//cout << "大型车" << endl;
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

					//检测出车辆
					isDetectSpeed=true;
					iscoilDetec=true;

					detectVechiles.push_back(vechile);
				}
			}

			//----车流检测2
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
			
			//没检测出车速时
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
			cout<<"车流："<<detectVechiles.size()<<endl;
			cout<<"车道："<<detectVechiles[detectVechiles.size()-1].laneNo<<endl;
			cout<<"车型："<<detectVechiles[detectVechiles.size()-1].vechileType<<endl;
			cout<<"车速："<<detectVechiles[detectVechiles.size()-1].speed<<endl;
			/*cout<<"车流："<<carNum<<endl;
			if(!vechileTypeInfo.empty())
			cout<<"车型："<<vechileTypeInfo[vechileTypeInfo.size()-1]<<endl;
			cout<<"车速："<<vechileSpeed<<endl;*/
		}
		
		if(isDetectSpeed&&isDetectVechile)
			isDetectResult=true;
		//////////////////////////////////////////////////////////////////////////

#endif
		
//#define  _NOT_GOOD_
#ifdef  _NOT_GOOD_

		//***********************前景******************************
		Mat gray;
		cvtColor(img, gray, CV_RGB2GRAY);//将帧数图像灰度化
        cvtColor(img, testAllGray, CV_RGB2GRAY);//将帧数图像灰度化
        testAll.testAndUpdate(&testAllGray);
        testAllMask = testAll.getMask();

        std::cout << "第"<<currentFrameNum<< "帧*******************************" << endl;
        //*****************车流检测**************************************
        VehicleFlowDetection flow = VehicleFlowDetection(laneNum);

		//Mat imgORI=img(coils[i]->reLocation());
        coils = flow.start(coils, &img);
        testingCoils = flow.reTestingCoils();
		//跨边检测，输入全部线圈
        flow.cross_edage(coils, crossEdageCoils, testingCoils);//crossEdageCoils = flow.cross_edage(coils, crossEdageCoils, testingCoils);
        crossEdageCoils = flow.count(crossEdageCoils);
        carNum += flow.reCarNum();
        cout << "车辆总数为：" << carNum << endl;

		if(flow.reCarNum()>0)
			isDetectResult=1;
        //*****************车流检测**************************************

        //vechileInfo.resize(carNum);
        bool speedDect=false;

        //*****************车速检测**************************************
        //cout << "coils.size():" << coils.size() << endl;

        for (size_t i = 0; i < coils.size(); i++)
        {
            if (coils[i]->reVelocityFlag())
            {
                if (coils[i]->velocity(FPS))
                {
                    speedDect=true;

                    cout << "触线后该车走的帧数：" << coils[i]->showTouchLineFrameNum() << endl;
                    cout << "该车的速度：" << coils[i]->reSpeed()<<"km/h" << endl;
                    //该车的速度
					vechileSpeed=coils[i]->reSpeed();
                    vechileSpeedInfo.push_back(coils[i]->reSpeed());
                    coils[i]->setVelocityFlag(false);
					//flag=1;

                }
            }
        }

        //没检测出车速时
        if(!speedDect&&flow.reCarNum()>0)
        {
            vechileSpeedInfo.push_back(0);
        }
        //*****************车速检测**************************************

        bool typeDect=false;

        //*****************车型检测**************************************
        vector<int> active = flow.reActiveCoils();//当前的活跃的线圈为
        for (size_t i = 0; i < active.size(); i++)
        {
            float carLen = float((coils[active[i]]->returnCarFrameNum() / FPS)*coils[active[i]]->reSpeed()*(1 / 3.6) - coils[active[i]]->reWDistanceLeft() / 100)*Angle_Body_Slope_Ratio;
			if (carLen < DemarcateParameter::Small_Car_Length_Threshold)//if (carLen < Small_Car_Length_Threshold)
            {
				vechileType=0;//"小型车";
                vechileTypeInfo.push_back(1);
                cout << "小型车" << endl;
                typeDect=true;

				//该车道车流数量
				//vechileNum4Lanes[active[0]]+=1;
            }
            /*else if (carLen < Middle_Car_Length_Threshold)
            {
				vechileType=1;//"中型车";
                vechileTypeInfo.push_back(2);
                cout << "中型车" << endl;
                typeDect=true;

				//该车道车流数量
				//vechileNum4Lanes[active[0]]+=1;
            }*/
            else
            {
				vechileType=2;//"大型车";
                vechileTypeInfo.push_back(3);
                cout << "大型车" << endl;
                typeDect=true;

				//该车道车流数量
				//vechileNum4Lanes[active[0]]+=1;
            }

			

        }

		if(!typeDect&&flow.reCarNum()>0)
		{
			vechileTypeInfo.push_back(0);
		}
        //*****************车型检测**************************************
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

//获取当前帧检测到的车速
double GetVechileSpeed()
{
	if(isDetectResult)
		return vechileSpeed;
	return -1;
}

//获取当前帧检测到的车型
int GetVechileType()
{
	if(isDetectResult)
		return vechileType;
	return -1;

}

//获取当前帧检测到的车的数量
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

int SetDetectParameters(float Small_Car_Length_Threshold,	//小型车长阈值
						int min_car_frame_num,			//最小的车尾通过虚拟线圈的帧数
						double touchEdgeThreshold,			//边的阈值
						double leftAndRightBoundaryRatio,	//左右边界检测的最小比率
						double similarity_scale_threshold	//相似度阈值
						)
{
	DemarcateParameter::Small_Car_Length_Threshold=Small_Car_Length_Threshold;//小型车长阈值
	DemarcateParameter::min_car_frame_num=min_car_frame_num;//最小的车尾通过虚拟线圈的帧数
	DemarcateParameter::touchEdgeThreshold=touchEdgeThreshold;//0.3;//边的阈值???
	DemarcateParameter::leftAndRightBoundaryRatio=leftAndRightBoundaryRatio;//0.35;//左右边界检测的最小比率
	DemarcateParameter::similarity_scale_threshold=similarity_scale_threshold;//0.9;//相似度阈值

	return 1;
}