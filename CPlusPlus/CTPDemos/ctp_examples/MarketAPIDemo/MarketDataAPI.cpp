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


//���ͻ����뽻�׺�̨������ͨ������ʱ����δ��¼ǰ�����÷��������á�
void MarketDataAPI::OnFrontConnected() 
{
	printf("OnFrontConnected\n");
	CThostFtdcReqUserLoginField reqUserLogin;

	// ��Ա����
	TThostFtdcBrokerIDType chBrokerID;
	// �����û�����
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
	// ������½����
	m_pUserApi->ReqUserLogin(&reqUserLogin, 0);
}

//���ͻ����뽻�׺�̨ͨ�����ӶϿ�ʱ���÷��������á���������������API���Զ��������ӣ��ͻ��˿ɲ�������
//@param nReason ����ԭ��
//        0x1001 �����ʧ��
//        0x1002 ����дʧ��
//        0x2001 ����������ʱ
//        0x2002 ��������ʧ��
//        0x2003 �յ�������
void MarketDataAPI::OnFrontDisconnected(int nReason) 
{
	// ��������������API���Զ��������ӣ��ͻ��˿ɲ�������
	printf("OnFrontDisconnected.\n");
}

//������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
//@param nTimeLapse �����ϴν��ձ��ĵ�ʱ��
void MarketDataAPI::OnHeartBeatWarning(int nTimeLapse) 
{
	printf("OnHeartBeatWarning.\n");
}


//��¼������Ӧ
void MarketDataAPI::OnRspUserLogin(CThostFtdcRspUserLoginField *pRspUserLogin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	printf("OnRspUserLogin:\n");
	printf("ErrorCode=[%d], ErrorMsg=[%s]\n", pRspInfo->ErrorID, pRspInfo->ErrorMsg);
	printf("RequestID=[%d], Chain=[%d]\n", nRequestID, bIsLast);

	if (pRspInfo->ErrorID != 0) {
		// �˵�ʧ�ܣ��ͻ�������д�����
		printf("Failed to login, errorcode=%d errormsg=%s requestid=%d chain = %d", pRspInfo->ErrorID, pRspInfo->ErrorMsg, nRequestID, bIsLast);
			exit(-1);
	}
	// �˵ǳɹ�
	// ��������
	char * Instrumnet[] = { "IF0809","IF0812" };
	m_pUserApi->SubscribeMarketData(Instrumnet, 2);
	//���˶�����
	//m_pUserApi->UnSubscribeMarketData(Instrumnet, 2);
}

//�ǳ�������Ӧ
void MarketDataAPI::OnRspUserLogout(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	printf("OnRspUserLogout.\n");
}

///����Ӧ��
void MarketDataAPI::OnRspError(CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	printf("OnRspError:\n");
	printf("ErrorCode=[%d], ErrorMsg=[%s]\n", pRspInfo->ErrorID, pRspInfo->ErrorMsg);
	printf("RequestID=[%d], Chain=[%d]\n", nRequestID, bIsLast);
	// �ͻ�������д�����
}

//��������Ӧ��
void MarketDataAPI::OnRspSubMarketData(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	printf("OnRspSubMarketData.\n");
}

//ȡ����������Ӧ��
void MarketDataAPI::OnRspUnSubMarketData(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	printf("OnRspUnSubMarketData.\n");
}

//����ѯ��Ӧ��
void MarketDataAPI::OnRspSubForQuoteRsp(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	printf("OnRspSubForQuoteRsp.\n");
}

//ȡ������ѯ��Ӧ��
void MarketDataAPI::OnRspUnSubForQuoteRsp(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	printf("OnRspUnSubForQuoteRsp.\n");
}

//�������֪ͨ
void MarketDataAPI::OnRtnDepthMarketData(CThostFtdcDepthMarketDataField *pDepthMarketData) 
{
	printf("OnRtnDepthMarketData.\n");
	// �������¼����
	//printf("ErrorCode=[%d], ErrorMsg=[%s]\n", pRspInfo->ErrorID, pRspInfo->ErrorMsg);
	//�յ��������
	//SetEvent(g_hEvent);
}

//ѯ��֪ͨ
void MarketDataAPI::OnRtnForQuoteRsp(CThostFtdcForQuoteRspField *pForQuoteRsp) 
{
	printf("OnRtnForQuoteRsp.\n");
}