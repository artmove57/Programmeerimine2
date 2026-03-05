using KooliProjekt.Data;
using KooliProjekt.Services;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class TeamServiceTests : ServiceTestBase
    {
        private readonly TeamService _service;

        public TeamServiceTests()
        {
            _service = new TeamService(DbContext);
        }

        [Fact]
        public async Task Get_ReturnsTeam_WhenTeamExists()
        {
            // Arrange
            var team = new Team { Name = "Test Team" };
            DbContext.Teams.Add(team);
            DbContext.SaveChanges();

            // Act
            var result = await _service.Get(team.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Team", result.Name);
            Assert.Equal(team.Id, result.Id);
        }

        [Fact]
        public async Task Get_ReturnsNull_WhenTeamDoesNotExist()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _service.Get(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task List_ReturnsAllTeams_WhenNoSearchProvided()
        {
            // Arrange
            DbContext.Teams.AddRange(
                new Team { Name = "Team A" },
                new Team { Name = "Team B" },
                new Team { Name = "Team C" }
            );
            DbContext.SaveChanges();

            // Act
            var result = await _service.List(1, 10, null);

            // Assert
            Assert.Equal(3, result.RowCount);
            Assert.Equal(3, result.Results.Count);
        }

        [Fact]
        public async Task List_FiltersTeamsByName_WhenSearchProvided()
        {
            // Arrange
            DbContext.Teams.AddRange(
                new Team { Name = "Manchester United" },
                new Team { Name = "Manchester City" },
                new Team { Name = "Arsenal" }
            );
            DbContext.SaveChanges();

            var search = new Search.TeamsSearch { Name = "Manchester" };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Equal(2, result.RowCount);
            Assert.All(result.Results, team => Assert.Contains("Manchester", team.Name));
        }

        [Fact]
        public async Task Save_AddsNewTeam_WhenIdIsZero()
        {
            // Arrange
            var newTeam = new Team { Id = 0, Name = "New Team" };

            // Act
            await _service.Save(newTeam);

            // Assert
            var teamInDb = DbContext.Teams.FirstOrDefault(t => t.Name == "New Team");
            Assert.NotNull(teamInDb);
            Assert.True(teamInDb.Id > 0);
        }

        [Fact]
        public async Task Save_UpdatesExistingTeam_WhenIdIsNotZero()
        {
            // Arrange
            var team = new Team { Name = "Original Name" };
            DbContext.Teams.Add(team);
            DbContext.SaveChanges();

            // Act
            team.Name = "Updated Name";
            await _service.Save(team);

            // Assert
            var updatedTeam = DbContext.Teams.Find(team.Id);
            Assert.NotNull(updatedTeam);
            Assert.Equal("Updated Name", updatedTeam.Name);
        }

        [Fact]
        public async Task Delete_RemovesTeam_WhenTeamExists()
        {
            // Arrange
            var team = new Team { Name = "Team to Delete" };
            DbContext.Teams.Add(team);
            DbContext.SaveChanges();
            var teamId = team.Id;

            // Act
            await _service.Delete(teamId);

            // Assert
            var deletedTeam = DbContext.Teams.Find(teamId);
            Assert.Null(deletedTeam);
            Assert.Equal(0, DbContext.Teams.Count());
        }

        [Fact]
        public async Task Delete_DoesNotThrow_WhenTeamDoesNotExist()
        {
            // Arrange
            var id = 999;

            // Act & Assert
            await _service.Delete(id);

            // No exception should be thrown
            Assert.Equal(0, DbContext.Teams.Count());
        }
    }
}
