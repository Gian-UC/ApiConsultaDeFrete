using Microsoft.AspNetCore.Mvc;
using EnvioRapidoApi.DTOs;
using EnvioRapidoApi.Models;
using EnvioRapidoApi.Services;
using System.Text.Json;

namespace EnvioRapidoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnviosController : ControllerBase
    {
        private readonly ViaCepService _viaCepService;
        private readonly MelhorEnvioService _melhorEnvioService;

        public EnviosController(ViaCepService viaCepService, MelhorEnvioService melhorEnvioService)
        {
            _viaCepService = viaCepService;
            _melhorEnvioService = melhorEnvioService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EnvioDTO dto)
        {
            if (string.IsNullOrEmpty(dto.OrigemCep) || string.IsNullOrEmpty(dto.DestinoCep))
                return BadRequest("Os CEPs são obrigatórios.");

            bool origemValida = await _viaCepService.ValidarCepAsync(dto.OrigemCep);
            bool destinoValido = await _viaCepService.ValidarCepAsync(dto.DestinoCep);

            if (!origemValida || !destinoValido)
                return BadRequest("CEP inválido.");

            var resultadoFrete = await _melhorEnvioService.CalcularFreteAsync(
                dto.OrigemCep,
                dto.DestinoCep,
                dto.Peso,
                dto.Altura,
                dto.Largura,
                dto.Comprimento
            );

            return Ok(JsonDocument.Parse(resultadoFrete));
        }
    }
}
