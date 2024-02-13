using ProductAPI.Entities;
namespace ProductAPI.Data
{
    /// <summary>
    /// Provides methods for initial population of categories and products Dummy data.
    /// </summary>
    public class DataSeeder
    {
        private readonly ILogger<DataSeeder> _logger;

        /// <summary>
        /// Constructor to initialize a new instance of <see cref="DataSeeder"/> class.
        /// </summary>
        /// <param name="logger">
        /// Logger object for logging messages of success and errors.
        /// </param>
        public DataSeeder(ILogger<DataSeeder> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// A method for seeding the initial categories loops through a list of provided categories, adds to the context Categories table and saves.
        /// </summary>
        /// <param name="context">
        /// Database instance used to perform operations on database.
        /// </param>
        public void SeedCategories(ApplicationDbContext context)
        {
            try
            {
                if (context.Categories.Any())
                {
                    _logger.LogInformation("Categories already exist. Skipping seeding.");
                    return;
                }

                List<string> clothingCategories = new List<string>
                {
                    "Tops",
                    "Bottoms",
                    "Dresses",
                    "Outerwear",
                    "Activewear",
                    "Sleepwear",
                    "Swimwear",
                    "Formalwear",
                    "Undergarments",
                    "Accessories",
                    "Unclassified"
                };

                var categories = new List<Category>();
                foreach (var category in clothingCategories)
                {
                    categories.Add(new Category { Name = category });
                }

                context.Categories.AddRange(categories);
                context.SaveChanges();
                _logger.LogInformation("Categories seeded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding categories.");
                throw;
            }
        }

        /// <summary>
        /// A method for seeding the initial products, depending on weather the categories are present in the database is seeding products by category or just a dozen of products.
        /// </summary>
        /// <param name="context">
        /// Database instance used to perform operations on database.
        /// </param>
        public void SeedProducts(ApplicationDbContext context)
        {
            try
            {
                if (context.Products.Any())
                {
                    _logger.LogInformation("Products already exist. Skipping seeding.");
                    return;
                }

                var products = new List<Product>();

                var categories = context.Categories.ToList();
                if (categories.Count == 0)
                {
                    SeedDozenProducts(context, products);
                }
                else
                {
                    SeedByCategory(products, categories);
                }
                
                context.Products.AddRange(products);
                context.SaveChanges();
                _logger.LogInformation("Products seeded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding products.");
                throw;
            }
        }
        /// <summary>
        /// Method for populating a list of the products with dummy data, iterates for 100 times, creating a 100 Products with incremented values.
        /// </summary>
        /// <param name="context"> Instance of database, used to add Unclassified category and to select a path for populating products list depending on weather the category was added or not.</param>
        /// <param name="products">Products list used to store and populate products locally.</param>
        private void SeedDozenProducts(ApplicationDbContext context, List<Product> products)
        {
            try
            {
                var cat = new Category { Name = "Unclassified" };
                var addedCat = context.Categories.Add(cat);
                context.SaveChanges();

                var catFromDb = context.Categories.Find(addedCat.Entity.Id);
                if (catFromDb != null)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        products.Add(new Product {Name = "Product" + i, Price = 100 + i, Category = catFromDb, CategoryId = catFromDb.Id});
                    }
                }
                else
                {
                    for (int i = 0; i < 100; i++)
                    {
                        products.Add(new Product {Name = "Product" + i, Price = 100 + i});
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"An error occured while seeding dozen products.");
                throw;
            }
           
        }

        /// <summary>
        /// Method for populating a list of the products by category iterates through each category in the list and adds a product to the products list with according category.
        /// </summary>
        /// <param name="categories"> Categories list, containing the instances of categories from database.</param>
        /// <param name="products"> Products list used to store and populate products locally.</param>
        private void SeedByCategory(List<Product> products, List<Category> categories)
        {
            try
            {
                var index = 0;
                foreach (var category in categories)
                {
                    products.Add(new Product
                        { Name = "Product" + category.Name, Price = 100 + index * 10, CategoryId = category.Id });
                    index++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occured while seeding the products by category");
                throw;
            }
           
        }
    }
}
