using PokemonTournament.Models;

namespace PokemonTournament.Infrastructure
{
    public class PokeClient : IPokeClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseURL;

        public PokeClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseURL  = configuration["APIURL:BaseUrl"]
            ?? throw new InvalidOperationException("APIURL:BaseUrl is not configured.");
        }

        public async Task<PokemonAPIResponse> GetPokemonAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<PokemonAPIResponse>($"{_baseURL}/{id}");
            return response ?? throw new InvalidOperationException($"PokeAPI returned no data for pokemon id {id}.");
        }
    }
}
