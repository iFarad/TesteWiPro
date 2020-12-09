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
                    Console.WriteLine($"Iniciando o processo da Moeda: {coin.Moeda} para o período de data {coin.DataInicio} até {coin.DataFim}");
                    Test2Controller controller = new Test2Controller();
                    var coinList = controller.GetListCoins(coin);
                    coinList = controller.GetQuotationCode(coinList);
                    coinList = controller.GetAllQuotationValue(coinList);
                    controller.GenerateFile(coinList);
                } else Console.WriteLine("Não foram encontrados itens a serem processados");
                stopWatch.Stop();
                double tempoTotal = (stopWatch.Elapsed.Minutes / 60) + stopWatch.Elapsed.Seconds;
                Console.WriteLine($"Tempo total de processamento do ciclo: {tempoTotal}.{stopWatch.Elapsed.Milliseconds} segundos. \r\n");
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
