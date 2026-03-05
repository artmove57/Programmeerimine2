using Moq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using KooliProjekt.WpfClient.API;
using KooliProjekt.WpfClient.Models;
using KooliProjekt.WpfClient.ViewModels;
using System.Reflection;

namespace KooliProjekt.WpfApp.UnitTests
{
    public class MainWindowViewModelTests
    {
        private MainWindowViewModel CreateViewModel()
        {
            // Create ViewModel - it will initialize with default ApiClient
            // For pure unit tests, we test the logic that doesn't require API calls
            var viewModel = new MainWindowViewModel();

            // Clear any data loaded in constructor to have a clean state
            viewModel.Teams.Clear();

            return viewModel;
        }

        #region Command Tests - Execute and CanExecute

        [Fact]
        public void NewCommand_CanExecute_ReturnsTrue()
        {
            // Arrange
            var viewModel = CreateViewModel();

            // Act
            var canExecute = viewModel.NewCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);
        }

        [Fact]
        public void NewCommand_Execute_CreatesNewTeamAndAddsToCollection()
        {
            // Arrange
            var viewModel = CreateViewModel();
            var initialCount = viewModel.Teams.Count;

            // Act
            viewModel.NewCommand.Execute(null);

            // Assert
            Assert.Equal(initialCount + 1, viewModel.Teams.Count);
            var newTeam = viewModel.Teams[viewModel.Teams.Count - 1];
            Assert.Equal(0, newTeam.Id);
            Assert.Equal(string.Empty, newTeam.Name);
        }

        [Fact]
        public void NewCommand_Execute_SetsSelectedTeam()
        {
            // Arrange
            var viewModel = CreateViewModel();

            // Act
            viewModel.NewCommand.Execute(null);

            // Assert
            Assert.NotNull(viewModel.SelectedTeam);
            Assert.Equal(0, viewModel.SelectedTeam.Id);
        }

        [Fact]
        public void SaveCommand_CanExecute_ReturnsFalse_WhenNoTeamSelected()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.SelectedTeam = null;

            // Act
            var canExecute = viewModel.SaveCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);
        }

        [Fact]
        public void SaveCommand_CanExecute_ReturnsFalse_WhenTeamNameIsEmpty()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.SelectedTeam = new Team { Id = 1, Name = "" };

            // Act
            var canExecute = viewModel.SaveCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);
        }

        [Fact]
        public void SaveCommand_CanExecute_ReturnsFalse_WhenTeamNameIsWhitespace()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.SelectedTeam = new Team { Id = 1, Name = "   " };

            // Act
            var canExecute = viewModel.SaveCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);
        }

        [Fact]
        public void SaveCommand_CanExecute_ReturnsTrue_WhenTeamHasValidName()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.SelectedTeam = new Team { Id = 1, Name = "Test Team" };

            // Act
            var canExecute = viewModel.SaveCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);
        }

        [Fact]
        public void DeleteCommand_CanExecute_ReturnsFalse_WhenNoTeamSelected()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.SelectedTeam = null;

            // Act
            var canExecute = viewModel.DeleteCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);
        }

        [Fact]
        public void DeleteCommand_CanExecute_ReturnsFalse_WhenTeamIdIsZero()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.SelectedTeam = new Team { Id = 0, Name = "New Team" };

            // Act
            var canExecute = viewModel.DeleteCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);
        }

        [Fact]
        public void DeleteCommand_CanExecute_ReturnsTrue_WhenTeamHasValidId()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.SelectedTeam = new Team { Id = 5, Name = "Existing Team" };

            // Act
            var canExecute = viewModel.DeleteCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);
        }

        [Fact]
        public void LoadCommand_CanExecute_ReturnsTrue()
        {
            // Arrange
            var viewModel = CreateViewModel();

            // Act
            var canExecute = viewModel.LoadCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);
        }

        #endregion

        #region SelectedItem Tests

        [Fact]
        public void SelectedTeam_SetValue_UpdatesProperty()
        {
            // Arrange
            var viewModel = CreateViewModel();
            var team = new Team { Id = 1, Name = "Test Team" };

            // Act
            viewModel.SelectedTeam = team;

            // Assert
            Assert.Equal(team, viewModel.SelectedTeam);
        }

        [Fact]
        public void SelectedTeam_SetToNewTeam_UpdatesStatusMessage()
        {
            // Arrange
            var viewModel = CreateViewModel();
            var newTeam = new Team { Id = 0, Name = "New Team" };

            // Act
            viewModel.SelectedTeam = newTeam;

            // Assert
            Assert.Contains("Uue meeskonna loomine", viewModel.StatusMessage);
        }

        [Fact]
        public void SelectedTeam_SetToExistingTeam_UpdatesStatusMessageWithName()
        {
            // Arrange
            var viewModel = CreateViewModel();
            var existingTeam = new Team { Id = 5, Name = "Existing Team" };

            // Act
            viewModel.SelectedTeam = existingTeam;

            // Assert
            Assert.Contains("Existing Team", viewModel.StatusMessage);
        }

        [Fact]
        public void SelectedTeam_SetValue_RaisesCanExecuteChangedForSaveCommand()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.SelectedTeam = null;
            var team = new Team { Id = 1, Name = "Test Team" };
            bool canExecuteBefore = viewModel.SaveCommand.CanExecute(null);

            // Act
            viewModel.SelectedTeam = team;
            bool canExecuteAfter = viewModel.SaveCommand.CanExecute(null);

            // Assert - Should be able to save after selecting team with valid name
            Assert.False(canExecuteBefore); // No team selected initially
            Assert.True(canExecuteAfter);    // Valid team selected
        }

        [Fact]
        public void SelectedTeam_SetValue_RaisesCanExecuteChangedForDeleteCommand()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.SelectedTeam = null;
            var team = new Team { Id = 5, Name = "Test Team" };
            bool canExecuteBefore = viewModel.DeleteCommand.CanExecute(null);

            // Act
            viewModel.SelectedTeam = team;
            bool canExecuteAfter = viewModel.DeleteCommand.CanExecute(null);

            // Assert
            Assert.False(canExecuteBefore); // No team selected initially
            Assert.True(canExecuteAfter);    // Valid team with Id > 0 selected
        }

        [Fact]
        public void SelectedTeam_SetToNull_DisablesCommands()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.SelectedTeam = new Team { Id = 1, Name = "Test Team" };

            // Act
            viewModel.SelectedTeam = null;

            // Assert
            Assert.False(viewModel.SaveCommand.CanExecute(null));
            Assert.False(viewModel.DeleteCommand.CanExecute(null));
        }

        #endregion

        #region ConfirmDelete Predicate Tests (CanExecute)

        [Fact]
        public void DeleteCommand_Predicate_ReturnsFalse_ForNewTeam()
        {
            // Arrange
            var viewModel = CreateViewModel();
            var newTeam = new Team { Id = 0, Name = "New Team" };
            viewModel.SelectedTeam = newTeam;

            // Act & Assert - Predicate should return false for new teams (Id = 0)
            Assert.False(viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public void DeleteCommand_Predicate_ReturnsTrue_ForExistingTeam()
        {
            // Arrange
            var viewModel = CreateViewModel();
            var existingTeam = new Team { Id = 10, Name = "Existing Team" };
            viewModel.SelectedTeam = existingTeam;

            // Act & Assert - Predicate should return true for existing teams (Id > 0)
            Assert.True(viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public void DeleteCommand_Predicate_ReturnsFalse_WhenNoSelection()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.SelectedTeam = null;

            // Act & Assert - Predicate should return false when nothing is selected
            Assert.False(viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public void SaveCommand_Predicate_ChecksNameNotEmpty()
        {
            // Arrange
            var viewModel = CreateViewModel();

            // Test with empty name
            viewModel.SelectedTeam = new Team { Id = 1, Name = "" };
            Assert.False(viewModel.SaveCommand.CanExecute(null));

            // Test with whitespace name
            viewModel.SelectedTeam = new Team { Id = 1, Name = "   " };
            Assert.False(viewModel.SaveCommand.CanExecute(null));

            // Test with null name (requires suppression)
            viewModel.SelectedTeam = new Team { Id = 1, Name = null! };
            Assert.False(viewModel.SaveCommand.CanExecute(null));

            // Test with valid name
            viewModel.SelectedTeam = new Team { Id = 1, Name = "Valid Name" };
            Assert.True(viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void SaveCommand_Predicate_WorksForNewAndExistingTeams()
        {
            // Arrange
            var viewModel = CreateViewModel();

            // Test with new team (Id = 0) with valid name
            viewModel.SelectedTeam = new Team { Id = 0, Name = "New Team" };
            Assert.True(viewModel.SaveCommand.CanExecute(null));

            // Test with existing team (Id > 0) with valid name
            viewModel.SelectedTeam = new Team { Id = 5, Name = "Existing Team" };
            Assert.True(viewModel.SaveCommand.CanExecute(null));
        }

        #endregion

        #region Collection and Property Tests

        [Fact]
        public void Teams_Collection_IsInitialized()
        {
            // Arrange & Act
            var viewModel = CreateViewModel();

            // Assert
            Assert.NotNull(viewModel.Teams);
            Assert.IsType<ObservableCollection<Team>>(viewModel.Teams);
        }

        [Fact]
        public void StatusMessage_IsInitialized()
        {
            // Arrange & Act
            var viewModel = CreateViewModel();

            // Assert
            Assert.NotNull(viewModel.StatusMessage);
        }

        [Fact]
        public void Commands_AreInitialized()
        {
            // Arrange & Act
            var viewModel = CreateViewModel();

            // Assert
            Assert.NotNull(viewModel.LoadCommand);
            Assert.NotNull(viewModel.NewCommand);
            Assert.NotNull(viewModel.SaveCommand);
            Assert.NotNull(viewModel.DeleteCommand);
        }

        #endregion

        #region Multiple Operations Tests

        [Fact]
        public void NewCommand_MultipleCalls_AddsMultipleTeams()
        {
            // Arrange
            var viewModel = CreateViewModel();
            var initialCount = viewModel.Teams.Count;

            // Act
            viewModel.NewCommand.Execute(null);
            viewModel.NewCommand.Execute(null);
            viewModel.NewCommand.Execute(null);

            // Assert
            Assert.Equal(initialCount + 3, viewModel.Teams.Count);
        }

        [Fact]
        public void NewCommand_AfterSelectingExisting_SelectsNewTeam()
        {
            // Arrange
            var viewModel = CreateViewModel();
            var existingTeam = new Team { Id = 5, Name = "Existing" };
            viewModel.Teams.Add(existingTeam);
            viewModel.SelectedTeam = existingTeam;
            var initialCount = viewModel.Teams.Count;

            // Act
            viewModel.NewCommand.Execute(null);

            // Assert
            Assert.Equal(initialCount + 1, viewModel.Teams.Count);
            Assert.NotEqual(existingTeam, viewModel.SelectedTeam);
            Assert.Equal(0, viewModel.SelectedTeam!.Id);
        }

        [Fact]
        public void SelectedTeam_ChangingFromValidToInvalid_UpdatesCommandStates()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.SelectedTeam = new Team { Id = 5, Name = "Valid Team" };
            Assert.True(viewModel.SaveCommand.CanExecute(null));
            Assert.True(viewModel.DeleteCommand.CanExecute(null));

            // Act - Change to team with invalid name
            viewModel.SelectedTeam = new Team { Id = 5, Name = "" };

            // Assert
            Assert.False(viewModel.SaveCommand.CanExecute(null)); // Can't save without name
            Assert.True(viewModel.DeleteCommand.CanExecute(null));  // Can still delete (has Id)
        }

        [Fact]
        public void Teams_CanAddAndRemoveManually()
        {
            // Arrange
            var viewModel = CreateViewModel();
            var team1 = new Team { Id = 1, Name = "Team 1" };
            var team2 = new Team { Id = 2, Name = "Team 2" };

            // Act
            viewModel.Teams.Add(team1);
            viewModel.Teams.Add(team2);
            var countAfterAdd = viewModel.Teams.Count;

            viewModel.Teams.Remove(team1);
            var countAfterRemove = viewModel.Teams.Count;

            // Assert
            Assert.Equal(2, countAfterAdd);
            Assert.Equal(1, countAfterRemove);
            Assert.Contains(team2, viewModel.Teams);
            Assert.DoesNotContain(team1, viewModel.Teams);
        }

        #endregion

        #region Property Change Notification Tests

        [Fact]
        public void SelectedTeam_SetValue_RaisesPropertyChanged()
        {
            // Arrange
            var viewModel = CreateViewModel();
            var team = new Team { Id = 1, Name = "Test Team" };
            bool propertyChangedRaised = false;

            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(viewModel.SelectedTeam))
                    propertyChangedRaised = true;
            };

            // Act
            viewModel.SelectedTeam = team;

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public void StatusMessage_SetValue_RaisesPropertyChanged()
        {
            // Arrange
            var viewModel = CreateViewModel();
            bool propertyChangedRaised = false;

            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(viewModel.StatusMessage))
                    propertyChangedRaised = true;
            };

            // Act
            viewModel.StatusMessage = "Test Message";

            // Assert
            Assert.True(propertyChangedRaised);
        }

        #endregion
    }
}
