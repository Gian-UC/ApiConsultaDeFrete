using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvioRapidoApi.DTOs
{
    public class EnvioDTO
    {
        public string OrigemCep { get; set; }
        public string DestinoCep { get; set; }
        public decimal Peso { get; set; }
        public decimal Altura { get; set; }
        public decimal Largura { get; set; }
        public decimal Comprimento { get; set; }        
    }
}