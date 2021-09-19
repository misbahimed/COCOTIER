using System;
using System.Collections.Generic;
using System.Text;

namespace BINANCE.BUSINESS.Dto
{

    public class ComputingMaxDto
    {
        public ComputingMaxDto()
        { }
        public decimal VariationMax { get; set; }
        public decimal BootMax { get; set; }

        public decimal Coeff { get; set; } = 1;

    }

}
