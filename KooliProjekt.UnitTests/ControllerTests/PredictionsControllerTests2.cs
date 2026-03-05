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
    public class PredictionsControllerTests2
    {
        private readonly Mock<IPredictionService> _mockService;
        private readonly PredictionsController _controller;

        public PredictionsControllerTests2()
        {
            _mockService = new Mock<IPredictionService>();
            _controller = new PredictionsController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithPredictionsIndexModel()
        {
            // Arrange
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

            _mockService.Setup(s => s.List(1, 5, It.IsAny<PredictionsSearch>()))
                      .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.Index(1, null);

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

            _mockService.Setup(s => s.List(1, 5, search))
                      .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.Index(1, search);

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

            _mockService.Setup(s => s.List(1, 5, search))
                      .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.Index(1, search);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PredictionsIndexModel>(viewResult.Model);
            Assert.Equal(1, model.Data.Results.Count);
            Assert.Equal("john", model.Search.UserEmail);
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
        public async Task Details_ReturnsNotFound_WhenPredictionNotFound()
        {
            // Arrange
            int? id = 1;
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync((Prediction)null);

            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithPrediction_WhenPredictionFound()
        {
            // Arrange
            int? id = 1;
            var prediction = new Prediction 
            { 
                Id = id.Value, 
                Matches = new Matches 
                { 
                    Id = 1, 
                    Name = "Test Match",
                    StartData = "2024-01-01",
                    EndData = "2024-01-01",
                    TotalPoints = 10
                },
                User = new IdentityUser { Id = "user1", Email = "test@example.com" }
            };
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync(prediction);

            // Act
            var result = await _controller.Details(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Details");
            Assert.Equal(prediction, viewResult.Model);
        }

        [Fact]
        public async Task Create_ReturnsViewResult()
        {
            // Arrange
            _mockService.Setup(s => s.GetMatchesSelectList(null))
                       .ReturnsAsync(new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<Matches>(), "Id", "Name"));
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
        public async Task Delete_ReturnsNotFound_WhenPredictionNotFound()
        {
            // Arrange
            int? id = 1;
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync((Prediction)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithPrediction_WhenPredictionFound()
        {
            // Arrange
            int? id = 1;
            var prediction = new Prediction 
            { 
                Id = id.Value, 
                Matches = new Matches 
                { 
                    Id = 1, 
                    Name = "Test Match",
                    StartData = "2024-01-01",
                    EndData = "2024-01-01",
                    TotalPoints = 10
                },
                User = new IdentityUser { Id = "user1", Email = "test@example.com" }
            };
            _mockService.Setup(s => s.Get(id.Value))
                       .ReturnsAsync(prediction);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Delete");
            Assert.Equal(prediction, viewResult.Model);
        }
    }
}
