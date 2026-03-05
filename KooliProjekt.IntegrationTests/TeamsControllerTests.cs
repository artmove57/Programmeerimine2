using System.Threading.Tasks;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class TeamsControllerTests : TestBase
    {
        [Theory]
        [InlineData("/Teams")]
        [InlineData("/Teams/Index")]
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
        [InlineData("/Teams?page=1")]
        [InlineData("/Teams/Index?page=1")]
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
        public async Task Get_IndexWithSearch_ReturnsSuccessAndCorrectContentType()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Teams?Search.Name=Manchester");

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
            var response = await client.GetAsync("/Teams/Details/1");

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
            var response = await client.GetAsync("/Teams/Details/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_Create_ReturnsSuccess()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Teams/Create");

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
            var response = await client.GetAsync("/Teams/Edit/1");

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
            var response = await client.GetAsync("/Teams/Edit/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_Delete_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Teams/Delete/1");

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
            var response = await client.GetAsync("/Teams/Delete/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
