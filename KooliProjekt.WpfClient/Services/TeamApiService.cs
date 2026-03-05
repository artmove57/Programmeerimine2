using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.WpfClient.Models;

namespace KooliProjekt.WpfClient.Services
{
    public class TeamApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7136/api/Teams"; // Muuda pordi number vastavalt oma rakendusele

        public TeamApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        // GET: /api/Teams - Kõik meeskonnad
        public async Task<List<Team>> GetAllTeamsAsync()
        {
            try
            {
                var teams = await _httpClient.GetFromJsonAsync<List<Team>>(BaseUrl);
                return teams ?? new List<Team>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Viga meeskondade laadimisel: {ex.Message}");
            }
        }

        // GET: /api/Teams/5 - Üks meeskond ID järgi
        public async Task<Team> GetTeamByIdAsync(int id)
        {
            try
            {
                var team = await _httpClient.GetFromJsonAsync<Team>($"{BaseUrl}/{id}");
                return team;
            }
            catch (Exception ex)
            {
                throw new Exception($"Viga meeskonna laadimisel: {ex.Message}");
            }
        }

        // POST: /api/Teams - Uue meeskonna loomine
        public async Task<Team> CreateTeamAsync(Team team)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(BaseUrl, team);
                response.EnsureSuccessStatusCode();
                
                var createdTeam = await response.Content.ReadFromJsonAsync<Team>();
                return createdTeam;
            }
            catch (Exception ex)
            {
                throw new Exception($"Viga meeskonna loomisel: {ex.Message}");
            }
        }

        // PUT: /api/Teams/5 - Meeskonna uuendamine
        public async Task UpdateTeamAsync(int id, Team team)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", team);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Viga meeskonna uuendamisel: {ex.Message}");
            }
        }

        // DELETE: /api/Teams/5 - Meeskonna kustutamine
        public async Task DeleteTeamAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Viga meeskonna kustutamisel: {ex.Message}");
            }
        }
    }
}
