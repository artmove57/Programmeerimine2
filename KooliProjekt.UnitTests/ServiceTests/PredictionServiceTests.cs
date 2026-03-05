using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class PredictionServiceTests : ServiceTestBase
    {
        private readonly PredictionService _service;

        public PredictionServiceTests()
        {
            _service = new PredictionService(DbContext);
        }

        [Fact]
        public async Task Get_ReturnsPrediction_WhenPredictionExists()
        {
            // Arrange
            var user = new IdentityUser { Id = "user1", UserName = "test@example.com", Email = "test@example.com" };
            var team = new Team { Name = "Test Team" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.Add(user);
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

            var prediction = new Prediction { MatchesId = match.Id, UserId = user.Id };
            DbContext.Predictions.Add(prediction);
            DbContext.SaveChanges();

            // Act
            var result = await _service.Get(prediction.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(prediction.Id, result.Id);
            Assert.NotNull(result.Matches);
            Assert.NotNull(result.User);
        }

        [Fact]
        public async Task Get_ReturnsNull_WhenPredictionDoesNotExist()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _service.Get(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task List_ReturnsAllPredictions_WhenNoSearchProvided()
        {
            // Arrange
            var user = new IdentityUser { Id = "user1", UserName = "test@example.com", Email = "test@example.com" };
            var team = new Team { Name = "Test Team" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.Add(user);
            DbContext.Teams.Add(team);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            var match1 = new Matches { Name = "Match 1", StartData = "2024-01-01", EndData = "2024-01-01", TotalPoints = 10, TeamId = team.Id, TournamentId = tournament.Id };
            var match2 = new Matches { Name = "Match 2", StartData = "2024-01-02", EndData = "2024-01-02", TotalPoints = 20, TeamId = team.Id, TournamentId = tournament.Id };
            DbContext.Matches.AddRange(match1, match2);
            DbContext.SaveChanges();

            DbContext.Predictions.AddRange(
                new Prediction { MatchesId = match1.Id, UserId = user.Id },
                new Prediction { MatchesId = match2.Id, UserId = user.Id }
            );
            DbContext.SaveChanges();

            // Act
            var result = await _service.List(1, 10, null);

            // Assert
            Assert.Equal(2, result.RowCount);
            Assert.Equal(2, result.Results.Count);
            Assert.All(result.Results, p => Assert.NotNull(p.Matches));
            Assert.All(result.Results, p => Assert.NotNull(p.User));
        }

        [Fact]
        public async Task List_FiltersByMatchName_WhenSearchMatchNameProvided()
        {
            // Arrange
            var user = new IdentityUser { Id = "user1", UserName = "test@example.com", Email = "test@example.com" };
            var team = new Team { Name = "Test Team" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.Add(user);
            DbContext.Teams.Add(team);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            var match1 = new Matches { Name = "Final Match", StartData = "2024-01-01", EndData = "2024-01-01", TotalPoints = 10, TeamId = team.Id, TournamentId = tournament.Id };
            var match2 = new Matches { Name = "Regular Match", StartData = "2024-01-02", EndData = "2024-01-02", TotalPoints = 20, TeamId = team.Id, TournamentId = tournament.Id };
            DbContext.Matches.AddRange(match1, match2);
            DbContext.SaveChanges();

            DbContext.Predictions.AddRange(
                new Prediction { MatchesId = match1.Id, UserId = user.Id },
                new Prediction { MatchesId = match2.Id, UserId = user.Id }
            );
            DbContext.SaveChanges();

            var search = new Search.PredictionsSearch { MatchName = "Final" };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Equal(1, result.RowCount);
            Assert.Contains("Final", result.Results.First().Matches.Name);
        }

        [Fact]
        public async Task List_FiltersByUserEmail_WhenSearchUserEmailProvided()
        {
            // Arrange
            var user1 = new IdentityUser { Id = "user1", UserName = "john@example.com", Email = "john@example.com" };
            var user2 = new IdentityUser { Id = "user2", UserName = "jane@example.com", Email = "jane@example.com" };
            var team = new Team { Name = "Test Team" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.AddRange(user1, user2);
            DbContext.Teams.Add(team);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            var match = new Matches { Name = "Test Match", StartData = "2024-01-01", EndData = "2024-01-01", TotalPoints = 10, TeamId = team.Id, TournamentId = tournament.Id };
            DbContext.Matches.Add(match);
            DbContext.SaveChanges();

            DbContext.Predictions.AddRange(
                new Prediction { MatchesId = match.Id, UserId = user1.Id },
                new Prediction { MatchesId = match.Id, UserId = user2.Id }
            );
            DbContext.SaveChanges();

            var search = new Search.PredictionsSearch { UserEmail = "john" };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Equal(1, result.RowCount);
            Assert.Contains("john", result.Results.First().User.Email);
        }

        [Fact]
        public async Task Save_AddsNewPrediction_WhenIdIsZero()
        {
            // Arrange
            var user = new IdentityUser { Id = "user1", UserName = "test@example.com", Email = "test@example.com" };
            var team = new Team { Name = "Test Team" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.Add(user);
            DbContext.Teams.Add(team);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            var match = new Matches { Name = "Test Match", StartData = "2024-01-01", EndData = "2024-01-01", TotalPoints = 10, TeamId = team.Id, TournamentId = tournament.Id };
            DbContext.Matches.Add(match);
            DbContext.SaveChanges();

            var newPrediction = new Prediction { Id = 0, MatchesId = match.Id, UserId = user.Id };

            // Act
            await _service.Save(newPrediction);

            // Assert
            var predictionInDb = DbContext.Predictions.FirstOrDefault(p => p.MatchesId == match.Id && p.UserId == user.Id);
            Assert.NotNull(predictionInDb);
            Assert.True(predictionInDb.Id > 0);
        }

        [Fact]
        public async Task Save_UpdatesExistingPrediction_WhenIdIsNotZero()
        {
            // Arrange
            var user1 = new IdentityUser { Id = "user1", UserName = "test@example.com", Email = "test@example.com" };
            var user2 = new IdentityUser { Id = "user2", UserName = "test2@example.com", Email = "test2@example.com" };
            var team = new Team { Name = "Test Team" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.AddRange(user1, user2);
            DbContext.Teams.Add(team);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            var match = new Matches { Name = "Test Match", StartData = "2024-01-01", EndData = "2024-01-01", TotalPoints = 10, TeamId = team.Id, TournamentId = tournament.Id };
            DbContext.Matches.Add(match);
            DbContext.SaveChanges();

            var prediction = new Prediction { MatchesId = match.Id, UserId = user1.Id };
            DbContext.Predictions.Add(prediction);
            DbContext.SaveChanges();

            // Act
            prediction.UserId = user2.Id;
            await _service.Save(prediction);

            // Assert
            var updatedPrediction = DbContext.Predictions.Find(prediction.Id);
            Assert.NotNull(updatedPrediction);
            Assert.Equal(user2.Id, updatedPrediction.UserId);
        }

        [Fact]
        public async Task Delete_RemovesPrediction_WhenPredictionExists()
        {
            // Arrange
            var user = new IdentityUser { Id = "user1", UserName = "test@example.com", Email = "test@example.com" };
            var team = new Team { Name = "Test Team" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.Add(user);
            DbContext.Teams.Add(team);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            var match = new Matches { Name = "Test Match", StartData = "2024-01-01", EndData = "2024-01-01", TotalPoints = 10, TeamId = team.Id, TournamentId = tournament.Id };
            DbContext.Matches.Add(match);
            DbContext.SaveChanges();

            var prediction = new Prediction { MatchesId = match.Id, UserId = user.Id };
            DbContext.Predictions.Add(prediction);
            DbContext.SaveChanges();
            var predictionId = prediction.Id;

            // Act
            await _service.Delete(predictionId);

            // Assert
            var deletedPrediction = DbContext.Predictions.Find(predictionId);
            Assert.Null(deletedPrediction);
            Assert.Equal(0, DbContext.Predictions.Count());
        }

        [Fact]
        public async Task Delete_DoesNotThrow_WhenPredictionDoesNotExist()
        {
            // Arrange
            var id = 999;

            // Act & Assert
            await _service.Delete(id);

            // No exception should be thrown
            Assert.Equal(0, DbContext.Predictions.Count());
        }
    }
}
