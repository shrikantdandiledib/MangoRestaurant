using Mango.Services.ShoppingCartAPI.Models.DTO;

namespace Mango.Services.ShoppingCart.IServices
{
    public interface ICartService
    {
        Task<CartDTO> GetCartByUserId(string userId);
        Task<CartDTO> CreateUpdateCart(CartDTO cartDto);
        Task<bool> RemoveFromCart(int cartDetailsId);
        //Task<bool> ApplyCoupon(string userId, string couponCode);
        //Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);
    }
}
