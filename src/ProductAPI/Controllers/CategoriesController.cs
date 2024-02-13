using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTO;
using ProductAPI.Entities;

namespace ProductAPI.Controllers;

/// <summary>
/// A class that represents the controller for the categories, used to handle the requests and responses for the categories.
/// </summary>
/// <param name="context">
/// The instance of the database context for the application, used to interact with the database.
/// </param>
[Route("/[controller]")]
[ApiController]
public class CategoriesController(ApplicationDbContext context): ControllerBase
{
    /// <summary>
    /// A method that returns all the categories in the database.
    /// </summary>
    /// <returns>
    /// An asynchronous operation that returns a list of categories.
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetProducts()
    {
        return await context.Categories
            .Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId
            })
            .ToListAsync();
    }
    
    /// <summary>
    /// A method that returns a category with a specific identifier or a not found response.
    /// </summary>
    /// <param name="id">
    /// Category identifier
    /// </param>
    /// <returns>
    /// An asynchronous operation that returns a category with a specific identifier or a not found response.
    /// </returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDTO>> GetProduct(int id)
    {
        var product = await context.Categories
            .Select(p => new CategoryDTO
            {
                Id = p.Id,
                Name = p.Name,
                ParentId = p.ParentId
            })
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }
    
    /// <summary>
    /// A method that updates a category with a specific identifier or a not found response.
    /// </summary>
    /// <param name="categoryDto">
    /// The instance of the category data transfer object.
    /// </param>
    /// <returns>
    /// An asynchronous operation that returns a category with a specific identifier or a not found response.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> PostProduct(CategoryDTO categoryDto)
    {
        var product = new Category
        {
            Name = categoryDto.Name,
            ParentId = categoryDto.ParentId
        };

        context.Categories.Add(product);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, categoryDto);
    }
    
    /// <summary>
    /// A method that deletes a category with a specific identifier or a not found response.
    /// </summary>
    /// <param name="id">
    /// The identifier of the category to be deleted.
    /// </param>
    /// <returns>
    /// An asynchronous operation that returns a no content response or a not found response.
    /// </returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await context.Categories.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        context.Categories.Remove(product);
        await context.SaveChangesAsync();

        return NoContent();
    }
}