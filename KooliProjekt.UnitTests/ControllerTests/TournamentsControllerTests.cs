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
        [Fact]
        public async Task Index_ReturnsViewResult_WithTournamentsIndexModel()
        {
            // Arrange
            var mockService = new Mock<ITournamentService>();
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

            mockService.Setup(s => s.List(1, 5, It.IsAny<TournamentsSearch>()))
                      .ReturnsAsync(expectedData);

            var controller = new TournamentsController(mockService.Object);

            // Act
            var result = await controller.Index(1, null);

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
            var mockService = new Mock<ITournamentService>();
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

            mockService.Setup(s => s.List(1, 5, search))
                      .ReturnsAsync(expectedData);

            var controller = new TournamentsController(mockService.Object);

            // Act
            var result = await controller.Index(1, search);

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
            var mockService = new Mock<ITournamentService>();
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

            mockService.Setup(s => s.List(1, 5, search))
                      .ReturnsAsync(expectedData);

            var controller = new TournamentsController(mockService.Object);

            // Act
            var result = await controller.Index(1, search);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<TournamentsIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal("English", model.Search.Description);
        }
    }
}
