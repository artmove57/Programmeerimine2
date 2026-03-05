using KooliProjekt.Data;
using KooliProjekt.Services;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class TournamentServiceTests : ServiceTestBase
    {
        private readonly TournamentService _service;

        public TournamentServiceTests()
        {
            _service = new TournamentService(DbContext);
        }

        [Fact]
        public async Task Get_ReturnsTournament_WhenTournamentExists()
        {
            // Arrange
            var tournament = new Tournament 
            { 
                Name = "Test Tournament", 
                Description = "Test Description", 
                StartData = "2024-01-01", 
                EndData = "2024-12-31" 
            };
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            // Act
            var result = await _service.Get(tournament.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Tournament", result.Name);
            Assert.Equal(tournament.Id, result.Id);
        }

        [Fact]
        public async Task Get_ReturnsNull_WhenTournamentDoesNotExist()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _service.Get(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task List_ReturnsAllTournaments_WhenNoSearchProvided()
        {
            // Arrange
            DbContext.Tournaments.AddRange(
                new Tournament { Name = "Premier League", Description = "English League", StartData = "2024-08-01", EndData = "2025-05-31" },
                new Tournament { Name = "La Liga", Description = "Spanish League", StartData = "2024-08-01", EndData = "2025-05-31" }
            );
            DbContext.SaveChanges();

            // Act
            var result = await _service.List(1, 10, null);

            // Assert
            Assert.Equal(2, result.RowCount);
            Assert.Equal(2, result.Results.Count);
        }

        [Fact]
        public async Task List_FiltersByName_WhenSearchNameProvided()
        {
            // Arrange
            DbContext.Tournaments.AddRange(
                new Tournament { Name = "Premier League", Description = "English League", StartData = "2024-08-01", EndData = "2025-05-31" },
                new Tournament { Name = "Champions League", Description = "European Cup", StartData = "2024-08-01", EndData = "2025-05-31" },
                new Tournament { Name = "World Cup", Description = "International", StartData = "2024-08-01", EndData = "2025-05-31" }
            );
            DbContext.SaveChanges();

            var search = new Search.TournamentsSearch { Name = "League" };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Equal(2, result.RowCount);
            Assert.All(result.Results, t => Assert.Contains("League", t.Name));
        }

        [Fact]
        public async Task List_FiltersByDescription_WhenSearchDescriptionProvided()
        {
            // Arrange
            DbContext.Tournaments.AddRange(
                new Tournament { Name = "Premier League", Description = "English League", StartData = "2024-08-01", EndData = "2025-05-31" },
                new Tournament { Name = "La Liga", Description = "Spanish League", StartData = "2024-08-01", EndData = "2025-05-31" },
                new Tournament { Name = "World Cup", Description = "International Tournament", StartData = "2024-08-01", EndData = "2025-05-31" }
            );
            DbContext.SaveChanges();

            var search = new Search.TournamentsSearch { Description = "League" };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Equal(2, result.RowCount);
        }

        [Fact]
        public async Task Save_AddsNewTournament_WhenIdIsZero()
        {
            // Arrange
            var newTournament = new Tournament 
            { 
                Id = 0, 
                Name = "New Tournament", 
                Description = "New Description", 
                StartData = "2024-08-01", 
                EndData = "2025-05-31" 
            };

            // Act
            await _service.Save(newTournament);

            // Assert
            var tournamentInDb = DbContext.Tournaments.FirstOrDefault(t => t.Name == "New Tournament");
            Assert.NotNull(tournamentInDb);
            Assert.True(tournamentInDb.Id > 0);
        }

        [Fact]
        public async Task Save_UpdatesExistingTournament_WhenIdIsNotZero()
        {
            // Arrange
            var tournament = new Tournament 
            { 
                Name = "Original Tournament", 
                Description = "Original Description", 
                StartData = "2024-08-01", 
                EndData = "2025-05-31" 
            };
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            // Act
            tournament.Name = "Updated Tournament";
            tournament.Description = "Updated Description";
            await _service.Save(tournament);

            // Assert
            var updatedTournament = DbContext.Tournaments.Find(tournament.Id);
            Assert.NotNull(updatedTournament);
            Assert.Equal("Updated Tournament", updatedTournament.Name);
            Assert.Equal("Updated Description", updatedTournament.Description);
        }

        [Fact]
        public async Task Delete_RemovesTournament_WhenTournamentExists()
        {
            // Arrange
            var tournament = new Tournament 
            { 
                Name = "To Delete", 
                Description = "Test", 
                StartData = "2024-08-01", 
                EndData = "2025-05-31" 
            };
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();
            var tournamentId = tournament.Id;

            // Act
            await _service.Delete(tournamentId);

            // Assert
            var deletedTournament = DbContext.Tournaments.Find(tournamentId);
            Assert.Null(deletedTournament);
            Assert.Equal(0, DbContext.Tournaments.Count());
        }

        [Fact]
        public async Task Delete_DoesNotThrow_WhenTournamentDoesNotExist()
        {
            // Arrange
            var id = 999;

            // Act & Assert
            await _service.Delete(id);

            // No exception should be thrown
            Assert.Equal(0, DbContext.Tournaments.Count());
        }
    }
}
