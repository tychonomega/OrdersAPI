using Microsoft.AspNetCore.Mvc.Testing;
using OrdersAPI.Main;
using System.Net;

namespace OrdersAPI.UnitTests.Systems.Controllers
{
    public class MessagesControllerShould
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public MessagesControllerShould(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            Environment.SetEnvironmentVariable("PORT", "6060");
            Environment.SetEnvironmentVariable("CLIENT_ORIGIN_URL", "http://localhost:4040");
            Environment.SetEnvironmentVariable("AUTH0_AUDIENCE", "https://orders.example.com");
            Environment.SetEnvironmentVariable("AUTH0_DOMAIN", "dev-hloe0m6fbmv25qkt.us.auth0.com");
            Environment.SetEnvironmentVariable("TEST_MODE", "true");
        }

        [Fact]
        public async Task ReturnPublicMessage()
        {
            // Arrange
            var _client = _factory.CreateClient();


            // Act
            var response = await _client.GetAsync("/api/messages/public");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            
            // Assert
            Assert.Equal("{\"text\":\"This is a public message.\"}", responseString);
        }

        [Fact]
        public async Task ReturnUnauthorizedProtectedMessageWithoutAuth()
        {
            // Arrange
            var _client = _factory.CreateClient();
          
            // Act
            var response = await _client.GetAsync("/api/messages/protected");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ReturnProtectedMessageWithAuth()
        {
            // Arrange
            var _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer abcde-12345");
            _client.DefaultRequestHeaders.Add("userId", "User1");
            // Act
            var response = await _client.GetAsync("/api/messages/protected");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("{\"text\":\"This is a protected message.\"}", responseString);
        }
    }
}
