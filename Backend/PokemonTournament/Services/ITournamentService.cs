using PokemonTournament.Enums;
using PokemonTournament.Models;

namespace PokemonTournament.Services
{
    public interface ITournamentService
    {
        Task<List<Pokemon>> GetTournamentResultsAsync(SortOptions sortOption, SortDirection sortDirection);
    }
}
