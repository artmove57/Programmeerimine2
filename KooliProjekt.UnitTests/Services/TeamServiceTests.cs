using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.Services
{
    public class TeamServiceTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task List_ReturnsAllTeams_WhenNoSearchProvided()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new TeamService(context);

            context.Teams.AddRange(
                new Team { Name = "Team A" },
                new Team { Name = "Team B" },
                new Team { Name = "Team C" }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await service.List(1, 10, null);

            // Assert
            Assert.Equal(3, result.RowCount);
            Assert.Equal(3, result.Results.Count);
        }

        [Fact]
        public async Task List_FiltersTeamsByName_WhenSearchProvided()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new TeamService(context);

            context.Teams.AddRange(
                new Team { Name = "Manchester United" },
                new Team { Name = "Manchester City" },
                new Team { Name = "Arsenal" }
            );
            await context.SaveChangesAsync();

            var search = new TeamsSearch { Name = "Manchester" };

            // Act
            var result = await service.List(1, 10, search);

            // Assert
            Assert.Equal(2, result.RowCount);
            Assert.All(result.Results, team => Assert.Contains("Manchester", team.Name));
        }

        [Fact]
        public async Task Get_ReturnsTeam_WhenTeamExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new TeamService(context);

            var team = new Team { Name = "Test Team" };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            // Act
            var result = await service.Get(team.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Team", result.Name);
        }

        [Fact]
        public async Task Get_ReturnsNull_WhenTeamDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new TeamService(context);

            // Act
            var result = await service.Get(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Save_AddsNewTeam_WhenIdIsZero()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new TeamService(context);

            var newTeam = new Team { Id = 0, Name = "New Team" };

            // Act
            await service.Save(newTeam);

            // Assert
            var teamInDb = await context.Teams.FirstOrDefaultAsync(t => t.Name == "New Team");
            Assert.NotNull(teamInDb);
            Assert.True(teamInDb.Id > 0);
        }

        [Fact]
        public async Task Save_UpdatesExistingTeam_WhenIdIsNotZero()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new TeamService(context);

            var team = new Team { Name = "Original Name" };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            // Act
            team.Name = "Updated Name";
            await service.Save(team);

            // Assert
            var updatedTeam = await context.Teams.FindAsync(team.Id);
            Assert.Equal("Updated Name", updatedTeam.Name);
        }

        [Fact]
        public async Task Delete_RemovesTeam_WhenTeamExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new TeamService(context);

            var team = new Team { Name = "Team to Delete" };
            context.Teams.Add(team);
            await context.SaveChangesAsync();
            var teamId = team.Id;

            // Act
            await service.Delete(teamId);

            // Assert
            var deletedTeam = await context.Teams.FindAsync(teamId);
            Assert.Null(deletedTeam);
        }

        [Fact]
        public async Task Delete_DoesNotThrow_WhenTeamDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new TeamService(context);

            // Act & Assert
            await service.Delete(999); // Should not throw exception
        }
    }
}
