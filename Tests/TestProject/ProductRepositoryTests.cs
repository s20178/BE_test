using BE_test.Models;
using BE_test.Repositories;

namespace TestProject
{
    public class ProductRepositoryTests
    {
        [Fact]
        public void Add_Product_ShouldAddProduct()
        {
            var repository = new ProductRepository();
            var product = new Product { Id = 1, Name = "Product1", Price = 10 };

            repository.Add(product);

            var addedProduct = repository.GetById(1);
            Assert.NotNull(addedProduct);
            Assert.Equal("Product1", addedProduct.Name);
            Assert.Equal(10, addedProduct.Price);
        }

        [Fact]
        public void GetAll_ShouldReturnAllProducts()
        {
            var repository = new ProductRepository();
            repository.Add(new Product { Id = 1, Name = "Product1", Price = 10 });
            repository.Add(new Product { Id = 2, Name = "Product2", Price = 20 });

            var products = repository.GetAll();

            Assert.Equal(2, products.Count());
        }

        [Fact]
        public void GetById_ShouldReturnCorrectProduct()
        {
            var repository = new ProductRepository();
            repository.Add(new Product { Id = 1, Name = "Product1", Price = 10 });

            var product = repository.GetById(1);

            Assert.NotNull(product);
            Assert.Equal("Product1", product.Name);
        }

        [Fact]
        public void Update_Product_ShouldUpdateProduct()
        {
            var repository = new ProductRepository();
            repository.Add(new Product { Id = 1, Name = "Product1", Price = 10 });
            var updatedProduct = new Product { Id = 1, Name = "UpdatedProduct", Price = 15 };

            repository.Update(updatedProduct);

            var product = repository.GetById(1);
            Assert.NotNull(product);
            Assert.Equal("UpdatedProduct", product.Name);
            Assert.Equal(15, product.Price);
        }

        [Fact]
        public void Delete_Product_ShouldRemoveProduct()
        {
            var repository = new ProductRepository();
            var product = new Product { Id = 1, Name = "Product1", Price = 10 };
            repository.Add(product);

            repository.Delete(product);

            var deletedProduct = repository.GetById(1);
            Assert.Null(deletedProduct);
        }
    }
}