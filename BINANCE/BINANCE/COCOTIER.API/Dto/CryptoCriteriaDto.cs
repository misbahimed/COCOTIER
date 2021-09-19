using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOTIER.API.Dto
{
    public class CryptoCriteriaDto
    {
       public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }
        public string Interval { get; set; }
        public List<string> Crypto { get; set; }
        public int Limit { get; set; }

        public string ComputingN { get; set; }
    }
}
