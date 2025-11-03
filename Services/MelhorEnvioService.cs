using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EnvioRapidoApi.Services
{
    public class MelhorEnvioService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;

        public MelhorEnvioService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _token = config["MelhorEnvio:Token"] ?? throw new Exception("Token do Melhor Envio não configurado!");
        }

        public async Task<string> CalcularFreteAsync(string cepOrigem, string cepDestino, decimal peso, decimal altura, decimal largura, decimal comprimento)
        {
            var requestBody = new
            {
                from = new { postal_code = cepOrigem },
                to = new { postal_code = cepDestino },
                packages = new[]
                {
                    new
                    {
                        height = altura,
                        width = largura,
                        length = comprimento,
                        weight = peso
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await _httpClient.PostAsync("https://sandbox.melhorenvio.com.br/api/v2/me/shipment/calculate", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao calcular frete: {error}");
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
