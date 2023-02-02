using Mango.Services.ShoppingCart.IServices;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Models.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ShoppingCart.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly ICartService _cartRepository;
        protected APIResponse<CartDTO> response;
        public CartAPIController(ICartService cartRepository)
        {
            _cartRepository = cartRepository;
            this.response = new APIResponse<CartDTO>();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
           
            try
            {
                var cartDto = await _cartRepository.GetCartByUserId(userId);
                response.Data = cartDto;
                if (response.Data != null)
                {
                    response.Message = "Details found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Exception = new List<string>() { ex.ToString() };
            }
            return response;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart(CartDTO cartDto)
        {
            try
            {
                var cartDt = await _cartRepository.CreateUpdateCart(cartDto);
                response.Data = cartDt;
                response.Message = "Saved Successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Exception = new List<string>() { ex.ToString() };
            }
            return response;
        }

        [HttpPost("UpdateCart")]
        public async Task<object> UpdateCart(CartDTO cartDto)
        {
            try
            {
                var cartDt = await _cartRepository.CreateUpdateCart(cartDto);
                response.Data = cartDt;
                response.Message = "Updated Successfully.";
            }
            catch (Exception ex) { response.Success = false; response.Exception = new List<string>() { ex.ToString() }; }
            return response;
        }

        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody] int cartId)
        {
            try
            {
                bool isSuccess = await _cartRepository.RemoveFromCart(cartId);
                response.Success = isSuccess;
                response.Message = "Removed Successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Exception = new List<string>() { ex.ToString() };
            }
            return response;
        }
    }
}
