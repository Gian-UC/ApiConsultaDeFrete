using Microsoft.AspNetCore.Mvc;
using EnvioRapidoApi.DTOs;
using EnvioRapidoApi.Models;
using EnvioRapidoApi.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using EnvioRapidoApi.Repositories;
using EnvioRapidoApi.Data;


namespace EnvioRapidoApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EnviosController : ControllerBase
    {
        private readonly ViaCepService _viaCepService;
        private readonly MelhorEnvioService _melhorEnvioService;
        private readonly EnvioRepository _envioRepository;

        private readonly RabbitMqService _rabbitMqService;


        public EnviosController(ViaCepService viaCepService, MelhorEnvioService melhorEnvioService, EnvioRepository envioRepository, RabbitMqService rabbitMqService)
        {
            _viaCepService = viaCepService;
            _melhorEnvioService = melhorEnvioService;
            _envioRepository = envioRepository;
            _rabbitMqService = rabbitMqService;
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

            // 💰 Calcula o frete antes de salvar
            decimal valorFrete = await _melhorEnvioService.CalcularFreteAsync(
                dto.OrigemCep,
                dto.DestinoCep,
                dto.Peso,
                dto.Altura,
                dto.Largura,
                dto.Comprimento
            );

            var envio = new Envio
            {
                OrigemCep = dto.OrigemCep,
                DestinoCep = dto.DestinoCep,
                Peso = dto.Peso,
                Altura = dto.Altura,
                Largura = dto.Largura,
                Comprimento = dto.Comprimento,
                ValorFrete = valorFrete
            };


            await _envioRepository.SalvarAsync(envio);

            var mensagem = new
            {
                envio.Id,
                envio.OrigemCep,
                envio.DestinoCep,
                envio.ValorFrete,
                Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            _rabbitMqService.PublicarMensagem(mensagem);

            return CreatedAtAction(nameof(GetById), new { id = envio.Id }, envio);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var envio = await _envioRepository.BuscarPorIdAsync(id);
            if (envio == null)
                return NotFound(new { msg = "Envio não encontrado." });
            return Ok(envio);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool removido = await _envioRepository.RemoverAsync(id);

            if (!removido)
                return NotFound(new { mensagem = "Envio não encontrado." });

            return Ok(new { mensagem = "Envio excluído com sucesso!" });
        }

    }
}
