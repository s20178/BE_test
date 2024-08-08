using BE_test.Controllers;
using BE_test.Interfaces;
using BE_test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<ILogger<ProductsController>> _mockLogger;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllProducts()
        {
            var products = new List<Product> { new Product { Id = 1, Name = "Product1", Price = 10 } };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(products);

            var result = _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Single(returnProducts);
        }

        [Fact]
        public void GetById_ExistingIdPassed_ShouldReturnProduct()
        {
            var product = new Product { Id = 1, Name = "Product1", Price = 10 };
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(product);

            var result = _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal("Product1", returnProduct.Name);
        }

        [Fact]
        public void GetById_NonExistingIdPassed_ShouldReturnNotFound()
        {
            _mockRepository.Setup(repo => repo.GetById(1)).Returns((Product)null);

            var result = _controller.GetById(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Add_ValidObjectPassed_ShouldReturnCreatedResponse()
        {
            var product = new Product { Id = 1, Name = "Product1", Price = 10 };

            var result = _controller.Add(product);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnProduct = Assert.IsType<Product>(createdAtActionResult.Value);
            Assert.Equal("Product1", returnProduct.Name);
        }

        [Fact]
        public void Update_ValidObjectPassed_ShouldReturnNoContent()
        {
            var product = new Product { Id = 1, Name = "Product1", Price = 10 };
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(product);

            var result = _controller.Update(1, product);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_ExistingIdPassed_ShouldReturnNoContent()
        {
            var product = new Product { Id = 1, Name = "Product1", Price = 10 };
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(product);

            var result = _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_NonExistingIdPassed_ShouldReturnNotFound()
        {
            _mockRepository.Setup(repo => repo.GetById(1)).Returns((Product)null);

            var result = _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
