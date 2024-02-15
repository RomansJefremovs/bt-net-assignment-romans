using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTO;
using ProductAPI.Entities;

namespace ProductAPI.Controllers;

/// <summary>
/// A class that represents the controller for the products, used to handle the requests and responses for the products.
/// </summary>
/// <param name="context">
/// The instance of the database context for the application, used to interact with the database.
/// </param>
[Route("/[controller]")]
[ApiController]
public class ProductsController(ApplicationDbContext context) : ControllerBase
{
    /// <summary>
    /// A method that returns all the products in the database.
    /// </summary>
    /// <returns>
    /// An asynchronous operation that returns a list of products.
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
    {
        var _logger = LoggerFactory
            .Create(builder => builder.AddConsole())
            .CreateLogger<ProductsController>();
        try
        {
            var products = await context
                .Products.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryId = p.CategoryId
                })
                .ToListAsync();

            if (!products.Any())
            {
                _logger.LogInformation("No products found in the database.");
                return NotFound("No products found.");
            }

            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving products.");
            return StatusCode(500, "Internal server error, please try again later."+ex.Message);
        }
    }

    /// <summary>
    ///  A method that returns a product with a specific identifier.
    /// </summary>
    /// <param name="id">
    /// Product identifier
    /// </param>
    /// <returns>
    /// An asynchronous operation that returns a product with a specific identifier or a not found response.
    /// </returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
        var product = await context
            .Products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId
            })
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    /// <summary>
    /// A method that creates a new product in the database.
    /// </summary>
    /// <param name="productDto">
    /// The product data transfer object that contains the information about the product.
    /// </param>
    /// <returns>
    /// An asynchronous operation that returns a created product or a bad request response.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<ProductDTO>> PostProduct(ProductDTO productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            CategoryId = productDto.CategoryId
        };

        context.Products.Add(product);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productDto);
    }

    /// <summary>
    /// A method that deletes a product with a specific identifier from the database.
    /// </summary>
    /// <param name="id">
    /// Product identifier
    /// </param>
    /// <returns>
    /// An asynchronous operation that returns a no content response or a not found response.
    /// </returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
