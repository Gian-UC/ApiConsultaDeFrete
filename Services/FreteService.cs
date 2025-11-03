using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvioRapidoApi.Services
{
    public class FreteService
    {
        public decimal CalcularFrete(decimal peso, decimal altura, decimal largura, decimal comprimento)
        {            
            decimal volume = altura * largura * comprimento;
            decimal valor = (peso * 0.5m) + (volume * 0.05m);
            return Math.Round(valor, 2);
        }
    }
}