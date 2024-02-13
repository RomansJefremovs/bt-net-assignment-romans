using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Entities;

public class Category
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
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Parent represents a parent category, for hierarchical purposes, since a category can be a sub to other category
    /// allows the navigation from child to parent (e.g. can be used in Breadcrumps)
    /// </summary>
    /// <value>
    /// Category class itself
    /// </value>
    /// <remarks>
    /// Can be nullable, since because some of the categories will be super partent with no parent categories.
    /// </remarks>
    /// <remarks>
    /// Virtual for lazy-load
    /// </remarks>
    public virtual Category? Parent { get; set; }
    
    /// <summary>
    /// Parent category unique identifier for the cases when details of the parent category do not need to be shown.
    /// </summary>
    /// <remarks>
    /// Can also be nullable if it is a super parent without any parent category
    /// </remarks>
    public int? ParentId { get; set; }
    
    /// <summary>
    /// The collection of children categories, that allows bidirectional navigation from parent to children, can be used to build hierarchical tree from top to bottom
    /// </summary>
    /// <value>
    /// List since it can be one to many
    /// </value>
    /// <remarks>
    /// Virtual for lazy-load
    /// </remarks>
    public virtual ICollection<Category> Children { get; set; } = new List<Category>();

}