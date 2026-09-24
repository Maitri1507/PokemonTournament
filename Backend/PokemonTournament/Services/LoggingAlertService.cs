namespace PokemonTournament.Services;

public sealed class LoggingAlertService : IAlertService
{
    private readonly ILogger<LoggingAlertService> _logger;

    public LoggingAlertService(ILogger<LoggingAlertService> logger)
    {
        _logger = logger;
    }

    public void Raise(string message, Exception exception)
    {
        _logger.LogError(exception, "ALERT: {AlertMessage}", message);
    }
}