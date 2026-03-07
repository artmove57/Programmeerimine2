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
        Task<Result<List<Team>>> GetAllAsync();
        Task<Result<Team>> GetByIdAsync(int id);
        Task<Result<Team>> CreateAsync(Team team);
        Task<Result> UpdateAsync(int id, Team team);
        Task<Result> DeleteAsync(int id);
    }
}
