using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Globalization;

namespace EnvioRapidoApi.Services
{
    public class MelhorEnvioService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;

        public MelhorEnvioService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;

            _token = config["MelhorEnvio:Token"] 
                ?? throw new Exception("Token do Melhor Envio não configurado!");

            // Define a base URL apenas uma vez (boa prática)
            _httpClient.BaseAddress = new Uri("https://sandbox.melhorenvio.com.br/api/v2/");
        }

        public async Task<decimal> CalcularFreteAsync(string cepOrigem, string cepDestino, decimal peso, decimal altura, decimal largura, decimal comprimento)
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

            using var request = new HttpRequestMessage(HttpMethod.Post, "me/shipment/calculate")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao calcular frete (HTTP {response.StatusCode}): {error}");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(jsonString);

            var firstOption = jsonDoc.RootElement.EnumerateArray().FirstOrDefault();
            if (firstOption.ValueKind == JsonValueKind.Undefined)
                throw new Exception("Nenhum valor de frete retornado pela API do Melhor Envio.");

            if (!firstOption.TryGetProperty("price", out var priceElement))
                throw new Exception("Resposta da API não contém o campo 'price'.");

            var priceString = priceElement.GetString();
            if (!decimal.TryParse(priceString, NumberStyles.Any, CultureInfo.InvariantCulture, out var valorFrete))
                throw new Exception($"Não foi possível converter o valor do frete: {priceString}");

            return valorFrete;
        }
    }
}
