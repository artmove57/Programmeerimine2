using System.Threading.Tasks;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class PredictionsControllerTests : TestBase
    {
        [Theory]
        [InlineData("/Predictions")]
        [InlineData("/Predictions/Index")]
        public async Task Get_Index_ReturnsSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/Predictions?page=1")]
        [InlineData("/Predictions/Index?page=1")]
        public async Task Get_IndexWithPage_ReturnsSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_IndexWithMatchNameSearch_ReturnsSuccessAndCorrectContentType()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Predictions?Search.MatchName=Match");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_IndexWithUserEmailSearch_ReturnsSuccessAndCorrectContentType()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Predictions?Search.UserEmail=user");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_Details_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Predictions/Details/1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_Details_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Predictions/Details/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_Create_ReturnsSuccess()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Predictions/Create");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_Edit_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Predictions/Edit/1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_Edit_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Predictions/Edit/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_Delete_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Predictions/Delete/1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Predictions/Delete/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
