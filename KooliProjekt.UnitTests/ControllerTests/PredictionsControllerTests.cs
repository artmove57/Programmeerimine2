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
    public class PredictionsControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithPredictionsIndexModel()
        {
            // Arrange
            var mockService = new Mock<IPredictionService>();
            var expectedData = new PagedResult<Prediction>
            {
                Results = new List<Prediction>
                {
                    new Prediction 
                    { 
                        Id = 1, 
                        Matches = new Matches 
                        { 
                            Id = 1, 
                            Name = "Match 1",
                            StartData = "2024-01-01",
                            EndData = "2024-01-01",
                            TotalPoints = 10
                        },
                        User = new IdentityUser { Id = "user1", Email = "user1@example.com" }
                    },
                    new Prediction 
                    { 
                        Id = 2, 
                        Matches = new Matches 
                        { 
                            Id = 2, 
                            Name = "Match 2",
                            StartData = "2024-01-02",
                            EndData = "2024-01-02",
                            TotalPoints = 20
                        },
                        User = new IdentityUser { Id = "user2", Email = "user2@example.com" }
                    }
                },
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 2
            };

            mockService.Setup(s => s.List(1, 5, It.IsAny<PredictionsSearch>()))
                      .ReturnsAsync(expectedData);

            var controller = new PredictionsController(mockService.Object);

            // Act
            var result = await controller.Index(1, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PredictionsIndexModel>(viewResult.Model);
            Assert.NotNull(model.Data);
            Assert.Equal(2, model.Data.Results.Count);
            Assert.NotNull(model.Search);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithMatchNameSearchFilter()
        {
            // Arrange
            var mockService = new Mock<IPredictionService>();
            var search = new PredictionsSearch { MatchName = "Final" };
            var expectedData = new PagedResult<Prediction>
            {
                Results = new List<Prediction>
                {
                    new Prediction 
                    { 
                        Id = 1, 
                        Matches = new Matches 
                        { 
                            Id = 1, 
                            Name = "Final Match",
                            StartData = "2024-01-01",
                            EndData = "2024-01-01",
                            TotalPoints = 10
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

            var controller = new PredictionsController(mockService.Object);

            // Act
            var result = await controller.Index(1, search);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PredictionsIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal("Final", model.Search.MatchName);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithUserEmailSearchFilter()
        {
            // Arrange
            var mockService = new Mock<IPredictionService>();
            var search = new PredictionsSearch { UserEmail = "john" };
            var expectedData = new PagedResult<Prediction>
            {
                Results = new List<Prediction>
                {
                    new Prediction 
                    { 
                        Id = 1, 
                        Matches = new Matches 
                        { 
                            Id = 1, 
                            Name = "Match 1",
                            StartData = "2024-01-01",
                            EndData = "2024-01-01",
                            TotalPoints = 10
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

            var controller = new PredictionsController(mockService.Object);

            // Act
            var result = await controller.Index(1, search);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PredictionsIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal("john", model.Search.UserEmail);
        }
    }
}
