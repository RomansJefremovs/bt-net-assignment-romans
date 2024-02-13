namespace ProductAPI.DTO;

/// <summary>
/// A class that represents the data transfer object for the category.
/// </summary>
public class CategoryDTO
{
    /// <summary>
    /// Unique identifier for database
    /// </summary>
    /// <value>
    /// Integer for simplicity
    /// </value>
    public int Id { get; set; }
    
    /// <summary>
    /// Represents a Name of the category
    /// </summary>
    /// <value>
    /// String, since it can be a combination of characters, numbers, special characters and identifiers
    /// </value>
    public string Name { get; set; }
    
    /// <summary>
    /// Parent category unique identifier for the cases when details of the parent category do not need to be shown.
    /// </summary>
    /// <remarks>
    /// Can also be nullable if it is a super parent without any parent category
    /// </remarks>
    public int? ParentId { get; set; }
    


}