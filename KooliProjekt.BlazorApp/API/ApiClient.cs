using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.BlazorApp.Models;

namespace KooliProjekt.BlazorApp.API
{
    /// <summary>
    /// ApiClient - Implementation of IApiClient for Blazor WebAssembly
    /// All methods return Result/Result<T> for error handling
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
        /// </summary>
        public async Task<Result<Team>> CreateAsync(Team team)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Teams", team);
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
        /// </summary>
        public async Task<Result> UpdateAsync(int id, Team team)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"Teams/{id}", team);
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
    }
}
