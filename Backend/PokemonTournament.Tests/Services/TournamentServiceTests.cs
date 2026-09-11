using Moq;
using PokemonTournament.Enums;
using PokemonTournament.Infrastructure;
using PokemonTournament.Models;
using PokemonTournament.Services;
using Microsoft.Extensions.Options;

namespace PokemonTournament.Tests.Services;

public class TournamentServiceTests
{
    [Fact]
    public async Task GetTournamentResultsAsync_RunsRoundRobinAndMapsPrimaryType()
    {
        var battleService = new Mock<IBattleService>();
        battleService
            .Setup(s => s.FightResult(It.IsAny<Pokemon>(), It.IsAny<Pokemon>()))
            .Returns((Pokemon _, Pokemon _) => BattleResults.FirstWins);

        var pokeClient = CreateClient();
        var service = CreateService(battleService, pokeClient);

        var results = await service.GetTournamentResultsAsync(SortOptions.Id, SortDirection.Asc);

        Assert.Equal(16, results.Count);
        Assert.All(results, pokemon =>
        {
            Assert.Equal("water", pokemon.Type);
            Assert.Equal(15, pokemon.Wins + pokemon.Losses);
            Assert.Equal(0, pokemon.Ties);
        });
        battleService.Verify(s => s.FightResult(It.IsAny<Pokemon>(), It.IsAny<Pokemon>()), Times.Exactly(120));
    }

    [Fact]
    public async Task GetTournamentResultsAsync_UsesUniqueParticipantsAndFightsEachPairOnce()
    {
        var battlePairs = new HashSet<(int FirstId, int SecondId)>();
        var battleService = new Mock<IBattleService>();
        battleService
            .Setup(s => s.FightResult(It.IsAny<Pokemon>(), It.IsAny<Pokemon>()))
            .Callback<Pokemon, Pokemon>((first, second) =>
            {
                Assert.NotEqual(first.Id, second.Id);
                var pair = first.Id < second.Id
                    ? (first.Id, second.Id)
                    : (second.Id, first.Id);
                Assert.True(battlePairs.Add(pair), $"Pair {pair} was fought more than once.");
            })
            .Returns(BattleResults.Ties);

        var service = CreateService(battleService, CreateClient());

        var results = await service.GetTournamentResultsAsync(SortOptions.Id, SortDirection.Asc);

        Assert.Equal(16, results.Count);
        Assert.Equal(results.Count, results.Select(pokemon => pokemon.Id).Distinct().Count());
        Assert.Equal(16 * 15 / 2, battlePairs.Count);
    }

    [Fact]
    public async Task GetTournamentResultsAsync_WhenSortingDescending_ReturnsReverseNameOrder()
    {
        var battleService = new Mock<IBattleService>();
        battleService
            .Setup(s => s.FightResult(It.IsAny<Pokemon>(), It.IsAny<Pokemon>()))
            .Returns((Pokemon _, Pokemon _) => BattleResults.Ties);
        var pokeClient = CreateClient();
        var service = CreateService(battleService, pokeClient);

        var results = await service.GetTournamentResultsAsync(SortOptions.Name, SortDirection.Desc);

        Assert.Equal(results.OrderByDescending(pokemon => pokemon.Name).Select(pokemon => pokemon.Name),
            results.Select(pokemon => pokemon.Name));
    }

    private static Mock<IPokeClient> CreateClient()
    {
        var client = new Mock<IPokeClient>();
        client
            .Setup(s => s.GetPokemonAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => new PokemonAPIResponse
            {
                Id = id,
                Name = $"pokemon-{id:D3}",
                BaseExperience = 100,
                Types = new List<PokemonSlotTypeInfo>
                {
                    new() { Slot = 2, PokemonAPIType = new PokemonAPIType { Name = "ignored" } },
                    new() { Slot = 1, PokemonAPIType = new PokemonAPIType { Name = "water" } }
                }
            });

        return client;
    }

    private static TournamentService CreateService(
        Mock<IBattleService> battleService,
        Mock<IPokeClient> pokeClient) =>
        new(
            battleService.Object,
            pokeClient.Object,
            Options.Create(new TournamentOptions
            {
                DefaultParticipantCount = 16,
                MinPokemonId = 1,
                MaxPokemonId = 151,
                MaxConcurrentRequests = 16
            }));
}