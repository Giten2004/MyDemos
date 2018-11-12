// TradeAPIDemo.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "TradeDataAPI.h"
#include <iostream>

using namespace std;

int main()
{
	CThostFtdcTraderApi *pUserApi = CThostFtdcTraderApi::CreateFtdcTraderApi();
	// ����һ���¼������ʵ��
	TradeDataAPI sh(pUserApi);
	// ע��һ�¼������ʵ��
	pUserApi->RegisterSpi(&sh);
	// ����˽����
	// TERT_RESTART:�ӱ������տ�ʼ�ش�
	// TERT_RESUME:���ϴ��յ�������
	// TERT_QUICK:ֻ���͵�¼��˽����������
	pUserApi->SubscribePrivateTopic(THOST_TE_RESUME_TYPE::THOST_TERT_RESUME);
	// ���Ĺ�����
	// TERT_RESTART:�ӱ������տ�ʼ�ش�
	// TERT_RESUME:���ϴ��յ�������
	// TERT_QUICK:ֻ���͵�¼�󹫹���������
	pUserApi->SubscribePublicTopic(THOST_TE_RESUME_TYPE::THOST_TERT_RESUME);
	// ���ý����й�ϵͳ����ĵ�ַ������ע������ַ����
	pUserApi->RegisterFront("tcp://180.168.146.187:10001");
	// ʹ�ͻ��˿�ʼ���̨����������
	pUserApi->Init();
	// �ͻ��˵ȴ������������
	//WaitForSingleObject(g_hEvent, INFINITE);
	system("pause");
	// �ͷ�APIʵ��
	pUserApi->Release();
	return 0;
}

