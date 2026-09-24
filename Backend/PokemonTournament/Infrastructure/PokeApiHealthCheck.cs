using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PokemonTournament.Infrastructure;

public sealed class PokeApiHealthCheck : IHealthCheck
{
    private readonly IPokeClient _pokeClient;

    public PokeApiHealthCheck(IPokeClient pokeClient)
    {
        _pokeClient = pokeClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var pokemon = await _pokeClient.GetPokemonAsync(1);
            return pokemon is null
                ? HealthCheckResult.Unhealthy("PokeAPI returned no data.")
                : HealthCheckResult.Healthy("PokeAPI is reachable.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy("PokeAPI is unavailable.", exception);
        }
    }
}