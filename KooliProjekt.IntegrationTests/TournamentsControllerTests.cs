using System.Threading.Tasks;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class TournamentsControllerTests : TestBase
    {
        [Theory]
        [InlineData("/Tournaments")]
        [InlineData("/Tournaments/Index")]
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
        [InlineData("/Tournaments?page=1")]
        [InlineData("/Tournaments/Index?page=1")]
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
        public async Task Get_IndexWithNameSearch_ReturnsSuccessAndCorrectContentType()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Tournaments?Search.Name=League");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_IndexWithDescriptionSearch_ReturnsSuccessAndCorrectContentType()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Tournaments?Search.Description=English");

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
            var response = await client.GetAsync("/Tournaments/Details/1");

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
            var response = await client.GetAsync("/Tournaments/Details/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_Create_ReturnsSuccess()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Tournaments/Create");

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
            var response = await client.GetAsync("/Tournaments/Edit/1");

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
            var response = await client.GetAsync("/Tournaments/Edit/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_Delete_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Tournaments/Delete/1");

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
            var response = await client.GetAsync("/Tournaments/Delete/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
