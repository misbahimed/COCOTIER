using BINANCE.BUSINESS.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BINANCE.BUSINESS
{
    public interface IComputing
    {
        public List<ComputingDataDto> GetComputingByFiltre(List<string> crypto, string interval, DateTime debut, DateTime fin, int limit, string ComputingN);

        public List<ComputingMaxDto> GetVariationMax(List<string> crypto, string interval, DateTime debut, DateTime fin, int limit, string ComputingN);
    }
}
