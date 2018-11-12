// MarketAPIDemo.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>

#include "MarketDataAPI.h"

using namespace std;

int main()
{
	// ����һ��CThostFtdcMdApiʵ��
	CThostFtdcMdApi *pUserApi = CThostFtdcMdApi::CreateFtdcMdApi();
	// ����һ���¼������ʵ��
	MarketDataAPI sh(pUserApi);
	// ע��һ�¼������ʵ��
	pUserApi->RegisterSpi(&sh);
	// ���ý����й�ϵͳ����ĵ�ַ������ע������ַ����
	//http://www.simnow.com.cn/product.action
	pUserApi->RegisterFront("tcp://180.168.146.187:10010");
	// ʹ�ͻ��˿�ʼ���̨����������
	pUserApi->Init();
	// �ͻ��˵ȴ������������
	//WaitForSingleObject(g_hEvent, INFINITE);
	
	system("pause");

	// �ͷ�APIʵ��
	pUserApi->Release();

	return 0;
}

