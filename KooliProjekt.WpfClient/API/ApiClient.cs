using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.WpfClient.Models;

namespace KooliProjekt.WpfClient.API
{
    /// <summary>
    /// ApiClient - üldine klass Web API-ga suhtlemiseks
    /// Kasutab HttpClient'i ja töötab JSON formaadis
    /// </summary>
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiClient(string baseUrl = "https://localhost:7136/api/Teams")
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        /// <summary>
        /// GET: Kõik objektid
        /// </summary>
        public async Task<List<Team>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();
                
                var teams = await response.Content.ReadFromJsonAsync<List<Team>>();
                return teams ?? new List<Team>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Viga andmete laadimisel: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// GET: Üks objekt ID järgi
        /// </summary>
        public async Task<Team> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadFromJsonAsync<Team>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Viga objekti laadimisel: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// POST: Loo uus objekt
        /// </summary>
        public async Task<Team> CreateAsync(Team team)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_baseUrl, team);
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadFromJsonAsync<Team>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Viga objekti loomisel: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// PUT: Uuenda olemasolevat objekti
        /// </summary>
        public async Task<bool> UpdateAsync(int id, Team team)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", team);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Viga objekti uuendamisel: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// DELETE: Kustuta objekt
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Viga objekti kustutamisel: {ex.Message}", ex);
            }
        }
    }
}
