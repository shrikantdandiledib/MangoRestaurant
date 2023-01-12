using Mango.Services.ProductAPI.Models.DTO;

namespace Mango.Web.Services.IServices
{
    public interface IProductService:IBaseService
    {
        Task<T> GetAllProductsAsync<T>(string accessToken);
        Task<T> GetProductByIdAsync<T>(int id, string accessToken);
        Task<T> CreateProduct<T>(ProductDTO productDTO, string accessToken);
        Task<T> UpdateProduct<T>(ProductDTO productDTO, string accessToken);
        Task<T> DeleteProduct<T>(int id, string accessToken);
    }
}
