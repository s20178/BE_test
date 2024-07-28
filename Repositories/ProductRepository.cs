using BE_test.Interfaces;
using BE_test.Models;


namespace BE_test.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new List<Product>();

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }
        public Product GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
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
        public void DeleteById(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}
