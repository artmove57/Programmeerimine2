using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class RankingsControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithRankingsIndexModel()
        {
            // Arrange
            var mockService = new Mock<IRankingService>();
            var expectedData = new PagedResult<Ranking>
            {
                Results = new List<Ranking>
                {
                    new Ranking 
                    { 
                        Id = 1, 
                        TotalPoints = 100,
                        Tournament = new Tournament 
                        { 
                            Id = 1, 
                            Name = "Premier League",
                            Description = "English League",
                            StartData = "2024-01-01",
                            EndData = "2024-12-31"
                        },
                        User = new IdentityUser { Id = "user1", Email = "user1@example.com" }
                    },
                    new Ranking 
                    { 
                        Id = 2, 
                        TotalPoints = 200,
                        Tournament = new Tournament 
                        { 
                            Id = 2, 
                            Name = "La Liga",
                            Description = "Spanish League",
                            StartData = "2024-01-01",
                            EndData = "2024-12-31"
                        },
                        User = new IdentityUser { Id = "user2", Email = "user2@example.com" }
                    }
                },
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 2
            };

            mockService.Setup(s => s.List(1, 5, It.IsAny<RankingsSearch>()))
                      .ReturnsAsync(expectedData);

            var controller = new RankingsController(mockService.Object);

            // Act
            var result = await controller.Index(1, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<RankingsIndexModel>(viewResult.Model);
            Assert.NotNull(model.Data);
            Assert.Equal(2, model.Data.Results.Count);
            Assert.NotNull(model.Search);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithTournamentNameSearchFilter()
        {
            // Arrange
            var mockService = new Mock<IRankingService>();
            var search = new RankingsSearch { TournamentName = "Premier" };
            var expectedData = new PagedResult<Ranking>
            {
                Results = new List<Ranking>
                {
                    new Ranking 
                    { 
                        Id = 1, 
                        TotalPoints = 100,
                        Tournament = new Tournament 
                        { 
                            Id = 1, 
                            Name = "Premier League",
                            Description = "English League",
                            StartData = "2024-01-01",
                            EndData = "2024-12-31"
                        },
                        User = new IdentityUser { Id = "user1", Email = "user1@example.com" }
                    }
                },
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 1
            };

            mockService.Setup(s => s.List(1, 5, search))
                      .ReturnsAsync(expectedData);

            var controller = new RankingsController(mockService.Object);

            // Act
            var result = await controller.Index(1, search);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<RankingsIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal("Premier", model.Search.TournamentName);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithUserEmailSearchFilter()
        {
            // Arrange
            var mockService = new Mock<IRankingService>();
            var search = new RankingsSearch { UserEmail = "john" };
            var expectedData = new PagedResult<Ranking>
            {
                Results = new List<Ranking>
                {
                    new Ranking 
                    { 
                        Id = 1, 
                        TotalPoints = 100,
                        Tournament = new Tournament 
                        { 
                            Id = 1, 
                            Name = "Premier League",
                            Description = "English League",
                            StartData = "2024-01-01",
                            EndData = "2024-12-31"
                        },
                        User = new IdentityUser { Id = "user1", Email = "john@example.com" }
                    }
                },
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 1
            };

            mockService.Setup(s => s.List(1, 5, search))
                      .ReturnsAsync(expectedData);

            var controller = new RankingsController(mockService.Object);

            // Act
            var result = await controller.Index(1, search);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<RankingsIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal("john", model.Search.UserEmail);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithMinPointsSearchFilter()
        {
            // Arrange
            var mockService = new Mock<IRankingService>();
            var search = new RankingsSearch { MinPoints = 150 };
            var expectedData = new PagedResult<Ranking>
            {
                Results = new List<Ranking>
                {
                    new Ranking 
                    { 
                        Id = 1, 
                        TotalPoints = 200,
                        Tournament = new Tournament 
                        { 
                            Id = 1, 
                            Name = "Premier League",
                            Description = "English League",
                            StartData = "2024-01-01",
                            EndData = "2024-12-31"
                        },
                        User = new IdentityUser { Id = "user1", Email = "user1@example.com" }
                    }
                },
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 1
            };

            mockService.Setup(s => s.List(1, 5, search))
                      .ReturnsAsync(expectedData);

            var controller = new RankingsController(mockService.Object);

            // Act
            var result = await controller.Index(1, search);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<RankingsIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal(150, model.Search.MinPoints);
        }
    }
}
