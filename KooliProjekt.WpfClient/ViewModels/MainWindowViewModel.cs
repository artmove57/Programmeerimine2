using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using KooliProjekt.WpfClient.API;
using KooliProjekt.WpfClient.Base;
using KooliProjekt.WpfClient.Commands;
using KooliProjekt.WpfClient.Models;

namespace KooliProjekt.WpfClient.ViewModels
{
    /// <summary>
    /// MainWindowViewModel - koordineerib MainWindow tööd
    /// Extendib NotifyPropertyChangedBase, et UI saaks muudatustest teada
    /// </summary>
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly ApiClient _apiClient;
        private Team? _selectedTeam;
        private string _statusMessage = "Valmis";

        #region Properties

        /// <summary>
        /// Kõikide meeskondade kollektsioon (seotud DataGrid'iga)
        /// </summary>
        public ObservableCollection<Team> Teams { get; set; }

        /// <summary>
        /// Valitud meeskond (seotud DataGrid'i SelectedItem'iga)
        /// OLULINE: Two-way binding võimaldab muudatusi mõlemas suunas
        /// </summary>
        public Team? SelectedTeam
        {
            get => _selectedTeam;
            set
            {
                if (SetProperty(ref _selectedTeam, value))
                {
                    // Kui valitakse meeskond, uuenda commandide staatust
                    SaveCommand.RaiseCanExecuteChanged();
                    DeleteCommand.RaiseCanExecuteChanged();

                    // Kui valitakse uus meeskond, uuenda olekuriba
                    if (value != null)
                    {
                        StatusMessage = value.Id == 0 
                            ? "Uue meeskonna loomine..." 
                            : $"Valitud: {value.Name}";
                    }
                }
            }
        }

        /// <summary>
        /// Olekuteade (seotud olekuribaga)
        /// </summary>
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command andmete laadimiseks
        /// </summary>
        public RelayCommand LoadCommand { get; set; }

        /// <summary>
        /// Command uue meeskonna loomiseks
        /// OLULINE: Loob uue tühja objekti ja lisab kollektsiooni
        /// </summary>
        public RelayCommand NewCommand { get; set; }

        /// <summary>
        /// Command meeskonna salvestamiseks (Create või Update)
        /// </summary>
        public RelayCommand SaveCommand { get; set; }

        /// <summary>
        /// Command meeskonna kustutamiseks
        /// </summary>
        public RelayCommand DeleteCommand { get; set; }

        #endregion

        public MainWindowViewModel()
        {
            // Initsialiseeri ApiClient
            _apiClient = new ApiClient();

            // Initsialiseeri kollektsioon
            Teams = new ObservableCollection<Team>();

            // Initsialiseeri commandid
            InitializeCommands();

            // Laadi andmed automaatselt
            _ = LoadDataAsync();
        }

        private void InitializeCommands()
        {
            // LoadCommand - laeb andmed API'st
            LoadCommand = new RelayCommand(
                execute: async _ => await LoadDataAsync(),
                canExecute: _ => true
            );

            // NewCommand - loob uue tühja meeskonna
            // OLULINE: Lisab uue objekti kollektsiooni ja valib selle
            NewCommand = new RelayCommand(
                execute: _ => CreateNewTeam(),
                canExecute: _ => true
            );

            // SaveCommand - salvestab meeskonna (uus või uuendatud)
            SaveCommand = new RelayCommand(
                execute: async _ => await SaveTeamAsync(),
                canExecute: _ => SelectedTeam != null && 
                                !string.IsNullOrWhiteSpace(SelectedTeam.Name)
            );

            // DeleteCommand - kustutab valitud meeskonna
            DeleteCommand = new RelayCommand(
                execute: async _ => await DeleteTeamAsync(),
                canExecute: _ => SelectedTeam != null && SelectedTeam.Id > 0
            );
        }

        #region Methods

        /// <summary>
        /// Laadi kõik meeskonnad API'st
        /// </summary>
        private async Task LoadDataAsync()
        {
            try
            {
                StatusMessage = "Laadin andmeid...";

                var teams = await _apiClient.GetAllAsync();

                Teams.Clear();
                foreach (var team in teams)
                {
                    Teams.Add(team);
                }

                StatusMessage = $"Laaditud {teams.Count} meeskonda - {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Viga: {ex.Message}";
                MessageBox.Show($"Viga andmete laadimisel:\n{ex.Message}", 
                    "Viga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Loo uus tühi meeskond
        /// OLULINE: Lisab uue objekti kollektsiooni JA valib selle automaatselt
        /// See võimaldab kasutajal kohe hakata sisestama andmeid
        /// </summary>
        private void CreateNewTeam()
        {
            // Loo uus tühi meeskond
            var newTeam = new Team
            {
                Id = 0,  // 0 tähendab, et see on uus objekt (pole veel salvestatud)
                Name = string.Empty
            };

            // Lisa kollektsiooni
            Teams.Add(newTeam);

            // OLULINE: Vali see automaatselt
            // See käivitab SelectedTeam setter'i, mis uuendab commandide staatust
            SelectedTeam = newTeam;

            StatusMessage = "Uue meeskonna loomine - sisesta nimi ja vajuta Salvesta";
        }

        /// <summary>
        /// Salvesta meeskond (Create või Update)
        /// </summary>
        private async Task SaveTeamAsync()
        {
            if (SelectedTeam == null)
                return;

            // Valideeri
            if (string.IsNullOrWhiteSpace(SelectedTeam.Name))
            {
                MessageBox.Show("Palun sisesta meeskonna nimi!", 
                    "Valideerimise viga", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (SelectedTeam.Id == 0)
                {
                    // CREATE - uus meeskond
                    StatusMessage = "Salvestan uut meeskonda...";
                    var createdTeam = await _apiClient.CreateAsync(SelectedTeam);

                    // Asenda ajutine meeskond loodud meeskonnaga (koos ID'ga)
                    var index = Teams.IndexOf(SelectedTeam);
                    if (index >= 0)
                    {
                        Teams[index] = createdTeam;
                        SelectedTeam = createdTeam;
                    }

                    MessageBox.Show($"Meeskond '{createdTeam.Name}' edukalt loodud!", 
                        "Õnnestus", MessageBoxButton.OK, MessageBoxImage.Information);

                    StatusMessage = $"Meeskond '{createdTeam.Name}' loodud - {DateTime.Now:HH:mm:ss}";
                }
                else
                {
                    // UPDATE - olemasolev meeskond
                    StatusMessage = "Salvestan muudatusi...";
                    await _apiClient.UpdateAsync(SelectedTeam.Id, SelectedTeam);

                    MessageBox.Show($"Meeskond '{SelectedTeam.Name}' edukalt uuendatud!", 
                        "Õnnestus", MessageBoxButton.OK, MessageBoxImage.Information);

                    StatusMessage = $"Meeskond '{SelectedTeam.Name}' uuendatud - {DateTime.Now:HH:mm:ss}";
                }

                // Värskenda nimekirja
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Viga: {ex.Message}";
                MessageBox.Show($"Viga salvestamisel:\n{ex.Message}", 
                    "Viga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Kustuta valitud meeskond
        /// </summary>
        private async Task DeleteTeamAsync()
        {
            if (SelectedTeam == null || SelectedTeam.Id == 0)
                return;

            // Küsi kasutajalt kinnitust
            var result = MessageBox.Show(
                $"Kas oled kindel, et soovid kustutada meeskonna '{SelectedTeam.Name}'?",
                "Kinnita kustutamine",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                StatusMessage = $"Kustutan meeskonda '{SelectedTeam.Name}'...";

                await _apiClient.DeleteAsync(SelectedTeam.Id);

                MessageBox.Show($"Meeskond '{SelectedTeam.Name}' edukalt kustutatud!", 
                    "Õnnestus", MessageBoxButton.OK, MessageBoxImage.Information);

                StatusMessage = $"Meeskond kustutatud - {DateTime.Now:HH:mm:ss}";

                // Eemalda nimekirjast
                Teams.Remove(SelectedTeam);
                SelectedTeam = null;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Viga: {ex.Message}";
                MessageBox.Show($"Viga kustutamisel:\n{ex.Message}", 
                    "Viga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
