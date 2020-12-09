using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test2.Models;

namespace Test2.Controller
{
    public class Test2Controller
    {
        public string pathDadosMoeda = Directory.GetCurrentDirectory() + @"..\..\..\Repository\Db\DadosMoeda.csv";
        public string pathDadosCotacao = Directory.GetCurrentDirectory() + @"..\..\..\Repository\Db\DadosCotacao.csv";
        public string pathDePara = Directory.GetCurrentDirectory() + @"..\..\..\Repository\Db\De-Para.csv";
        public List<CoinsModel> GetListCoins(CoinsModel apiCoin)
        {
            try
            {
                List<CoinsModel> dadosMoeda = new List<CoinsModel>();
                List<CoinsModel> coinList = new List<CoinsModel>();

                using (StreamReader sr = new StreamReader(pathDadosMoeda))
                {
                    string headerLine = sr.ReadLine();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        CoinsModel coin = new CoinsModel();
                        var arrayLine = line.Split(';');
                        coin.Moeda = arrayLine.ElementAt(0);
                        coin.DataCotacao = arrayLine.ElementAt(1);
                        dadosMoeda.Add(coin);
                    }

                    foreach (var coin in dadosMoeda)
                    {
                        if (dateChecker(apiCoin.DataInicio, apiCoin.DataFim, coin.DataCotacao))
                        {
                            coinList.Add(coin);
                        }
                    }
                }

                return coinList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new ArgumentException(e.Message);
            }
        }

        public List<CoinsModel> GetQuotationCode(List<CoinsModel> coinList)
        {
            try
            {
                List<CoinsModel> dePara = new List<CoinsModel>();

                using (StreamReader sr = new StreamReader(pathDePara))
                {
                    string headerLine = sr.ReadLine();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        CoinsModel coin = new CoinsModel();
                        var arrayLine = line.Split(';');
                        coin.Moeda = arrayLine.ElementAt(0);
                        coin.CodigoCotacao = Convert.ToInt32(arrayLine.ElementAt(1));
                        dePara.Add(coin);
                    }

                    foreach (var coin in coinList)
                    {
                        coin.CodigoCotacao = (from d in dePara
                                              where (d.Moeda == coin.Moeda)
                                              select d.CodigoCotacao).First();
                    }

                }

                return coinList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new ArgumentException(e.Message);
            }
        }

        public List<CoinsModel> GetAllQuotationValue(List<CoinsModel> coinList)
        {
            try
            {
                List<CoinsModel> dadosCotacao = new List<CoinsModel>();

                using (StreamReader sr = new StreamReader(pathDadosCotacao))
                {
                    string headerLine = sr.ReadLine();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        CoinsModel coin = new CoinsModel();
                        var arrayLine = line.Split(';');
                        coin.ValorCotacao = Convert.ToDouble(arrayLine.ElementAt(0));
                        coin.CodigoCotacao = Convert.ToInt32(arrayLine.ElementAt(1));
                        coin.DataCotacao = Convert.ToDateTime(arrayLine.ElementAt(2)).ToString("yyyy-MM-dd");
                        dadosCotacao.Add(coin);
                    }

                    foreach (var coin in coinList)
                    {
                        coin.ValorCotacao = (from d in dadosCotacao
                                             where (d.CodigoCotacao == coin.CodigoCotacao) && (d.DataCotacao == coin.DataCotacao)
                                             select d.ValorCotacao).First();
                    }
                }
                return coinList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new ArgumentException(e.Message);
            }
        }

        public void GenerateFile(List<CoinsModel> coinList)
        {
            try
            {
                string path = Directory.GetCurrentDirectory() + @"..\..\..\Repository\Resultado_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".csv";

                using (StreamWriter file = new StreamWriter(path, true))
                {
                    file.WriteLine("ID_MOEDA; DATA_REF; VL_COTACAO");
                    foreach (var coin in coinList)
                    {
                        file.WriteLine(coin.Moeda + ";" + coin.DataCotacao + ";" + coin.ValorCotacao);
                    }
                }

                Console.WriteLine($"Arquivo Resultado_{ DateTime.Now.ToString("yyyyMMdd_hhmmss")}.csv gerado e alocado na pasta Repository.");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool dateChecker(string dataInicio, string dataFim, string dataCotacao)
        {
            DateTime startDate = DateTime.Parse(dataInicio);
            DateTime endDate = DateTime.Parse(dataFim);
            DateTime quotationDate = DateTime.Parse(dataCotacao);

            if(startDate > endDate)
            {
                startDate = DateTime.Parse(dataFim);
                endDate = DateTime.Parse(dataInicio);
            }


            if (quotationDate >= startDate && quotationDate <= endDate)
            {
                return true;
            }
            else return false;
        }
    }
}
