using PokemonTournament.Enums;
using PokemonTournament.Infrastructure;
using PokemonTournament.Models;
using Microsoft.Extensions.Options;

namespace PokemonTournament.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly IBattleService _battleService;
        private readonly IPokeClient _pokeClient;
        private readonly TournamentOptions _options;


        public TournamentService(
            IBattleService battleService,
            IPokeClient pokeClient,
            IOptions<TournamentOptions> options)
        {
            _battleService = battleService;
            _pokeClient = pokeClient;
            _options = options.Value;

            if (_options.MinPokemonId < 1 ||
                _options.MaxPokemonId < _options.MinPokemonId ||
                _options.DefaultParticipantCount < 2 ||
                _options.DefaultParticipantCount > _options.MaxPokemonId - _options.MinPokemonId + 1 ||
                _options.MaxConcurrentRequests < 1)
            {
                throw new ArgumentException("Tournament configuration is invalid.");
            }
        }

        public async Task<List<Pokemon>> GetTournamentResultsAsync(
            SortOptions sortOption,
            SortDirection sortDirection)
        {
            var randomIds = SelectRandomIds();

            // Get the json from API to fetch pokemons
            var responses = new List<PokemonAPIResponse>(randomIds.Count);

            bool stopProcessing = false;

            await Parallel.ForEachAsync(
                randomIds,
                new ParallelOptions { MaxDegreeOfParallelism = _options.MaxConcurrentRequests },
                async (id, cancellationToken) =>
                {
                    if (stopProcessing)
                        return;

                    try
                    {
                        var response = await _pokeClient.GetPokemonAsync(id);

                        if (response != null && response.Types != null)
                        {
                            lock (responses)
                            {
                                responses.Add(response);
                            }
                        }
                        else
                        {
                            stopProcessing = true;
                        }
                    }
                    catch
                    {
                        stopProcessing = true;
                    }
                });

            // If something failed then return null so controller can decide
            if (stopProcessing || responses.Count != randomIds.Count)
            {
                return null;
            }

            // Convert to DTO
            var roaster = responses.Select(response => ConvertToPokemon(response)).ToList();

            // Generate tournament internally
            RunRoundRobin(roaster);
            
            // give result based on sorting
            var sortedResult = Sort(roaster, sortOption, sortDirection).ToList();
            return sortedResult;
        }

        #region private methods

        private List<int> SelectRandomIds()
        {
            var ids = Enumerable.Range( _options.MinPokemonId,_options.MaxPokemonId - _options.MinPokemonId + 1).ToArray();

            for (var index = 0; index < _options.DefaultParticipantCount; index++)
            {
                var swapIndex = Random.Shared.Next(index, ids.Length);
                (ids[index], ids[swapIndex]) = (ids[swapIndex], ids[index]);
            }

            return ids.Take(_options.DefaultParticipantCount).ToList();
        }

        //private List<int> SelectRandomIds()
        //{
        //    var ids = new HashSet<int>();

        //    while (ids.Count < _options.DefaultParticipantCount)
        //    {
        //        int id = Random.Shared.Next(_options.MinPokemonId, _options.MaxPokemonId + 1);
        //        ids.Add(id); 
        //    }

        //    return ids.ToList();
        //}


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
