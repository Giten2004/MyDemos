// TradeAPIDemo.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "TradeDataAPI.h"
#include <iostream>

using namespace std;

int main()
{
	CThostFtdcTraderApi *pUserApi = CThostFtdcTraderApi::CreateFtdcTraderApi();
	// 产生一个事件处理的实例
	TradeDataAPI sh(pUserApi);
	// 注册一事件处理的实例
	pUserApi->RegisterSpi(&sh);
	// 订阅私有流
	// TERT_RESTART:从本交易日开始重传
	// TERT_RESUME:从上次收到的续传
	// TERT_QUICK:只传送登录后私有流的内容
	pUserApi->SubscribePrivateTopic(THOST_TE_RESUME_TYPE::THOST_TERT_RESUME);
	// 订阅公共流
	// TERT_RESTART:从本交易日开始重传
	// TERT_RESUME:从上次收到的续传
	// TERT_QUICK:只传送登录后公共流的内容
	pUserApi->SubscribePublicTopic(THOST_TE_RESUME_TYPE::THOST_TERT_RESUME);
	// 设置交易托管系统服务的地址，可以注册多个地址备用
	pUserApi->RegisterFront("tcp://180.168.146.187:10001");
	// 使客户端开始与后台服务建立连接
	pUserApi->Init();
	// 客户端等待报单操作完成
	//WaitForSingleObject(g_hEvent, INFINITE);
	system("pause");
	// 释放API实例
	pUserApi->Release();
	return 0;
}

