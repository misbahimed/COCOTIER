using System;
using System.Collections.Generic;
using System.IO;
using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Objects.Spot;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;

namespace BINANCE.BUSINESS
{
    class Program
    {
        static void Main(string[] args)
        {

            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials("Z5tdtZR6QR51iksQSdk1fQyEGZiMlCcKx9Q3Xg8ssoxQdWpENXB4BIciZorci1F5", "EUhWncReMHKHt9ogBTlkJ8ahuPRRWf0tK8zFsody967wJ2Ndik3CtcN3DtWE16Lz"),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });

            BinanceSocketClient.SetDefaultOptions(new BinanceSocketClientOptions()
            {
                ApiCredentials = new ApiCredentials("Z5tdtZR6QR51iksQSdk1fQyEGZiMlCcKx9Q3Xg8ssoxQdWpENXB4BIciZorci1F5", "EUhWncReMHKHt9ogBTlkJ8ahuPRRWf0tK8zFsody967wJ2Ndik3CtcN3DtWE16Lz"),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });

            var client = new Binance.Net.BinanceClient();
            var klines = client.Spot.Market.GetKlines(
                symbol: "ETHUSDT",
                interval: Binance.Net.Enums.KlineInterval.EightHour,
                startTime: DateTime.Now.AddYears(-1),
                endTime: DateTime.Now,
                limit: 1000);

            var klineseth = client.Spot.Market.GetKlines(
                symbol: "ETHUSDT",
                interval: Binance.Net.Enums.KlineInterval.EightHour,
                startTime: DateTime.Now.AddDays(-2),
                endTime: DateTime.Now,
                limit: 200);

            // translate Binance prices to ScottPlot OHLC objects
            List<ScottPlot.OHLC> ohlcs = new List<ScottPlot.OHLC>();
            foreach (var price in klines.Data)
            {
                var ohlc = new ScottPlot.OHLC(
                    open: (double)price.Open,
                    high: (double)price.High,
                    low: (double)price.Low,
                    close: (double)price.Close,
                    timeStart: 1,
                    timeSpan: 1);
                ohlcs.Add(ohlc);
            }

            // plot an array of OHLC objects
            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotCandlestick(ohlcs.ToArray());
            //plt.Ticks(dateTimeX: true);
            plt.YLabel("Price (USD)");
            plt.Title("ScottPlot Display of Bitcoin Prices");
            //plt.Layout(y2ScaleWidth: 20);
            plt.SaveFig("binance.png");

            //using (var client = new BinanceClient())
            //{
            //    // Spot.Market | Spot market info endpoints
            //    client.Spot.Market.GetBookPrice("BTCUSDT");
            //    var getBookPriceBTCUSDT = client.Spot.Market.GetBookPrice("BTCUSDT");
            //    // Spot.Order | Spot order info endpoints
            //    client.Spot.Order.GetAllOrders("BTCUSDT");
            //    var getAllOrdersBTCUSDT = client.Spot.Order.GetAllOrders("BTCUSDT");
            //    // Spot.System | Spot system endpoints
            //    client.Spot.System.GetExchangeInfo();
            //    var info = client.Spot.System.GetExchangeInfo();
            //    // Spot.UserStream | Spot user stream endpoints. Should be used to subscribe to a user stream with the socket client
            //    client.Spot.UserStream.StartUserStream();
            //    // Spot.Futures | Transfer to/from spot from/to the futures account + cross-collateral endpoints
            //    client.Spot.Futures.TransferFuturesAccount("ASSET", 1, FuturesTransferType.FromSpotToUsdtFutures);

            //    // FuturesCoin | Coin-M general endpoints
            //    client.FuturesCoin.GetPositionInformation();
            //    var getPositionInformation = client.FuturesCoin.GetPositionInformation();
            //    // FuturesCoin.Market | Coin-M futures market endpoints
            //    client.FuturesCoin.Market.GetBookPrices("BTCUSD");
            //    var getBookPrices = client.FuturesCoin.Market.GetBookPrices("BTCUSD");
            //    // FuturesCoin.Order | Coin-M futures order endpoints
            //    client.FuturesCoin.Order.GetMyTrades();
            //    var getMyTrades = client.FuturesCoin.Order.GetMyTrades();
            //    // FuturesCoin.Account | Coin-M account info
            //    client.FuturesCoin.Account.GetAccountInfo();
            //    var GetAccountInfo = client.FuturesCoin.Account.GetAccountInfo();
            //    // FuturesCoin.System | Coin-M system endpoints
            //    client.FuturesCoin.System.GetExchangeInfo();
            //    var getExchangeInfo = client.FuturesCoin.System.GetExchangeInfo();
            //    // FuturesCoin.UserStream | Coin-M user stream endpoints. Should be used to subscribe to a user stream with the socket client
            //    client.FuturesCoin.UserStream.StartUserStream();

            //    // FuturesUsdt | USDT-M general endpoints
            //    client.FuturesUsdt.GetPositionInformation();
            //    var getPositionInformationFuturesUsdt = client.FuturesUsdt.GetPositionInformation();
            //    // FuturesUsdt.Market | USDT-M futures market endpoints
            //    client.FuturesUsdt.Market.GetBookPrices("BTCUSDT");
            //    var getBookPricesFuturesUsdt = client.FuturesUsdt.Market.GetBookPrices("BTCUSDT");
            //    // FuturesUsdt.Order | USDT-M futures order endpoints
            //    client.FuturesUsdt.Order.GetMyTrades("BTCUSDT");
            //    var getMyTradesFuturesUsdt = client.FuturesUsdt.Order.GetMyTrades("BTCUSDT");
            //    // FuturesUsdt.Account | USDT-M account info
            //    client.FuturesUsdt.Account.GetAccountInfo();
            //    var getAccountInfoFuturesUsdt = client.FuturesUsdt.Account.GetAccountInfo();
            //    // FuturesUsdt.System | USDT-M system endpoints
            //    client.FuturesUsdt.System.GetExchangeInfo();
            //    var getExchangeInfoFuturesUsdt = client.FuturesUsdt.System.GetExchangeInfo();
            //    // FuturesUsdt.UserStream | USDT-M user stream endpoints. Should be used to subscribe to a user stream with the socket client
            //    client.FuturesUsdt.UserStream.StartUserStream();

            //    // General | General/account endpoints
            //    client.General.GetAccountInfo();
            //    var generalGetAccountInfo = client.General.GetAccountInfo();
            //    // Lending | Lending endpoints
            //    client.Lending.GetFlexibleProductList();
            //    var getFlexibleProductList = client.Lending.GetFlexibleProductList();
            //    // Margin | Margin general/account info
            //    client.Margin.GetMarginAccountInfo();
            //    var getMarginAccountInfo = client.Margin.GetMarginAccountInfo();
            //    // Margin.Market | Margin market endpoints
            //    client.Margin.Market.GetMarginPairs();
            //    var getMarginPairs = client.Margin.Market.GetMarginPairs();
            //    // Margin.Order | Margin order endpoints
            //    client.Margin.Order.GetAllMarginAccountOrders("BTCUSDT");
            //    var getAllMarginAccountOrders = client.Margin.Order.GetAllMarginAccountOrders("BTCUSDT");
            //    // Margin.UserStream | Margin user stream endpoints. Should be used to subscribe to a user stream with the socket client
            //    client.Margin.UserStream.StartUserStream();
            //    // Margin.IsolatedUserStream | Isolated margin user stream endpoints. Should be used to subscribe to a user stream with the socket client
            //    client.Margin.IsolatedUserStream.StartIsolatedMarginUserStream("BTCUSDT");

            //    // Mining | Mining endpoints
            //    client.Mining.GetMiningCoinList();
            //    var getMiningCoinList = client.Mining.GetMiningCoinList();
            //    // SubAccount | Sub account management
            //    client.SubAccount.TransferSubAccount("fromEmail", "toEmail", "asset", 1);

            //    // Brokerage | Brokerage management
            //    client.Brokerage.CreateSubAccountAsync();

            //    // WithdrawDeposit | Withdraw and deposit endpoints
            //    client.WithdrawDeposit.GetWithdrawalHistory();
            //    var getWithdrawalHistory = client.WithdrawDeposit.GetWithdrawalHistory();
            //}

            var socketClient = new BinanceSocketClient();
            // Spot | Spot market and user subscription methods
            socketClient.Spot.SubscribeToAllBookTickerUpdates(data =>
            {
                // Handle data
                var data_ = data;
            });

            // FuturesCoin | Coin-M futures market and user subscription methods
            socketClient.FuturesCoin.SubscribeToAllBookTickerUpdates(data =>
            {
                // Handle data
                var data_ = data;
            });

            // FuturesUsdt | USDT-M futures market and user subscription methods
            socketClient.FuturesUsdt.SubscribeToAllBookTickerUpdates(data =>
            {
                // Handle data
                var data_ = data;
            });

            // Unsubscribe
            socketClient.UnsubscribeAll();

            Console.ReadLine();




            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
