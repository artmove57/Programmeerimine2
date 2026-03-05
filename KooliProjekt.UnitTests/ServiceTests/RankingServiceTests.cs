using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class RankingServiceTests : ServiceTestBase
    {
        private readonly RankingService _service;

        public RankingServiceTests()
        {
            _service = new RankingService(DbContext);
        }

        [Fact]
        public async Task Get_ReturnsRanking_WhenRankingExists()
        {
            // Arrange
            var user = new IdentityUser { Id = "user1", UserName = "test@example.com", Email = "test@example.com" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.Add(user);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            var ranking = new Ranking { TotalPoints = 100, TournamentId = tournament.Id, UserId = user.Id };
            DbContext.Rankings.Add(ranking);
            DbContext.SaveChanges();

            // Act
            var result = await _service.Get(ranking.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.TotalPoints);
            Assert.NotNull(result.Tournament);
            Assert.NotNull(result.User);
        }

        [Fact]
        public async Task Get_ReturnsNull_WhenRankingDoesNotExist()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _service.Get(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task List_ReturnsAllRankings_WhenNoSearchProvided()
        {
            // Arrange
            var user = new IdentityUser { Id = "user1", UserName = "test@example.com", Email = "test@example.com" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.Add(user);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            DbContext.Rankings.AddRange(
                new Ranking { TotalPoints = 100, TournamentId = tournament.Id, UserId = user.Id },
                new Ranking { TotalPoints = 200, TournamentId = tournament.Id, UserId = user.Id }
            );
            DbContext.SaveChanges();

            // Act
            var result = await _service.List(1, 10, null);

            // Assert
            Assert.Equal(2, result.RowCount);
            Assert.Equal(2, result.Results.Count);
            Assert.All(result.Results, r => Assert.NotNull(r.Tournament));
            Assert.All(result.Results, r => Assert.NotNull(r.User));
        }

        [Fact]
        public async Task List_FiltersByMinPoints_WhenMinPointsProvided()
        {
            // Arrange
            var user = new IdentityUser { Id = "user1", UserName = "test@example.com", Email = "test@example.com" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.Add(user);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            DbContext.Rankings.AddRange(
                new Ranking { TotalPoints = 50, TournamentId = tournament.Id, UserId = user.Id },
                new Ranking { TotalPoints = 150, TournamentId = tournament.Id, UserId = user.Id },
                new Ranking { TotalPoints = 250, TournamentId = tournament.Id, UserId = user.Id }
            );
            DbContext.SaveChanges();

            var search = new Search.RankingsSearch { MinPoints = 100 };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Equal(2, result.RowCount);
            Assert.All(result.Results, r => Assert.True(r.TotalPoints >= 100));
        }

        [Fact]
        public async Task List_FiltersByTournamentName_WhenTournamentNameProvided()
        {
            // Arrange
            var user = new IdentityUser { Id = "user1", UserName = "test@example.com", Email = "test@example.com" };
            var tournament1 = new Tournament { Name = "Premier League", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            var tournament2 = new Tournament { Name = "Champions League", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.Add(user);
            DbContext.Tournaments.AddRange(tournament1, tournament2);
            DbContext.SaveChanges();

            DbContext.Rankings.AddRange(
                new Ranking { TotalPoints = 100, TournamentId = tournament1.Id, UserId = user.Id },
                new Ranking { TotalPoints = 200, TournamentId = tournament2.Id, UserId = user.Id }
            );
            DbContext.SaveChanges();

            var search = new Search.RankingsSearch { TournamentName = "Premier" };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Equal(1, result.RowCount);
            Assert.Contains("Premier", result.Results.First().Tournament.Name);
        }

        [Fact]
        public async Task List_FiltersByUserEmail_WhenUserEmailProvided()
        {
            // Arrange
            var user1 = new IdentityUser { Id = "user1", UserName = "john@example.com", Email = "john@example.com" };
            var user2 = new IdentityUser { Id = "user2", UserName = "jane@example.com", Email = "jane@example.com" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.AddRange(user1, user2);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            DbContext.Rankings.AddRange(
                new Ranking { TotalPoints = 100, TournamentId = tournament.Id, UserId = user1.Id },
                new Ranking { TotalPoints = 200, TournamentId = tournament.Id, UserId = user2.Id }
            );
            DbContext.SaveChanges();

            var search = new Search.RankingsSearch { UserEmail = "john" };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Equal(1, result.RowCount);
            Assert.Contains("john", result.Results.First().User.Email);
        }

        [Fact]
        public async Task Save_AddsNewRanking_WhenIdIsZero()
        {
            // Arrange
            var user = new IdentityUser { Id = "user1", UserName = "test@example.com", Email = "test@example.com" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.Add(user);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            var newRanking = new Ranking { Id = 0, TotalPoints = 150, TournamentId = tournament.Id, UserId = user.Id };

            // Act
            await _service.Save(newRanking);

            // Assert
            var rankingInDb = DbContext.Rankings.FirstOrDefault(r => r.TotalPoints == 150);
            Assert.NotNull(rankingInDb);
            Assert.True(rankingInDb.Id > 0);
        }

        [Fact]
        public async Task Save_UpdatesExistingRanking_WhenIdIsNotZero()
        {
            // Arrange
            var user = new IdentityUser { Id = "user1", UserName = "test@example.com", Email = "test@example.com" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.Add(user);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            var ranking = new Ranking { TotalPoints = 100, TournamentId = tournament.Id, UserId = user.Id };
            DbContext.Rankings.Add(ranking);
            DbContext.SaveChanges();

            // Act
            ranking.TotalPoints = 200;
            await _service.Save(ranking);

            // Assert
            var updatedRanking = DbContext.Rankings.Find(ranking.Id);
            Assert.NotNull(updatedRanking);
            Assert.Equal(200, updatedRanking.TotalPoints);
        }

        [Fact]
        public async Task Delete_RemovesRanking_WhenRankingExists()
        {
            // Arrange
            var user = new IdentityUser { Id = "user1", UserName = "test@example.com", Email = "test@example.com" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            DbContext.Users.Add(user);
            DbContext.Tournaments.Add(tournament);
            DbContext.SaveChanges();

            var ranking = new Ranking { TotalPoints = 100, TournamentId = tournament.Id, UserId = user.Id };
            DbContext.Rankings.Add(ranking);
            DbContext.SaveChanges();
            var rankingId = ranking.Id;

            // Act
            await _service.Delete(rankingId);

            // Assert
            var deletedRanking = DbContext.Rankings.Find(rankingId);
            Assert.Null(deletedRanking);
            Assert.Equal(0, DbContext.Rankings.Count());
        }

        [Fact]
        public async Task Delete_DoesNotThrow_WhenRankingDoesNotExist()
        {
            // Arrange
            var id = 999;

            // Act & Assert
            await _service.Delete(id);

            // No exception should be thrown
            Assert.Equal(0, DbContext.Rankings.Count());
        }
    }
}
