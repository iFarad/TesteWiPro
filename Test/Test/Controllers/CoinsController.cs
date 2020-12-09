using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Test.Models;
using Test.Repository;

namespace Test.Controllers
{
    public class CoinsController : ApiController
    {
        private Coins coin;

        [HttpGet]
        public Coins GetItemFila()
        {
            try
            {
                var result = CoinsRepository.CoinsList.Last();
                CoinsRepository.CoinsList.Remove(result);
                return result;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound,"Não há itens na lista"));
            }
        }

        [HttpPost]
        public void AddItemFila(string moeda, DateTime data_inicio, DateTime data_fim)
        {
            try
            {
                coin = new Coins(moeda, data_inicio, data_fim);
                CoinsRepository.CoinsList.Add(coin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
