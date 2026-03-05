using KooliProjekt.Data;
using KooliProjekt.Services;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class MatchesServiceTests : ServiceTestBase
    {
        private readonly MatchesService _service;

        public MatchesServiceTests()
        {
            _service = new MatchesService(DbContext);
        }

        [Fact]
        public async Task Get_ReturnsMatch_WhenMatchExists()
        {
            // Arrange
            var team = new Team { Name = "Test Team" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Teams.Add(team);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            var match = new Matches 
            { 
                Name = "Test Match", 
                StartData = "2024-01-01", 
                EndData = "2024-01-01", 
                TotalPoints = 10,
                TeamId = team.Id,
                TournamentId = tournament.Id
            };
            DbContext.Matches.Add(match);
            DbContext.SaveChanges();

            // Act
            var result = await _service.Get(match.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Match", result.Name);
            Assert.Equal(match.Id, result.Id);
            Assert.NotNull(result.Team);
            Assert.NotNull(result.Tournament);
        }

        [Fact]
        public async Task Get_ReturnsNull_WhenMatchDoesNotExist()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _service.Get(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task List_ReturnsAllMatches_WhenNoSearchProvided()
        {
            // Arrange
            var team = new Team { Name = "Test Team" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Teams.Add(team);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            DbContext.Matches.AddRange(
                new Matches { Name = "Match 1", StartData = "2024-01-01", EndData = "2024-01-01", TotalPoints = 10, TeamId = team.Id, TournamentId = tournament.Id },
                new Matches { Name = "Match 2", StartData = "2024-01-02", EndData = "2024-01-02", TotalPoints = 20, TeamId = team.Id, TournamentId = tournament.Id }
            );
            DbContext.SaveChanges();

            // Act
            var result = await _service.List(1, 10, null);

            // Assert
            Assert.Equal(2, result.RowCount);
            Assert.Equal(2, result.Results.Count);
            Assert.All(result.Results, m => Assert.NotNull(m.Team));
            Assert.All(result.Results, m => Assert.NotNull(m.Tournament));
        }

        [Fact]
        public async Task List_FiltersByName_WhenSearchNameProvided()
        {
            // Arrange
            var team = new Team { Name = "Test Team" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Teams.Add(team);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            DbContext.Matches.AddRange(
                new Matches { Name = "Final Match", StartData = "2024-01-01", EndData = "2024-01-01", TotalPoints = 10, TeamId = team.Id, TournamentId = tournament.Id },
                new Matches { Name = "Semi Final", StartData = "2024-01-02", EndData = "2024-01-02", TotalPoints = 20, TeamId = team.Id, TournamentId = tournament.Id },
                new Matches { Name = "Quarter", StartData = "2024-01-03", EndData = "2024-01-03", TotalPoints = 15, TeamId = team.Id, TournamentId = tournament.Id }
            );
            DbContext.SaveChanges();

            var search = new Search.MatchesSearch { Name = "Final" };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Equal(2, result.RowCount);
            Assert.All(result.Results, m => Assert.Contains("Final", m.Name));
        }

        [Fact]
        public async Task List_FiltersByTeamName_WhenSearchTeamNameProvided()
        {
            // Arrange
            var team1 = new Team { Name = "Manchester United" };
            var team2 = new Team { Name = "Arsenal" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Teams.AddRange(team1, team2);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            DbContext.Matches.AddRange(
                new Matches { Name = "Match 1", StartData = "2024-01-01", EndData = "2024-01-01", TotalPoints = 10, TeamId = team1.Id, TournamentId = tournament.Id },
                new Matches { Name = "Match 2", StartData = "2024-01-02", EndData = "2024-01-02", TotalPoints = 20, TeamId = team2.Id, TournamentId = tournament.Id }
            );
            DbContext.SaveChanges();

            var search = new Search.MatchesSearch { TeamName = "Manchester" };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Equal(1, result.RowCount);
            Assert.Contains("Manchester", result.Results.First().Team.Name);
        }

        [Fact]
        public async Task Save_AddsNewMatch_WhenIdIsZero()
        {
            // Arrange
            var team = new Team { Name = "Test Team" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Teams.Add(team);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            var newMatch = new Matches 
            { 
                Id = 0, 
                Name = "New Match", 
                StartData = "2024-01-01", 
                EndData = "2024-01-01", 
                TotalPoints = 10,
                TeamId = team.Id,
                TournamentId = tournament.Id
            };

            // Act
            await _service.Save(newMatch);

            // Assert
            var matchInDb = DbContext.Matches.FirstOrDefault(m => m.Name == "New Match");
            Assert.NotNull(matchInDb);
            Assert.True(matchInDb.Id > 0);
        }

        [Fact]
        public async Task Save_UpdatesExistingMatch_WhenIdIsNotZero()
        {
            // Arrange
            var team = new Team { Name = "Test Team" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Teams.Add(team);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            var match = new Matches 
            { 
                Name = "Original Match", 
                StartData = "2024-01-01", 
                EndData = "2024-01-01", 
                TotalPoints = 10,
                TeamId = team.Id,
                TournamentId = tournament.Id
            };
            DbContext.Matches.Add(match);
            DbContext.SaveChanges();

            // Act
            match.Name = "Updated Match";
            match.TotalPoints = 20;
            await _service.Save(match);

            // Assert
            var updatedMatch = DbContext.Matches.Find(match.Id);
            Assert.NotNull(updatedMatch);
            Assert.Equal("Updated Match", updatedMatch.Name);
            Assert.Equal(20, updatedMatch.TotalPoints);
        }

        [Fact]
        public async Task Delete_RemovesMatch_WhenMatchExists()
        {
            // Arrange
            var team = new Team { Name = "Test Team" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Teams.Add(team);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            var match = new Matches 
            { 
                Name = "To Delete", 
                StartData = "2024-01-01", 
                EndData = "2024-01-01", 
                TotalPoints = 10,
                TeamId = team.Id,
                TournamentId = tournament.Id
            };
            DbContext.Matches.Add(match);
            DbContext.SaveChanges();
            var matchId = match.Id;

            // Act
            await _service.Delete(matchId);

            // Assert
            var deletedMatch = DbContext.Matches.Find(matchId);
            Assert.Null(deletedMatch);
            Assert.Equal(0, DbContext.Matches.Count());
        }

        [Fact]
        public async Task Delete_DoesNotThrow_WhenMatchDoesNotExist()
        {
            // Arrange
            var id = 999;

            // Act & Assert
            await _service.Delete(id);

            // No exception should be thrown
            Assert.Equal(0, DbContext.Matches.Count());
        }
    }
}
