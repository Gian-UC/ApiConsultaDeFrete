using System.Text.Json;

namespace EnvioRapidoApi.Services
{
    public class ViaCepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ValidarCepAsync(string cep)
        {
            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
            
            if (!response.IsSuccessStatusCode)
                return false;

            var jsonString = await response.Content.ReadAsStringAsync();

            using var json = JsonDocument.Parse(jsonString);

            // Verifica se o campo "erro" existe no JSON
            if (json.RootElement.TryGetProperty("erro", out var erroProperty))
            {
                return false; // CEP inválido
            }

            return true; // CEP válido
        }
    }
}
