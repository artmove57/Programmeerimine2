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
    public class MatchesControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public MatchesControllerTests()
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
            using var response = await _client.GetAsync("/Matches");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/Matches")]
        [InlineData("/Matches/Index")]
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
        [InlineData("/Matches/Details")]
        [InlineData("/Matches/Details/999")]
        [InlineData("/Matches/Delete")]
        [InlineData("/Matches/Delete/999")]
        [InlineData("/Matches/Edit")]
        [InlineData("/Matches/Edit/999")]
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
            var match = _context.Matches.First();

            // Act
            var response = await _client.GetAsync("/Matches/Details/" + match.Id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_Create_ReturnsSuccess()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/Matches/Create");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_Edit_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var match = _context.Matches.First();

            // Act
            var response = await _client.GetAsync("/Matches/Edit/" + match.Id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_Delete_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var match = _context.Matches.First();

            // Act
            var response = await _client.GetAsync("/Matches/Delete/" + match.Id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_should_save_new_match()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "Id", "0" },
                { "Name", "Integration Test Match" },
                { "StartData", "2024-01-01" },
                { "EndData", "2024-01-01" },
                { "TotalPoints", "15" },
                { "TeamId", "1" },
                { "TournamentId", "1" }
            };

            using var content = new FormUrlEncodedContent(formValues);
            
            // Act
            using var response = await _client.PostAsync("/Matches/Create", content);

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_match()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "Id", "0" },
                { "Name", "" },
                { "StartData", "" },
                { "EndData", "" },
                { "TotalPoints", "0" },
                { "TeamId", "0" },
                { "TournamentId", "0" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Matches/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Edit_should_update_existing_match()
        {
            // Arrange
            var match = _context.Matches.First();
            var matchId = match.Id;
            var formValues = new Dictionary<string, string>
            {
                { "Id", matchId.ToString() },
                { "Name", "Updated Match Name" },
                { "StartData", "2024-01-01" },
                { "EndData", "2024-01-01" },
                { "TotalPoints", "20" },
                { "TeamId", "1" },
                { "TournamentId", "1" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Matches/Edit/" + matchId, content);

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);
        }

        [Fact]
        public async Task Edit_should_not_update_invalid_match()
        {
            // Arrange
            var match = _context.Matches.First();
            var matchId = match.Id;
            var formValues = new Dictionary<string, string>
            {
                { "Id", matchId.ToString() },
                { "Name", "" },
                { "StartData", "" },
                { "EndData", "" },
                { "TotalPoints", "0" },
                { "TeamId", "0" },
                { "TournamentId", "0" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Matches/Edit/" + matchId, content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_should_remove_match()
        {
            // Arrange
            var formValues = new Dictionary<string, string>();
            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Matches/Delete/10", content);

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);
        }
    }
}
