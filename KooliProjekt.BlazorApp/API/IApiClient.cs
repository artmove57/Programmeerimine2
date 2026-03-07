using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.BlazorApp.Models;

namespace KooliProjekt.BlazorApp.API
{
    /// <summary>
    /// IApiClient - Interface for API operations
    /// Allows dependency injection and testability
    /// </summary>
    public interface IApiClient
    {
        // Teams
        Task<Result<List<Team>>> GetAllAsync();
        Task<Result<Team>> GetByIdAsync(int id);
        Task<Result<Team>> CreateAsync(Team team);
        Task<Result> UpdateAsync(int id, Team team);
        Task<Result> DeleteAsync(int id);

        // Matches
        Task<Result<List<Match>>> GetAllMatchesAsync();
        Task<Result<Match>> GetMatchByIdAsync(int id);
        Task<Result<Match>> CreateMatchAsync(Match match);
        Task<Result> UpdateMatchAsync(int id, Match match);
        Task<Result> DeleteMatchAsync(int id);

        // Lookup data for dropdowns
        Task<Result<List<Team>>> GetTeamsForDropdownAsync();
        Task<Result<List<Tournament>>> GetTournamentsForDropdownAsync();
    }
}
