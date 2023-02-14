using Mango.Services.Helpers;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly IHttpClientFactory _clientFactory;

        public CartService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> AddToCartAsync<T>(CartDTO cartDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                APIType = Constants.APIType.POST,
                Data = cartDto,
                Url = Constants.ShoppingCartAPIBase + "/api/cart/AddCart",
                AccessToken = token
            });
        }

        public async Task<T> GetCartByUserIdAsnyc<T>(string userId, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                APIType = Constants.APIType.GET,
                Url = Constants.ProductAPIBase + "/api/cart/GetCart/" + userId,
                AccessToken = token
            });
        }

        public async Task<T> RemoveFromCartAsync<T>(int cartId, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                APIType = Constants.APIType.POST,
                Data = cartId,
                Url = Constants.ShoppingCartAPIBase + "/api/cart/RemoveCart",
                AccessToken = token
            });
        }

        public async Task<T> UpdateCartAsync<T>(CartDTO cartDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                APIType = Constants.APIType.POST,
                Data = cartDto,
                Url = Constants.ShoppingCartAPIBase + "/api/cart/UpdateCart",
                AccessToken = token
            });
        }
    }
}
