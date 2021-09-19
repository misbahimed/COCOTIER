using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BINANCE.BUSINESS.Dto
{
    public class ComputingDataDto
    {
        public string Crypto { get; set; }

        public List<ComputingDto> Computings { get; set; }
    }
}
