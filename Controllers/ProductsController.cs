using BE_test.Interfaces;
using BE_test.Models;
using Microsoft.AspNetCore.Mvc;

namespace BE_test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Getting all products info");
            var products = _repository.GetAll();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation("Getting product with ID {ProductId}", id);
            var product = _repository.GetById(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", id);
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        public IActionResult Add([FromBody] Product product)
        {
            _logger.LogInformation("Adding new product with ID {ProductId}", product.Id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingProduct = _repository.GetById(product.Id);
            if (existingProduct != null)
            {
                _logger.LogWarning("Product with ID {ProductId} already exists", product.Id);
                return Conflict(new { message = $"A product with ID {product.Id} already exists." });
            }

            _repository.Add(product);
            _logger.LogInformation("Product with ID {ProductId} added successfully", product.Id);
            return CreatedAtAction(nameof(GetById), new {id = product.Id}, product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            _logger.LogInformation("Updating product with ID {ProductId}", id);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for product with ID {ProductId}", id);
                return BadRequest();
            }
            if(id != product.Id)
            {
                _logger.LogWarning("Product ID in URL does not match ID in body for product with ID {ProductId}", id);
                return BadRequest(new { message = "Product ID in the URL does not match the product ID in the body." });
            }
            var existingProduct = _repository.GetById(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            product.Id = id;
            _repository.Update(product);
            _logger.LogInformation("Product with ID {ProductId} updated successfully", product.Id);
            return NoContent();
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Deleting product with ID {ProductId}", id);
            var product = _repository.GetById(id);
            if(product == null)
            {
                return NotFound();
            }
            _repository.DeleteById(id);
            _logger.LogInformation("Product with ID {ProductId} deleted successfully", id);
            return NoContent();
        }
    }
}
