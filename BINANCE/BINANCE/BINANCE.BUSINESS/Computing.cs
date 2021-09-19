using Binance.Net;
using Binance.Net.Objects.Spot;
using BINANCE.BUSINESS.Dto;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BINANCE.BUSINESS
{
    public class Computing : IComputing
    {
        public Computing()
        {
        }
        public List<ComputingDataDto> GetComputingByFiltre(List<string> cryptos, string interval, DateTime debut, DateTime fin, int limit, string computingN)
        {

            //BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            //{
            //    ApiCredentials = new ApiCredentials("Z5tdtZR6QR51iksQSdk1fQyEGZiMlCcKx9Q3Xg8ssoxQdWpENXB4BIciZorci1F5", "EUhWncReMHKHt9ogBTlkJ8ahuPRRWf0tK8zFsody967wJ2Ndik3CtcN3DtWE16Lz"),
            //    LogVerbosity = LogVerbosity.Debug,
            //    LogWriters = new List<TextWriter> { Console.Out }
            //});

            //BinanceSocketClient.SetDefaultOptions(new BinanceSocketClientOptions()
            //{
            //    ApiCredentials = new ApiCredentials("Z5tdtZR6QR51iksQSdk1fQyEGZiMlCcKx9Q3Xg8ssoxQdWpENXB4BIciZorci1F5", "EUhWncReMHKHt9ogBTlkJ8ahuPRRWf0tK8zFsody967wJ2Ndik3CtcN3DtWE16Lz"),
            //    LogVerbosity = LogVerbosity.Debug,
            //    LogWriters = new List<TextWriter> { Console.Out }
            //});

            //var client = new BinanceClient();

            //var klinsses = client.Spot.Market.GetKlines(
            //    symbol: "BTCUSDT",
            //    interval: Binance.Net.Enums.KlineInterval.EightHour,
            //    startTime: DateTime.Now.AddYears(-1),
            //    endTime: DateTime.Now,
            //    limit: 100).Data;


            //var klines = client.Spot.Market.GetKlines(
            //    symbol: "ETHUSDT",
            //    interval: Binance.Net.Enums.KlineInterval.EightHour,
            //    startTime: DateTime.Now.AddYears(-1),
            //    endTime: DateTime.Now,
            //    limit: 100).Data as IEnumerable<Binance.Net.Objects.Spot.MarketData.BinanceSpotKline>;

            //var computings = klines;

            List<ComputingDataDto> computingsData = new List<ComputingDataDto>();

            Binance.Net.Enums.KlineInterval inervalBack = MapInterval(interval);

            foreach (var crypto in cryptos)
            {
                List<ComputingDto> computings = new List<ComputingDto>();
                computings.AddRange(GetComputingByCrypto(crypto, inervalBack, debut, fin, limit).OrderBy(x => x.OpenTime));
                if (computings.Count() > 0)
                {
                    computings[0].Coeff = computings[0].Variation;
                    for (int i = 1; i < computings.Count(); i++)
                    {
                        computings[i].Coeff = computings[i].Variation * computings[i - 1].Coeff;

                        if(computingN == "n-1")
                        {
                            computings[i].ComputingN = computings[i].Close / computings[i - 1].Open;
                        }
                        if (computingN == "n-2")
                        {
                            if (i == 1)
                            {
                                computings[i].ComputingN = 0;
                            }
                            else
                            {
                                computings[i].ComputingN = computings[i].Close / computings[i - 2].Open;
                            }
                            
                        }
                    }
                }
                ComputingDataDto computingData = new ComputingDataDto();
                computingData.Crypto = crypto;
                computingData.Computings = computings;
                computingsData.Add(computingData);
            }

            return computingsData;

        }
        public List<ComputingMaxDto> GetVariationMax(List<string> cryptos, string interval, DateTime debut, DateTime fin, int limit, string computingN)
        {

            var computingByFiltre = GetComputingByFiltre(cryptos, interval, debut, fin, limit, computingN) ;
            var computingsMax = new List<ComputingMaxDto>();
            var firstComputing = computingByFiltre[0];

            decimal Coeff = 1;
            for (int i = 0; i < firstComputing.Computings.Count(); i++)
            {
                decimal maxVariation = 0;
                if (computingN == "normal")
                {
                    maxVariation = computingByFiltre[0].Computings[i].Variation;
                }
                else
                {
                    maxVariation = computingByFiltre[0].Computings[i].ComputingN;
                }

                decimal bootMax = 0;
                for (int j = 0; j < computingByFiltre.Count(); j++)
                {
                    if (computingN == "normal")
                    {
                        if (computingByFiltre[j].Computings[i].Variation > maxVariation)
                        {
                            maxVariation = computingByFiltre[j].Computings[i].Variation;
                        }
                    }
                    else
                    {
                        if (computingByFiltre[j].Computings[i].ComputingN > maxVariation)
                        {
                            maxVariation = computingByFiltre[j].Computings[i].ComputingN;
                        }
                    }



                }
                decimal previousmaxVariation = 0;
                if( i > 0)
                {
                    for (int j = 0; j < computingByFiltre.Count(); j++)
                    {
                        if (computingByFiltre[j].Computings[i - 1].Variation > previousmaxVariation)
                        {
                            previousmaxVariation = computingByFiltre[j].Computings[i - 1].Variation;
                        }
                    }

                    for (int j = 0; j < computingByFiltre.Count(); j++)
                    {
                        if (computingByFiltre[j].Computings[i - 1].Variation == previousmaxVariation)
                        {

                            if (i == firstComputing.Computings.Count() - 1)
                            {
                                bootMax = 0;
                            }
                            else
                            {
                                bootMax = computingByFiltre[j].Computings[i].Variation;
                            }
                        }

                        if (i > 0)
                        {
                            var lastCoeff = computingsMax.Last().Coeff;
                            Coeff = bootMax * lastCoeff;
                        }

                    }
                }


                var computingMax = new ComputingMaxDto();
                computingMax.VariationMax = maxVariation;
                computingMax.BootMax = bootMax;
                computingMax.Coeff = Coeff;
                computingsMax.Add(computingMax);
            }


            return computingsMax;
        }


        public Binance.Net.Enums.KlineInterval MapInterval(string interval)
        {
            Binance.Net.Enums.KlineInterval inervalBack;
            switch (interval)
            {
                case "OneMinute":
                    inervalBack = Binance.Net.Enums.KlineInterval.OneMinute;
                    break;
                case "ThreeMinutes":
                    inervalBack = Binance.Net.Enums.KlineInterval.ThreeMinutes;
                    break;
                case "FiveMinutes":
                    inervalBack = Binance.Net.Enums.KlineInterval.FiveMinutes;
                    break;
                case "FifteenMinutes":
                    inervalBack = Binance.Net.Enums.KlineInterval.FifteenMinutes;
                    break;
                case "ThirtyMinutes":
                    inervalBack = Binance.Net.Enums.KlineInterval.ThirtyMinutes;
                    break;
                case "OneHour":
                    inervalBack = Binance.Net.Enums.KlineInterval.OneHour;
                    break;
                case "TwoHour":
                    inervalBack = Binance.Net.Enums.KlineInterval.TwoHour;
                    break;
                case "FourHour":
                    inervalBack = Binance.Net.Enums.KlineInterval.FourHour;
                    break;
                case "SixHour":
                    inervalBack = Binance.Net.Enums.KlineInterval.SixHour;
                    break;
                case "EightHour":
                    inervalBack = Binance.Net.Enums.KlineInterval.EightHour;
                    break;
                case "TwelveHour":
                    inervalBack = Binance.Net.Enums.KlineInterval.TwelveHour;
                    break;
                case "OneDay":
                    inervalBack = Binance.Net.Enums.KlineInterval.OneDay;
                    break;
                case "ThreeDay":
                    inervalBack = Binance.Net.Enums.KlineInterval.ThreeDay;
                    break;
                case "OneWeek":
                    inervalBack = Binance.Net.Enums.KlineInterval.OneWeek;
                    break;
                default:
                    inervalBack = Binance.Net.Enums.KlineInterval.OneMonth;
                    break;
            }
            return inervalBack;
        }
        public List<ComputingDto> GetComputingByCrypto(string crypto, Binance.Net.Enums.KlineInterval interval, DateTime debut, DateTime fin, int limit)
        {
            var client = new BinanceClient();
            var klines = client.Spot.Market.GetKlines(
            symbol: crypto,
            interval: interval,
            startTime: debut,
            endTime: fin,
            limit: limit).Data as IEnumerable<Binance.Net.Objects.Spot.MarketData.BinanceSpotKline>;


            List<ComputingDto> computings = new List<ComputingDto>();

            foreach (var i in klines)
            {
                computings.Add(new ComputingDto()
                {
                    OpenTime = i.OpenTime,
                    Open = i.Open,
                    Close = i.Close
                });
            }


            return computings;
        }

    }
}
