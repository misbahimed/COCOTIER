using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BINANCE.BUSINESS;
using COCOTIER.API.Dto;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace COCOTIER.API.Controllers
{
    [Route("api/computing")]
    [ApiController]
    public class Computing : ControllerBase
    {

        private readonly IComputing _computing;
        public Computing(IComputing computing)
        {
            _computing = computing;
        }

        // GET: api/<Computing>
        [HttpGet]
        public List<BINANCE.BUSINESS.Dto.ComputingDto> Get()
        {

            //var getComp = _computing.GetComputingByFiltre();

            return null;
        }

        // GET api/<Computing>/5
        [HttpPost("get-crypto")]
        public List<BINANCE.BUSINESS.Dto.ComputingDataDto> GetCrypto([FromBody] CryptoCriteriaDto cryptoCriteria)
        {

            var getComp = _computing.GetComputingByFiltre(cryptoCriteria.Crypto, cryptoCriteria.Interval, cryptoCriteria.Debut, cryptoCriteria.Fin, cryptoCriteria.Limit, cryptoCriteria.ComputingN);

            return getComp;
        }

        [HttpPost("get-variation-max")]
        public List<BINANCE.BUSINESS.Dto.ComputingMaxDto> GetVariationMax([FromBody] CryptoCriteriaDto cryptoCriteria)
        {

            var getComp = _computing.GetVariationMax(cryptoCriteria.Crypto, cryptoCriteria.Interval, cryptoCriteria.Debut, cryptoCriteria.Fin, cryptoCriteria.Limit, cryptoCriteria.ComputingN);

            return getComp;
        }

        // POST api/<Computing>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Computing>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Computing>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
