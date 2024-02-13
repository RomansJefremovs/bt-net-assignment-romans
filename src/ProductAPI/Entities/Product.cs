using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Entities;

/// <summary>
/// Model describing the Product
/// </summary>
    public class Product
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
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        
        /// <summary>
        /// Represents the category object that can be used to identify the details of the product
        /// </summary>
        /// <value>
        /// A Category class defined in a separate file Category
        /// </value>
        /// <remarks>
        /// Virtual for lazy-load
        /// </remarks>
        public virtual Category Category { get; set; }
        
        /// <summary>
        /// Represent a unique Category object, that can be used in identification operations and won't require entire Category object to be used
        /// </summary>
        public int CategoryId { get; set; }
    }