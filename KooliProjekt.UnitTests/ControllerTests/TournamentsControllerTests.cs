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
    public class TournamentsControllerTests
    {
        private readonly Mock<ITournamentService> _mockService;
        private readonly TournamentsController _controller;

        public TournamentsControllerTests()
        {
            _mockService = new Mock<ITournamentService>();
            _controller = new TournamentsController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithTournamentsIndexModel()
        {
            // Arrange
            var expectedData = new PagedResult<Tournament>
            {
                Results = new List<Tournament>
                {
                    new Tournament { Id = 1, Name = "Premier League", Description = "English League", StartData = "2024-08-01", EndData = "2025-05-31" },
                    new Tournament { Id = 2, Name = "La Liga", Description = "Spanish League", StartData = "2024-08-01", EndData = "2025-05-31" }
                },
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 2
            };

            _mockService.Setup(s => s.List(1, 5, It.IsAny<TournamentsSearch>()))
                      .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.Index(1, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<TournamentsIndexModel>(viewResult.Model);
            Assert.NotNull(model.Data);
            Assert.Equal(2, model.Data.Results.Count);
            Assert.NotNull(model.Search);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithNameSearchFilter()
        {
            // Arrange
            var search = new TournamentsSearch { Name = "League" };
            var expectedData = new PagedResult<Tournament>
            {
                Results = new List<Tournament>
                {
                    new Tournament { Id = 1, Name = "Premier League", Description = "English League", StartData = "2024-08-01", EndData = "2025-05-31" }
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
            var model = Assert.IsType<TournamentsIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal("League", model.Search.Name);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithDescriptionSearchFilter()
        {
            // Arrange
            var search = new TournamentsSearch { Description = "English" };
            var expectedData = new PagedResult<Tournament>
            {
                Results = new List<Tournament>
                {
                    new Tournament { Id = 1, Name = "Premier League", Description = "English League", StartData = "2024-08-01", EndData = "2025-05-31" }
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
            var model = Assert.IsType<TournamentsIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal("English", model.Search.Description);
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
        public async Task Details_ReturnsNotFound_WhenTournamentNotFound()
        {
            // Arrange
            int? id = 1;
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync((Tournament)null);

            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithTournament_WhenTournamentFound()
        {
            // Arrange
            int? id = 1;
            var tournament = new Tournament { Id = id.Value, Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync(tournament);

            // Act
            var result = await _controller.Details(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Details");
            Assert.Equal(tournament, viewResult.Model);
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
        public async Task Delete_ReturnsNotFound_WhenTournamentNotFound()
        {
            // Arrange
            int? id = 1;
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync((Tournament)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithTournament_WhenTournamentFound()
        {
            // Arrange
            int? id = 1;
            var tournament = new Tournament { Id = id.Value, Name = "Test Tournament", Description = "Test", StartData = "2024-01-01", EndData = "2024-12-31" };
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync(tournament);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Delete");
            Assert.Equal(tournament, viewResult.Model);
        }
    }
}
