using Mango.Services.ProductAPI.Models.DTO;

namespace Mango.Web.Services.IServices
{
    public interface IProductService:IBaseService
    {
        Task<T> GetAllProductsAsync<T>();
        Task<T> GetProductByIdAsync<T>(int id);
        Task<T> CreateProduct<T>(ProductDTO productDTO);
        Task<T> UpdateProduct<T>(ProductDTO productDTO);
        Task<T> DeleteProduct<T>(int id);
    }
}
