using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using KooliProjekt.WinFormsApp.API;
using KooliProjekt.WinFormsApp.Models;

namespace KooliProjekt.WinFormsApp;

public partial class Form1 : Form
{
    private readonly ApiClient _apiClient;
    private List<Team> _teams;
    private Team? _selectedTeam;

    public Form1()
    {
        InitializeComponent();
        _apiClient = new ApiClient();
        _teams = new List<Team>();
    }

    // Form load event - load data from API
    private async void Form1_Load(object sender, EventArgs e)
    {
        await LoadDataAsync();
    }

    // Load all teams from API
    private async Task LoadDataAsync()
    {
        try
        {
            statusLabel.Text = "Loading data...";

            var result = await _apiClient.GetAllAsync();

            if (!result.IsSuccess)
            {
                statusLabel.Text = $"Error: {result.ErrorMessage}";
                MessageBox.Show($"Error loading data:\n{result.ErrorMessage}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _teams = result.Data!;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _teams;

            statusLabel.Text = $"Loaded {_teams.Count} teams - {DateTime.Now:HH:mm:ss}";
        }
        catch (Exception ex)
        {
            statusLabel.Text = $"Error: {ex.Message}";
            MessageBox.Show($"Unexpected error:\n{ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // DataGridView selection changed
    private void DataGridView1_SelectionChanged(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is Team team)
        {
            _selectedTeam = team;
            idTextBox.Text = team.Id.ToString();
            nameTextBox.Text = team.Name;
            statusLabel.Text = $"Selected: {team.Name}";
        }
    }

    // NEW BUTTON: Create new team
    // Hint: Create new Team object with Id=0, clear form, enable editing
    private void NewButton_Click(object sender, EventArgs e)
    {
        _selectedTeam = new Team 
        { 
            Id = 0, 
            Name = string.Empty 
        };

        idTextBox.Text = "0";
        nameTextBox.Text = string.Empty;
        nameTextBox.Focus();

        statusLabel.Text = "Creating new team - enter name and click Save";
    }

    // SAVE BUTTON: Save team (Create or Update)
    // Hint: If Id == 0 -> POST (Create), else PUT (Update)
    private async void SaveButton_Click(object sender, EventArgs e)
    {
        // Validate
        if (string.IsNullOrWhiteSpace(nameTextBox.Text))
        {
            MessageBox.Show("Please enter team name!", 
                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            // Get values from form
            var teamId = int.Parse(idTextBox.Text);
            var teamName = nameTextBox.Text.Trim();

            if (teamId == 0)
            {
                // CREATE - new team
                statusLabel.Text = "Saving new team...";

                var newTeam = new Team 
                { 
                    Id = 0, 
                    Name = teamName 
                };

                var result = await _apiClient.CreateAsync(newTeam);

                if (!result.IsSuccess)
                {
                    statusLabel.Text = $"Error: {result.ErrorMessage}";
                    MessageBox.Show($"Error creating team:\n{result.ErrorMessage}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show($"Team '{result.Data!.Name}' created successfully!", 
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                statusLabel.Text = $"Team '{result.Data.Name}' created";
            }
            else
            {
                // UPDATE - existing team
                statusLabel.Text = "Updating team...";

                var teamToUpdate = new Team 
                { 
                    Id = teamId, 
                    Name = teamName 
                };

                var result = await _apiClient.UpdateAsync(teamId, teamToUpdate);

                if (!result.IsSuccess)
                {
                    statusLabel.Text = $"Error: {result.ErrorMessage}";
                    MessageBox.Show($"Error updating team:\n{result.ErrorMessage}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show($"Team '{teamName}' updated successfully!", 
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                statusLabel.Text = $"Team '{teamName}' updated";
            }

            // Reload data
            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            statusLabel.Text = $"Error: {ex.Message}";
            MessageBox.Show($"Unexpected error:\n{ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // DELETE BUTTON: Delete selected team
    // Hint: Check if team is selected, ask confirmation, call DELETE API
    private async void DeleteButton_Click(object sender, EventArgs e)
    {
        if (_selectedTeam == null || _selectedTeam.Id == 0)
        {
            MessageBox.Show("Please select a team from the list!", 
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Ask for confirmation
        var confirmResult = MessageBox.Show(
            $"Are you sure you want to delete team '{_selectedTeam.Name}'?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmResult != DialogResult.Yes)
            return;

        try
        {
            statusLabel.Text = $"Deleting team '{_selectedTeam.Name}'...";

            var result = await _apiClient.DeleteAsync(_selectedTeam.Id);

            if (!result.IsSuccess)
            {
                statusLabel.Text = $"Error: {result.ErrorMessage}";
                MessageBox.Show($"Error deleting team:\n{result.ErrorMessage}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show($"Team '{_selectedTeam.Name}' deleted successfully!", 
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            statusLabel.Text = $"Team deleted - {DateTime.Now:HH:mm:ss}";

            // Clear form
            idTextBox.Text = string.Empty;
            nameTextBox.Text = string.Empty;
            _selectedTeam = null;

            // Reload data
            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            statusLabel.Text = $"Error: {ex.Message}";
            MessageBox.Show($"Unexpected error:\n{ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Refresh button
    private async void RefreshButton_Click(object sender, EventArgs e)
    {
        await LoadDataAsync();
    }
}
