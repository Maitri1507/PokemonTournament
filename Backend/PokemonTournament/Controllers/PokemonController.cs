using Microsoft.AspNetCore.Mvc;
using PokemonTournament.Enums;
using PokemonTournament.Models;
using PokemonTournament.Services;

namespace PokemonTournament.Controllers
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonController : ControllerBase
    {
        #region Dependencies

        private readonly ITournamentService _tournamentService;

        public PokemonController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        #endregion

        #region Endpoints

        [HttpGet("tournament/statistics")]
        public async Task<IActionResult> GetTournamentStatistics(
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDirection)
        {
            // verify if sortBy parameter is provided and not empty or whitespace
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return BadRequest(new { error = "sortBy parameter is required" });
            }

            // verify the sortBy parameter is a valid SortOptions enum value
            if (!Enum.TryParse(sortBy, true, out SortOptions sortField) ||
                !Enum.IsDefined(typeof(SortOptions), sortField))
            {
                return BadRequest(new { error = "sortBy parameter is invalid" });
            }

            //verify the sortDirection parameter is a valid SortDirection enum value, if provided
            SortDirection? direction;

            if (string.IsNullOrWhiteSpace(sortDirection))
            {
                direction = SortDirection.Asc;
            }
            else if (Enum.TryParse(sortDirection, true, out SortDirection parsedDirection) &&
                     Enum.IsDefined(typeof(SortDirection), parsedDirection))
            {
                direction = parsedDirection;
            }
            else
            {
                direction = null;
            }

            if (direction is null)
            {
                return BadRequest(new { error = "sortDirection parameter is invalid" });
            }

            // call the tournament service
            var roster = await _tournamentService.GetTournamentResultsAsync(
                sortField,
                direction.Value);

            if (roster == null)
            {
                return StatusCode(503, "PokeAPI is unavailable. Please try again later.");
            }

            // convert the result to DTOs for frontend
            var result = roster.Select(p => new PokemonDTO(p.Id, p.Name, p.Type, p.Wins, p.Losses, p.Ties));

            return Ok(result);
        }

        #endregion
    }
}
