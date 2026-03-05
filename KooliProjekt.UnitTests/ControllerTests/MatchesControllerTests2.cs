using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class MatchesControllerTests
    {
        private readonly Mock<IMatchesService> _mockService;
        private readonly MatchesController _controller;

        public MatchesControllerTests()
        {
            _mockService = new Mock<IMatchesService>();
            _controller = new MatchesController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithMatchesIndexModel()
        {
            // Arrange
            var expectedData = new PagedResult<Matches>
            {
                Results = new List<Matches>
                {
                    new Matches 
                    { 
                        Id = 1, 
                        Name = "Match 1", 
                        StartData = "2024-01-01", 
                        EndData = "2024-01-01", 
                        TotalPoints = 10,
                        Team = new Team { Id = 1, Name = "Team A" },
                        Tournament = new Tournament { Id = 1, Name = "Tournament 1", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" }
                    },
                    new Matches 
                    { 
                        Id = 2, 
                        Name = "Match 2", 
                        StartData = "2024-01-02", 
                        EndData = "2024-01-02", 
                        TotalPoints = 20,
                        Team = new Team { Id = 2, Name = "Team B" },
                        Tournament = new Tournament { Id = 1, Name = "Tournament 1", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" }
                    }
                },
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 2
            };

            _mockService.Setup(s => s.List(1, 5, It.IsAny<MatchesSearch>()))
                      .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.Index(1, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<MatchesIndexModel>(viewResult.Model);
            Assert.NotNull(model.Data);
            Assert.Equal(2, model.Data.Results.Count);
            Assert.NotNull(model.Search);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithMatchNameSearchFilter()
        {
            // Arrange
            var search = new MatchesSearch { Name = "Final" };
            var expectedData = new PagedResult<Matches>
            {
                Results = new List<Matches>
                {
                    new Matches 
                    { 
                        Id = 1, 
                        Name = "Final Match", 
                        StartData = "2024-01-01", 
                        EndData = "2024-01-01", 
                        TotalPoints = 10,
                        Team = new Team { Id = 1, Name = "Team A" },
                        Tournament = new Tournament { Id = 1, Name = "Tournament 1", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" }
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
            var model = Assert.IsType<MatchesIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal("Final", model.Search.Name);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithTeamNameSearchFilter()
        {
            // Arrange
            var search = new MatchesSearch { TeamName = "Manchester" };
            var expectedData = new PagedResult<Matches>
            {
                Results = new List<Matches>
                {
                    new Matches 
                    { 
                        Id = 1, 
                        Name = "Match 1", 
                        StartData = "2024-01-01", 
                        EndData = "2024-01-01", 
                        TotalPoints = 10,
                        Team = new Team { Id = 1, Name = "Manchester United" },
                        Tournament = new Tournament { Id = 1, Name = "Tournament 1", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" }
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
            var model = Assert.IsType<MatchesIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal("Manchester", model.Search.TeamName);
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
        public async Task Details_ReturnsNotFound_WhenMatchNotFound()
        {
            // Arrange
            int? id = 1;
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync((Matches)null);

            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithMatch_WhenMatchFound()
        {
            // Arrange
            int? id = 1;
            var match = new Matches 
            { 
                Id = id.Value, 
                Name = "Test Match",
                StartData = "2024-01-01",
                EndData = "2024-01-01",
                TotalPoints = 10,
                Team = new Team { Id = 1, Name = "Team A" },
                Tournament = new Tournament { Id = 1, Name = "Tournament 1", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" }
            };
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync(match);

            // Act
            var result = await _controller.Details(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Details");
            Assert.Equal(match, viewResult.Model);
        }

        [Fact]
        public async Task Create_ReturnsViewResult()
        {
            // Arrange
            _mockService.Setup(s => s.GetTeamsSelectList(null))
                       .ReturnsAsync(new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<Team>(), "Id", "Name"));
            _mockService.Setup(s => s.GetTournamentsSelectList(null))
                       .ReturnsAsync(new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<Tournament>(), "Id", "Name"));

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
        public async Task Delete_ReturnsNotFound_WhenMatchNotFound()
        {
            // Arrange
            int? id = 1;
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync((Matches)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithMatch_WhenMatchFound()
        {
            // Arrange
            int? id = 1;
            var match = new Matches 
            { 
                Id = id.Value, 
                Name = "Test Match",
                StartData = "2024-01-01",
                EndData = "2024-01-01",
                TotalPoints = 10,
                Team = new Team { Id = 1, Name = "Team A" },
                Tournament = new Tournament { Id = 1, Name = "Tournament 1", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" }
            };
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync(match);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Delete");
            Assert.Equal(match, viewResult.Model);
        }

        [Fact]
        public async Task CreatePost_SavesMatchAndRedirects_WhenModelStateIsValid()
        {
            // Arrange
            var match = new Matches { Id = 0, Name = "New Match", StartData = "2024-01-01", EndData = "2024-01-01", TotalPoints = 10, TeamId = 1, TournamentId = 1 };
            _mockService.Setup(s => s.Save(match))
                       .Returns(Task.CompletedTask)
                       .Verifiable();

            // Act
            var result = await _controller.Create(match);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.VerifyAll();
        }

        [Fact]
        public async Task CreatePost_ReturnsViewWithModel_WhenModelStateIsInvalid()
        {
            // Arrange
            var match = new Matches { Id = 0, Name = "", StartData = "", EndData = "", TotalPoints = 0, TeamId = 1, TournamentId = 1 };
            _controller.ModelState.AddModelError("Name", "Name is required");
            _mockService.Setup(s => s.GetTeamsSelectList(It.IsAny<int?>()))
                       .ReturnsAsync(new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<Team>(), "Id", "Name"));
            _mockService.Setup(s => s.GetTournamentsSelectList(It.IsAny<int?>()))
                       .ReturnsAsync(new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<Tournament>(), "Id", "Name"));

            // Act
            var result = await _controller.Create(match);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(match, viewResult.Model);
        }

        [Fact]
        public async Task EditPost_ReturnsNotFound_WhenIdMismatch()
        {
            // Arrange
            int id = 1;
            var match = new Matches { Id = 2, Name = "Test Match", StartData = "2024-01-01", EndData = "2024-01-01", TotalPoints = 10, TeamId = 1, TournamentId = 1 };

            // Act
            var result = await _controller.Edit(id, match);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_SavesMatchAndRedirects_WhenModelStateIsValid()
        {
            // Arrange
            int id = 1;
            var match = new Matches { Id = id, Name = "Updated Match", StartData = "2024-01-01", EndData = "2024-01-01", TotalPoints = 20, TeamId = 1, TournamentId = 1 };
            _mockService.Setup(s => s.Save(match))
                       .Returns(Task.CompletedTask)
                       .Verifiable();

            // Act
            var result = await _controller.Edit(id, match);

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
            var match = new Matches { Id = id, Name = "", StartData = "", EndData = "", TotalPoints = 0, TeamId = 1, TournamentId = 1 };
            _controller.ModelState.AddModelError("Name", "Name is required");
            _mockService.Setup(s => s.GetTeamsSelectList(It.IsAny<int?>()))
                       .ReturnsAsync(new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<Team>(), "Id", "Name"));
            _mockService.Setup(s => s.GetTournamentsSelectList(It.IsAny<int?>()))
                       .ReturnsAsync(new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<Tournament>(), "Id", "Name"));

            // Act
            var result = await _controller.Edit(id, match);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(match, viewResult.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_DeletesMatchAndRedirects()
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
