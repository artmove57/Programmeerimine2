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
        [Fact]
        public async Task Index_ReturnsViewResult_WithMatchesIndexModel()
        {
            // Arrange
            var mockService = new Mock<IMatchesService>();
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

            mockService.Setup(s => s.List(1, 5, It.IsAny<MatchesSearch>()))
                      .ReturnsAsync(expectedData);

            var controller = new MatchesController(mockService.Object);

            // Act
            var result = await controller.Index(1, null);

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
            var mockService = new Mock<IMatchesService>();
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

            mockService.Setup(s => s.List(1, 5, search))
                      .ReturnsAsync(expectedData);

            var controller = new MatchesController(mockService.Object);

            // Act
            var result = await controller.Index(1, search);

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
            var mockService = new Mock<IMatchesService>();
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

            mockService.Setup(s => s.List(1, 5, search))
                      .ReturnsAsync(expectedData);

            var controller = new MatchesController(mockService.Object);

            // Act
            var result = await controller.Index(1, search);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<MatchesIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal("Manchester", model.Search.TeamName);
        }
    }
}
