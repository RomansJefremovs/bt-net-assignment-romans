using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTO;

namespace Gateway.Controllers;

/// <summary>
/// An API Gateway controller for the Categories API distributing requests to the Products API Categories controller.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    /// <summary>
    /// The HTTP client factory for creating HTTP clients to send requests to the Products API.
    /// </summary>
    private readonly IHttpClientFactory _clientFactory;

    /// <summary>
    /// Constructor that initializes a new instance of the <see cref="CategoriesController"/> class.
    /// </summary>
    /// <param name="clientFactory">
    /// The HTTP client factory for creating HTTP clients to send requests to the Products API.
    /// </param>
    public CategoriesController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    /// <summary>
    /// Gets all categories from the Products API from Category Table.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the action result that is an HTTP response.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        try
        {
            var client = _clientFactory.CreateClient("ProductsAPI");
            var response = await client.GetAsync("Categories/");

            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadAsStringAsync();
                return Ok(products);
            }

            return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }catch (Exception ex)
        {
            return StatusCode(500, "Internal server error, please try again later." + ex.Message);
        }
    }

    /// <summary>
    /// Gets a category by its identifier from the Products API from Category Table.
    /// </summary>
    /// <param name="id">
    /// The identifier of the category to get.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the action result that is an HTTP response.
    /// </returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        try
        {
            var client = _clientFactory.CreateClient("ProductsAPI");
            var response = await client.GetAsync($"Categories/{id}");

            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadAsStringAsync();
                return Ok(product);
            }

            return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }catch (Exception ex)
        {
            return StatusCode(500, "Internal server error, please try again later." + ex.Message);
        }
    }

    /// <summary>
    /// Posts a category to the Products API Category Table.
    /// </summary>
    /// <param name="category">
    /// The category to post.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the action result that is an HTTP response.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> PostCategory([FromBody] CategoryDTO category)
    {
        try
        {
            var client = _clientFactory.CreateClient("ProductsAPI");
            var response = await client.PostAsJsonAsync("Categories/", category);
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }catch (Exception ex)
        {
            return StatusCode(500, "Internal server error, please try again later." + ex.Message);
        }
    }
    
    /// <summary>
    /// Deletes a category by its identifier in the Products API Category Table.
    /// </summary>
    /// <param name="id">
    /// The identifier of the category to delete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the action result that is an HTTP response.
    /// </returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            var client = _clientFactory.CreateClient("ProductsAPI");
            var response = await client.DeleteAsync($"Categories/{id}");

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }catch (Exception ex)
        {
            return StatusCode(500, "Internal server error, please try again later." + ex.Message);
        }
    }
}
