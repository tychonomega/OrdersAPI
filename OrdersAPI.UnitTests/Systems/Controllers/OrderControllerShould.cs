using Microsoft.AspNetCore.Mvc.Testing;
using OrdersAPI.Main;
using System.Net;

namespace OrdersAPI.UnitTests.Systems.Controllers
{
    public class OrderControllerShould
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public OrderControllerShould(WebApplicationFactory<Program> factory)
        {
            _factory = factory;

            _factory = factory;
            Environment.SetEnvironmentVariable("PORT", "6060");
            Environment.SetEnvironmentVariable("CLIENT_ORIGIN_URL", "http://localhost:4040");
            Environment.SetEnvironmentVariable("AUTH0_AUDIENCE", "https://orders.example.com");
            Environment.SetEnvironmentVariable("AUTH0_DOMAIN", "dev-hloe0m6fbmv25qkt.us.auth0.com");
            Environment.SetEnvironmentVariable("TEST_MODE", "true");
            Environment.SetEnvironmentVariable("MongoDB__ConnectionString", "mongodb://root:example@host.docker.internal:27017/");
            Environment.SetEnvironmentVariable("MongoDB__Database", "Orders");
        }

        [Fact]
        public async Task GetAllReturnUnauthorized()
        {
            // Arrange
            var _client = _factory.CreateClient();

            // Act
            var response = await _client.GetAsync("/api/Orders");
            
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetAllReturnOK()
        {
            // Arrange
            var _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer abcde-12345");
            _client.DefaultRequestHeaders.Add("userId", "User1");

            // Act
            var response = await _client.GetAsync("/api/Orders");

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetSingleReturnUnauthorized()
        {
            // Arrange
            var _client = _factory.CreateClient();

            // Act
            var response = await _client.GetAsync("/api/Orders/6610c2d857538fcb2f444f47");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetSingleReturnNoContentForRandomItem()
        {
            // Arrange

            var _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer abcde-12345");
            _client.DefaultRequestHeaders.Add("userId", "User1");
            // Act
            var response = await _client.GetAsync("/api/Orders/6610c2d857538fcb2f444f47");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
