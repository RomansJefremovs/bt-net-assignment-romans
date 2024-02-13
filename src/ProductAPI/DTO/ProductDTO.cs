namespace ProductAPI.DTO;
/// <summary>
/// Data transfer object for Product entity.
/// </summary>
public class ProductDTO
{
    /// <summary>
    /// Unique identifier for the product for the database
    /// </summary>
    public int Id { get; set; }
        
    /// <summary>
    /// Name of the product
    /// </summary>
    /// <value>
    /// Should be a string representing the name of the product
    /// </value>
    public string Name { get; set; }
        
    /// <summary>
    /// Represents the exact value of the product
    /// </summary>
    /// <value>
    /// Should be decimal, since decimal is the best primitive data type to describe financial value
    /// </value>
    public decimal Price { get; set; }
        
    /// <summary>
    /// Represent a unique Category object, that can be used in identification operations and won't require entire Category object to be used
    /// </summary>
    public int CategoryId { get; set; }
}