using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Test2.Controller;
using Test2.Models;

namespace Test2
{
    class Program
    {
        static public HttpClient httpClient = new HttpClient();
        static public CoinsModel coin;
        static public void Main(string[] args)
        {
            httpClient.BaseAddress = new Uri("https://localhost:44344/");
            while (true)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                GetCoin().GetAwaiter().GetResult();
                if (coin != null)
                {
                    Test2Controller controller = new Test2Controller();
                    var coinList = controller.GetListCoins(coin);
                    coinList = controller.GetQuotationCode(coinList);
                    coinList = controller.GetAllQuotationValue(coinList);
                    controller.GenerateFile(coinList);
                }
                stopWatch.Stop();
                Console.WriteLine($"Tempo total de processamento do ciclo: {stopWatch.Elapsed.Seconds},{stopWatch.Elapsed.Milliseconds} segundos. \r\n");
                Thread.Sleep(120000);
            }
        }

        static public async Task<CoinsModel> GetCoin()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("api/Coins");
                if (response.IsSuccessStatusCode)
                {
                    coin = await response.Content.ReadAsAsync<CoinsModel>();
                } else coin = null;
                return coin;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new ArgumentException(e.Message);
            }
        }

    }
}
