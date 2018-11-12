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


///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
void TradeDataAPI::OnFrontConnected()
{
	printf("OnFrontConnected\n");
	CThostFtdcReqUserLoginField reqUserLogin;

	strcpy(reqUserLogin.BrokerID, "9999");
	strcpy(reqUserLogin.UserID, "073423");
	strcpy(reqUserLogin.Password, "123456@shit");

	// 发出登陆请求
	m_pUserApi->ReqUserLogin(&reqUserLogin, 0);
}

///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
///@param nReason 错误原因
///        0x1001 网络读失败
///        0x1002 网络写失败
///        0x2001 接收心跳超时
///        0x2002 发送心跳失败
///        0x2003 收到错误报文
void TradeDataAPI::OnFrontDisconnected(int nReason)
{
	// 当发生这个情况后，API会自动重新连接，客户端可不做处理
	printf("OnFrontDisconnected.\n");
}

///心跳超时警告。当长时间未收到报文时，该方法被调用。
///@param nTimeLapse 距离上次接收报文的时间
void TradeDataAPI::OnHeartBeatWarning(int nTimeLapse)
{}

///客户端认证响应
void TradeDataAPI::OnRspAuthenticate(CThostFtdcRspAuthenticateField *pRspAuthenticateField, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}


///登录请求响应
void TradeDataAPI::OnRspUserLogin(CThostFtdcRspUserLoginField *pRspUserLogin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	printf("OnRspUserLogin:\n");
	printf("ErrorCode=[%d], ErrorMsg=[%s]\n", pRspInfo->ErrorID, pRspInfo->ErrorMsg);
	printf("RequestID=[%d], Chain=[%d]\n", nRequestID, bIsLast);
	if (pRspInfo->ErrorID != 0) {
		// 端登失败，客户端需进行错误处理
		printf("Failed to login, errorcode=%d errormsg=%s requestid=%d chain=%d", pRspInfo->ErrorID, pRspInfo->ErrorMsg, nRequestID, bIsLast);
		exit(-1);
	}
	else {
		printf("登录成功\n");
	}

	// 端登成功,发出报单录入请求
	CThostFtdcInputOrderField ord;

	memset(&ord, 0, sizeof(ord));
	//经纪公司代码
	strcpy(ord.BrokerID, "9999");
	//投资者代码
	strcpy(ord.InvestorID, "073423");
	// 合约代码
	strcpy(ord.InstrumentID, "cn0601");
	///报单引用
	strcpy(ord.OrderRef, "000000000001");
	// 用户代码
	strcpy(ord.UserID, "073423");
	// 报单价格条件
	ord.OrderPriceType = THOST_FTDC_OPT_LimitPrice;
	// 买卖方向
	ord.Direction = THOST_FTDC_D_Buy;
	// 组合开平标志
	strcpy(ord.CombOffsetFlag, "0");
	// 组合投机套保标志
	strcpy(ord.CombHedgeFlag, "1");
	// 价格
	ord.LimitPrice = 50000;
	// 数量
	ord.VolumeTotalOriginal = 10;
	// 有效期类型
	ord.TimeCondition = THOST_FTDC_TC_GFD;
	// GTD日期
	strcpy(ord.GTDDate, "");
	// 成交量类型
	ord.VolumeCondition = THOST_FTDC_VC_AV;
	// 最小成交量
	ord.MinVolume = 0;
	// 触发条件
	ord.ContingentCondition = THOST_FTDC_CC_Immediately;
	// 止损价
	ord.StopPrice = 0;
	// 强平原因
	ord.ForceCloseReason = THOST_FTDC_FCC_NotForceClose;
	// 自动挂起标志
	ord.IsAutoSuspend = 0;
	m_pUserApi->ReqOrderInsert(&ord, 1);
}

///登出请求响应
void TradeDataAPI::OnRspUserLogout(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///用户口令更新请求响应
void TradeDataAPI::OnRspUserPasswordUpdate(CThostFtdcUserPasswordUpdateField *pUserPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///资金账户口令更新请求响应
void TradeDataAPI::OnRspTradingAccountPasswordUpdate(CThostFtdcTradingAccountPasswordUpdateField *pTradingAccountPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///报单录入请求响应
void TradeDataAPI::OnRspOrderInsert(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	printf("OnRspOrderInsert.\n");
	// 输出报单录入结果
	printf("ErrorCode=[%d], ErrorMsg=[%s]\n", pRspInfo->ErrorID, pRspInfo->ErrorMsg);
	// 通知报单录入完成
	// TODO 映射功能到linux
	//SetEvent(g_hEvent);
}

///预埋单录入请求响应
void TradeDataAPI::OnRspParkedOrderInsert(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///预埋撤单录入请求响应
void TradeDataAPI::OnRspParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///报单操作请求响应
void TradeDataAPI::OnRspOrderAction(CThostFtdcInputOrderActionField *pInputOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///查询最大报单数量响应
void TradeDataAPI::OnRspQueryMaxOrderVolume(CThostFtdcQueryMaxOrderVolumeField *pQueryMaxOrderVolume, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///投资者结算结果确认响应
void TradeDataAPI::OnRspSettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///删除预埋单响应
void TradeDataAPI::OnRspRemoveParkedOrder(CThostFtdcRemoveParkedOrderField *pRemoveParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///删除预埋撤单响应
void TradeDataAPI::OnRspRemoveParkedOrderAction(CThostFtdcRemoveParkedOrderActionField *pRemoveParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///执行宣告录入请求响应
void TradeDataAPI::OnRspExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///执行宣告操作请求响应
void TradeDataAPI::OnRspExecOrderAction(CThostFtdcInputExecOrderActionField *pInputExecOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///询价录入请求响应
void TradeDataAPI::OnRspForQuoteInsert(CThostFtdcInputForQuoteField *pInputForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///报价录入请求响应
void TradeDataAPI::OnRspQuoteInsert(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///报价操作请求响应
void TradeDataAPI::OnRspQuoteAction(CThostFtdcInputQuoteActionField *pInputQuoteAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///批量报单操作请求响应
void TradeDataAPI::OnRspBatchOrderAction(CThostFtdcInputBatchOrderActionField *pInputBatchOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///申请组合录入请求响应
void TradeDataAPI::OnRspCombActionInsert(CThostFtdcInputCombActionField *pInputCombAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询报单响应
void TradeDataAPI::OnRspQryOrder(CThostFtdcOrderField *pOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询成交响应
void TradeDataAPI::OnRspQryTrade(CThostFtdcTradeField *pTrade, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询投资者持仓响应
void TradeDataAPI::OnRspQryInvestorPosition(CThostFtdcInvestorPositionField *pInvestorPosition, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询资金账户响应
void TradeDataAPI::OnRspQryTradingAccount(CThostFtdcTradingAccountField *pTradingAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询投资者响应
void TradeDataAPI::OnRspQryInvestor(CThostFtdcInvestorField *pInvestor, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询交易编码响应
void TradeDataAPI::OnRspQryTradingCode(CThostFtdcTradingCodeField *pTradingCode, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询合约保证金率响应
void TradeDataAPI::OnRspQryInstrumentMarginRate(CThostFtdcInstrumentMarginRateField *pInstrumentMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询合约手续费率响应
void TradeDataAPI::OnRspQryInstrumentCommissionRate(CThostFtdcInstrumentCommissionRateField *pInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询交易所响应
void TradeDataAPI::OnRspQryExchange(CThostFtdcExchangeField *pExchange, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询产品响应
void TradeDataAPI::OnRspQryProduct(CThostFtdcProductField *pProduct, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询合约响应
void TradeDataAPI::OnRspQryInstrument(CThostFtdcInstrumentField *pInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询行情响应
void TradeDataAPI::OnRspQryDepthMarketData(CThostFtdcDepthMarketDataField *pDepthMarketData, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询投资者结算结果响应
void TradeDataAPI::OnRspQrySettlementInfo(CThostFtdcSettlementInfoField *pSettlementInfo, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询转帐银行响应
void TradeDataAPI::OnRspQryTransferBank(CThostFtdcTransferBankField *pTransferBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询投资者持仓明细响应
void TradeDataAPI::OnRspQryInvestorPositionDetail(CThostFtdcInvestorPositionDetailField *pInvestorPositionDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询客户通知响应
void TradeDataAPI::OnRspQryNotice(CThostFtdcNoticeField *pNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询结算信息确认响应
void TradeDataAPI::OnRspQrySettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询投资者持仓明细响应
void TradeDataAPI::OnRspQryInvestorPositionCombineDetail(CThostFtdcInvestorPositionCombineDetailField *pInvestorPositionCombineDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///查询保证金监管系统经纪公司资金账户密钥响应
void TradeDataAPI::OnRspQryCFMMCTradingAccountKey(CThostFtdcCFMMCTradingAccountKeyField *pCFMMCTradingAccountKey, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询仓单折抵信息响应
void TradeDataAPI::OnRspQryEWarrantOffset(CThostFtdcEWarrantOffsetField *pEWarrantOffset, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询投资者品种/跨品种保证金响应
void TradeDataAPI::OnRspQryInvestorProductGroupMargin(CThostFtdcInvestorProductGroupMarginField *pInvestorProductGroupMargin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询交易所保证金率响应
void TradeDataAPI::OnRspQryExchangeMarginRate(CThostFtdcExchangeMarginRateField *pExchangeMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询交易所调整保证金率响应
void TradeDataAPI::OnRspQryExchangeMarginRateAdjust(CThostFtdcExchangeMarginRateAdjustField *pExchangeMarginRateAdjust, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询汇率响应
void TradeDataAPI::OnRspQryExchangeRate(CThostFtdcExchangeRateField *pExchangeRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询二级代理操作员银期权限响应
void TradeDataAPI::OnRspQrySecAgentACIDMap(CThostFtdcSecAgentACIDMapField *pSecAgentACIDMap, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询产品报价汇率
void TradeDataAPI::OnRspQryProductExchRate(CThostFtdcProductExchRateField *pProductExchRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询产品组
void TradeDataAPI::OnRspQryProductGroup(CThostFtdcProductGroupField *pProductGroup, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询期权交易成本响应
void TradeDataAPI::OnRspQryOptionInstrTradeCost(CThostFtdcOptionInstrTradeCostField *pOptionInstrTradeCost, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询期权合约手续费响应
void TradeDataAPI::OnRspQryOptionInstrCommRate(CThostFtdcOptionInstrCommRateField *pOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询执行宣告响应
void TradeDataAPI::OnRspQryExecOrder(CThostFtdcExecOrderField *pExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询询价响应
void TradeDataAPI::OnRspQryForQuote(CThostFtdcForQuoteField *pForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询报价响应
void TradeDataAPI::OnRspQryQuote(CThostFtdcQuoteField *pQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询组合合约安全系数响应
void TradeDataAPI::OnRspQryCombInstrumentGuard(CThostFtdcCombInstrumentGuardField *pCombInstrumentGuard, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询申请组合响应
void TradeDataAPI::OnRspQryCombAction(CThostFtdcCombActionField *pCombAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询转帐流水响应
void TradeDataAPI::OnRspQryTransferSerial(CThostFtdcTransferSerialField *pTransferSerial, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询银期签约关系响应
void TradeDataAPI::OnRspQryAccountregister(CThostFtdcAccountregisterField *pAccountregister, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///错误应答
void TradeDataAPI::OnRspError(CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	printf("OnRspError:\n");
	printf("ErrorCode=[%d], ErrorMsg=[%s]\n", pRspInfo->ErrorID, pRspInfo->ErrorMsg);
	printf("RequestID=[%d], Chain=[%d]\n", nRequestID, bIsLast);
	// 客户端需进行错误处理
	//{客户端的错误处理}
}

///报单通知
void TradeDataAPI::OnRtnOrder(CThostFtdcOrderField *pOrder)
{
	printf("OnRtnOrder:\n");
	printf("OrderSysID=[%s]\n", pOrder->OrderSysID);
}

///成交通知
void TradeDataAPI::OnRtnTrade(CThostFtdcTradeField *pTrade)
{}

///报单录入错误回报
void TradeDataAPI::OnErrRtnOrderInsert(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo)
{}

///报单操作错误回报
void TradeDataAPI::OnErrRtnOrderAction(CThostFtdcOrderActionField *pOrderAction, CThostFtdcRspInfoField *pRspInfo)
{}

///合约交易状态通知
void TradeDataAPI::OnRtnInstrumentStatus(CThostFtdcInstrumentStatusField *pInstrumentStatus)
{}

///交易通知
void TradeDataAPI::OnRtnTradingNotice(CThostFtdcTradingNoticeInfoField *pTradingNoticeInfo)
{}

///提示条件单校验错误
void TradeDataAPI::OnRtnErrorConditionalOrder(CThostFtdcErrorConditionalOrderField *pErrorConditionalOrder)
{}

///执行宣告通知
void TradeDataAPI::OnRtnExecOrder(CThostFtdcExecOrderField *pExecOrder)
{}

///执行宣告录入错误回报
void TradeDataAPI::OnErrRtnExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo)
{}

///执行宣告操作错误回报
void TradeDataAPI::OnErrRtnExecOrderAction(CThostFtdcExecOrderActionField *pExecOrderAction, CThostFtdcRspInfoField *pRspInfo)
{}


///询价录入错误回报
void TradeDataAPI::OnErrRtnForQuoteInsert(CThostFtdcInputForQuoteField *pInputForQuote, CThostFtdcRspInfoField *pRspInfo)
{}

///报价通知
void TradeDataAPI::OnRtnQuote(CThostFtdcQuoteField *pQuote)
{}

///报价录入错误回报
void TradeDataAPI::OnErrRtnQuoteInsert(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo)
{}

///报价操作错误回报
void TradeDataAPI::OnErrRtnQuoteAction(CThostFtdcQuoteActionField *pQuoteAction, CThostFtdcRspInfoField *pRspInfo)
{}

///询价通知
void TradeDataAPI::OnRtnForQuoteRsp(CThostFtdcForQuoteRspField *pForQuoteRsp)
{}

///保证金监控中心用户令牌
void TradeDataAPI::OnRtnCFMMCTradingAccountToken(CThostFtdcCFMMCTradingAccountTokenField *pCFMMCTradingAccountToken)
{}

///批量报单操作错误回报
void TradeDataAPI::OnErrRtnBatchOrderAction(CThostFtdcBatchOrderActionField *pBatchOrderAction, CThostFtdcRspInfoField *pRspInfo)
{}

///申请组合通知
void TradeDataAPI::OnRtnCombAction(CThostFtdcCombActionField *pCombAction)
{}

///申请组合录入错误回报
void TradeDataAPI::OnErrRtnCombActionInsert(CThostFtdcInputCombActionField *pInputCombAction, CThostFtdcRspInfoField *pRspInfo)
{}

///请求查询签约银行响应
void TradeDataAPI::OnRspQryContractBank(CThostFtdcContractBankField *pContractBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询预埋单响应
void TradeDataAPI::OnRspQryParkedOrder(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询预埋撤单响应
void TradeDataAPI::OnRspQryParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询交易通知响应
void TradeDataAPI::OnRspQryTradingNotice(CThostFtdcTradingNoticeField *pTradingNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询经纪公司交易参数响应
void TradeDataAPI::OnRspQryBrokerTradingParams(CThostFtdcBrokerTradingParamsField *pBrokerTradingParams, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
}

///请求查询经纪公司交易算法响应
void TradeDataAPI::OnRspQryBrokerTradingAlgos(CThostFtdcBrokerTradingAlgosField *pBrokerTradingAlgos, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询监控中心用户令牌
void TradeDataAPI::OnRspQueryCFMMCTradingAccountToken(CThostFtdcQueryCFMMCTradingAccountTokenField *pQueryCFMMCTradingAccountToken, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///银行发起银行资金转期货通知
void TradeDataAPI::OnRtnFromBankToFutureByBank(CThostFtdcRspTransferField *pRspTransfer)
{}

///银行发起期货资金转银行通知
void TradeDataAPI::OnRtnFromFutureToBankByBank(CThostFtdcRspTransferField *pRspTransfer)
{}

///银行发起冲正银行转期货通知
void TradeDataAPI::OnRtnRepealFromBankToFutureByBank(CThostFtdcRspRepealField *pRspRepeal)
{}

///银行发起冲正期货转银行通知
void TradeDataAPI::OnRtnRepealFromFutureToBankByBank(CThostFtdcRspRepealField *pRspRepeal)
{}

///期货发起银行资金转期货通知
void TradeDataAPI::OnRtnFromBankToFutureByFuture(CThostFtdcRspTransferField *pRspTransfer)
{}

///期货发起期货资金转银行通知
void TradeDataAPI::OnRtnFromFutureToBankByFuture(CThostFtdcRspTransferField *pRspTransfer)
{}

///系统运行时期货端手工发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
void TradeDataAPI::OnRtnRepealFromBankToFutureByFutureManual(CThostFtdcRspRepealField *pRspRepeal)
{}

///系统运行时期货端手工发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
void TradeDataAPI::OnRtnRepealFromFutureToBankByFutureManual(CThostFtdcRspRepealField *pRspRepeal)
{}

///期货发起查询银行余额通知
void TradeDataAPI::OnRtnQueryBankBalanceByFuture(CThostFtdcNotifyQueryAccountField *pNotifyQueryAccount)
{}

///期货发起银行资金转期货错误回报
void TradeDataAPI::OnErrRtnBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo)
{}

///期货发起期货资金转银行错误回报
void TradeDataAPI::OnErrRtnFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo)
{}

///系统运行时期货端手工发起冲正银行转期货错误回报
void TradeDataAPI::OnErrRtnRepealBankToFutureByFutureManual(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo)
{}

///系统运行时期货端手工发起冲正期货转银行错误回报
void TradeDataAPI::OnErrRtnRepealFutureToBankByFutureManual(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo)
{}

///期货发起查询银行余额错误回报
void TradeDataAPI::OnErrRtnQueryBankBalanceByFuture(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo)
{}

///期货发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
void TradeDataAPI::OnRtnRepealFromBankToFutureByFuture(CThostFtdcRspRepealField *pRspRepeal)
{}

///期货发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
void TradeDataAPI::OnRtnRepealFromFutureToBankByFuture(CThostFtdcRspRepealField *pRspRepeal)
{}

///期货发起银行资金转期货应答
void TradeDataAPI::OnRspFromBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///期货发起期货资金转银行应答
void TradeDataAPI::OnRspFromFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///期货发起查询银行余额应答
void TradeDataAPI::OnRspQueryBankAccountMoneyByFuture(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///银行发起银期开户通知
void TradeDataAPI::OnRtnOpenAccountByBank(CThostFtdcOpenAccountField *pOpenAccount)
{}

///银行发起银期销户通知
void TradeDataAPI::OnRtnCancelAccountByBank(CThostFtdcCancelAccountField *pCancelAccount)
{}

///银行发起变更银行账号通知
void TradeDataAPI::OnRtnChangeAccountByBank(CThostFtdcChangeAccountField *pChangeAccount)
{}
