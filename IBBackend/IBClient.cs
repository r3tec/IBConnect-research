/* Copyright (C) 2019 Interactive Brokers LLC. All rights reserved. This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */
using IBApi;
using IBBackend.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace IBBackend
{
    public class IBClient : EWrapper
    {
        private EClientSocket clientSocket;
        private int nextOrderId;
        private int clientId;

        public int ClientId
        {
            get { return clientId; }
            set { clientId = value; }
        }

        SynchronizationContext sc;

        public IBClient(EReaderSignal signal)
        {
            clientSocket = new EClientSocket(this, signal);
            sc = new SynchronizationContext();
        }

        public EClientSocket ClientSocket
        {
            get { return clientSocket; }
            private set { clientSocket = value; }
        }

        public int NextOrderId
        {
            get { return nextOrderId; }
            set { nextOrderId = value; }
        }

        public event Action<int, int, string, Exception> Error;

        void EWrapper.error(Exception e)
        {

            var tmpEvent = Error;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(0, 0, null, e), null);
        }

        void EWrapper.error(string str)
        {
            var tmpEvent = Error;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(0, 0, str, null), null);
        }

        void EWrapper.error(int id, int errorCode, string errorMsg)
        {
            var tmpEvent = Error;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(id, errorCode, errorMsg, null), null);
        }

        public event Action ConnectionClosed;

        void EWrapper.connectionClosed()
        {
            var tmpEvent = ConnectionClosed;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<long> CurrentTime;

        void EWrapper.currentTime(long time)
        {
            var tmpEvent = CurrentTime;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(time), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<TickPriceMessage> TickPrice;

        void EWrapper.tickPrice(int tickerId, int field, double price, TickAttrib attribs)
        {
            var tmpEvent = TickPrice;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new TickPriceMessage(tickerId, field, price, attribs)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);

        }

        public event Action<TickSizeMessage> TickSize;

        void EWrapper.tickSize(int tickerId, int field, int size)
        {
            var tmpEvent = TickSize;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new TickSizeMessage(tickerId, field, size)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);

        }

        public event Action<int, int, string> TickString;

        void EWrapper.tickString(int tickerId, int tickType, string value)
        {
            var tmpEvent = TickString;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(tickerId, tickType, value), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int, int, double> TickGeneric;

        void EWrapper.tickGeneric(int tickerId, int field, double value)
        {
            var tmpEvent = TickGeneric;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(tickerId, field, value), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int, int, double, string, double, int, string, double, double> TickEFP;

        void EWrapper.tickEFP(int tickerId, int tickType, double basisPoints, string formattedBasisPoints, double impliedFuture, int holdDays, string futureLastTradeDate, double dividendImpact, double dividendsToLastTradeDate)
        {
            var tmpEvent = TickEFP;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(tickerId, tickType, basisPoints, formattedBasisPoints, impliedFuture, holdDays, futureLastTradeDate, dividendImpact, dividendsToLastTradeDate), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int> TickSnapshotEnd;

        void EWrapper.tickSnapshotEnd(int tickerId)
        {
            var tmpEvent = TickSnapshotEnd;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(tickerId), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<ConnectionStatusMessage> NextValidId;

        void EWrapper.nextValidId(int orderId)
        {
            var tmpEvent = NextValidId;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new ConnectionStatusMessage(true)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);

            NextOrderId = orderId;
        }

        public event Action<int, DeltaNeutralContract> DeltaNeutralValidation;

        void EWrapper.deltaNeutralValidation(int reqId, DeltaNeutralContract deltaNeutralContract)
        {
            var tmpEvent = DeltaNeutralValidation;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(reqId, deltaNeutralContract), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<ManagedAccountsMessage> ManagedAccounts;

        void EWrapper.managedAccounts(string accountsList)
        {
            var tmpEvent = ManagedAccounts;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new ManagedAccountsMessage(accountsList)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<TickOptionMessage> TickOptionCommunication;

        void EWrapper.tickOptionComputation(int tickerId, int field, double impliedVolatility, double delta, double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice)
        {
            var tmpEvent = TickOptionCommunication;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new TickOptionMessage(tickerId, field, impliedVolatility, delta, optPrice, pvDividend, gamma, vega, theta, undPrice)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<AccountSummaryMessage> AccountSummary;

        void EWrapper.accountSummary(int reqId, string account, string tag, string value, string currency)
        {
            var tmpEvent = AccountSummary;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new AccountSummaryMessage(reqId, account, tag, value, currency)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<AccountSummaryEndMessage> AccountSummaryEnd;

        void EWrapper.accountSummaryEnd(int reqId)
        {
            var tmpEvent = AccountSummaryEnd;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new AccountSummaryEndMessage(reqId)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<AccountValueMessage> UpdateAccountValue;

        void EWrapper.updateAccountValue(string key, string value, string currency, string accountName)
        {
            var tmpEvent = UpdateAccountValue;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new AccountValueMessage(key, value, currency, accountName)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<UpdatePortfolioMessage> UpdatePortfolio;

        void EWrapper.updatePortfolio(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealizedPNL, double realizedPNL, string accountName)
        {
            var tmpEvent = UpdatePortfolio;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new UpdatePortfolioMessage(contract, position, marketPrice, marketValue, averageCost, unrealizedPNL, realizedPNL, accountName)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<UpdateAccountTimeMessage> UpdateAccountTime;

        void EWrapper.updateAccountTime(string timestamp)
        {
            var tmpEvent = UpdateAccountTime;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new UpdateAccountTimeMessage(timestamp)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<AccountDownloadEndMessage> AccountDownloadEnd;

        void EWrapper.accountDownloadEnd(string account)
        {
            var tmpEvent = AccountDownloadEnd;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new AccountDownloadEndMessage(account)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<OrderStatusMessage> OrderStatus;

        void EWrapper.orderStatus(int orderId, string status, double filled, double remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
        {
            var tmpEvent = OrderStatus;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new OrderStatusMessage(orderId, status, filled, remaining, avgFillPrice, permId, parentId, lastFillPrice, clientId, whyHeld, mktCapPrice)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<OpenOrderMessage> OpenOrder;

        void EWrapper.openOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
            var tmpEvent = OpenOrder;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new OpenOrderMessage(orderId, contract, order, orderState)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action OpenOrderEnd;

        void EWrapper.openOrderEnd()
        {
            var tmpEvent = OpenOrderEnd;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<ContractDetailsMessage> ContractDetails;

        void EWrapper.contractDetails(int reqId, ContractDetails contractDetails)
        {
            var tmpEvent = ContractDetails;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new ContractDetailsMessage(reqId, contractDetails)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int> ContractDetailsEnd;

        void EWrapper.contractDetailsEnd(int reqId)
        {
            var tmpEvent = ContractDetailsEnd;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(reqId), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<ExecutionMessage> ExecDetails;

        void EWrapper.execDetails(int reqId, Contract contract, Execution execution)
        {
            var tmpEvent = ExecDetails;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new ExecutionMessage(reqId, contract, execution)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int> ExecDetailsEnd;

        void EWrapper.execDetailsEnd(int reqId)
        {
            var tmpEvent = ExecDetailsEnd;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(reqId), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<CommissionReport> CommissionReport;

        void EWrapper.commissionReport(CommissionReport commissionReport)
        {
            var tmpEvent = CommissionReport;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(commissionReport), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<FundamentalsMessage> FundamentalData;

        void EWrapper.fundamentalData(int reqId, string data)
        {
            var tmpEvent = FundamentalData;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new FundamentalsMessage(data)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<HistoricalDataMessage> HistoricalData;

        void EWrapper.historicalData(int reqId, Bar bar)
        {
            var tmpEvent = HistoricalData;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new HistoricalDataMessage(reqId, bar)), null);
        }

        public event Action<HistoricalDataEndMessage> HistoricalDataEnd;

        void EWrapper.historicalDataEnd(int reqId, string startDate, string endDate)
        {
            var tmpEvent = HistoricalDataEnd;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new HistoricalDataEndMessage(reqId, startDate, endDate)), null);
        }

        public event Action<MarketDataTypeMessage> MarketDataType;

        void EWrapper.marketDataType(int reqId, int marketDataType)
        {
            var tmpEvent = MarketDataType;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new MarketDataTypeMessage(reqId, marketDataType)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<DeepBookMessage> UpdateMktDepth;

        void EWrapper.updateMktDepth(int tickerId, int position, int operation, int side, double price, int size)
        {
            var tmpEvent = UpdateMktDepth;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new DeepBookMessage(tickerId, position, operation, side, price, size, "", false)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<DeepBookMessage> UpdateMktDepthL2;

        void EWrapper.updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, int side, double price, int size, bool isSmartDepth)
        {
            var tmpEvent = UpdateMktDepthL2;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new DeepBookMessage(tickerId, position, operation, side, price, size, marketMaker, isSmartDepth)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int, int, String, String> UpdateNewsBulletin;

        void EWrapper.updateNewsBulletin(int msgId, int msgType, String message, String origExchange)
        {
            var tmpEvent = UpdateNewsBulletin;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(msgId, msgType, message, origExchange), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<PositionMessage> Position;

        void EWrapper.position(string account, Contract contract, double pos, double avgCost)
        {
            var tmpEvent = Position;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new PositionMessage(account, contract, pos, avgCost)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action PositionEnd;

        void EWrapper.positionEnd()
        {
            var tmpEvent = PositionEnd;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<RealTimeBarMessage> RealtimeBar;

        void EWrapper.realtimeBar(int reqId, long time, double open, double high, double low, double close, long volume, double WAP, int count)
        {
            var tmpEvent = RealtimeBar;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new RealTimeBarMessage(reqId, time, open, high, low, close, volume, WAP, count)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<string> ScannerParameters;

        void EWrapper.scannerParameters(string xml)
        {
            var tmpEvent = ScannerParameters;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(xml), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<ScannerMessage> ScannerData;

        void EWrapper.scannerData(int reqId, int rank, ContractDetails contractDetails, string distance, string benchmark, string projection, string legsStr)
        {
            var tmpEvent = ScannerData;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new ScannerMessage(reqId, rank, contractDetails, distance, benchmark, projection, legsStr)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int> ScannerDataEnd;

        void EWrapper.scannerDataEnd(int reqId)
        {
            var tmpEvent = ScannerDataEnd;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(reqId), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<AdvisorDataMessage> ReceiveFA;

        void EWrapper.receiveFA(int faDataType, string faXmlData)
        {
            var tmpEvent = ReceiveFA;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new AdvisorDataMessage(faDataType, faXmlData)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<BondContractDetailsMessage> BondContractDetails;

        void EWrapper.bondContractDetails(int requestId, ContractDetails contractDetails)
        {
            var tmpEvent = BondContractDetails;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new BondContractDetailsMessage(requestId, contractDetails)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<string> VerifyMessageAPI;

        void EWrapper.verifyMessageAPI(string apiData)
        {
            var tmpEvent = VerifyMessageAPI;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(apiData), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }
        public event Action<bool, string> VerifyCompleted;

        void EWrapper.verifyCompleted(bool isSuccessful, string errorText)
        {
            var tmpEvent = VerifyCompleted;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(isSuccessful, errorText), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);

        }

        public event Action<string, string> VerifyAndAuthMessageAPI;

        void EWrapper.verifyAndAuthMessageAPI(string apiData, string xyzChallenge)
        {
            var tmpEvent = VerifyAndAuthMessageAPI;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(apiData, xyzChallenge), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<bool, string> VerifyAndAuthCompleted;

        void EWrapper.verifyAndAuthCompleted(bool isSuccessful, string errorText)
        {
            var tmpEvent = VerifyAndAuthCompleted;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(isSuccessful, errorText), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int, string> DisplayGroupList;

        void EWrapper.displayGroupList(int reqId, string groups)
        {
            var tmpEvent = DisplayGroupList;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(reqId, groups), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int, string> DisplayGroupUpdated;

        void EWrapper.displayGroupUpdated(int reqId, string contractInfo)
        {
            var tmpEvent = DisplayGroupUpdated;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(reqId, contractInfo), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }


        void EWrapper.connectAck()
        {
            //if (ClientSocket.asyncEConnect)
                ClientSocket.startApi();
        }

        public event Action<PositionMultiMessage> PositionMulti;

        void EWrapper.positionMulti(int reqId, string account, string modelCode, Contract contract, double pos, double avgCost)
        {
            var tmpEvent = PositionMulti;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new PositionMultiMessage(reqId, account, modelCode, contract, pos, avgCost)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int> PositionMultiEnd;

        void EWrapper.positionMultiEnd(int reqId)
        {
            var tmpEvent = PositionMultiEnd;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(reqId), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<AccountUpdateMultiMessage> AccountUpdateMulti;

        void EWrapper.accountUpdateMulti(int reqId, string account, string modelCode, string key, string value, string currency)
        {
            var tmpEvent = AccountUpdateMulti;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new AccountUpdateMultiMessage(reqId, account, modelCode, key, value, currency)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int> AccountUpdateMultiEnd;

        void EWrapper.accountUpdateMultiEnd(int reqId)
        {
            var tmpEvent = AccountUpdateMultiEnd;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(reqId), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<SecurityDefinitionOptionParameterMessage> SecurityDefinitionOptionParameter;

        void EWrapper.securityDefinitionOptionParameter(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes)
        {
            var tmpEvent = SecurityDefinitionOptionParameter;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new SecurityDefinitionOptionParameterMessage(reqId, exchange, underlyingConId, tradingClass, multiplier, expirations, strikes)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int> SecurityDefinitionOptionParameterEnd;

        void EWrapper.securityDefinitionOptionParameterEnd(int reqId)
        {
            var tmpEvent = SecurityDefinitionOptionParameterEnd;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(reqId), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<SoftDollarTiersMessage> SoftDollarTiers;

        void EWrapper.softDollarTiers(int reqId, SoftDollarTier[] tiers)
        {
            var tmpEvent = SoftDollarTiers;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new SoftDollarTiersMessage(reqId, tiers)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<FamilyCode[]> FamilyCodes;

        void EWrapper.familyCodes(FamilyCode[] familyCodes)
        {
            var tmpEvent = FamilyCodes;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(familyCodes), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<SymbolSamplesMessage> SymbolSamples;

        void EWrapper.symbolSamples(int reqId, ContractDescription[] contractDescriptions)
        {
            var tmpEvent = SymbolSamples;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new SymbolSamplesMessage(reqId, contractDescriptions)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }


        public event Action<DepthMktDataDescription[]> MktDepthExchanges;

        void EWrapper.mktDepthExchanges(DepthMktDataDescription[] depthMktDataDescriptions)
        {
            var tmpEvent = MktDepthExchanges;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(depthMktDataDescriptions), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<TickNewsMessage> TickNews;

        void EWrapper.tickNews(int tickerId, long timeStamp, string providerCode, string articleId, string headline, string extraData)
        {
            var tmpEvent = TickNews;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new TickNewsMessage(tickerId, timeStamp, providerCode, articleId, headline, extraData)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int, Dictionary<int, KeyValuePair<string, char>>> SmartComponents;

        void EWrapper.smartComponents(int reqId, Dictionary<int, KeyValuePair<string, char>> theMap)
        {
            var tmpEvent = SmartComponents;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(reqId, theMap), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<TickReqParamsMessage> TickReqParams;

        void EWrapper.tickReqParams(int tickerId, double minTick, string bboExchange, int snapshotPermissions)
        {
            var tmpEvent = TickReqParams;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new TickReqParamsMessage(tickerId, minTick, bboExchange, snapshotPermissions)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<NewsProvider[]> NewsProviders;

        void EWrapper.newsProviders(NewsProvider[] newsProviders)
        {
            var tmpEvent = NewsProviders;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(newsProviders), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<NewsArticleMessage> NewsArticle;

        void EWrapper.newsArticle(int requestId, int articleType, string articleText)
        {
            var tmpEvent = NewsArticle;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new NewsArticleMessage(requestId, articleType, articleText)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<HistoricalNewsMessage> HistoricalNews;

        void EWrapper.historicalNews(int requestId, string time, string providerCode, string articleId, string headline)
        {
            var tmpEvent = HistoricalNews;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new HistoricalNewsMessage(requestId, time, providerCode, articleId, headline)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<HistoricalNewsEndMessage> HistoricalNewsEnd;

        void EWrapper.historicalNewsEnd(int requestId, bool hasMore)
        {
            var tmpEvent = HistoricalNewsEnd;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new HistoricalNewsEndMessage(requestId, hasMore)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<HeadTimestampMessage> HeadTimestamp;

        void EWrapper.headTimestamp(int reqId, string headTimestamp)
        {
            var tmpEvent = HeadTimestamp;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new HeadTimestampMessage(reqId, headTimestamp)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<HistogramDataMessage> HistogramData;

        void EWrapper.histogramData(int reqId, HistogramEntry[] data)
        {
            var tmpEvent = HistogramData;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new HistogramDataMessage(reqId, data)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<HistoricalDataMessage> HistoricalDataUpdate;

        void EWrapper.historicalDataUpdate(int reqId, Bar bar)
        {
            var tmpEvent = HistoricalDataUpdate;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new HistoricalDataMessage(reqId, bar)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int, int, string> RerouteMktDataReq;

        void EWrapper.rerouteMktDataReq(int reqId, int conId, string exchange)
        {
            var tmpEvent = RerouteMktDataReq;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(reqId, conId, exchange), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<int, int, string> RerouteMktDepthReq;

        void EWrapper.rerouteMktDepthReq(int reqId, int conId, string exchange)
        {
            var tmpEvent = RerouteMktDepthReq;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(reqId, conId, exchange), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<MarketRuleMessage> MarketRule;

        void EWrapper.marketRule(int marketRuleId, PriceIncrement[] priceIncrements)
        {
            var tmpEvent = MarketRule;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new MarketRuleMessage(marketRuleId, priceIncrements)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<PnLMessage> pnl;

        void EWrapper.pnl(int reqId, double dailyPnL, double unrealizedPnL, double realizedPnL)
        {
            var tmpEvent = pnl;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new PnLMessage(reqId, dailyPnL, unrealizedPnL, realizedPnL)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<PnLSingleMessage> pnlSingle;

        void EWrapper.pnlSingle(int reqId, int pos, double dailyPnL, double unrealizedPnL, double realizedPnL, double value)
        {
            var tmpEvent = pnlSingle;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new PnLSingleMessage(reqId, pos, dailyPnL, unrealizedPnL, realizedPnL, value)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<HistoricalTickMessage> historicalTick;

        void EWrapper.historicalTicks(int reqId, HistoricalTick[] ticks, bool done)
        {
            var tmpEvent = historicalTick;

            if (tmpEvent != null)
                ticks.ToList().ForEach(tick => sc.Post((t) => tmpEvent(new HistoricalTickMessage(reqId, tick.Time, tick.Price, tick.Size)), null));
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<HistoricalTickBidAskMessage> historicalTickBidAsk;

        void EWrapper.historicalTicksBidAsk(int reqId, HistoricalTickBidAsk[] ticks, bool done)
        {
            var tmpEvent = historicalTickBidAsk;

            if (tmpEvent != null)
                ticks.ToList().ForEach(tick => sc.Post((t) =>
                    tmpEvent(new HistoricalTickBidAskMessage(reqId, tick.Time, tick.TickAttribBidAsk, tick.PriceBid, tick.PriceAsk, tick.SizeBid, tick.SizeAsk)), null));
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<HistoricalTickLastMessage> historicalTickLast;

        void EWrapper.historicalTicksLast(int reqId, HistoricalTickLast[] ticks, bool done)
        {
            var tmpEvent = historicalTickLast;

            if (tmpEvent != null)
                ticks.ToList().ForEach(tick => sc.Post((t) =>
                    tmpEvent(new HistoricalTickLastMessage(reqId, tick.Time, tick.TickAttribLast, tick.Price, tick.Size, tick.Exchange, tick.SpecialConditions)), null));
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<TickByTickAllLastMessage> tickByTickAllLast;

        void EWrapper.tickByTickAllLast(int reqId, int tickType, long time, double price, int size, TickAttribLast tickAttribLast, string exchange, string specialConditions)
        {
            var tmpEvent = tickByTickAllLast;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new TickByTickAllLastMessage(reqId, tickType, time, price, size, tickAttribLast, exchange, specialConditions)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<TickByTickBidAskMessage> tickByTickBidAsk;

        void EWrapper.tickByTickBidAsk(int reqId, long time, double bidPrice, double askPrice, int bidSize, int askSize, TickAttribBidAsk tickAttribBidAsk)
        {
            var tmpEvent = tickByTickBidAsk;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new TickByTickBidAskMessage(reqId, time, bidPrice, askPrice, bidSize, askSize, tickAttribBidAsk)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<TickByTickMidPointMessage> tickByTickMidPoint;

        void EWrapper.tickByTickMidPoint(int reqId, long time, double midPoint)
        {
            var tmpEvent = tickByTickMidPoint;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new TickByTickMidPointMessage(reqId, time, midPoint)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<OrderBoundMessage> OrderBound;

        void EWrapper.orderBound(long orderId, int apiClientId, int apiOrderId)
        {
            var tmpEvent = OrderBound;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new OrderBoundMessage(orderId, apiClientId, apiOrderId)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action<CompletedOrderMessage> CompletedOrder;

        void EWrapper.completedOrder(Contract contract, Order order, OrderState orderState)
        {
            var tmpEvent = CompletedOrder;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(new CompletedOrderMessage(contract, order, orderState)), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }

        public event Action CompletedOrdersEnd;

        void EWrapper.completedOrdersEnd()
        {
            var tmpEvent = CompletedOrdersEnd;

            if (tmpEvent != null)
                sc.Post((t) => tmpEvent(), null);
            else if (Error != null)
                sc.Post((t) => Error(int.MinValue, 0, MethodBase.GetCurrentMethod().Name, null), null);
        }
        public Task<Contract> ResolveContractAsync(int conId, string refExch)
        {
            var reqId = new Random(DateTime.Now.Millisecond).Next();
            var resolveResult = new TaskCompletionSource<Contract>();
            var resolveContract_Error = new Action<int, int, string, Exception>((id, code, msg, ex) =>
            {
                if (reqId != id)
                    return;

                resolveResult.SetResult(null);
            });
            var resolveContract = new Action<ContractDetailsMessage>(msg =>
            {
                if (msg.RequestId == reqId)
                    resolveResult.SetResult(msg.ContractDetails.Contract);
            });
            var contractDetailsEnd = new Action<int>(id =>
            {
                if (reqId == id && !resolveResult.Task.IsCompleted)
                    resolveResult.SetResult(null);
            });

            var tmpError = Error;
            var tmpContractDetails = ContractDetails;
            var tmpContractDetailsEnd = ContractDetailsEnd;

            Error = resolveContract_Error;
            ContractDetails = resolveContract;
            ContractDetailsEnd = contractDetailsEnd;

            resolveResult.Task.ContinueWith(t =>
            {
                Error = tmpError;
                ContractDetails = tmpContractDetails;
                ContractDetailsEnd = tmpContractDetailsEnd;
            });

            ClientSocket.reqContractDetails(reqId, new Contract() { ConId = conId, Exchange = refExch });

            return resolveResult.Task;
        }

        public Task<Contract[]> ResolveContractAsync(string secType, string symbol, string currency, string exchange)
        {
            var reqId = new Random(DateTime.Now.Millisecond).Next();
            var res = new TaskCompletionSource<Contract[]>();
            var contractList = new List<Contract>();
            var resolveContract_Error = new Action<int, int, string, Exception>((id, code, msg, ex) =>
            {
                if (reqId != id)
                    return;

                res.SetResult(new Contract[0]);
            });
            var contractDetails = new Action<ContractDetailsMessage>(msg =>
            {
                if (reqId != msg.RequestId)
                    return;

                contractList.Add(msg.ContractDetails.Contract);
            });
            var contractDetailsEnd = new Action<int>(id =>
            {
                if (reqId == id)
                    res.SetResult(contractList.ToArray());
            });

            var tmpError = Error;
            var tmpContractDetails = ContractDetails;
            var tmpContractDetailsEnd = ContractDetailsEnd;

            Error = resolveContract_Error;
            ContractDetails = contractDetails;
            ContractDetailsEnd = contractDetailsEnd;

            res.Task.ContinueWith(t =>
            {
                Error = tmpError;
                ContractDetails = tmpContractDetails;
                ContractDetailsEnd = tmpContractDetailsEnd;
            });

            ClientSocket.reqContractDetails(reqId, new Contract() { SecType = secType, Symbol = symbol, Currency = currency, Exchange = exchange });

            return res.Task;
        }
    }
}
