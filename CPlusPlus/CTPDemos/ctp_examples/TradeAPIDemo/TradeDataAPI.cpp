#pragma once

#include "stdafx.h"
#include "TradeDataAPI.h"


TradeDataAPI::TradeDataAPI(CThostFtdcTraderApi *pUserApi)
{
	m_pUserApi = pUserApi;
}


TradeDataAPI::~TradeDataAPI()
{
}


///���ͻ����뽻�׺�̨������ͨ������ʱ����δ��¼ǰ�����÷��������á�
void TradeDataAPI::OnFrontConnected()
{
	printf("OnFrontConnected\n");
	CThostFtdcReqUserLoginField reqUserLogin;

	strcpy(reqUserLogin.BrokerID, "9999");
	strcpy(reqUserLogin.UserID, "073423");
	strcpy(reqUserLogin.Password, "123456@shit");

	// ������½����
	m_pUserApi->ReqUserLogin(&reqUserLogin, 0);
}

///���ͻ����뽻�׺�̨ͨ�����ӶϿ�ʱ���÷��������á���������������API���Զ��������ӣ��ͻ��˿ɲ�������
///@param nReason ����ԭ��
///        0x1001 �����ʧ��
///        0x1002 ����дʧ��
///        0x2001 ����������ʱ
///        0x2002 ��������ʧ��
///        0x2003 �յ�������
void TradeDataAPI::OnFrontDisconnected(int nReason)
{
	// ��������������API���Զ��������ӣ��ͻ��˿ɲ�������
	printf("OnFrontDisconnected.\n");
}

///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
///@param nTimeLapse �����ϴν��ձ��ĵ�ʱ��
void TradeDataAPI::OnHeartBeatWarning(int nTimeLapse)
{}

///�ͻ�����֤��Ӧ
void TradeDataAPI::OnRspAuthenticate(CThostFtdcRspAuthenticateField *pRspAuthenticateField, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}


///��¼������Ӧ
void TradeDataAPI::OnRspUserLogin(CThostFtdcRspUserLoginField *pRspUserLogin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	printf("OnRspUserLogin:\n");
	printf("ErrorCode=[%d], ErrorMsg=[%s]\n", pRspInfo->ErrorID, pRspInfo->ErrorMsg);
	printf("RequestID=[%d], Chain=[%d]\n", nRequestID, bIsLast);
	if (pRspInfo->ErrorID != 0) {
		// �˵�ʧ�ܣ��ͻ�������д�����
		printf("Failed to login, errorcode=%d errormsg=%s requestid=%d chain=%d", pRspInfo->ErrorID, pRspInfo->ErrorMsg, nRequestID, bIsLast);
		exit(-1);
	}
	else {
		printf("��¼�ɹ�\n");
	}

	// �˵ǳɹ�,��������¼������
	CThostFtdcInputOrderField ord;

	memset(&ord, 0, sizeof(ord));
	//���͹�˾����
	strcpy(ord.BrokerID, "9999");
	//Ͷ���ߴ���
	strcpy(ord.InvestorID, "073423");
	// ��Լ����
	strcpy(ord.InstrumentID, "cn0601");
	///��������
	strcpy(ord.OrderRef, "000000000001");
	// �û�����
	strcpy(ord.UserID, "073423");
	// �����۸�����
	ord.OrderPriceType = THOST_FTDC_OPT_LimitPrice;
	// ��������
	ord.Direction = THOST_FTDC_D_Buy;
	// ��Ͽ�ƽ��־
	strcpy(ord.CombOffsetFlag, "0");
	// ���Ͷ���ױ���־
	strcpy(ord.CombHedgeFlag, "1");
	// �۸�
	ord.LimitPrice = 50000;
	// ����
	ord.VolumeTotalOriginal = 10;
	// ��Ч������
	ord.TimeCondition = THOST_FTDC_TC_GFD;
	// GTD����
	strcpy(ord.GTDDate, "");
	// �ɽ�������
	ord.VolumeCondition = THOST_FTDC_VC_AV;
	// ��С�ɽ���
	ord.MinVolume = 0;
	// ��������
	ord.ContingentCondition = THOST_FTDC_CC_Immediately;
	// ֹ���
	ord.StopPrice = 0;
	// ǿƽԭ��
	ord.ForceCloseReason = THOST_FTDC_FCC_NotForceClose;
	// �Զ������־
	ord.IsAutoSuspend = 0;
	m_pUserApi->ReqOrderInsert(&ord, 1);
}

///�ǳ�������Ӧ
void TradeDataAPI::OnRspUserLogout(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�û��������������Ӧ
void TradeDataAPI::OnRspUserPasswordUpdate(CThostFtdcUserPasswordUpdateField *pUserPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�ʽ��˻��������������Ӧ
void TradeDataAPI::OnRspTradingAccountPasswordUpdate(CThostFtdcTradingAccountPasswordUpdateField *pTradingAccountPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///����¼��������Ӧ
void TradeDataAPI::OnRspOrderInsert(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	printf("OnRspOrderInsert.\n");
	// �������¼����
	printf("ErrorCode=[%d], ErrorMsg=[%s]\n", pRspInfo->ErrorID, pRspInfo->ErrorMsg);
	// ֪ͨ����¼�����
	// TODO ӳ�书�ܵ�linux
	//SetEvent(g_hEvent);
}

///Ԥ��¼��������Ӧ
void TradeDataAPI::OnRspParkedOrderInsert(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///Ԥ�񳷵�¼��������Ӧ
void TradeDataAPI::OnRspParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///��������������Ӧ
void TradeDataAPI::OnRspOrderAction(CThostFtdcInputOrderActionField *pInputOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///��ѯ��󱨵�������Ӧ
void TradeDataAPI::OnRspQueryMaxOrderVolume(CThostFtdcQueryMaxOrderVolumeField *pQueryMaxOrderVolume, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///Ͷ���߽�����ȷ����Ӧ
void TradeDataAPI::OnRspSettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///ɾ��Ԥ����Ӧ
void TradeDataAPI::OnRspRemoveParkedOrder(CThostFtdcRemoveParkedOrderField *pRemoveParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///ɾ��Ԥ�񳷵���Ӧ
void TradeDataAPI::OnRspRemoveParkedOrderAction(CThostFtdcRemoveParkedOrderActionField *pRemoveParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///ִ������¼��������Ӧ
void TradeDataAPI::OnRspExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///ִ���������������Ӧ
void TradeDataAPI::OnRspExecOrderAction(CThostFtdcInputExecOrderActionField *pInputExecOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///ѯ��¼��������Ӧ
void TradeDataAPI::OnRspForQuoteInsert(CThostFtdcInputForQuoteField *pInputForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///����¼��������Ӧ
void TradeDataAPI::OnRspQuoteInsert(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///���۲���������Ӧ
void TradeDataAPI::OnRspQuoteAction(CThostFtdcInputQuoteActionField *pInputQuoteAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///������������������Ӧ
void TradeDataAPI::OnRspBatchOrderAction(CThostFtdcInputBatchOrderActionField *pInputBatchOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�������¼��������Ӧ
void TradeDataAPI::OnRspCombActionInsert(CThostFtdcInputCombActionField *pInputCombAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ������Ӧ
void TradeDataAPI::OnRspQryOrder(CThostFtdcOrderField *pOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ�ɽ���Ӧ
void TradeDataAPI::OnRspQryTrade(CThostFtdcTradeField *pTrade, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯͶ���ֲ߳���Ӧ
void TradeDataAPI::OnRspQryInvestorPosition(CThostFtdcInvestorPositionField *pInvestorPosition, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ�ʽ��˻���Ӧ
void TradeDataAPI::OnRspQryTradingAccount(CThostFtdcTradingAccountField *pTradingAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯͶ������Ӧ
void TradeDataAPI::OnRspQryInvestor(CThostFtdcInvestorField *pInvestor, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ���ױ�����Ӧ
void TradeDataAPI::OnRspQryTradingCode(CThostFtdcTradingCodeField *pTradingCode, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ��Լ��֤������Ӧ
void TradeDataAPI::OnRspQryInstrumentMarginRate(CThostFtdcInstrumentMarginRateField *pInstrumentMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ��Լ����������Ӧ
void TradeDataAPI::OnRspQryInstrumentCommissionRate(CThostFtdcInstrumentCommissionRateField *pInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ��������Ӧ
void TradeDataAPI::OnRspQryExchange(CThostFtdcExchangeField *pExchange, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ��Ʒ��Ӧ
void TradeDataAPI::OnRspQryProduct(CThostFtdcProductField *pProduct, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ��Լ��Ӧ
void TradeDataAPI::OnRspQryInstrument(CThostFtdcInstrumentField *pInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ������Ӧ
void TradeDataAPI::OnRspQryDepthMarketData(CThostFtdcDepthMarketDataField *pDepthMarketData, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯͶ���߽�������Ӧ
void TradeDataAPI::OnRspQrySettlementInfo(CThostFtdcSettlementInfoField *pSettlementInfo, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯת��������Ӧ
void TradeDataAPI::OnRspQryTransferBank(CThostFtdcTransferBankField *pTransferBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯͶ���ֲ߳���ϸ��Ӧ
void TradeDataAPI::OnRspQryInvestorPositionDetail(CThostFtdcInvestorPositionDetailField *pInvestorPositionDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ�ͻ�֪ͨ��Ӧ
void TradeDataAPI::OnRspQryNotice(CThostFtdcNoticeField *pNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ������Ϣȷ����Ӧ
void TradeDataAPI::OnRspQrySettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯͶ���ֲ߳���ϸ��Ӧ
void TradeDataAPI::OnRspQryInvestorPositionCombineDetail(CThostFtdcInvestorPositionCombineDetailField *pInvestorPositionCombineDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///��ѯ��֤����ϵͳ���͹�˾�ʽ��˻���Կ��Ӧ
void TradeDataAPI::OnRspQryCFMMCTradingAccountKey(CThostFtdcCFMMCTradingAccountKeyField *pCFMMCTradingAccountKey, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ�ֵ��۵���Ϣ��Ӧ
void TradeDataAPI::OnRspQryEWarrantOffset(CThostFtdcEWarrantOffsetField *pEWarrantOffset, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯͶ����Ʒ��/��Ʒ�ֱ�֤����Ӧ
void TradeDataAPI::OnRspQryInvestorProductGroupMargin(CThostFtdcInvestorProductGroupMarginField *pInvestorProductGroupMargin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ��������֤������Ӧ
void TradeDataAPI::OnRspQryExchangeMarginRate(CThostFtdcExchangeMarginRateField *pExchangeMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ������������֤������Ӧ
void TradeDataAPI::OnRspQryExchangeMarginRateAdjust(CThostFtdcExchangeMarginRateAdjustField *pExchangeMarginRateAdjust, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ������Ӧ
void TradeDataAPI::OnRspQryExchangeRate(CThostFtdcExchangeRateField *pExchangeRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ�����������Ա����Ȩ����Ӧ
void TradeDataAPI::OnRspQrySecAgentACIDMap(CThostFtdcSecAgentACIDMapField *pSecAgentACIDMap, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ��Ʒ���ۻ���
void TradeDataAPI::OnRspQryProductExchRate(CThostFtdcProductExchRateField *pProductExchRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ��Ʒ��
void TradeDataAPI::OnRspQryProductGroup(CThostFtdcProductGroupField *pProductGroup, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ��Ȩ���׳ɱ���Ӧ
void TradeDataAPI::OnRspQryOptionInstrTradeCost(CThostFtdcOptionInstrTradeCostField *pOptionInstrTradeCost, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ��Ȩ��Լ��������Ӧ
void TradeDataAPI::OnRspQryOptionInstrCommRate(CThostFtdcOptionInstrCommRateField *pOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯִ��������Ӧ
void TradeDataAPI::OnRspQryExecOrder(CThostFtdcExecOrderField *pExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯѯ����Ӧ
void TradeDataAPI::OnRspQryForQuote(CThostFtdcForQuoteField *pForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ������Ӧ
void TradeDataAPI::OnRspQryQuote(CThostFtdcQuoteField *pQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ��Ϻ�Լ��ȫϵ����Ӧ
void TradeDataAPI::OnRspQryCombInstrumentGuard(CThostFtdcCombInstrumentGuardField *pCombInstrumentGuard, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ���������Ӧ
void TradeDataAPI::OnRspQryCombAction(CThostFtdcCombActionField *pCombAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯת����ˮ��Ӧ
void TradeDataAPI::OnRspQryTransferSerial(CThostFtdcTransferSerialField *pTransferSerial, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ����ǩԼ��ϵ��Ӧ
void TradeDataAPI::OnRspQryAccountregister(CThostFtdcAccountregisterField *pAccountregister, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///����Ӧ��
void TradeDataAPI::OnRspError(CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	printf("OnRspError:\n");
	printf("ErrorCode=[%d], ErrorMsg=[%s]\n", pRspInfo->ErrorID, pRspInfo->ErrorMsg);
	printf("RequestID=[%d], Chain=[%d]\n", nRequestID, bIsLast);
	// �ͻ�������д�����
	//{�ͻ��˵Ĵ�����}
}

///����֪ͨ
void TradeDataAPI::OnRtnOrder(CThostFtdcOrderField *pOrder)
{
	printf("OnRtnOrder:\n");
	printf("OrderSysID=[%s]\n", pOrder->OrderSysID);
}

///�ɽ�֪ͨ
void TradeDataAPI::OnRtnTrade(CThostFtdcTradeField *pTrade)
{}

///����¼�����ر�
void TradeDataAPI::OnErrRtnOrderInsert(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo)
{}

///������������ر�
void TradeDataAPI::OnErrRtnOrderAction(CThostFtdcOrderActionField *pOrderAction, CThostFtdcRspInfoField *pRspInfo)
{}

///��Լ����״̬֪ͨ
void TradeDataAPI::OnRtnInstrumentStatus(CThostFtdcInstrumentStatusField *pInstrumentStatus)
{}

///����֪ͨ
void TradeDataAPI::OnRtnTradingNotice(CThostFtdcTradingNoticeInfoField *pTradingNoticeInfo)
{}

///��ʾ������У�����
void TradeDataAPI::OnRtnErrorConditionalOrder(CThostFtdcErrorConditionalOrderField *pErrorConditionalOrder)
{}

///ִ������֪ͨ
void TradeDataAPI::OnRtnExecOrder(CThostFtdcExecOrderField *pExecOrder)
{}

///ִ������¼�����ر�
void TradeDataAPI::OnErrRtnExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo)
{}

///ִ�������������ر�
void TradeDataAPI::OnErrRtnExecOrderAction(CThostFtdcExecOrderActionField *pExecOrderAction, CThostFtdcRspInfoField *pRspInfo)
{}


///ѯ��¼�����ر�
void TradeDataAPI::OnErrRtnForQuoteInsert(CThostFtdcInputForQuoteField *pInputForQuote, CThostFtdcRspInfoField *pRspInfo)
{}

///����֪ͨ
void TradeDataAPI::OnRtnQuote(CThostFtdcQuoteField *pQuote)
{}

///����¼�����ر�
void TradeDataAPI::OnErrRtnQuoteInsert(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo)
{}

///���۲�������ر�
void TradeDataAPI::OnErrRtnQuoteAction(CThostFtdcQuoteActionField *pQuoteAction, CThostFtdcRspInfoField *pRspInfo)
{}

///ѯ��֪ͨ
void TradeDataAPI::OnRtnForQuoteRsp(CThostFtdcForQuoteRspField *pForQuoteRsp)
{}

///��֤���������û�����
void TradeDataAPI::OnRtnCFMMCTradingAccountToken(CThostFtdcCFMMCTradingAccountTokenField *pCFMMCTradingAccountToken)
{}

///����������������ر�
void TradeDataAPI::OnErrRtnBatchOrderAction(CThostFtdcBatchOrderActionField *pBatchOrderAction, CThostFtdcRspInfoField *pRspInfo)
{}

///�������֪ͨ
void TradeDataAPI::OnRtnCombAction(CThostFtdcCombActionField *pCombAction)
{}

///�������¼�����ر�
void TradeDataAPI::OnErrRtnCombActionInsert(CThostFtdcInputCombActionField *pInputCombAction, CThostFtdcRspInfoField *pRspInfo)
{}

///�����ѯǩԼ������Ӧ
void TradeDataAPI::OnRspQryContractBank(CThostFtdcContractBankField *pContractBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯԤ����Ӧ
void TradeDataAPI::OnRspQryParkedOrder(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯԤ�񳷵���Ӧ
void TradeDataAPI::OnRspQryParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ����֪ͨ��Ӧ
void TradeDataAPI::OnRspQryTradingNotice(CThostFtdcTradingNoticeField *pTradingNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ���͹�˾���ײ�����Ӧ
void TradeDataAPI::OnRspQryBrokerTradingParams(CThostFtdcBrokerTradingParamsField *pBrokerTradingParams, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
}

///�����ѯ���͹�˾�����㷨��Ӧ
void TradeDataAPI::OnRspQryBrokerTradingAlgos(CThostFtdcBrokerTradingAlgosField *pBrokerTradingAlgos, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ��������û�����
void TradeDataAPI::OnRspQueryCFMMCTradingAccountToken(CThostFtdcQueryCFMMCTradingAccountTokenField *pQueryCFMMCTradingAccountToken, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///���з��������ʽ�ת�ڻ�֪ͨ
void TradeDataAPI::OnRtnFromBankToFutureByBank(CThostFtdcRspTransferField *pRspTransfer)
{}

///���з����ڻ��ʽ�ת����֪ͨ
void TradeDataAPI::OnRtnFromFutureToBankByBank(CThostFtdcRspTransferField *pRspTransfer)
{}

///���з����������ת�ڻ�֪ͨ
void TradeDataAPI::OnRtnRepealFromBankToFutureByBank(CThostFtdcRspRepealField *pRspRepeal)
{}

///���з�������ڻ�ת����֪ͨ
void TradeDataAPI::OnRtnRepealFromFutureToBankByBank(CThostFtdcRspRepealField *pRspRepeal)
{}

///�ڻ����������ʽ�ת�ڻ�֪ͨ
void TradeDataAPI::OnRtnFromBankToFutureByFuture(CThostFtdcRspTransferField *pRspTransfer)
{}

///�ڻ������ڻ��ʽ�ת����֪ͨ
void TradeDataAPI::OnRtnFromFutureToBankByFuture(CThostFtdcRspTransferField *pRspTransfer)
{}

///ϵͳ����ʱ�ڻ����ֹ������������ת�ڻ��������д�����Ϻ��̷��ص�֪ͨ
void TradeDataAPI::OnRtnRepealFromBankToFutureByFutureManual(CThostFtdcRspRepealField *pRspRepeal)
{}

///ϵͳ����ʱ�ڻ����ֹ���������ڻ�ת�����������д�����Ϻ��̷��ص�֪ͨ
void TradeDataAPI::OnRtnRepealFromFutureToBankByFutureManual(CThostFtdcRspRepealField *pRspRepeal)
{}

///�ڻ������ѯ�������֪ͨ
void TradeDataAPI::OnRtnQueryBankBalanceByFuture(CThostFtdcNotifyQueryAccountField *pNotifyQueryAccount)
{}

///�ڻ����������ʽ�ת�ڻ�����ر�
void TradeDataAPI::OnErrRtnBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo)
{}

///�ڻ������ڻ��ʽ�ת���д���ر�
void TradeDataAPI::OnErrRtnFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo)
{}

///ϵͳ����ʱ�ڻ����ֹ������������ת�ڻ�����ر�
void TradeDataAPI::OnErrRtnRepealBankToFutureByFutureManual(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo)
{}

///ϵͳ����ʱ�ڻ����ֹ���������ڻ�ת���д���ر�
void TradeDataAPI::OnErrRtnRepealFutureToBankByFutureManual(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo)
{}

///�ڻ������ѯ����������ر�
void TradeDataAPI::OnErrRtnQueryBankBalanceByFuture(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo)
{}

///�ڻ������������ת�ڻ��������д�����Ϻ��̷��ص�֪ͨ
void TradeDataAPI::OnRtnRepealFromBankToFutureByFuture(CThostFtdcRspRepealField *pRspRepeal)
{}

///�ڻ���������ڻ�ת�����������д�����Ϻ��̷��ص�֪ͨ
void TradeDataAPI::OnRtnRepealFromFutureToBankByFuture(CThostFtdcRspRepealField *pRspRepeal)
{}

///�ڻ����������ʽ�ת�ڻ�Ӧ��
void TradeDataAPI::OnRspFromBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�ڻ������ڻ��ʽ�ת����Ӧ��
void TradeDataAPI::OnRspFromFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�ڻ������ѯ�������Ӧ��
void TradeDataAPI::OnRspQueryBankAccountMoneyByFuture(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///���з������ڿ���֪ͨ
void TradeDataAPI::OnRtnOpenAccountByBank(CThostFtdcOpenAccountField *pOpenAccount)
{}

///���з�����������֪ͨ
void TradeDataAPI::OnRtnCancelAccountByBank(CThostFtdcCancelAccountField *pCancelAccount)
{}

///���з����������˺�֪ͨ
void TradeDataAPI::OnRtnChangeAccountByBank(CThostFtdcChangeAccountField *pChangeAccount)
{}
