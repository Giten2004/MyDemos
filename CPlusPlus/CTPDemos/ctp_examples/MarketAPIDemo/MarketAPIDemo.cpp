// MarketAPIDemo.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>

#include "MarketDataAPI.h"

using namespace std;

int main()
{
	// 产生一个CThostFtdcMdApi实例
	CThostFtdcMdApi *pUserApi = CThostFtdcMdApi::CreateFtdcMdApi();
	// 产生一个事件处理的实例
	MarketDataAPI sh(pUserApi);
	// 注册一事件处理的实例
	pUserApi->RegisterSpi(&sh);
	// 设置交易托管系统服务的地址，可以注册多个地址备用
	//http://www.simnow.com.cn/product.action
	pUserApi->RegisterFront("tcp://180.168.146.187:10010");
	// 使客户端开始与后台服务建立连接
	pUserApi->Init();
	// 客户端等待报单操作完成
	//WaitForSingleObject(g_hEvent, INFINITE);
	
	system("pause");

	// 释放API实例
	pUserApi->Release();

	return 0;
}

