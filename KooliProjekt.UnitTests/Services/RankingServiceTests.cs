using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.Services
{
    public class RankingServiceTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task List_ReturnsAllRankings_WhenNoSearchProvided()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new RankingService(context);

            var user = new IdentityUser { Id = "user1", Email = "test@example.com" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-08-01", EndData = "2025-05-31" };
            
            context.Users.Add(user);
            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();

            context.Rankings.AddRange(
                new Ranking { TotalPoints = 100, TournamentId = tournament.Id, UserId = user.Id },
                new Ranking { TotalPoints = 200, TournamentId = tournament.Id, UserId = user.Id }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await service.List(1, 10, null);

            // Assert
            Assert.Equal(2, result.RowCount);
            Assert.Equal(2, result.Results.Count);
        }

        [Fact]
        public async Task List_FiltersByMinPoints_WhenMinPointsProvided()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new RankingService(context);

            var user = new IdentityUser { Id = "user1", Email = "test@example.com" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-08-01", EndData = "2025-05-31" };
            
            context.Users.Add(user);
            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();

            context.Rankings.AddRange(
                new Ranking { TotalPoints = 50, TournamentId = tournament.Id, UserId = user.Id },
                new Ranking { TotalPoints = 150, TournamentId = tournament.Id, UserId = user.Id },
                new Ranking { TotalPoints = 250, TournamentId = tournament.Id, UserId = user.Id }
            );
            await context.SaveChangesAsync();

            var search = new RankingsSearch { MinPoints = 100 };

            // Act
            var result = await service.List(1, 10, search);

            // Assert
            Assert.Equal(2, result.RowCount);
            Assert.All(result.Results, r => Assert.True(r.TotalPoints >= 100));
        }

        [Fact]
        public async Task List_FiltersByTournamentName_WhenProvided()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new RankingService(context);

            var user = new IdentityUser { Id = "user1", Email = "test@example.com" };
            var tournament1 = new Tournament { Name = "Premier League", Description = "Test", StartData = "2024-08-01", EndData = "2025-05-31" };
            var tournament2 = new Tournament { Name = "Champions League", Description = "Test", StartData = "2024-08-01", EndData = "2025-05-31" };
            
            context.Users.Add(user);
            context.Tournaments.AddRange(tournament1, tournament2);
            await context.SaveChangesAsync();

            context.Rankings.AddRange(
                new Ranking { TotalPoints = 100, TournamentId = tournament1.Id, UserId = user.Id },
                new Ranking { TotalPoints = 200, TournamentId = tournament2.Id, UserId = user.Id }
            );
            await context.SaveChangesAsync();

            var search = new RankingsSearch { TournamentName = "Premier" };

            // Act
            var result = await service.List(1, 10, search);

            // Assert
            Assert.Equal(1, result.RowCount);
            Assert.Contains("Premier", result.Results.First().Tournament.Name);
        }

        [Fact]
        public async Task List_FiltersByUserEmail_WhenProvided()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new RankingService(context);

            var user1 = new IdentityUser { Id = "user1", Email = "john@example.com" };
            var user2 = new IdentityUser { Id = "user2", Email = "jane@example.com" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-08-01", EndData = "2025-05-31" };
            
            context.Users.AddRange(user1, user2);
            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();

            context.Rankings.AddRange(
                new Ranking { TotalPoints = 100, TournamentId = tournament.Id, UserId = user1.Id },
                new Ranking { TotalPoints = 200, TournamentId = tournament.Id, UserId = user2.Id }
            );
            await context.SaveChangesAsync();

            var search = new RankingsSearch { UserEmail = "john" };

            // Act
            var result = await service.List(1, 10, search);

            // Assert
            Assert.Equal(1, result.RowCount);
            Assert.Contains("john", result.Results.First().User.Email);
        }

        [Fact]
        public async Task Get_ReturnsRanking_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new RankingService(context);

            var user = new IdentityUser { Id = "user1", Email = "test@example.com" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-08-01", EndData = "2025-05-31" };
            
            context.Users.Add(user);
            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();

            var ranking = new Ranking { TotalPoints = 100, TournamentId = tournament.Id, UserId = user.Id };
            context.Rankings.Add(ranking);
            await context.SaveChangesAsync();

            // Act
            var result = await service.Get(ranking.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.TotalPoints);
        }

        [Fact]
        public async Task Save_AddsNewRanking_WhenIdIsZero()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new RankingService(context);

            var user = new IdentityUser { Id = "user1", Email = "test@example.com" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-08-01", EndData = "2025-05-31" };
            
            context.Users.Add(user);
            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();

            var newRanking = new Ranking { Id = 0, TotalPoints = 150, TournamentId = tournament.Id, UserId = user.Id };

            // Act
            await service.Save(newRanking);

            // Assert
            var rankingInDb = await context.Rankings.FirstOrDefaultAsync(r => r.TotalPoints == 150);
            Assert.NotNull(rankingInDb);
            Assert.True(rankingInDb.Id > 0);
        }

        [Fact]
        public async Task Delete_RemovesRanking_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new RankingService(context);

            var user = new IdentityUser { Id = "user1", Email = "test@example.com" };
            var tournament = new Tournament { Name = "Test Tournament", Description = "Test", StartData = "2024-08-01", EndData = "2025-05-31" };
            
            context.Users.Add(user);
            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();

            var ranking = new Ranking { TotalPoints = 100, TournamentId = tournament.Id, UserId = user.Id };
            context.Rankings.Add(ranking);
            await context.SaveChangesAsync();
            var rankingId = ranking.Id;

            // Act
            await service.Delete(rankingId);

            // Assert
            var deletedRanking = await context.Rankings.FindAsync(rankingId);
            Assert.Null(deletedRanking);
        }
    }
}
