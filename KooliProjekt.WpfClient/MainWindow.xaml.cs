using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using KooliProjekt.WpfClient.Models;
using KooliProjekt.WpfClient.Services;

namespace KooliProjekt.WpfClient;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly TeamApiService _apiService;
    private Team _currentTeam;
    private bool _isNewTeam;

    public MainWindow()
    {
        InitializeComponent();
        _apiService = new TeamApiService();
    }

    // Akna laadimisel laadi meeskonnad
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadTeamsAsync();
    }

    // Laadi kõik meeskonnad API'st
    private async System.Threading.Tasks.Task LoadTeamsAsync()
    {
        try
        {
            UpdateStatus("Laadin meeskondi...");
            var teams = await _apiService.GetAllTeamsAsync();
            TeamsListBox.ItemsSource = teams;
            UpdateStatus($"Laaditud {teams.Count} meeskonda");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Viga: {ex.Message}");
            MessageBox.Show($"Viga meeskondade laadimisel:\n{ex.Message}", 
                "Viga", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // Meeskonna valimine nimekirjast
    private void TeamsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TeamsListBox.SelectedItem is Team selectedTeam)
        {
            _currentTeam = selectedTeam;
            _isNewTeam = false;
            DisplayTeam(selectedTeam);
            UpdateStatus($"Valitud: {selectedTeam.Name}");
        }
    }

    // Kuva meeskonna andmed vormil
    private void DisplayTeam(Team team)
    {
        IdTextBox.Text = team.Id.ToString();
        NameTextBox.Text = team.Name;
    }

    // Tühjenda vorm
    private void ClearForm()
    {
        IdTextBox.Text = string.Empty;
        NameTextBox.Text = string.Empty;
        TeamsListBox.SelectedItem = null;
    }

    // Uue meeskonna nupp
    private void NewButton_Click(object sender, RoutedEventArgs e)
    {
        _isNewTeam = true;
        _currentTeam = new Team { Id = 0 };
        ClearForm();
        NameTextBox.Focus();
        UpdateStatus("Uue meeskonna loomine...");
    }

    // Salvestamise nupp
    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // Valideeri sisend
        if (string.IsNullOrWhiteSpace(NameTextBox.Text))
        {
            MessageBox.Show("Palun sisesta meeskonna nimi!", 
                "Valideerimise viga", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            if (_isNewTeam)
            {
                // Loo uus meeskond
                var newTeam = new Team
                {
                    Id = 0,
                    Name = NameTextBox.Text.Trim()
                };

                UpdateStatus("Salvestan uut meeskonda...");
                var createdTeam = await _apiService.CreateTeamAsync(newTeam);

                MessageBox.Show($"Meeskond '{createdTeam.Name}' edukalt loodud!", 
                    "Õnnestus", MessageBoxButton.OK, MessageBoxImage.Information);

                UpdateStatus($"Meeskond '{createdTeam.Name}' loodud");
            }
            else
            {
                // Uuenda olemasolevat meeskonda
                if (_currentTeam == null)
                {
                    MessageBox.Show("Palun vali meeskond nimekirjast!", 
                        "Viga", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _currentTeam.Name = NameTextBox.Text.Trim();

                UpdateStatus("Salvestan muudatusi...");
                await _apiService.UpdateTeamAsync(_currentTeam.Id, _currentTeam);

                MessageBox.Show($"Meeskond '{_currentTeam.Name}' edukalt uuendatud!", 
                    "Õnnestus", MessageBoxButton.OK, MessageBoxImage.Information);

                UpdateStatus($"Meeskond '{_currentTeam.Name}' uuendatud");
            }

            // Värskenda nimekirja
            await LoadTeamsAsync();
            ClearForm();
            _isNewTeam = false;
        }
        catch (Exception ex)
        {
            UpdateStatus($"Viga: {ex.Message}");
            MessageBox.Show($"Viga salvestamisel:\n{ex.Message}", 
                "Viga", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // Kustutamise nupp
    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentTeam == null || _isNewTeam)
        {
            MessageBox.Show("Palun vali meeskond nimekirjast!", 
                "Viga", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var result = MessageBox.Show(
            $"Kas oled kindel, et soovid kustutada meeskonna '{_currentTeam.Name}'?",
            "Kinnita kustutamine",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                UpdateStatus($"Kustutan meeskonda '{_currentTeam.Name}'...");
                await _apiService.DeleteTeamAsync(_currentTeam.Id);

                MessageBox.Show($"Meeskond '{_currentTeam.Name}' edukalt kustutatud!", 
                    "Õnnestus", MessageBoxButton.OK, MessageBoxImage.Information);

                UpdateStatus($"Meeskond '{_currentTeam.Name}' kustutatud");

                // Värskenda nimekirja
                await LoadTeamsAsync();
                ClearForm();
                _currentTeam = null;
            }
            catch (Exception ex)
            {
                UpdateStatus($"Viga: {ex.Message}");
                MessageBox.Show($"Viga kustutamisel:\n{ex.Message}", 
                    "Viga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Tühistamise nupp
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        ClearForm();
        _isNewTeam = false;
        _currentTeam = null;
        UpdateStatus("Tühistatud");
    }

    // Värskendamise nupp
    private async void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        await LoadTeamsAsync();
    }

    // Uuenda olekuriba
    private void UpdateStatus(string message)
    {
        StatusTextBlock.Text = $"{DateTime.Now:HH:mm:ss} - {message}";
    }
}
