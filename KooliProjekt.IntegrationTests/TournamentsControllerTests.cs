using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class TournamentsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public TournamentsControllerTests()
        {
            var options = new WebApplicationFactoryClientOptions 
            { 
                AllowAutoRedirect = false 
            };
            _client = Factory.CreateClient(options);
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_success()
        {
            // Arrange

            // Act
            using var response = await _client.GetAsync("/Tournaments");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/Tournaments")]
        [InlineData("/Tournaments/Index")]
        public async Task Get_Index_ReturnsSuccessAndCorrectContentType(string url)
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(url);

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

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_IndexWithNameSearch_ReturnsSuccessAndCorrectContentType()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/Tournaments?Search.Name=League");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_IndexWithDescriptionSearch_ReturnsSuccessAndCorrectContentType()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/Tournaments?Search.Description=English");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/Tournaments/Details")]
        [InlineData("/Tournaments/Details/999")]
        [InlineData("/Tournaments/Delete")]
        [InlineData("/Tournaments/Delete/999")]
        [InlineData("/Tournaments/Edit")]
        [InlineData("/Tournaments/Edit/999")]
        public async Task Should_return_notfound(string url)
        {
            // Arrange

            // Act
            using var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_Details_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var tournament = _context.Tournaments.First();

            // Act
            var response = await _client.GetAsync("/Tournaments/Details/" + tournament.Id);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_Create_ReturnsSuccess()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/Tournaments/Create");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_Edit_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var tournament = _context.Tournaments.First();

            // Act
            var response = await _client.GetAsync("/Tournaments/Edit/" + tournament.Id);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_Delete_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var tournament = _context.Tournaments.First();

            // Act
            var response = await _client.GetAsync("/Tournaments/Delete/" + tournament.Id);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Create_should_save_new_tournament()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "Id", "0" },
                { "Name", "Integration Test Tournament" },
                { "Description", "Test Description" },
                { "StartData", "2024-01-01" },
                { "EndData", "2024-12-31" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Tournaments/Create", content);

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);

            var tournament = _context.Tournaments.FirstOrDefault(t => t.Name == "Integration Test Tournament");
            Assert.NotNull(tournament);
            Assert.NotEqual(0, tournament.Id);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_tournament()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "Id", "0" },
                { "Name", "" },
                { "Description", "" },
                { "StartData", "" },
                { "EndData", "" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Tournaments/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.False(_context.Tournaments.Any(t => t.Name == ""));
        }

        [Fact]
        public async Task Edit_should_update_existing_tournament()
        {
            // Arrange
            var tournament = _context.Tournaments.First();
            var tournamentId = tournament.Id;
            var formValues = new Dictionary<string, string>
            {
                { "Id", tournamentId.ToString() },
                { "Name", "Updated Tournament Name" },
                { "Description", "Updated Description" },
                { "StartData", "2024-01-01" },
                { "EndData", "2024-12-31" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Tournaments/Edit/" + tournamentId, content);

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);
        }

        [Fact]
        public async Task Edit_should_not_update_invalid_tournament()
        {
            // Arrange
            var tournament = _context.Tournaments.First();
            var tournamentId = tournament.Id;
            var formValues = new Dictionary<string, string>
            {
                { "Id", tournamentId.ToString() },
                { "Name", "" },
                { "Description", "" },
                { "StartData", "" },
                { "EndData", "" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Tournaments/Edit/" + tournamentId, content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_should_remove_tournament()
        {
            // Arrange
            var formValues = new Dictionary<string, string>();
            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Tournaments/Delete/10", content);

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);
        }
    }
}
