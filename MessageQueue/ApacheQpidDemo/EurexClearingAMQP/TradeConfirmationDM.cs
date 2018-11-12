using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EurexClearingAMQP
{
    public enum TradeReportTransType
    {
        New = 0,
        Cancel = 1,
        Replace = 2,
        Reverse = 4
    }

    public enum TradeReportType
    {
        Submit = 0,
        Alleged = 1,
        Accept = 2,
        No_Was = 5,
        Trade_Report_Cancel = 6,
        Alleged_New = 11,
        Alleged_No_Was = 13
    }

    public enum TrdType
    {
        RegularTrade = 0,
        OTC_Block_Trade = 1,
        /// <summary>
        /// EFP (Exchange for Physical)
        /// </summary>
        EFP = 2,
        /// <summary>
        /// Exchange for Swap (EFS )
        /// </summary>
        EFS = 12,
        OTC = 54,
        /// <summary>
        /// Exchange Basis Facility (EBF)
        /// </summary>
        EBF = 55,
        Vola_Trade = 1000,
        EFPFin_Trade = 1001,
        EFPIndexFutures_Trade = 1002
    }

    public enum TradeHandlingInstr
    {
        Trade_Confirmation = 0,
        TradeHandlingInstr_3 = 3,
        TradeHandlingInstr_7 = 7
    }

    public enum OrigTradeHandlingInstr
    {
        OrigTradeHandlingInstr_0 = 0,
        OneParty_Report_For_Pass_Through = 3,
        ThirdParty_Report_For_Pass_Through = 7
    }

    public enum MultiLegReportingType
    {
        /// <summary>
        /// Single security (default if not specified)
        /// </summary>
        One = 1,
        /// <summary>
        /// Individual leg of a multi-leg security
        /// </summary>
        Two = 2
    }

    public enum RootPartyRole
    {
        ExecutingFirm = 1,
        ClearingFirm = 4,
        EnteringFirm = 7,
        /// <summary>
        /// Settlement Location (formerly FIX 4.2 SettlLocation)
        /// </summary>
        SettlementLocation = 10,
        /// <summary>
        /// Executing Trader (associated with Executing Firm - actually executes)
        /// </summary>
        ExecutingTrader = 12,
        ContraFirm = 17,
        EnteringTrader = 36,
        PositionAccount = 38,
        ContraInvestorID = 39,
        Transfer2Firm = 40,
        ExecutingUnit = 59,
        GiveUpFirm = 95,//Give-up (Trading) Firm
        TakeUpFirm = 96, //Take-up (Trading) Firm
        R97 = 97,
        R98 = 98,
        R101 = 101
    }

    public enum PutOrCall
    {
        Put = 0,
        Call = 1
    }

    public enum SecurityIDSource
    {
        ISINNumber = 4,
        ExchangeSymbol = 8
    }

    public enum EventType
    {
        Activation = 5,
        Swap_Start_Date = 8,
        Swap_End_Date = 9
    }

    public enum PosAmtType
    {
        /// <summary>
        /// Premium Amount
        /// </summary>
        PREM = 0
    }

    public enum TrdRegTimestampType
    {
        ExecutionTime = 1,
        TrdRegTimestampType_9 = 9
    }

    public enum Side
    {
        Buy = 1,
        Sell = 2,
        /// <summary>
        /// Undisclosed (valid for IOI and List Order messages only)
        /// </summary>
        Undisclosed = 7
    }

    public enum PositionEffect
    {
        C = 0,
        O = 1
    }

    public enum SideTrdRegTimestampType
    {
        ExecutionTime = 1,
        SideTrdRegTimestampType9 = 9
    }

    public enum TradeAllocIndicator
    {
        AllocationNotRequired = 0,
        /// <summary>
        /// Allocation required (give-up trade) allocation information not provided (incomplete)
        /// </summary>
        Allocation = 1,
        /// <summary>
        /// Use allocation provided with the trade
        /// </summary>
        UseAllocation = 2,
        /// <summary>
        /// Allocation to claim account
        /// </summary>
        AllocationTo = 5,
        TradeAllocIndicator6 = 6
    }

    public enum AggressorIndicator
    {

        /// <summary>
        /// Order initiator is passive
        /// </summary>
        N = 0,
        /// <summary>
        /// Order initiator is aggressor
        /// </summary>
        Y = 1
    }

    public enum OrderCategory
    {
        Order = 1,
        Quote = 2
    }

    public enum PosType
    {
        /// <summary>
        /// Allocation Trade Qty
        /// </summary>
        ALC,
        /// <summary>
        /// Option Assignment
        /// </summary>
        AS,
        /// <summary>
        /// Corporate Action Adjustment
        /// </summary>
        CAA,
        /// <summary>
        /// Delivery Qty
        /// </summary>
        DLV,
        /// <summary>
        /// Delivery Notice Qty
        /// </summary>
        DN,
        /// <summary>
        /// Option Exercise Qty
        /// </summary>
        EX,
        /// <summary>
        /// Adjustment Qty
        /// </summary>
        PA,
        /// <summary>
        /// Receive Quantity
        /// </summary>
        RCV,
        /// <summary>
        /// Total Transaction Qty
        /// </summary>
        TOT,
        /// <summary>
        /// Transfer Trade Qty
        /// </summary>
        TRF,
        /// <summary>
        /// Transaction from Exercise
        /// </summary>
        TX
    }

    public enum OrdType
    {
        Market = 1,
        Limit = 2
    }

    public enum OrdStatus
    {
        PartiallyFilled = 1,
        Filled = 2
    }

    public enum RelatedInstrumentType
    {
        RelatedInstrumentType3 = 3
    }

    public class TradeConfirmationDM
    {
        public string RptID { get; private set; }
        public TradeReportTransType TransTyp { get; private set; }
        public TradeReportType RptTyp { get; private set; }
        public TrdType TrdTyp { get; private set; }
        public TradeHandlingInstr TrdHandlInst { get; private set; }
        public OrigTradeHandlingInstr? OrigTrdHandlInst { get; private set; }
        public string TrnsfrRsn { get; private set; }
        public string TotNumTrdRpts { get; private set; }
        public string RptRefID { get; private set; }
        public string MtchID { get; private set; }
        public decimal LastQty { private set; get; }
        public decimal LastPx { private set; get; }
        public string Ccy { private set; get; }
        public string LastMkt { private set; get; }
        public string TrdDt { private set; get; }
        public string BizDt { private set; get; }
        public MultiLegReportingType MLegRptTyp { private set; get; }
        public string SettlDt { private set; get; }
        public string LastUpdateTm { private set; get; }
        public StandardHeader Hdr { private set; get; }
        public IList<RootParty> Pty { private set; get; }
        public Instrument Instrmt { private set; get; }
        public PositionAmountData Amt { private set; get; }
        public TrdRegTimestamps TrdRegTS { private set; get; }
        public TrdCapRptSideGrp RptSide { private set; get; }

        public static TradeConfirmationDM ParseXML(string fixmlStr)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(fixmlStr);
            XmlNode rootNode = xdoc.DocumentElement;

            //
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("ns", "www.eurexchange.com/technology");

            //
            TradeConfirmationDM domainModel = new TradeConfirmationDM();

            var trdCaptRptNode = rootNode.SelectSingleNode("/ns:FIXML/ns:TrdCaptRpt", nsmgr);
            if (trdCaptRptNode == null)
                throw new NotSupportedException("This domain class only support TradeComfirmation FIXML message!");

            domainModel.RptID = trdCaptRptNode.Attributes["RptID"].Value;
            domainModel.TransTyp = EnumUtil<TradeReportTransType>.Parse(int.Parse(trdCaptRptNode.Attributes["TransTyp"].Value));
            domainModel.RptTyp = EnumUtil<TradeReportType>.Parse(int.Parse(trdCaptRptNode.Attributes["RptTyp"].Value));
            domainModel.TrdTyp = EnumUtil<TrdType>.Parse(int.Parse(trdCaptRptNode.Attributes["TrdTyp"].Value));
            domainModel.TrdHandlInst = EnumUtil<TradeHandlingInstr>.Parse(int.Parse(trdCaptRptNode.Attributes["TrdHandlInst"].Value));
            if (trdCaptRptNode.Attributes["OrigTrdHandlInst"] != null)
                domainModel.OrigTrdHandlInst = EnumUtil<OrigTradeHandlingInstr>.Parse(int.Parse(trdCaptRptNode.Attributes["OrigTrdHandlInst"].Value));
            domainModel.TrnsfrRsn = trdCaptRptNode.Attributes["TrnsfrRsn"].Value;
            if (trdCaptRptNode.Attributes["TotNumTrdRpts"] != null)
                domainModel.TotNumTrdRpts = trdCaptRptNode.Attributes["TotNumTrdRpts"].Value;
            if (trdCaptRptNode.Attributes["RptRefID"] != null)
                domainModel.RptRefID = trdCaptRptNode.Attributes["RptRefID"].Value;
            domainModel.MtchID = trdCaptRptNode.Attributes["MtchID"].Value;
            domainModel.LastQty = decimal.Parse(trdCaptRptNode.Attributes["LastQty"].Value);
            domainModel.LastPx = decimal.Parse(trdCaptRptNode.Attributes["LastPx"].Value);
            domainModel.Ccy = trdCaptRptNode.Attributes["Ccy"].Value;
            domainModel.LastMkt = trdCaptRptNode.Attributes["LastMkt"].Value;
            domainModel.TrdDt = trdCaptRptNode.Attributes["TrdDt"].Value;
            domainModel.BizDt = trdCaptRptNode.Attributes["BizDt"].Value;
            domainModel.MLegRptTyp = EnumUtil<MultiLegReportingType>.Parse(int.Parse(trdCaptRptNode.Attributes["MLegRptTyp"].Value));
            if (trdCaptRptNode.Attributes["SettlDt"] != null)
                domainModel.SettlDt = trdCaptRptNode.Attributes["SettlDt"].Value;
            domainModel.LastUpdateTm = trdCaptRptNode.Attributes["LastUpdateTm"].Value;

            domainModel.Hdr = new StandardHeader(trdCaptRptNode, nsmgr);
            domainModel.Pty = GetPtyList(trdCaptRptNode, nsmgr);
            domainModel.Instrmt = new Instrument(trdCaptRptNode, nsmgr);
            //
            var amtNode = trdCaptRptNode.SelectSingleNode("./ns:Amt", nsmgr);
            if (amtNode != null)
                domainModel.Amt = new PositionAmountData(amtNode);

            domainModel.TrdRegTS = new TrdRegTimestamps(trdCaptRptNode, nsmgr);
            domainModel.RptSide = new TrdCapRptSideGrp(trdCaptRptNode, nsmgr);

            return domainModel;
        }

        private static IList<RootParty> GetPtyList(XmlNode parentNode, XmlNamespaceManager nsmgr)
        {
            var ptyList = new List<RootParty>();
            var ptyNodes = parentNode.SelectNodes("./ns:Pty", nsmgr);
            if (ptyNodes == null)
                return null;
            foreach (XmlNode ptyNode in ptyNodes)
            {
                ptyList.Add(new RootParty(ptyNode));
            }

            return ptyList;
        }
    }

    //Hdr node
    public class StandardHeader
    {
        public string SID { private set; get; }
        public string TID { private set; get; }
        public string SSub { private set; get; }
        public string Snt { private set; get; }

        public StandardHeader(XmlNode parentNode, XmlNamespaceManager nsmgr)
        {
            var hdrNode = parentNode.SelectSingleNode("./ns:Hdr", nsmgr);
            if (hdrNode == null)
                throw new ArgumentNullException("/FIXML/TrdCaptRpt/Hdr can't be null");
            SID = hdrNode.Attributes["SID"].Value;
            TID = hdrNode.Attributes["TID"].Value;
            Snt = hdrNode.Attributes["Snt"].Value;
            if (hdrNode.Attributes["SSub"] != null)
                SSub = hdrNode.Attributes["SSub"].Value;
        }
    }

    public class RootParty
    {
        public string ID { private set; get; }
        public RootPartyRole R { private set; get; }

        public RootParty(XmlNode currentPtyNode)
        {
            ID = currentPtyNode.Attributes["ID"].Value;
            R = EnumUtil<RootPartyRole>.Parse(int.Parse(currentPtyNode.Attributes["R"].Value));
        }
    }

    public class Instrument
    {
        public string Sym { private set; get; }
        public string MMY { private set; get; }
        public decimal? StrkPx { private set; get; }
        public string OptAt { private set; get; }
        public PutOrCall? PutCall { private set; get; }
        public decimal? CpnRt { private set; get; }
        public string ID { private set; get; }
        public SecurityIDSource? Src { private set; get; }
        public EvntGrp Evnt { private set; get; }

        public Instrument(XmlNode parentNode, XmlNamespaceManager nsmgr)
        {
            var instrmtNode = parentNode.SelectSingleNode("./ns:Instrmt", nsmgr);
            if (instrmtNode == null)
                throw new ArgumentNullException("/FIXML/TrdCaptRpt/Instrmt can't be null");
            Sym = instrmtNode.Attributes["Sym"].Value;
            MMY = instrmtNode.Attributes["MMY"].Value;
            StrkPx = instrmtNode.Attributes["StrkPx"] == null ? default(decimal?) : decimal.Parse(instrmtNode.Attributes["StrkPx"].Value);
            OptAt = instrmtNode.Attributes["OptAt"] == null ? null : instrmtNode.Attributes["OptAt"].Value;
            if (instrmtNode.Attributes["PutCall"] != null)
                PutCall = EnumUtil<PutOrCall>.Parse(int.Parse(instrmtNode.Attributes["PutCall"].Value));
            CpnRt = instrmtNode.Attributes["CpnRt"] == null ? default(decimal?) : decimal.Parse(instrmtNode.Attributes["CpnRt"].Value);
            ID = instrmtNode.Attributes["ID"] == null ? null : instrmtNode.Attributes["ID"].Value;

            if (instrmtNode.Attributes["Src"] != null)
                Src = EnumUtil<SecurityIDSource>.Parse(int.Parse(instrmtNode.Attributes["Src"].Value));

            //todo: Event
        }
    }

    public class EvntGrp
    {
        public EventType? EventTyp { private set; get; }
        public string Dt { private set; get; }
    }

    public class PositionAmountData
    {
        public decimal? Amt { private set; get; }
        public PosAmtType? Type { private set; get; }

        public PositionAmountData(XmlNode currentNode)
        {
            if (currentNode.Attributes["Amt"] != null)
                Amt = decimal.Parse(currentNode.Attributes["Amt"].Value);
            if (currentNode.Attributes["Type"] != null)
                Type = (PosAmtType)Enum.Parse(typeof(PosAmtType), currentNode.Attributes["Type"].Value);
        }
    }

    public class TrdRegTimestamps
    {
        public string TS { private set; get; }
        public TrdRegTimestampType Typ { private set; get; }

        public TrdRegTimestamps(XmlNode parentNode, XmlNamespaceManager nsmgr)
        {
            var trdRegTSNode = parentNode.SelectSingleNode("./ns:TrdRegTS", nsmgr);
            if (trdRegTSNode == null)
                throw new ArgumentNullException("/FIXML/TrdCaptRpt/TrdRegTS can't be null");

            TS = trdRegTSNode.Attributes["TS"].Value;
            Typ = EnumUtil<TrdRegTimestampType>.Parse(int.Parse(trdRegTSNode.Attributes["Typ"].Value));
        }
    }

    public class TrdCapRptSideGrp
    {
        public Side Side { private set; get; }
        public string TrdID { private set; get; }
        public PositionEffect PosEfct { private set; get; }
        public string Txt1 { private set; get; }
        public string Txt2 { private set; get; }
        public string Txt3 { private set; get; }
        public string GUTxt1 { private set; get; }
        public string GUTxt2 { private set; get; }
        public string GUTxt3 { private set; get; }
        public TradeAllocIndicator AllocInd { private set; get; }
        public AggressorIndicator? AgrsrInd { private set; get; }
        public OrderCategory? OrdCat { private set; get; }
        public string StrategyLinkID { private set; get; }
        public SideTrdRegTS TrdRegTS { private set; get; }
        public IList<TradePositionQty> Qty { private set; get; }
        public TradeReportOrderDetail TrdRptOrdDetl { private set; get; }

        public TrdCapRptSideGrp(XmlNode parentNode, XmlNamespaceManager nsmgr)
        {
            var rptSideNode = parentNode.SelectSingleNode("./ns:RptSide", nsmgr);
            if (rptSideNode == null)
                throw new ArgumentNullException("/FIXML/TrdCaptRpt/RptSide can't be null");
            Side = EnumUtil<Side>.Parse(int.Parse(rptSideNode.Attributes["Side"].Value));
            if (rptSideNode.Attributes["TrdID"] != null)
                TrdID = rptSideNode.Attributes["TrdID"].Value;
            PosEfct = rptSideNode.Attributes["PosEfct"].Value.Equals("C", StringComparison.InvariantCultureIgnoreCase)
                ? PositionEffect.C
                : PositionEffect.O;
            if (rptSideNode.Attributes["Txt1"] != null)
                Txt1 = rptSideNode.Attributes["Txt1"].Value;
            if (rptSideNode.Attributes["Txt2"] != null)
                Txt2 = rptSideNode.Attributes["Txt2"].Value;
            if (rptSideNode.Attributes["Txt3"] != null)
                Txt3 = rptSideNode.Attributes["Txt3"].Value;
            //
            if (rptSideNode.Attributes["GUTxt1"] != null)
                GUTxt1 = rptSideNode.Attributes["GUTxt1"].Value;
            if (rptSideNode.Attributes["GUTxt2"] != null)
                GUTxt2 = rptSideNode.Attributes["GUTxt2"].Value;
            if (rptSideNode.Attributes["GUTxt3"] != null)
                GUTxt3 = rptSideNode.Attributes["GUTxt3"].Value;

            AllocInd = EnumUtil<TradeAllocIndicator>.Parse(int.Parse(rptSideNode.Attributes["AllocInd"].Value));
            if (rptSideNode.Attributes["AgrsrInd"] != null)
                AgrsrInd = rptSideNode.Attributes["AgrsrInd"].Value.Equals("N", StringComparison.InvariantCultureIgnoreCase) ? AggressorIndicator.N : AggressorIndicator.Y;
            if (rptSideNode.Attributes["OrdCat"] != null)
                OrdCat = EnumUtil<OrderCategory>.Parse(int.Parse(rptSideNode.Attributes["OrdCat"].Value));

            if (rptSideNode.Attributes["StrategyLinkID"] != null)
                StrategyLinkID = rptSideNode.Attributes["StrategyLinkID"].Value;

            //
            TrdRegTS = new SideTrdRegTS(rptSideNode, nsmgr);
            Qty = GetQtyList(rptSideNode, nsmgr);
            TrdRptOrdDetl = new TradeReportOrderDetail(rptSideNode, nsmgr);
        }

        private static IList<TradePositionQty> GetQtyList(XmlNode parentNode, XmlNamespaceManager nsmgr)
        {
            var qtyList = new List<TradePositionQty>();
            var qtyNodes = parentNode.SelectNodes("./ns:Qty", nsmgr);
            if (qtyNodes == null)
                return null;
            foreach (XmlNode qtyNode in qtyNodes)
            {
                qtyList.Add(new TradePositionQty(qtyNode));
            }

            return qtyList;
        }
    }

    public class SideTrdRegTS
    {
        public string TS { private set; get; }
        public SideTrdRegTimestampType? Typ { private set; get; }

        public SideTrdRegTS(XmlNode parentNode, XmlNamespaceManager nsmgr)
        {
            var trdRegTSNode = parentNode.SelectSingleNode("./ns:TrdRegTS", nsmgr);
            if (trdRegTSNode == null)
                throw new ArgumentNullException("/FIXML/TrdCaptRpt/RptSide/TrdRegTS can't be null");

            TS = trdRegTSNode.Attributes["TS"].Value;
            Typ = EnumUtil<SideTrdRegTimestampType>.Parse(int.Parse(trdRegTSNode.Attributes["Typ"].Value));
        }
    }

    public class TradePositionQty
    {
        public PosType Typ { private set; get; }
        public decimal Long { private set; get; }
        public decimal Short { private set; get; }

        public TradePositionQty(XmlNode curerntNode)
        {
            Typ = (PosType)Enum.Parse(typeof(PosType), curerntNode.Attributes["Typ"].Value);
            Long = decimal.Parse(curerntNode.Attributes["Long"].Value);
            Short = decimal.Parse(curerntNode.Attributes["Short"].Value);
        }
    }

    public class TradeReportOrderDetail
    {
        public string OrdID { private set; get; }
        public string OrdID2 { private set; get; }
        public string ClOrdID { private set; get; }
        public OrdType? OrdTyp { private set; get; }
        public OrdStatus OrdStat { private set; get; }
        //OrderQty
        public OrderQtyData OrdQty { private set; get; }
        public RelatedInstrumentGrp ReltdInstrmt { private set; get; }

        public TradeReportOrderDetail(XmlNode parentNode, XmlNamespaceManager nsmgr)
        {
            var trdRptOrdDetlNode = parentNode.SelectSingleNode("./ns:TrdRptOrdDetl", nsmgr);
            if (trdRptOrdDetlNode == null)
                throw new ArgumentNullException("/FIXML/TrdCaptRpt/TrdRptOrdDetl can't be null");

            OrdID = trdRptOrdDetlNode.Attributes["OrdID"].Value;
            OrdID2 = trdRptOrdDetlNode.Attributes["OrdID2"].Value;

            if (trdRptOrdDetlNode.Attributes["ClOrdID"] != null)
                ClOrdID = trdRptOrdDetlNode.Attributes["ClOrdID"].Value;
            if (trdRptOrdDetlNode.Attributes["OrdTyp"] != null)
                OrdTyp = EnumUtil<OrdType>.Parse(int.Parse(trdRptOrdDetlNode.Attributes["OrdTyp"].Value));

            OrdStat = EnumUtil<OrdStatus>.Parse(int.Parse(trdRptOrdDetlNode.Attributes["OrdStat"].Value));
            OrdQty = new OrderQtyData(trdRptOrdDetlNode, nsmgr);

            var ordQtyNode = parentNode.SelectSingleNode("./ns:ReltdInstrmt", nsmgr);
            if (ordQtyNode != null)
                ReltdInstrmt = new RelatedInstrumentGrp(ordQtyNode);
        }
    }

    public class OrderQtyData
    {
        public decimal Qty { private set; get; }

        public OrderQtyData(XmlNode parentNode, XmlNamespaceManager nsmgr)
        {
            var ordQtyNode = parentNode.SelectSingleNode("./ns:OrdQty", nsmgr);
            if (ordQtyNode == null)
                throw new ArgumentNullException("/FIXML/TrdCaptRpt/TrdRptOrdDetl/OrdQty can't be null");

            Qty = decimal.Parse(ordQtyNode.Attributes["Qty"].Value);
        }
    }

    public class RelatedInstrumentGrp
    {
        public RelatedInstrumentType? InstrmtTyp { private set; get; }
        public string Sym { private set; get; }
        public string ProdCmplx { private set; get; }

        public RelatedInstrumentGrp(XmlNode currentNode)
        {
            if (currentNode.Attributes["InstrmtTyp"] != null)
                InstrmtTyp = EnumUtil<RelatedInstrumentType>.Parse(int.Parse(currentNode.Attributes["InstrmtTyp"].Value));
            if (currentNode.Attributes["Sym"] != null)
                Sym = currentNode.Attributes["Sym"].Value;
            if (currentNode.Attributes["ProdCmplx"] != null)
                ProdCmplx = currentNode.Attributes["ProdCmplx"].Value;
        }
    }

    public static class EnumUtil<T> where T : struct
    {
        public static T Parse(int enumValue)
        {
            var isDefined = IsDefined(enumValue);
            if (!isDefined)
                throw new ArgumentException(string.Format("{0} is not a defined value for enum type {1}", enumValue, typeof(T).FullName));

            return (T)Enum.ToObject(typeof(T), enumValue);
        }

        private static bool IsDefined(object enumValue)
        {
            return Enum.IsDefined(typeof(T), enumValue);
        }

    }
}
