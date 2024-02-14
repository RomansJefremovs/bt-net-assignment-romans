using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class ProductsApiTests : IClassFixture<WebApplicationFactory<ProductAPI.Startup>>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly HttpClient _client;

        public ProductsApiTests(WebApplicationFactory<ProductAPI.Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost:5095/")
            });
        }

        [Fact]
        public async Task Get_Products_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/Products");
            _testOutputHelper.WriteLine(response.ToString());
            _testOutputHelper.WriteLine(_client.BaseAddress.ToString());
    
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async Task Get_ProductById_ReturnsSuccessStatusCode()
        {
            var testProductId = 1;
            
            var response = await _client.GetAsync($"/Products/{testProductId}");
            _testOutputHelper.WriteLine(response.ToString());
       
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}