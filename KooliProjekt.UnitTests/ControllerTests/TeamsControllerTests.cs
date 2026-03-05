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
    public class TeamsControllerTests
    {
        private readonly Mock<ITeamService> _mockService;
        private readonly TeamsController _controller;

        public TeamsControllerTests()
        {
            _mockService = new Mock<ITeamService>();
            _controller = new TeamsController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithTeamsIndexModel()
        {
            // Arrange
            var expectedData = new PagedResult<Team>
            {
                Results = new List<Team>
                {
                    new Team { Id = 1, Name = "Team A" },
                    new Team { Id = 2, Name = "Team B" }
                },
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 2
            };

            _mockService.Setup(s => s.List(1, 5, It.IsAny<TeamsSearch>()))
                      .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.Index(1, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<TeamsIndexModel>(viewResult.Model);
            Assert.NotNull(model.Data);
            Assert.Equal(2, model.Data.Results.Count);
            Assert.NotNull(model.Search);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithSearchFilter()
        {
            // Arrange
            var search = new TeamsSearch { Name = "Manchester" };
            var expectedData = new PagedResult<Team>
            {
                Results = new List<Team>
                {
                    new Team { Id = 1, Name = "Manchester United" }
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
            var model = Assert.IsType<TeamsIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal("Manchester", model.Search.Name);
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
        public async Task Details_ReturnsNotFound_WhenTeamNotFound()
        {
            // Arrange
            int? id = 1;
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync((Team)null);

            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithTeam_WhenTeamFound()
        {
            // Arrange
            int? id = 1;
            var team = new Team { Id = id.Value, Name = "Test Team" };
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync(team);

            // Act
            var result = await _controller.Details(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Details");
            Assert.Equal(team, viewResult.Model);
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Create");
        }

        [Fact]
        public async Task CreatePost_SavesTeamAndRedirects_WhenModelStateIsValid()
        {
            // Arrange
            var team = new Team { Id = 0, Name = "New Team" };
            _mockService.Setup(s => s.Save(team))
                       .Returns(Task.CompletedTask)
                       .Verifiable();

            // Act
            var result = await _controller.Create(team);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.VerifyAll();
        }

        [Fact]
        public async Task CreatePost_ReturnsViewWithModel_WhenModelStateIsInvalid()
        {
            // Arrange
            var team = new Team { Id = 0, Name = "" };
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(team);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(team, viewResult.Model);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Edit(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenTeamNotFound()
        {
            // Arrange
            int? id = 1;
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync((Team)null);

            // Act
            var result = await _controller.Edit(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WithTeam_WhenTeamFound()
        {
            // Arrange
            int? id = 1;
            var team = new Team { Id = id.Value, Name = "Test Team" };
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync(team);

            // Act
            var result = await _controller.Edit(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(team, viewResult.Model);
        }

        [Fact]
        public async Task EditPost_ReturnsNotFound_WhenIdMismatch()
        {
            // Arrange
            int id = 1;
            var team = new Team { Id = 2, Name = "Test Team" };

            // Act
            var result = await _controller.Edit(id, team);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_SavesTeamAndRedirects_WhenModelStateIsValid()
        {
            // Arrange
            int id = 1;
            var team = new Team { Id = id, Name = "Updated Team" };
            _mockService.Setup(s => s.Save(team))
                       .Returns(Task.CompletedTask)
                       .Verifiable();

            // Act
            var result = await _controller.Edit(id, team);

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
            var team = new Team { Id = id, Name = "" };
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Edit(id, team);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(team, viewResult.Model);
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
        public async Task Delete_ReturnsNotFound_WhenTeamNotFound()
        {
            // Arrange
            int? id = 1;
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync((Team)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithTeam_WhenTeamFound()
        {
            // Arrange
            int? id = 1;
            var team = new Team { Id = id.Value, Name = "Test Team" };
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync(team);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Delete");
            Assert.Equal(team, viewResult.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_DeletesTeamAndRedirects()
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
