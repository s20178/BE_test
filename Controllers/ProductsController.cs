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
            var products = _repository.GetAll();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        public IActionResult Add([FromBody] Product product)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingProduct = _repository.GetById(product.Id);
            if (existingProduct != null)
            {
                return Conflict(new { message = $"A product with ID {product.Id} already exists." });
            }

            _repository.Add(product);
            return CreatedAtAction(nameof(GetById), new {id = product.Id}, product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            if(id != product.Id)
            {
                return BadRequest(new { message = "Product ID in the URL does not match the product ID in the body." });
            }
            var existingProduct = _repository.GetById(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            product.Id = id;
            _repository.Update(product);
            return NoContent();
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var product = _repository.GetById(id);
            if(product == null)
            {
                return NotFound();
            }
            _repository.DeleteById(id);
            return NoContent();
        }
    }
}
