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
    /// Kõik meetodid tagastavad Result/Result<T> vigade käsitlemiseks
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
        /// Tagastab Result<List<Team>> - kas õnnestus andmetega või viga
        /// </summary>
        public async Task<Result<List<Team>>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();

                var teams = await response.Content.ReadFromJsonAsync<List<Team>>();

                if (teams == null)
                {
                    return Result<List<Team>>.Failure("API tagastas null väärtuse");
                }

                return Result<List<Team>>.Success(teams);
            }
            catch (HttpRequestException ex)
            {
                return Result<List<Team>>.Failure($"Võrgu viga andmete laadimisel: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result<List<Team>>.Failure($"Viga andmete laadimisel: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// GET: Üks objekt ID järgi
        /// Tagastab Result<Team> - kas õnnestus andmetega või viga
        /// </summary>
        public async Task<Result<Team>> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
                response.EnsureSuccessStatusCode();

                var team = await response.Content.ReadFromJsonAsync<Team>();

                if (team == null)
                {
                    return Result<Team>.Failure($"Meeskonda ID-ga {id} ei leitud");
                }

                return Result<Team>.Success(team);
            }
            catch (HttpRequestException ex)
            {
                return Result<Team>.Failure($"Võrgu viga meeskonna laadimisel: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result<Team>.Failure($"Viga meeskonna laadimisel: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// POST: Loo uus objekt
        /// Tagastab Result<Team> - kas õnnestus loodud objektiga või viga
        /// </summary>
        public async Task<Result<Team>> CreateAsync(Team team)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_baseUrl, team);
                response.EnsureSuccessStatusCode();

                var createdTeam = await response.Content.ReadFromJsonAsync<Team>();

                if (createdTeam == null)
                {
                    return Result<Team>.Failure("API ei tagastanud loodud meeskonda");
                }

                return Result<Team>.Success(createdTeam);
            }
            catch (HttpRequestException ex)
            {
                return Result<Team>.Failure($"Võrgu viga meeskonna loomisel: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result<Team>.Failure($"Viga meeskonna loomisel: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// PUT: Uuenda olemasolevat objekti
        /// Tagastab Result - kas õnnestus või viga
        /// </summary>
        public async Task<Result> UpdateAsync(int id, Team team)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", team);
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (HttpRequestException ex)
            {
                return Result.Failure($"Võrgu viga meeskonna uuendamisel: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Viga meeskonna uuendamisel: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// DELETE: Kustuta objekt
        /// Tagastab Result - kas õnnestus või viga
        /// </summary>
        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (HttpRequestException ex)
            {
                return Result.Failure($"Võrgu viga meeskonna kustutamisel: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Viga meeskonna kustutamisel: {ex.Message}", ex);
            }
        }
    }
}
