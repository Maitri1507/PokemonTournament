namespace PokemonTournament.Services;

public interface IAlertService
{
    void Raise(string message, Exception exception);
}