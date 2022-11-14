using Mango.Services.ProductAPI.DTO;

namespace Mango.Services.ProductAPI.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>> GetProducts(); 
        Task<ProductDTO> GetProduct(int productId); 
        Task<ProductDTO> CreateUpdateProduct(ProductDTO product);
        Task<bool> DeleteProduct(int productId);
    }
}
