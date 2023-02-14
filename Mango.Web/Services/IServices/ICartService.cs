using Mango.Services.ShoppingCartAPI.Models.DTO;

namespace Mango.Web.Services.IServices
{
    public interface ICartService
    {
        Task<T> GetCartByUserIdAsnyc<T>(string userId, string token = null);
        Task<T> AddToCartAsync<T>(CartDTO cartDto, string token = null);
        Task<T> UpdateCartAsync<T>(CartDTO cartDto, string token = null);
        Task<T> RemoveFromCartAsync<T>(int cartId, string token = null);
    }
}
