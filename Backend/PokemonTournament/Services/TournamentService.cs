using PokemonTournament.Enums;
using PokemonTournament.Infrastructure;
using PokemonTournament.Models;
using System;
using System.Numerics;

namespace PokemonTournament.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly IBattleService _battleService;
        private readonly IPokeClient _pokeClient;

        private const int Total = 16;
        private const int MinPokemonId = 1;
        private const int MaxPokemonId = 151;


        public TournamentService(IBattleService battleService, IPokeClient pokeClient)
        {
            _battleService = battleService;
            _pokeClient = pokeClient;
        }

        public async Task<List<Pokemon>> GetTournamentResultsAsync(SortOptions sortOption, SortDirection sortDirection)
        {
            // Generate 16 random ids from the given range
            var ids = Enumerable.Range(MinPokemonId, MaxPokemonId);
            var randomIds = ids.OrderBy(_ => Random.Shared.Next()).Take(Total).ToList();

            // Get the json from API to fetch pokemons
            var tasks = randomIds.Select(id => _pokeClient.GetPokemonAsync(id));
            var apiResponses = await Task.WhenAll(tasks);

            // Convert to DTO
            var roaster = apiResponses.Select(response => ConvertToPokemon(response)).ToList();

            // Generate tournament internally
            RunRoundRobin(roaster);
            
            // give result based on sorting
            var sortedResult = Sort(roaster, sortOption, sortDirection).ToList();
            return sortedResult;
        }

        #region private methods

        private void RunRoundRobin(IList<Pokemon> roster)
        {
            for (var i = 0; i < roster.Count; i++)
            {
                for (var j = i + 1; j < roster.Count; j++)
                {
                    switch (_battleService.FightResult(roster[i], roster[j]))
                    {
                        case BattleResults.FirstWins:
                            roster[i].Wins++;
                            roster[j].Losses++;
                            break;
                        case BattleResults.SecondWins:
                            roster[j].Wins++;
                            roster[i].Losses++;
                            break;
                        default:
                            roster[i].Ties++;
                            roster[j].Ties++;
                            break;
                    }
                }
            }
        }

        private static IList<Pokemon> Sort(List<Pokemon> roster,SortOptions sortBy,SortDirection sortDirection)
        {
            List<Pokemon> sorted;

            if (sortBy == SortOptions.Wins)
            {
                sorted = roster.OrderBy(p => p.Wins).ToList();
            }
            else if (sortBy == SortOptions.Losses)
            {
                sorted = roster.OrderBy(p => p.Losses).ToList();
            }
            else if (sortBy == SortOptions.Ties)
            {
                sorted = roster.OrderBy(p => p.Ties).ToList();
            }
            else if (sortBy == SortOptions.Name)
            {
                sorted = roster.OrderBy(p => p.Name).ToList();
            }
            else if (sortBy ==  SortOptions.Id)
            {
                sorted = roster.OrderBy(p => p.Id).ToList();
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(sortBy));
            }

            if (sortDirection == SortDirection.Desc)
            {
                sorted.Reverse();
            }

            return sorted;
        }


        private static Pokemon ConvertToPokemon(PokemonAPIResponse response)
        {
            string primaryType = "";
            foreach (var t in response.Types)
            {
                if (t.Slot == 1)
                {
                    primaryType = t.PokemonAPIType.Name;
                    break;
                }
            }
            return new Pokemon
            {
                Id = response.Id,
                Name = response.Name,
                Type = primaryType,
                BaseExperience = response.BaseExperience,
            };
        }

        #endregion
    }
}
