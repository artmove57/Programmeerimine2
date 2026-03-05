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
        private readonly Mock<IRankingService> _mockService;
        private readonly RankingsController _controller;

        public RankingsControllerTests()
        {
            _mockService = new Mock<IRankingService>();
            _controller = new RankingsController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithRankingsIndexModel()
        {
            // Arrange
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

            _mockService.Setup(s => s.List(1, 5, It.IsAny<RankingsSearch>()))
                      .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.Index(1, null);

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

            _mockService.Setup(s => s.List(1, 5, search))
                      .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.Index(1, search);

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

            _mockService.Setup(s => s.List(1, 5, search))
                      .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.Index(1, search);

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

            _mockService.Setup(s => s.List(1, 5, search))
                      .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.Index(1, search);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<RankingsIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal(150, model.Search.MinPoints);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenRankingNotFound()
        {
            // Arrange
            int? id = 1;
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync((Ranking)null);

            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithRanking_WhenRankingFound()
        {
            // Arrange
            int? id = 1;
            var ranking = new Ranking 
            { 
                Id = id.Value, 
                TotalPoints = 100,
                Tournament = new Tournament 
                { 
                    Id = 1, 
                    Name = "Test Tournament",
                    Description = "Test",
                    StartData = "2024-01-01",
                    EndData = "2024-12-31"
                },
                User = new IdentityUser { Id = "user1", Email = "test@example.com" }
            };
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync(ranking);

            // Act
            var result = await _controller.Details(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Details");
            Assert.Equal(ranking, viewResult.Model);
        }

        [Fact]
        public async Task Create_ReturnsViewResult()
        {
            // Arrange
            _mockService.Setup(s => s.GetTournamentsSelectList(null))
                       .ReturnsAsync(new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<Tournament>(), "Id", "Name"));
            _mockService.Setup(s => s.GetUsersSelectList(null))
                       .ReturnsAsync(new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<IdentityUser>(), "Id", "Email"));

            // Act
            var result = await _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Create");
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenRankingNotFound()
        {
            // Arrange
            int? id = 1;
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync((Ranking)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithRanking_WhenRankingFound()
        {
            // Arrange
            int? id = 1;
            var ranking = new Ranking 
            { 
                Id = id.Value, 
                TotalPoints = 100,
                Tournament = new Tournament 
                { 
                    Id = 1, 
                    Name = "Test Tournament",
                    Description = "Test",
                    StartData = "2024-01-01",
                    EndData = "2024-12-31"
                },
                User = new IdentityUser { Id = "user1", Email = "test@example.com" }
            };
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync(ranking);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Delete");
            Assert.Equal(ranking, viewResult.Model);
        }

        [Fact]
        public async Task CreatePost_SavesRankingAndRedirects_WhenModelStateIsValid()
        {
            // Arrange
            var ranking = new Ranking { Id = 0, TotalPoints = 100, TournamentId = 1, UserId = "user1" };
            _mockService.Setup(s => s.Save(ranking))
                       .Returns(Task.CompletedTask)
                       .Verifiable();

            // Act
            var result = await _controller.Create(ranking);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.VerifyAll();
        }

        [Fact]
        public async Task CreatePost_ReturnsViewWithModel_WhenModelStateIsInvalid()
        {
            // Arrange
            var ranking = new Ranking { Id = 0, TotalPoints = 0, TournamentId = 0, UserId = "" };
            _controller.ModelState.AddModelError("TournamentId", "Tournament is required");
            _mockService.Setup(s => s.GetTournamentsSelectList(It.IsAny<int?>()))
                       .ReturnsAsync(new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<Tournament>(), "Id", "Name"));
            _mockService.Setup(s => s.GetUsersSelectList(It.IsAny<string>()))
                       .ReturnsAsync(new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<IdentityUser>(), "Id", "Email"));

            // Act
            var result = await _controller.Create(ranking);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(ranking, viewResult.Model);
        }

        [Fact]
        public async Task EditPost_ReturnsNotFound_WhenIdMismatch()
        {
            // Arrange
            int id = 1;
            var ranking = new Ranking { Id = 2, TotalPoints = 100, TournamentId = 1, UserId = "user1" };

            // Act
            var result = await _controller.Edit(id, ranking);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_SavesRankingAndRedirects_WhenModelStateIsValid()
        {
            // Arrange
            int id = 1;
            var ranking = new Ranking { Id = id, TotalPoints = 200, TournamentId = 1, UserId = "user1" };
            _mockService.Setup(s => s.Save(ranking))
                       .Returns(Task.CompletedTask)
                       .Verifiable();

            // Act
            var result = await _controller.Edit(id, ranking);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.VerifyAll();
        }

        [Fact]
        public async Task EditPost_ReturnsViewWithModel_WhenModelStateIsInvalid()
        {
            // Arrange
            int id = 1;
            var ranking = new Ranking { Id = id, TotalPoints = 0, TournamentId = 0, UserId = "" };
            _controller.ModelState.AddModelError("TournamentId", "Tournament is required");
            _mockService.Setup(s => s.GetTournamentsSelectList(It.IsAny<int?>()))
                       .ReturnsAsync(new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<Tournament>(), "Id", "Name"));
            _mockService.Setup(s => s.GetUsersSelectList(It.IsAny<string>()))
                       .ReturnsAsync(new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<IdentityUser>(), "Id", "Email"));

            // Act
            var result = await _controller.Edit(id, ranking);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(ranking, viewResult.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_DeletesRankingAndRedirects()
        {
            // Arrange
            int id = 1;
            _mockService.Setup(s => s.Delete(id))
                       .Returns(Task.CompletedTask)
                       .Verifiable();

            // Act
            var result = await _controller.DeleteConfirmed(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.VerifyAll();
        }
    }
}
