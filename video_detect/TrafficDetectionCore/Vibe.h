#pragma once
#include <iostream>
#include "opencv2/opencv.hpp"

using namespace cv;
using namespace std;

#define NUM_SAMPLES 20      //每个像素点的样本个数
#define MIN_MATCHES 2       //#min指数
#define RADIUS 20       //Sqthere半径
#define SUBSAMPLE_FACTOR 16 //子采样概率


class ViBe_BGS
{
public:
    ViBe_BGS(void);
    ~ViBe_BGS(void);

    void init(Mat* _image);   //初始化void init(const Mat _image);   //初始化
    void processFirstFrame(Mat* _image);
    void testAndUpdate(Mat* _image);  //更新
    Mat getMask(void){ return m_mask; };
    void deleteSamples(){
        //delete samples;
    };
public:
    /*Mat m_samples[NUM_SAMPLES];
    Mat m_foregroundMatchCount;*/
    //unsigned char ***samples;
    vector<vector<vector<unsigned char> > > samples;
    Mat m_mask;
};

