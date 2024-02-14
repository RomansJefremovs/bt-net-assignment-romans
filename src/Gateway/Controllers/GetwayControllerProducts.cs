using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTO;

namespace Gateway.Controllers
{
    /// <summary>
    /// An API Gateway controller for the Products API distributing requests to the Products API Products controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// The HTTP client factory for creating HTTP clients to send requests to the Products API.
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Constructor that initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="clientFactory">
        /// The HTTP client factory for creating HTTP clients to send requests to the Products API.
        /// </param>
        public ProductsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Gets all products from the Products API from Product Table.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the action result that is an HTTP response.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var client = _clientFactory.CreateClient("ProductsAPI");
            var response = await client.GetAsync("Products");

            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadAsStringAsync();
                return Ok(products);
            }

            return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Gets a product by its identifier from the Products API from Product Table.
        /// </summary>
        /// <param name="id">
        /// The identifier of the product to get.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the action result that is an HTTP response.
        /// </returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var client = _clientFactory.CreateClient("ProductsAPI");
            var response = await client.GetAsync($"Products/{id}");

            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadAsStringAsync();
                return Ok(product);
            }

            return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Posts a product to the Products API to the Product Table.
        /// </summary>
        /// <param name="product">
        /// The product to post.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the action result that is an HTTP response.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDTO product)
        {
            var client = _clientFactory.CreateClient("ProductsAPI");
            var response = await client.PostAsJsonAsync("Products", product);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Delete a product in the Products API in the Product Table.
        /// </summary>
        /// <param name="id">
        /// The identifier of the product to delete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the action result that is an HTTP response.
        /// </returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _clientFactory.CreateClient("ProductsAPI");
            var response = await client.DeleteAsync($"Products/{id}");

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }
    }
}