using System;
using System.Collections.Generic;
using System.Text;

namespace BINANCE.BUSINESS.Dto
{
    public class ComputingDto
    {
        public ComputingDto()
        { }
        public decimal Close { get; set; }
        public decimal Open { get; set; }
        public DateTime OpenTime { get; set; }

        public decimal Variation => Close / Open;

        public decimal Coeff { get; set; }

        public decimal ComputingN { get; set; }

    }
}
