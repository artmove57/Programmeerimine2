using System;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.WinFormsApp.API;
using KooliProjekt.WinFormsApp.Models;
using KooliProjekt.WinFormsApp.Views;

namespace KooliProjekt.WinFormsApp.Presenters
{
    /// <summary>
    /// TeamPresenter - coordinates View and Model (API)
    /// Contains all business logic
    /// </summary>
    public class TeamPresenter
    {
        private readonly ITeamView _view;
        private readonly ApiClient _apiClient;

        public TeamPresenter(ITeamView view, ApiClient apiClient)
        {
            _view = view;
            _apiClient = apiClient;

            // Subscribe to View events
            _view.LoadRequested += OnLoadRequested;
            _view.NewRequested += OnNewRequested;
            _view.SaveRequested += OnSaveRequested;
            _view.DeleteRequested += OnDeleteRequested;
        }

        /// <summary>
        /// Load all teams from API
        /// </summary>
        private async void OnLoadRequested(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        /// <summary>
        /// Create new team
        /// </summary>
        private void OnNewRequested(object? sender, EventArgs e)
        {
            _view.TeamId = 0;
            _view.TeamName = string.Empty;
            _view.SelectedTeam = null;
            _view.StatusMessage = "Creating new team - enter name and click Save";
        }

        /// <summary>
        /// Save team (Create or Update)
        /// </summary>
        private async void OnSaveRequested(object? sender, EventArgs e)
        {
            await SaveTeamAsync();
        }

        /// <summary>
        /// Delete selected team
        /// </summary>
        private async void OnDeleteRequested(object? sender, EventArgs e)
        {
            await DeleteTeamAsync();
        }

        /// <summary>
        /// Load all teams from API
        /// </summary>
        public async Task LoadDataAsync()
        {
            try
            {
                _view.StatusMessage = "Loading data...";

                var result = await _apiClient.GetAllAsync();

                if (!result.IsSuccess)
                {
                    _view.StatusMessage = $"Error: {result.ErrorMessage}";
                    _view.ShowError($"Error loading data:\n{result.ErrorMessage}");
                    return;
                }

                _view.Teams = result.Data!;
                _view.StatusMessage = $"Loaded {result.Data!.Count} teams - {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                _view.StatusMessage = $"Error: {ex.Message}";
                _view.ShowError($"Unexpected error:\n{ex.Message}");
            }
        }

        /// <summary>
        /// Save team (Create or Update based on ID)
        /// </summary>
        private async Task SaveTeamAsync()
        {
            // Validate
            if (string.IsNullOrWhiteSpace(_view.TeamName))
            {
                _view.ShowError("Please enter team name!");
                return;
            }

            try
            {
                var teamId = _view.TeamId;
                var teamName = _view.TeamName.Trim();

                if (teamId == 0)
                {
                    // CREATE - new team
                    _view.StatusMessage = "Saving new team...";

                    var newTeam = new Team
                    {
                        Id = 0,
                        Name = teamName
                    };

                    var result = await _apiClient.CreateAsync(newTeam);

                    if (!result.IsSuccess)
                    {
                        _view.StatusMessage = $"Error: {result.ErrorMessage}";
                        _view.ShowError($"Error creating team:\n{result.ErrorMessage}");
                        return;
                    }

                    _view.ShowSuccess($"Team '{result.Data!.Name}' created successfully!");
                    _view.StatusMessage = $"Team '{result.Data.Name}' created";
                }
                else
                {
                    // UPDATE - existing team
                    _view.StatusMessage = "Updating team...";

                    var teamToUpdate = new Team
                    {
                        Id = teamId,
                        Name = teamName
                    };

                    var result = await _apiClient.UpdateAsync(teamId, teamToUpdate);

                    if (!result.IsSuccess)
                    {
                        _view.StatusMessage = $"Error: {result.ErrorMessage}";
                        _view.ShowError($"Error updating team:\n{result.ErrorMessage}");
                        return;
                    }

                    _view.ShowSuccess($"Team '{teamName}' updated successfully!");
                    _view.StatusMessage = $"Team '{teamName}' updated";
                }

                // Reload data
                await LoadDataAsync();
                _view.ClearForm();
            }
            catch (Exception ex)
            {
                _view.StatusMessage = $"Error: {ex.Message}";
                _view.ShowError($"Unexpected error:\n{ex.Message}");
            }
        }

        /// <summary>
        /// Delete selected team
        /// </summary>
        private async Task DeleteTeamAsync()
        {
            var selectedTeam = _view.SelectedTeam;

            if (selectedTeam == null || selectedTeam.Id == 0)
            {
                _view.ShowError("Please select a team from the list!");
                return;
            }

            try
            {
                _view.StatusMessage = $"Deleting team '{selectedTeam.Name}'...";

                var result = await _apiClient.DeleteAsync(selectedTeam.Id);

                if (!result.IsSuccess)
                {
                    _view.StatusMessage = $"Error: {result.ErrorMessage}";
                    _view.ShowError($"Error deleting team:\n{result.ErrorMessage}");
                    return;
                }

                _view.ShowSuccess($"Team '{selectedTeam.Name}' deleted successfully!");
                _view.StatusMessage = $"Team deleted - {DateTime.Now:HH:mm:ss}";

                // Reload data
                await LoadDataAsync();
                _view.ClearForm();
            }
            catch (Exception ex)
            {
                _view.StatusMessage = $"Error: {ex.Message}";
                _view.ShowError($"Unexpected error:\n{ex.Message}");
            }
        }
    }
}
