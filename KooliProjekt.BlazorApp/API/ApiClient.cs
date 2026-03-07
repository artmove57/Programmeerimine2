using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using KooliProjekt.BlazorApp.Models;

namespace KooliProjekt.BlazorApp.API
{
    /// <summary>
    /// ApiClient - Implementation of IApiClient for Blazor WebAssembly
    /// All methods return Result/Result<T> for error handling
    /// Save methods check for validation errors from server
    /// </summary>
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// GET: All teams
        /// </summary>
        public async Task<Result<List<Team>>> GetAllAsync()
        {
            try
            {
                var teams = await _httpClient.GetFromJsonAsync<List<Team>>("Teams");

                if (teams == null)
                {
                    return Result<List<Team>>.Failure("API returned null");
                }

                return Result<List<Team>>.Success(teams);
            }
            catch (HttpRequestException ex)
            {
                return Result<List<Team>>.Failure($"Network error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result<List<Team>>.Failure($"Error loading data: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// GET: One team by ID
        /// </summary>
        public async Task<Result<Team>> GetByIdAsync(int id)
        {
            try
            {
                var team = await _httpClient.GetFromJsonAsync<Team>($"Teams/{id}");

                if (team == null)
                {
                    return Result<Team>.Failure($"Team with ID {id} not found");
                }

                return Result<Team>.Success(team);
            }
            catch (HttpRequestException ex)
            {
                return Result<Team>.Failure($"Network error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result<Team>.Failure($"Error loading team: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// POST: Create new team
        /// Checks for validation errors from server (400 Bad Request)
        /// </summary>
        public async Task<Result<Team>> CreateAsync(Team team)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Teams", team);

                // Check for validation errors (400 Bad Request)
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return await HandleValidationErrorAsync<Team>(response);
                }

                response.EnsureSuccessStatusCode();

                var createdTeam = await response.Content.ReadFromJsonAsync<Team>();

                if (createdTeam == null)
                {
                    return Result<Team>.Failure("API did not return created team");
                }

                return Result<Team>.Success(createdTeam);
            }
            catch (HttpRequestException ex)
            {
                return Result<Team>.Failure($"Network error creating team: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result<Team>.Failure($"Error creating team: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// PUT: Update existing team
        /// Checks for validation errors from server (400 Bad Request)
        /// </summary>
        public async Task<Result> UpdateAsync(int id, Team team)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"Teams/{id}", team);

                // Check for validation errors (400 Bad Request)
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return await HandleValidationErrorAsync(response);
                }

                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (HttpRequestException ex)
            {
                return Result.Failure($"Network error updating team: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error updating team: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// DELETE: Delete team
        /// </summary>
        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Teams/{id}");
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (HttpRequestException ex)
            {
                return Result.Failure($"Network error deleting team: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error deleting team: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Handle validation errors from server response
        /// Parses ASP.NET Core validation problem details
        /// </summary>
        private async Task<Result<T>> HandleValidationErrorAsync<T>(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                var problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(content, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (problemDetails?.Errors != null)
                {
                    var errors = new Dictionary<string, List<string>>();
                    foreach (var error in problemDetails.Errors)
                    {
                        errors[error.Key] = error.Value.ToList();
                    }
                    return Result<T>.ValidationFailure(errors);
                }

                return Result<T>.Failure("Validation failed");
            }
            catch
            {
                return Result<T>.Failure("Validation failed");
            }
        }

        /// <summary>
        /// Handle validation errors from server response (non-generic)
        /// </summary>
        private async Task<Result> HandleValidationErrorAsync(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                var problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(content, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (problemDetails?.Errors != null)
                {
                    var errors = new Dictionary<string, List<string>>();
                    foreach (var error in problemDetails.Errors)
                    {
                        errors[error.Key] = error.Value.ToList();
                    }
                    return Result.ValidationFailure(errors);
                }

                return Result.Failure("Validation failed");
            }
            catch
            {
                return Result.Failure("Validation failed");
            }
        }

        #region Matches Operations

        /// <summary>
        /// GET: All matches
        /// </summary>
        public async Task<Result<List<Match>>> GetAllMatchesAsync()
        {
            try
            {
                var matches = await _httpClient.GetFromJsonAsync<List<Match>>("MatchesApi");

                if (matches == null)
                {
                    return Result<List<Match>>.Failure("API returned null");
                }

                return Result<List<Match>>.Success(matches);
            }
            catch (HttpRequestException ex)
            {
                return Result<List<Match>>.Failure($"Network error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result<List<Match>>.Failure($"Error loading matches: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// GET: One match by ID
        /// </summary>
        public async Task<Result<Match>> GetMatchByIdAsync(int id)
        {
            try
            {
                var match = await _httpClient.GetFromJsonAsync<Match>($"MatchesApi/{id}");

                if (match == null)
                {
                    return Result<Match>.Failure($"Match with ID {id} not found");
                }

                return Result<Match>.Success(match);
            }
            catch (HttpRequestException ex)
            {
                return Result<Match>.Failure($"Network error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result<Match>.Failure($"Error loading match: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// POST: Create new match
        /// </summary>
        public async Task<Result<Match>> CreateMatchAsync(Match match)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("MatchesApi", match);

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return await HandleValidationErrorAsync<Match>(response);
                }

                response.EnsureSuccessStatusCode();

                var createdMatch = await response.Content.ReadFromJsonAsync<Match>();

                if (createdMatch == null)
                {
                    return Result<Match>.Failure("API did not return created match");
                }

                return Result<Match>.Success(createdMatch);
            }
            catch (HttpRequestException ex)
            {
                return Result<Match>.Failure($"Network error creating match: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result<Match>.Failure($"Error creating match: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// PUT: Update existing match
        /// </summary>
        public async Task<Result> UpdateMatchAsync(int id, Match match)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"MatchesApi/{id}", match);

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return await HandleValidationErrorAsync(response);
                }

                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (HttpRequestException ex)
            {
                return Result.Failure($"Network error updating match: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error updating match: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// DELETE: Delete match
        /// </summary>
        public async Task<Result> DeleteMatchAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"MatchesApi/{id}");
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (HttpRequestException ex)
            {
                return Result.Failure($"Network error deleting match: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error deleting match: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// GET: Teams for dropdown
        /// </summary>
        public async Task<Result<List<Team>>> GetTeamsForDropdownAsync()
        {
            try
            {
                var teams = await _httpClient.GetFromJsonAsync<List<Team>>("MatchesApi/teams");

                if (teams == null)
                {
                    return Result<List<Team>>.Failure("API returned null");
                }

                return Result<List<Team>>.Success(teams);
            }
            catch (Exception ex)
            {
                return Result<List<Team>>.Failure($"Error loading teams: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// GET: Tournaments for dropdown
        /// </summary>
        public async Task<Result<List<Tournament>>> GetTournamentsForDropdownAsync()
        {
            try
            {
                var tournaments = await _httpClient.GetFromJsonAsync<List<Tournament>>("MatchesApi/tournaments");

                if (tournaments == null)
                {
                    return Result<List<Tournament>>.Failure("API returned null");
                }

                return Result<List<Tournament>>.Success(tournaments);
            }
            catch (Exception ex)
            {
                return Result<List<Tournament>>.Failure($"Error loading tournaments: {ex.Message}", ex);
            }
        }

        #endregion
    }

    /// <summary>
    /// ValidationProblemDetails - matches ASP.NET Core validation response format
    /// </summary>
    internal class ValidationProblemDetails
    {
        public string? Title { get; set; }
        public int Status { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
