using PokemonTournament.Models;

namespace PokemonTournament.Infrastructure
{
    public interface IPokeClient
    {
        Task<PokemonAPIResponse> GetPokemonAsync(int id);
    }
}
