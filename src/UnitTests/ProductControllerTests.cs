using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Controllers;
using ProductAPI.Data;
using ProductAPI.DTO;
using ProductAPI.Entities;

namespace UnitTests
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetProducts_ReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetProducts_ReturnsAllProducts")
                .Options;
            
            using (var context = new ApplicationDbContext(options))
            {
                context.Products.AddRange(
                    new Product { Id = 1, Name = "Product 1", Price = 10, CategoryId = 1 },
                    new Product { Id = 2, Name = "Product 2", Price = 20, CategoryId = 1 }
                );
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ProductsController(context);
                
                var result = await controller.GetProducts();
                
                var actionResult = Assert.IsType<ActionResult<IEnumerable<ProductDTO>>>(result);
                var returnValue = Assert.IsAssignableFrom<IEnumerable<ProductDTO>>(actionResult.Value);
                var productList = returnValue.ToList();

                Assert.Equal(2, productList.Count);
                Assert.Equal("Product 1", productList[0].Name);
                Assert.Equal("Product 2", productList[1].Name);
            }
        }

        [Fact]
        public async Task GetProductByID_ReturnsProduct()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetProducts_ReturnsAllProducts")
                .Options;
            
            using (var context = new ApplicationDbContext(options))
            {
                context.Products.AddRange(
                    new Product { Id = 1, Name = "Product 1", Price = 10, CategoryId = 1 },
                    new Product { Id = 2, Name = "Product 2", Price = 20, CategoryId = 1 }
                );
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ProductsController(context);
                
                var result = await controller.GetProduct(1);
                
                var actionResult = Assert.IsType<ActionResult<ProductDTO>>(result);
                var returnValue = Assert.IsAssignableFrom<ProductDTO>(actionResult.Value);
         
                Assert.Equal("Product 1", returnValue.Name);
                Assert.Equal(1, returnValue.Id);
            }
            
        }
    }
}