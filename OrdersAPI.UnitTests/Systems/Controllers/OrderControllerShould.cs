using App.Data.Models;
using App.Models.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using OrdersAPI.Main;
using System.Net;
using System.Text;
using System.Text.Json;
using ThirdParty.Json.LitJson;

namespace OrdersAPI.UnitTests.Systems.Controllers
{
    public class OrderControllerShould
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private MongoClient _client;
        private IMongoDatabase _db;
        private IMongoCollection<Order> _orders;

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
            Environment.SetEnvironmentVariable("MongoDB__Database", "IntegrationOrders");

            // Each test will get its own "fresh copy of the DB.  This strategy can be argued on performance vs reliability vs others, but it is fine for the size of application and time constraints.
            // In a properly designed application this would likey move elsewhere entirely, so not trying to make it perfect.
            InitializeDBForIntegrationTests("mongodb://root:example@host.docker.internal:27017/", "IntegrationOrders");
        }

        #region Post
        [Fact]
        public async Task PostReturnUnauthorized()
        {
            // Arrange
            var _client = _factory.CreateClient();

            // Act
            var response = await _client.PostAsync("/api/Orders", new StringContent("[]", Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task PostDataOk()
        {
            // Arrange
            var _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer abcde-12345");
            _client.DefaultRequestHeaders.Add("userId", "User1");

            IEnumerable<OrderItem> items = new List<OrderItem>() {
                new OrderItem
                {
                    Name = "Item99",
                    Quantity = 1,
                }
                };

            PostOrderDto dto = new PostOrderDto() { Items = items };

            string json = dto.ToJson();

            // Act
            var response = await _client.PostAsync("/api/Orders", new StringContent(json, Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //Verify data made it to DB.  This could be argued as bad design, but it was fast for this effort.  Could also call through the API to retrieve data.
            Order order = _orders.Find(o => o.Items.Any(i => i.Name == "Item99")).Single();

            Assert.NotNull(order);
        }
        #endregion

        #region GetAll


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

            //convert to objects
            IEnumerable<GetOrderDto> orders = JsonSerializer.Deserialize<IEnumerable<GetOrderDto>>(responseString);

            // Should be 3 orders
            Assert.Equal(3, orders.Count());

            // Should not have recalled data for any user other than User1, there is data for another user in the DB
            Assert.True(orders.All(o => o.User == "User1"));

        }
        #endregion

        #region GetSingle
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
        public async Task GetSingleReturnNoContentForNonexistentOrder()
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

        [Fact]
        public async Task GetSingleReturnOKForOwningUser()
        {
            // Arrange

            var _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer abcde-12345");
            _client.DefaultRequestHeaders.Add("userId", "User2");

            // Act
            var response = await _client.GetAsync("/api/Orders/6612b4034d5dcaadfa568e7f");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


            //convert to objects
            GetOrderDto order = JsonSerializer.Deserialize<GetOrderDto>(responseString);
            Assert.NotNull(order);            
        }

        [Fact]
        public async Task GetSingleReturnNoContentForIncorrectUser()
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

        #endregion

        #region Put
        [Fact]
        public async Task PutReturnUnauthorized()
        {
            // Arrange
            var _client = _factory.CreateClient();

            // Act
            var response = await _client.PutAsync("/api/Orders/6610c2d857538fcb2f444f47", new StringContent("[]", Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task PutDataOkForOwningUser()
        {
            // Arrange
            var _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer abcde-12345");
            _client.DefaultRequestHeaders.Add("userId", "User1");

            IEnumerable<OrderItem> items = new List<OrderItem>()
            {
                new OrderItem
                {
                    Name = "Item555",
                    Quantity = 1,
                },
                new OrderItem
                {
                    Name = "Item666",
                    Quantity = 2,
                }
            };

            string json = items.ToJson();



            // Act
            var response = await _client.PutAsync("/api/Orders/6612f4064d5acaadfa568e7f", new StringContent(json, Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Order order = _orders.Find(o => o.Id.Equals("6612f4064d5acaadfa568e7f")).Single();

            Assert.NotNull(order);
            Assert.Equal(2, order.Items.Count());
            Assert.Equal(1, order.Items.Where(i => i.Name == "Item555").Count());
            Assert.Equal(1, order.Items.Where(i => i.Name == "Item666").Count());

        }

        [Fact]
        public async Task PutDataOkForNonOwningUser()
        {
            // Arrange
            var _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer abcde-12345");
            _client.DefaultRequestHeaders.Add("userId", "User2");

            IEnumerable<OrderItem> items = new List<OrderItem>()
            {
                new OrderItem
                {
                    Name = "Item555",
                    Quantity = 1,
                },
                new OrderItem
                {
                    Name = "Item666",
                    Quantity = 2,
                }
            };

            string json = items.ToJson();



            // Act
            var response = await _client.PutAsync("/api/Orders/6612f4064d5acaadfa568e7f", new StringContent(json, Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Order order = _orders.Find(o => o.Id.Equals("6612f4064d5acaadfa568e7f")).Single();

            // Order still exists
            Assert.NotNull(order);

            // But was not updated
            Assert.Equal(1, order.Items.Count());
            Assert.Equal(0, order.Items.Where(i => i.Name == "Item555").Count());
            Assert.Equal(0, order.Items.Where(i => i.Name == "Item666").Count());

        }
        #endregion

        #region Delete
        [Fact]
        public async Task DeleteReturnUnauthorized()
        {
            // Arrange
            var _client = _factory.CreateClient();

            // Act
            var response = await _client.DeleteAsync("/api/Orders/6610c2d857538fcb2f444f47");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task DeleteReturnOKForRandomOrder()
        {
            // Arrange
            var _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer abcde-12345");
            _client.DefaultRequestHeaders.Add("userId", "User1");

            // Act
            var response = await _client.DeleteAsync("/api/Orders/6610c2d857538fcb2f444f47");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteReturnOKForOwningUser()
        {
            // Arrange
            var _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer abcde-12345");
            _client.DefaultRequestHeaders.Add("userId", "User1");

            // Act
            var response = await _client.DeleteAsync("/api/Orders/6612f4064d5dcaadfa568e7f");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Order order = _orders.Find(o => o.Id.Equals("6612f4064d5dcaadfa568e7f")).SingleOrDefault();

            // Item was removed
            Assert.Null(order);
            
        }

        [Fact]
        public async Task DeleteReturnOKForNonOwningUser()
        {
            // Arrange
            var _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer abcde-12345");
            _client.DefaultRequestHeaders.Add("userId", "User2");

            // Act
            var response = await _client.DeleteAsync("/api/Orders/6612f4064d5dcaadfa568e7f");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Order order = _orders.Find(o => o.Id.Equals("6612f4064d5dcaadfa568e7f")).SingleOrDefault();

            // Item was not removed
            Assert.NotNull(order);
        }
        #endregion






        #region DBInitialization

        private void InitializeDBForIntegrationTests(string mongoConnectionString, string database)
        {
            _client = new MongoClient(mongoConnectionString);

            // Drop integration to "start fresh"
            if (_client.ListDatabaseNames().ToList().Contains(database))
            {
                _client.DropDatabase(database);
            }

            _db = _client.GetDatabase(database);
            _orders = _db.GetCollection<Order>("Orders");

            var orders = GenerateDocuments();
            // Inserts the new documents into the restaurants collection
            _orders.InsertMany(orders);
        }

        private IEnumerable<Order> GenerateDocuments()
        {
            //A set of known data to run against for integration tests.

            return new[]
            {
                new Order
                {
                    Id = "6612f4064d5acaadfa568e7f",
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    User = "User1",
                    Items = new[]
                    {
                        new OrderItem {
                            Name = "Item1",
                            Quantity = 5
                       }
                    }
                },
                new Order
                {
                    Id = "6612f4062d5acaadfa568e7f",
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    User = "User1",
                    Items = new[]
                    {
                        new OrderItem {
                            Name = "Item2",
                            Quantity = 1
                       }
                    }
                },
                new Order
                {
                    Id = "6612f4064d5dcaadfa568e7f",
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    User = "User1",
                    Items = new[]
                    {
                        new OrderItem {
                            Name = "Item3",
                            Quantity = 5
                       },
                        new OrderItem {
                            Name = "Item3",
                            Quantity = 5
                       }
                    }
                },
                new Order
                {
                    Id = "6212f4064d5dcaadfa568e7f",
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    User = "User2",
                    Items = new[]
                    {
                        new OrderItem {
                            Name = "Item4",
                            Quantity = 2
                       },
                        new OrderItem {
                            Name = "Item1",
                            Quantity = 24
                       }
                    }
                },
                new Order
                {
                    Id = "6612b4034d5dcaadfa568e7f",
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    User = "User2",
                    Items = new[]
                    {
                        new OrderItem {
                            Name = "Item8",
                            Quantity = 63
                       },
                        new OrderItem {
                            Name = "Item2",
                            Quantity = 2
                       }
                    }
                }
            };
        } 
        #endregion
    }
}
