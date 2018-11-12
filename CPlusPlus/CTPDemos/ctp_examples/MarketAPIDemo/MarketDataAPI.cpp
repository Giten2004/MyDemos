#pragma once

#include "stdafx.h"
#include "MarketDataAPI.h"


MarketDataAPI::MarketDataAPI(CThostFtdcMdApi *pUserApi)
{
	m_pUserApi = pUserApi;
}


MarketDataAPI::~MarketDataAPI()
{
}


//当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
void MarketDataAPI::OnFrontConnected() 
{
	printf("OnFrontConnected\n");
	CThostFtdcReqUserLoginField reqUserLogin;

	// 会员代码
	TThostFtdcBrokerIDType chBrokerID;
	// 交易用户代码
	TThostFtdcUserIDType chUserID;

	// get BrokerID
	//printf("BrokerID:");
	//scanf("%s", &chBrokerID);
	strcpy(reqUserLogin.BrokerID, "9999");
	// get userid
	//printf("userid:");
	//scanf("%s", &chUserID);
	strcpy(reqUserLogin.UserID, "073423");
	// get password
	//printf("password:");
	//scanf("%s", &reqUserLogin.Password);

	strcpy(reqUserLogin.Password, "123456@shit");
	// 发出登陆请求
	m_pUserApi->ReqUserLogin(&reqUserLogin, 0);
}

//当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
//@param nReason 错误原因
//        0x1001 网络读失败
//        0x1002 网络写失败
//        0x2001 接收心跳超时
//        0x2002 发送心跳失败
//        0x2003 收到错误报文
void MarketDataAPI::OnFrontDisconnected(int nReason) 
{
	// 当发生这个情况后，API会自动重新连接，客户端可不做处理
	printf("OnFrontDisconnected.\n");
}

//心跳超时警告。当长时间未收到报文时，该方法被调用。
//@param nTimeLapse 距离上次接收报文的时间
void MarketDataAPI::OnHeartBeatWarning(int nTimeLapse) 
{
	printf("OnHeartBeatWarning.\n");
}


//登录请求响应
void MarketDataAPI::OnRspUserLogin(CThostFtdcRspUserLoginField *pRspUserLogin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	printf("OnRspUserLogin:\n");
	printf("ErrorCode=[%d], ErrorMsg=[%s]\n", pRspInfo->ErrorID, pRspInfo->ErrorMsg);
	printf("RequestID=[%d], Chain=[%d]\n", nRequestID, bIsLast);

	if (pRspInfo->ErrorID != 0) {
		// 端登失败，客户端需进行错误处理
		printf("Failed to login, errorcode=%d errormsg=%s requestid=%d chain = %d", pRspInfo->ErrorID, pRspInfo->ErrorMsg, nRequestID, bIsLast);
			exit(-1);
	}
	// 端登成功
	// 订阅行情
	char * Instrumnet[] = { "IF0809","IF0812" };
	m_pUserApi->SubscribeMarketData(Instrumnet, 2);
	//或退订行情
	//m_pUserApi->UnSubscribeMarketData(Instrumnet, 2);
}

//登出请求响应
void MarketDataAPI::OnRspUserLogout(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	printf("OnRspUserLogout.\n");
}

///错误应答
void MarketDataAPI::OnRspError(CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	printf("OnRspError:\n");
	printf("ErrorCode=[%d], ErrorMsg=[%s]\n", pRspInfo->ErrorID, pRspInfo->ErrorMsg);
	printf("RequestID=[%d], Chain=[%d]\n", nRequestID, bIsLast);
	// 客户端需进行错误处理
}

//订阅行情应答
void MarketDataAPI::OnRspSubMarketData(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	printf("OnRspSubMarketData.\n");
}

//取消订阅行情应答
void MarketDataAPI::OnRspUnSubMarketData(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	printf("OnRspUnSubMarketData.\n");
}

//订阅询价应答
void MarketDataAPI::OnRspSubForQuoteRsp(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	printf("OnRspSubForQuoteRsp.\n");
}

//取消订阅询价应答
void MarketDataAPI::OnRspUnSubForQuoteRsp(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	printf("OnRspUnSubForQuoteRsp.\n");
}

//深度行情通知
void MarketDataAPI::OnRtnDepthMarketData(CThostFtdcDepthMarketDataField *pDepthMarketData) 
{
	printf("OnRtnDepthMarketData.\n");
	// 输出报单录入结果
	//printf("ErrorCode=[%d], ErrorMsg=[%s]\n", pRspInfo->ErrorID, pRspInfo->ErrorMsg);
	//收到深度行情
	//SetEvent(g_hEvent);
}

//询价通知
void MarketDataAPI::OnRtnForQuoteRsp(CThostFtdcForQuoteRspField *pForQuoteRsp) 
{
	printf("OnRtnForQuoteRsp.\n");
}