using BE_test.Interfaces;
using BE_test.Models;


namespace BE_test.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = [];

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }
        public Product? GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Product name cannot be null or empty.", nameof(product));
            }

            if (product.Price <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero.", nameof(product));
            }

            _products.Add(product);
        }

        public void Update(Product product)
        {
            var exisitingProduct = GetById(product.Id);
            if (exisitingProduct != null)
            {
                exisitingProduct.Name = product.Name;
                exisitingProduct.Price = product.Price;
            }
        }
        public void Delete(Product product)
        {

            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}
