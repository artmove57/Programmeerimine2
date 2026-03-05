using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.Services
{
    public class TournamentServiceTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task List_ReturnsAllTournaments_WhenNoSearchProvided()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new TournamentService(context);

            context.Tournaments.AddRange(
                new Tournament { Name = "Premier League", Description = "English League", StartData = "2024-08-01", EndData = "2025-05-31" },
                new Tournament { Name = "La Liga", Description = "Spanish League", StartData = "2024-08-01", EndData = "2025-05-31" }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await service.List(1, 10, null);

            // Assert
            Assert.Equal(2, result.RowCount);
            Assert.Equal(2, result.Results.Count);
        }

        [Fact]
        public async Task List_FiltersByName_WhenSearchNameProvided()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new TournamentService(context);

            context.Tournaments.AddRange(
                new Tournament { Name = "Premier League", Description = "English League", StartData = "2024-08-01", EndData = "2025-05-31" },
                new Tournament { Name = "Champions League", Description = "European Cup", StartData = "2024-08-01", EndData = "2025-05-31" },
                new Tournament { Name = "World Cup", Description = "International", StartData = "2024-08-01", EndData = "2025-05-31" }
            );
            await context.SaveChangesAsync();

            var search = new TournamentsSearch { Name = "League" };

            // Act
            var result = await service.List(1, 10, search);

            // Assert
            Assert.Equal(2, result.RowCount);
            Assert.All(result.Results, t => Assert.Contains("League", t.Name));
        }

        [Fact]
        public async Task List_FiltersByDescription_WhenSearchDescriptionProvided()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new TournamentService(context);

            context.Tournaments.AddRange(
                new Tournament { Name = "Premier League", Description = "English League", StartData = "2024-08-01", EndData = "2025-05-31" },
                new Tournament { Name = "La Liga", Description = "Spanish League", StartData = "2024-08-01", EndData = "2025-05-31" },
                new Tournament { Name = "World Cup", Description = "International Tournament", StartData = "2024-08-01", EndData = "2025-05-31" }
            );
            await context.SaveChangesAsync();

            var search = new TournamentsSearch { Description = "League" };

            // Act
            var result = await service.List(1, 10, search);

            // Assert
            Assert.Equal(2, result.RowCount);
        }

        [Fact]
        public async Task Get_ReturnsTournament_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new TournamentService(context);

            var tournament = new Tournament 
            { 
                Name = "Test Tournament", 
                Description = "Test", 
                StartData = "2024-08-01", 
                EndData = "2025-05-31" 
            };
            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();

            // Act
            var result = await service.Get(tournament.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Tournament", result.Name);
        }

        [Fact]
        public async Task Save_AddsNewTournament_WhenIdIsZero()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new TournamentService(context);

            var newTournament = new Tournament 
            { 
                Id = 0, 
                Name = "New Tournament", 
                Description = "New", 
                StartData = "2024-08-01", 
                EndData = "2025-05-31" 
            };

            // Act
            await service.Save(newTournament);

            // Assert
            var tournamentInDb = await context.Tournaments.FirstOrDefaultAsync(t => t.Name == "New Tournament");
            Assert.NotNull(tournamentInDb);
            Assert.True(tournamentInDb.Id > 0);
        }

        [Fact]
        public async Task Delete_RemovesTournament_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new TournamentService(context);

            var tournament = new Tournament 
            { 
                Name = "To Delete", 
                Description = "Test", 
                StartData = "2024-08-01", 
                EndData = "2025-05-31" 
            };
            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();
            var tournamentId = tournament.Id;

            // Act
            await service.Delete(tournamentId);

            // Assert
            var deletedTournament = await context.Tournaments.FindAsync(tournamentId);
            Assert.Null(deletedTournament);
        }
    }
}
