using Microsoft.AspNetCore.Mvc;
using Moq;
using PokemonTournament.Controllers;
using PokemonTournament.Enums;
using PokemonTournament.Models;
using PokemonTournament.Services;

namespace PokemonTournament.Tests.Controllers;

public class PokemonControllerTests
{
    private readonly Mock<ITournamentService> _tournamentServiceMock = new();
    private readonly PokemonController _controller;

    public PokemonControllerTests()
    {
        _controller = new PokemonController(_tournamentServiceMock.Object);
    }

    [Fact]
    public async Task GetTournamentStatistics_WhenSortByMissing_ReturnsBadRequest()
    {
        var result = await _controller.GetTournamentStatistics(null, null);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("sortBy parameter is required", GetErrorMessage(badRequest.Value));
    }

    [Fact]
    public async Task GetTournamentStatistics_WhenSortByInvalid_ReturnsBadRequest()
    {
        var result = await _controller.GetTournamentStatistics("test", null);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("sortBy parameter is invalid", GetErrorMessage(badRequest.Value));
    }

    [Fact]
    public async Task GetTournamentStatistics_WhenSortDirectionInvalid_ReturnsBadRequest()
    {
        var result = await _controller.GetTournamentStatistics("ties", "test");

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("sortDirection parameter is invalid", GetErrorMessage(badRequest.Value));
    }

    [Fact]
    public async Task GetTournamentStatistics_WhenParametersAreValid_ReturnsOkWithRoster()
    {
        var roster = new List<Pokemon>
        {
            new() { Id = 1, Name = "bulbasaur", Type = "grass", Wins = 6, Losses = 6, Ties = 4 },
        };

        _tournamentServiceMock
            .Setup(s => s.GetTournamentResultsAsync(SortOptions.Wins, SortDirection.Desc))
            .ReturnsAsync(roster);

        var result = await _controller.GetTournamentStatistics("wins", "desc");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var statistics = Assert.IsAssignableFrom<IEnumerable<PokemonDTO>>(okResult.Value);
        Assert.Single(statistics);
    }

    private static string? GetErrorMessage(object? value) =>
        value?.GetType().GetProperty("error")?.GetValue(value) as string;
}
