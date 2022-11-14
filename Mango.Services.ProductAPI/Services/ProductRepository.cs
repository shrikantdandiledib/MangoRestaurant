using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.DTO;
using Mango.Services.ProductAPI.Repository;

namespace Mango.Services.ProductAPI.Models.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContexts _db;
        public Task<ProductDTO> CreateUpdateProduct(ProductDTO product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDTO> GetProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDTO>> GetProducts()
        {
            throw new NotImplementedException();
        }
    }
}
