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
        [Fact]
        public async Task Index_ReturnsViewResult_WithTeamsIndexModel()
        {
            // Arrange
            var mockService = new Mock<ITeamService>();
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

            mockService.Setup(s => s.List(1, 5, It.IsAny<TeamsSearch>()))
                      .ReturnsAsync(expectedData);

            var controller = new TeamsController(mockService.Object);

            // Act
            var result = await controller.Index(1, null);

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
            var mockService = new Mock<ITeamService>();
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

            mockService.Setup(s => s.List(1, 5, search))
                      .ReturnsAsync(expectedData);

            var controller = new TeamsController(mockService.Object);

            // Act
            var result = await controller.Index(1, search);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<TeamsIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal("Manchester", model.Search.Name);
        }
    }
}
