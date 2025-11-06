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


        /// <summary>
        /// Calcula o valor do frete, registra o envio no banco e publica no RabbitMQ.
        /// </summary>
        /// <param name="dto">Dados necessários para calcular o frete.</param>
        /// <returns>Dados do envio salvo + valor de frete calculado.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EnvioDTO dto)
        {
            if (string.IsNullOrEmpty(dto.OrigemCep) || string.IsNullOrEmpty(dto.DestinoCep))
                return BadRequest("Os CEPs são obrigatórios.");

            bool origemValida = await _viaCepService.ValidarCepAsync(dto.OrigemCep);
            bool destinoValido = await _viaCepService.ValidarCepAsync(dto.DestinoCep);

            if (!origemValida || !destinoValido)
                return BadRequest("CEP inválido.");

            // 🆕 Status 409 - envio duplicado
            if (await _envioRepository.ExisteEnvioIgualAsync(dto.OrigemCep, dto.DestinoCep, dto.Peso))
                return Conflict(new { mensagem = "Já existe um envio igual cadastrado." }); // 409    

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

            // 🆕 Status 202 - aceito para processamento
            return Accepted(new
            {
                mensagem = "Envio registrado e enviado para processamento.",
                envio.Id,
                envio.ValorFrete
            });
        }


        /// <summary>
        /// Consulta um envio específico pelo ID.
        /// </summary>
        /// <param name="id">ID do envio a ser consultado.</param>
        /// <returns>Dados do envio ou 404 caso não exista.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var envio = await _envioRepository.BuscarPorIdAsync(id);

            if (envio == null)
                return NotFound(new { mensagem = "Envio não encontrado." });

            var resposta = new
            {
                envio.Id,
                envio.OrigemCep,
                envio.DestinoCep,
                envio.Peso,
                envio.Altura,
                envio.Largura,
                envio.Comprimento,
                ValorFrete = envio.ValorFrete.ToString("C2"), // 💰 formatado em moeda
                DataConsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
            };

            return Ok(resposta);
        }


        /// <summary>
        /// Exclui um envio já registrado no banco.
        /// Requer autenticação JWT.
        /// </summary>
        /// <param name="id">ID do envio a excluir.</param>
        /// <returns>Status da operação.</returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool removido = await _envioRepository.RemoverAsync(id);

            if (!removido)
                return NotFound(new { mensagem = "Envio não encontrado." });

            return Ok(new { mensagem = "Envio excluído com sucesso!" });
        }


        /// <summary>
        /// Verifica se o envio existe sem retornar o corpo da resposta.
        /// </summary>
        [HttpHead("{id}")]
        public async Task<IActionResult> Head(int id)
        {
            var envio = await _envioRepository.BuscarPorIdAsync(id);

            if (envio == null)
                return NotFound(); // 404

            return NoContent(); // 204 → Existe, mas não retorna corpo
        }


        /// <summary>
        /// Informa os métodos suportados para /api/envios.
        /// </summary>
        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Add("Allow", "GET, POST, DELETE, HEAD, OPTIONS");
            return Ok(); // 200
        }

    }
}
