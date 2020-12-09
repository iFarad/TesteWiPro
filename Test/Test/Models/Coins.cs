using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    public class Coins
    {
        public string Moeda { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }

        public Coins(string moeda, DateTime dataInicio, DateTime dataFim)
        {
            this.Moeda = moeda;
            this.DataInicio = dataInicio.ToString("yyyy-MM-dd");
            this.DataFim = dataFim.ToString("yyyy-MM-dd");
        }
    }
}