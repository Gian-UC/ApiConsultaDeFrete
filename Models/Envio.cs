using System.ComponentModel.DataAnnotations.Schema;

namespace EnvioRapidoApi.Models
{
    public class Envio
    {
        public int Id { get; set; }

        public string OrigemCep { get; set; } = string.Empty;
        public string DestinoCep { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Peso { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Altura { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Largura { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Comprimento { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorFrete { get; set; }
    }
}
